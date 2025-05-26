using inventrack_backend_demo.DTOs;
using inventrack_backend_demo.Model;
using inventrack_backend_demo.Data;
using Microsoft.EntityFrameworkCore;

namespace inventrack_backend_demo.Repositories
{
    #region Order Management Repositories

    public interface IOrderRepository
    {
        Task<OrderDto> GetOrderByIdAsync(int orderId);
        Task<List<OrderDto>> GetOrdersByCustomerAsync(int customerId);
        Task<List<OrderDto>> GetOrdersByStatusAsync(string status);
        Task<OrderDto> CreateOrderAsync(OrderCreateDto orderCreateDto);
        Task<OrderDto> UpdateOrderAsync(int orderId, OrderDto orderDto);
        Task<bool> CancelOrderAsync(int orderId);
    }

    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OrderDto> GetOrderByIdAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.Customer)
                    .ThenInclude(c => c.User)
                .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                .Include(o => o.Items)
                    .ThenInclude(i => i.Variant)
                .Include(o => o.Items)
                    .ThenInclude(i => i.Warehouse)
                .Include(o => o.StatusHistory)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null) return null;

            return new OrderDto
            {
                OrderId = order.OrderId,
                OrderNumber = order.OrderNumber,
                CustomerId = order.CustomerId,
                OrderDate = order.OrderDate,
                OrderSource = order.OrderSource,
                Status = order.Status,
                SubTotal = order.SubTotal,
                TaxAmount = order.TaxAmount,
                ShippingAmount = order.ShippingAmount,
                DiscountAmount = order.DiscountAmount,
                TotalAmount = order.TotalAmount,
                PaymentMethod = order.PaymentMethod,
                PaymentStatus = order.PaymentStatus,
                BillingAddress = order.BillingAddress,
                ShippingAddress = order.ShippingAddress,
                Notes = order.Notes,
                ExpectedDeliveryDate = order.ExpectedDeliveryDate,
                Customer = new CustomerDto
                {
                    CustomerId = order.Customer.CustomerId,
                    FirstName = order.Customer.FirstName,
                    LastName = order.Customer.LastName,
                    Email = order.Customer.Email,
                    User = new UserDto
                    {
                        UserId = order.Customer.User.UserId,
                        Username = order.Customer.User.Username
                    }
                },
                Items = order.Items?.Select(i => new OrderItemDto
                {
                    OrderItemId = i.OrderItemId,
                    OrderId = i.OrderId,
                    ProductId = i.ProductId,
                    VariantId = i.VariantId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    Discount = i.Discount,
                    TotalPrice = i.TotalPrice,
                    Status = i.Status,
                    WarehouseId = i.WarehouseId,
                    Product = new ProductDto
                    {
                        ProductId = i.Product.ProductId,
                        SKU = i.Product.SKU,
                        Name = i.Product.Name
                    },
                    Variant = i.Variant != null ? new ProductVariantDto
                    {
                        VariantId = i.Variant.VariantId,
                        VariantName = i.Variant.VariantName
                    } : null,
                    Warehouse = new WarehouseDto
                    {
                        WarehouseId = i.Warehouse.WarehouseId,
                        WarehouseName = i.Warehouse.WarehouseName
                    }
                }).ToList(),
                StatusHistory = order.StatusHistory?.Select(sh => new OrderStatusDto
                {
                    StatusId = sh.StatusId,
                    OrderId = sh.OrderId,
                    Status = sh.Status,
                    StatusDate = sh.StatusDate,
                    Notes = sh.Notes
                }).ToList()
            };
        }

        public async Task<List<OrderDto>> GetOrdersByCustomerAsync(int customerId)
        {
            return await _context.Orders
                .Where(o => o.CustomerId == customerId)
                .Include(o => o.Customer)
                .Select(o => new OrderDto
                {
                    OrderId = o.OrderId,
                    OrderNumber = o.OrderNumber,
                    CustomerId = o.CustomerId,
                    OrderDate = o.OrderDate,
                    Status = o.Status,
                    TotalAmount = o.TotalAmount,
                    PaymentStatus = o.PaymentStatus,
                    Customer = new CustomerDto
                    {
                        CustomerId = o.Customer.CustomerId,
                        FirstName = o.Customer.FirstName,
                        LastName = o.Customer.LastName
                    }
                }).ToListAsync();
        }

        public async Task<List<OrderDto>> GetOrdersByStatusAsync(string status)
        {
            return await _context.Orders
                .Where(o => o.Status == status)
                .Include(o => o.Customer)
                .Select(o => new OrderDto
                {
                    OrderId = o.OrderId,
                    OrderNumber = o.OrderNumber,
                    CustomerId = o.CustomerId,
                    OrderDate = o.OrderDate,
                    Status = o.Status,
                    TotalAmount = o.TotalAmount,
                    Customer = new CustomerDto
                    {
                        CustomerId = o.Customer.CustomerId,
                        FirstName = o.Customer.FirstName,
                        LastName = o.Customer.LastName
                    }
                }).ToListAsync();
        }

        public async Task<OrderDto> CreateOrderAsync(OrderCreateDto orderCreateDto)
        {
            var order = new Order
            {
                OrderNumber = GenerateOrderNumber(),
                CustomerId = orderCreateDto.CustomerId,
                OrderDate = DateTime.UtcNow,
                OrderSource = orderCreateDto.OrderSource,
                Status = "Pending",
                BillingAddress = orderCreateDto.BillingAddress,
                ShippingAddress = orderCreateDto.ShippingAddress,
                Notes = orderCreateDto.Notes
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Add items
            foreach (var itemDto in orderCreateDto.Items)
            {
                var product = await _context.Products.FindAsync(itemDto.ProductId);
                if (product == null) continue;

                var orderItem = new OrderItem
                {
                    OrderId = order.OrderId,
                    ProductId = itemDto.ProductId,
                    VariantId = itemDto.VariantId,
                    Quantity = itemDto.Quantity,
                    UnitPrice = itemDto.UnitPrice,
                    Discount = itemDto.Discount,
                    TotalPrice = itemDto.Quantity * (itemDto.UnitPrice - itemDto.Discount),
                    Status = "Pending",
                    WarehouseId = itemDto.WarehouseId
                };

                _context.OrderItems.Add(orderItem);
            }

            // Calculate totals
            order.SubTotal = order.Items.Sum(item => item.TotalPrice);
            order.TaxAmount = order.SubTotal * 0.1m; // Example 10% tax
            order.ShippingAmount = CalculateShipping(order);
            order.TotalAmount = order.SubTotal + order.TaxAmount + order.ShippingAmount;

            // Add initial status
            _context.OrderStatuses.Add(new OrderStatus
            {
                OrderId = order.OrderId,
                Status = "Pending",
                StatusDate = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();

            return await GetOrderByIdAsync(order.OrderId);
        }

        public async Task<OrderDto> UpdateOrderAsync(int orderId, OrderDto orderDto)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return null;

            order.Status = orderDto.Status;
            order.PaymentStatus = orderDto.PaymentStatus;
            order.Notes = orderDto.Notes;

            if (orderDto.Status != order.Status)
            {
                _context.OrderStatuses.Add(new OrderStatus
                {
                    OrderId = orderId,
                    Status = orderDto.Status,
                    StatusDate = DateTime.UtcNow
                });
            }

            await _context.SaveChangesAsync();
            return await GetOrderByIdAsync(orderId);
        }

        public async Task<bool> CancelOrderAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return false;

            if (order.Status == "Shipped" || order.Status == "Delivered")
                return false; // Can't cancel shipped/delivered orders

            order.Status = "Cancelled";
            _context.OrderStatuses.Add(new OrderStatus
            {
                OrderId = orderId,
                Status = "Cancelled",
                StatusDate = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            return true;
        }

        private string GenerateOrderNumber()
        {
            return "ORD-" + DateTime.Now.ToString("yyyyMMdd") + "-" + new Random().Next(1000, 9999);
        }

        private decimal CalculateShipping(Order order)
        {
            // Simple shipping calculation - could be replaced with more complex logic
            return 10.00m; // Flat rate shipping for example
        }
    }

    public interface IOrderItemRepository
    {
        Task<OrderItemDto> GetOrderItemByIdAsync(int orderItemId);
        Task<List<OrderItemDto>> GetOrderItemsByOrderAsync(int orderId);
        Task<OrderItemDto> UpdateOrderItemStatusAsync(int orderItemId, string status);
    }

    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OrderItemDto> GetOrderItemByIdAsync(int orderItemId)
        {
            var item = await _context.OrderItems
                .Include(i => i.Product)
                .Include(i => i.Variant)
                .Include(i => i.Warehouse)
                .FirstOrDefaultAsync(i => i.OrderItemId == orderItemId);

            if (item == null) return null;

            return new OrderItemDto
            {
                OrderItemId = item.OrderItemId,
                OrderId = item.OrderId,
                ProductId = item.ProductId,
                VariantId = item.VariantId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                Discount = item.Discount,
                TotalPrice = item.TotalPrice,
                Status = item.Status,
                WarehouseId = item.WarehouseId,
                Product = new ProductDto
                {
                    ProductId = item.Product.ProductId,
                    SKU = item.Product.SKU,
                    Name = item.Product.Name
                },
                Variant = item.Variant != null ? new ProductVariantDto
                {
                    VariantId = item.Variant.VariantId,
                    VariantName = item.Variant.VariantName
                } : null,
                Warehouse = new WarehouseDto
                {
                    WarehouseId = item.Warehouse.WarehouseId,
                    WarehouseName = item.Warehouse.WarehouseName
                }
            };
        }

        public async Task<List<OrderItemDto>> GetOrderItemsByOrderAsync(int orderId)
        {
            return await _context.OrderItems
                .Where(i => i.OrderId == orderId)
                .Include(i => i.Product)
                .Include(i => i.Variant)
                .Select(i => new OrderItemDto
                {
                    OrderItemId = i.OrderItemId,
                    OrderId = i.OrderId,
                    ProductId = i.ProductId,
                    VariantId = i.VariantId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    Discount = i.Discount,
                    TotalPrice = i.TotalPrice,
                    Status = i.Status,
                    Product = new ProductDto
                    {
                        ProductId = i.Product.ProductId,
                        SKU = i.Product.SKU,
                        Name = i.Product.Name
                    },
                    Variant = i.Variant != null ? new ProductVariantDto
                    {
                        VariantId = i.Variant.VariantId,
                        VariantName = i.Variant.VariantName
                    } : null
                }).ToListAsync();
        }

        public async Task<OrderItemDto> UpdateOrderItemStatusAsync(int orderItemId, string status)
        {
            var item = await _context.OrderItems.FindAsync(orderItemId);
            if (item == null) return null;

            item.Status = status;
            await _context.SaveChangesAsync();

            return await GetOrderItemByIdAsync(orderItemId);
        }
    }

    public interface IOrderStatusRepository
    {
        Task<List<OrderStatusDto>> GetStatusHistoryByOrderAsync(int orderId);
        Task<OrderStatusDto> AddOrderStatusAsync(int orderId, string status, string notes = null);
    }

    public class OrderStatusRepository : IOrderStatusRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderStatusRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<OrderStatusDto>> GetStatusHistoryByOrderAsync(int orderId)
        {
            return await _context.OrderStatuses
                .Where(s => s.OrderId == orderId)
                .OrderByDescending(s => s.StatusDate)
                .Select(s => new OrderStatusDto
                {
                    StatusId = s.StatusId,
                    OrderId = s.OrderId,
                    Status = s.Status,
                    StatusDate = s.StatusDate,
                    Notes = s.Notes
                }).ToListAsync();
        }

        public async Task<OrderStatusDto> AddOrderStatusAsync(int orderId, string status, string notes = null)
        {
            var orderStatus = new OrderStatus
            {
                OrderId = orderId,
                Status = status,
                StatusDate = DateTime.UtcNow,
                Notes = notes
            };

            _context.OrderStatuses.Add(orderStatus);
            await _context.SaveChangesAsync();

            return new OrderStatusDto
            {
                StatusId = orderStatus.StatusId,
                OrderId = orderStatus.OrderId,
                Status = orderStatus.Status,
                StatusDate = orderStatus.StatusDate,
                Notes = orderStatus.Notes
            };
        }
    }

    public interface IOrderShipmentRepository
    {
        Task<OrderShipmentDto> GetShipmentByIdAsync(int shipmentId);
        Task<List<OrderShipmentDto>> GetShipmentsByOrderAsync(int orderId);
        Task<OrderShipmentDto> CreateShipmentAsync(OrderShipmentDto shipmentDto);
        Task<OrderShipmentDto> UpdateShipmentAsync(int shipmentId, OrderShipmentDto shipmentDto);
    }

    public class OrderShipmentRepository : IOrderShipmentRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderShipmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OrderShipmentDto> GetShipmentByIdAsync(int shipmentId)
        {
            var shipment = await _context.OrderShipments
                .Include(s => s.Order)
                .FirstOrDefaultAsync(s => s.ShipmentId == shipmentId);

            if (shipment == null) return null;

            return new OrderShipmentDto
            {
                ShipmentId = shipment.ShipmentId,
                OrderId = shipment.OrderId,
                ShipmentNumber = shipment.ShipmentNumber,
                ShipmentDate = shipment.ShipmentDate,
                Carrier = shipment.Carrier,
                TrackingNumber = shipment.TrackingNumber,
                Status = shipment.Status,
                ExpectedDeliveryDate = shipment.ExpectedDeliveryDate,
                ActualDeliveryDate = shipment.ActualDeliveryDate,
                DeliveredTo = shipment.DeliveredTo,
                SignatureRequired = shipment.SignatureRequired,
                ProofOfDeliveryUrl = shipment.ProofOfDeliveryUrl,
                Order = new OrderDto
                {
                    OrderId = shipment.Order.OrderId,
                    OrderNumber = shipment.Order.OrderNumber
                }
            };
        }

        public async Task<List<OrderShipmentDto>> GetShipmentsByOrderAsync(int orderId)
        {
            return await _context.OrderShipments
                .Where(s => s.OrderId == orderId)
                .Select(s => new OrderShipmentDto
                {
                    ShipmentId = s.ShipmentId,
                    OrderId = s.OrderId,
                    ShipmentNumber = s.ShipmentNumber,
                    ShipmentDate = s.ShipmentDate,
                    Carrier = s.Carrier,
                    TrackingNumber = s.TrackingNumber,
                    Status = s.Status,
                    ExpectedDeliveryDate = s.ExpectedDeliveryDate,
                    ActualDeliveryDate = s.ActualDeliveryDate
                }).ToListAsync();
        }

        public async Task<OrderShipmentDto> CreateShipmentAsync(OrderShipmentDto shipmentDto)
        {
            var shipment = new OrderShipment
            {
                OrderId = shipmentDto.OrderId,
                ShipmentNumber = GenerateShipmentNumber(),
                ShipmentDate = DateTime.UtcNow,
                Carrier = shipmentDto.Carrier,
                TrackingNumber = shipmentDto.TrackingNumber,
                Status = "Shipped",
                ExpectedDeliveryDate = shipmentDto.ExpectedDeliveryDate,
                SignatureRequired = shipmentDto.SignatureRequired
            };

            _context.OrderShipments.Add(shipment);
            await _context.SaveChangesAsync();

            // Update order status if first shipment
            var order = await _context.Orders.FindAsync(shipmentDto.OrderId);
            if (order != null && order.Status == "Pending")
            {
                order.Status = "Shipped";
                await _context.SaveChangesAsync();
            }

            return await GetShipmentByIdAsync(shipment.ShipmentId);
        }

        public async Task<OrderShipmentDto> UpdateShipmentAsync(int shipmentId, OrderShipmentDto shipmentDto)
        {
            var shipment = await _context.OrderShipments.FindAsync(shipmentId);
            if (shipment == null) return null;

            shipment.TrackingNumber = shipmentDto.TrackingNumber;
            shipment.Status = shipmentDto.Status;
            shipment.ActualDeliveryDate = shipmentDto.ActualDeliveryDate;
            shipment.DeliveredTo = shipmentDto.DeliveredTo;
            shipment.ProofOfDeliveryUrl = shipmentDto.ProofOfDeliveryUrl;

            // Update order status if all items delivered
            if (shipment.Status == "Delivered")
            {
                var order = await _context.Orders
                    .Include(o => o.Items)
                    .FirstOrDefaultAsync(o => o.OrderId == shipment.OrderId);

                if (order != null && order.Items.All(i => i.Status == "Delivered"))
                {
                    order.Status = "Delivered";
                }
            }

            await _context.SaveChangesAsync();
            return await GetShipmentByIdAsync(shipmentId);
        }

        private string GenerateShipmentNumber()
        {
            return "SH-" + DateTime.Now.ToString("yyyyMMdd") + "-" + new Random().Next(1000, 9999);
        }
    }

    public interface IPaymentRepository
    {
        Task<PaymentDto> GetPaymentByIdAsync(int paymentId);
        Task<List<PaymentDto>> GetPaymentsByOrderAsync(int orderId);
        Task<PaymentDto> CreatePaymentAsync(PaymentDto paymentDto);
    }

    public class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationDbContext _context;

        public PaymentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaymentDto> GetPaymentByIdAsync(int paymentId)
        {
            var payment = await _context.Payments
                .Include(p => p.Order)
                .FirstOrDefaultAsync(p => p.PaymentId == paymentId);

            if (payment == null) return null;

            return new PaymentDto
            {
                PaymentId = payment.PaymentId,
                OrderId = payment.OrderId,
                PaymentMethod = payment.PaymentMethod,
                Amount = payment.Amount,
                TransactionId = payment.TransactionId,
                Status = payment.Status,
                PaymentDate = payment.PaymentDate,
                AuthorizationCode = payment.AuthorizationCode,
                RefundAmount = payment.RefundAmount,
                RefundDate = payment.RefundDate,
                Notes = payment.Notes,
                Order = new OrderDto
                {
                    OrderId = payment.Order.OrderId,
                    OrderNumber = payment.Order.OrderNumber
                }
            };
        }

        public async Task<List<PaymentDto>> GetPaymentsByOrderAsync(int orderId)
        {
            return await _context.Payments
                .Where(p => p.OrderId == orderId)
                .Select(p => new PaymentDto
                {
                    PaymentId = p.PaymentId,
                    OrderId = p.OrderId,
                    PaymentMethod = p.PaymentMethod,
                    Amount = p.Amount,
                    TransactionId = p.TransactionId,
                    Status = p.Status,
                    PaymentDate = p.PaymentDate
                }).ToListAsync();
        }

        public async Task<PaymentDto> CreatePaymentAsync(PaymentDto paymentDto)
        {
            var payment = new Payment
            {
                OrderId = paymentDto.OrderId,
                PaymentMethod = paymentDto.PaymentMethod,
                Amount = paymentDto.Amount,
                TransactionId = paymentDto.TransactionId,
                Status = paymentDto.Status,
                PaymentDate = DateTime.UtcNow,
                AuthorizationCode = paymentDto.AuthorizationCode,
                Notes = paymentDto.Notes
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            // Update order payment status
            var order = await _context.Orders.FindAsync(paymentDto.OrderId);
            if (order != null)
            {
                var totalPaid = await _context.Payments
                    .Where(p => p.OrderId == order.OrderId && p.Status == "Completed")
                    .SumAsync(p => p.Amount);

                if (totalPaid >= order.TotalAmount)
                {
                    order.PaymentStatus = "Paid";
                }
                else if (totalPaid > 0)
                {
                    order.PaymentStatus = "Partial";
                }

                await _context.SaveChangesAsync();
            }

            return await GetPaymentByIdAsync(payment.PaymentId);
        }
    }

    public interface IReturnRepository
    {
        Task<ReturnDto> GetReturnByIdAsync(int returnId);
        Task<List<ReturnDto>> GetReturnsByOrderAsync(int orderId);
        Task<ReturnDto> CreateReturnAsync(ReturnDto returnDto);
        Task<ReturnDto> UpdateReturnAsync(int returnId, ReturnDto returnDto);
    }

    public class ReturnRepository : IReturnRepository
    {
        private readonly ApplicationDbContext _context;

        public ReturnRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ReturnDto> GetReturnByIdAsync(int returnId)
        {
            var returnOrder = await _context.Returns
                .Include(r => r.Order)
                .Include(r => r.ApprovedByUser)
                .Include(r => r.Items)
                    .ThenInclude(i => i.OrderItem)
                        .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(r => r.ReturnId == returnId);

            if (returnOrder == null) return null;

            return new ReturnDto
            {
                ReturnId = returnOrder.ReturnId,
                OrderId = returnOrder.OrderId,
                ReturnNumber = returnOrder.ReturnNumber,
                RequestDate = returnOrder.RequestDate,
                Status = returnOrder.Status,
                ReturnReason = returnOrder.ReturnReason,
                ReturnType = returnOrder.ReturnType,
                ApprovedByUserId = returnOrder.ApprovedByUserId,
                ApprovalDate = returnOrder.ApprovalDate,
                ReceivedDate = returnOrder.ReceivedDate,
                RefundAmount = returnOrder.RefundAmount,
                RefundDate = returnOrder.RefundDate,
                Notes = returnOrder.Notes,
                Order = new OrderDto
                {
                    OrderId = returnOrder.Order.OrderId,
                    OrderNumber = returnOrder.Order.OrderNumber
                },
                ApprovedByUser = returnOrder.ApprovedByUser != null ? new UserDto
                {
                    UserId = returnOrder.ApprovedByUser.UserId,
                    Username = returnOrder.ApprovedByUser.Username
                } : null,
                Items = returnOrder.Items?.Select(i => new ReturnItemDto
                {
                    ReturnItemId = i.ReturnItemId,
                    ReturnId = i.ReturnId,
                    OrderItemId = i.OrderItemId,
                    Quantity = i.Quantity,
                    ReturnReason = i.ReturnReason,
                    Condition = i.Condition,
                    Status = i.Status,
                    OrderItem = new OrderItemDto
                    {
                        OrderItemId = i.OrderItem.OrderItemId,
                        ProductId = i.OrderItem.ProductId,
                        Product = new ProductDto
                        {
                            ProductId = i.OrderItem.Product.ProductId,
                            Name = i.OrderItem.Product.Name
                        }
                    }
                }).ToList()
            };
        }

        public async Task<List<ReturnDto>> GetReturnsByOrderAsync(int orderId)
        {
            return await _context.Returns
                .Where(r => r.OrderId == orderId)
                .Select(r => new ReturnDto
                {
                    ReturnId = r.ReturnId,
                    OrderId = r.OrderId,
                    ReturnNumber = r.ReturnNumber,
                    RequestDate = r.RequestDate,
                    Status = r.Status,
                    ReturnReason = r.ReturnReason,
                    RefundAmount = r.RefundAmount
                }).ToListAsync();
        }

        public async Task<ReturnDto> CreateReturnAsync(ReturnDto returnDto)
        {
            var returnOrder = new Return
            {
                OrderId = returnDto.OrderId,
                ReturnNumber = GenerateReturnNumber(),
                RequestDate = DateTime.UtcNow,
                Status = "Requested",
                ReturnReason = returnDto.ReturnReason,
                ReturnType = returnDto.ReturnType,
                Notes = returnDto.Notes
            };

            _context.Returns.Add(returnOrder);
            await _context.SaveChangesAsync();

            return await GetReturnByIdAsync(returnOrder.ReturnId);
        }

        public async Task<ReturnDto> UpdateReturnAsync(int returnId, ReturnDto returnDto)
        {
            var returnOrder = await _context.Returns.FindAsync(returnId);
            if (returnOrder == null) return null;

            returnOrder.Status = returnDto.Status;
            returnOrder.ApprovedByUserId = returnDto.ApprovedByUserId;
            returnOrder.ApprovalDate = returnDto.ApprovalDate;
            returnOrder.ReceivedDate = returnDto.ReceivedDate;
            returnOrder.RefundAmount = (decimal)returnDto.RefundAmount;
            returnOrder.RefundDate = returnDto.RefundDate;
            returnOrder.Notes = returnDto.Notes;

            await _context.SaveChangesAsync();
            return await GetReturnByIdAsync(returnId);
        }

        private string GenerateReturnNumber()
        {
            return "RET-" + DateTime.Now.ToString("yyyyMMdd") + "-" + new Random().Next(1000, 9999);
        }
    }

    public interface IDeliveryRepository
    {
        Task<DeliveryDto> GetDeliveryByIdAsync(int deliveryId);
        Task<List<DeliveryDto>> GetDeliveriesByShipmentAsync(int shipmentId);
        Task<DeliveryDto> CreateDeliveryAsync(DeliveryDto deliveryDto);
        Task<DeliveryDto> UpdateDeliveryAsync(int deliveryId, DeliveryDto deliveryDto);
    }

    public class DeliveryRepository : IDeliveryRepository
    {
        private readonly ApplicationDbContext _context;

        public DeliveryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DeliveryDto> GetDeliveryByIdAsync(int deliveryId)
        {
            var delivery = await _context.Deliveries
                .Include(d => d.Shipment)
                .Include(d => d.DeliveryPerson)
                .FirstOrDefaultAsync(d => d.DeliveryId == deliveryId);

            if (delivery == null) return null;

            return new DeliveryDto
            {
                DeliveryId = delivery.DeliveryId,
                ShipmentId = delivery.ShipmentId,
                DeliveryPersonId = delivery.DeliveryPersonId,
                ScheduledDate = delivery.ScheduledDate,
                EstimatedTimeWindow = delivery.EstimatedTimeWindow,
                Status = delivery.Status,
                ActualDeliveryTime = delivery.ActualDeliveryTime,
                GeoLocation = delivery.GeoLocation,
                Notes = delivery.Notes,
                Shipment = new OrderShipmentDto
                {
                    ShipmentId = delivery.Shipment.ShipmentId,
                    ShipmentNumber = delivery.Shipment.ShipmentNumber
                },
                DeliveryPerson = new UserDto
                {
                    UserId = delivery.DeliveryPerson.UserId,
                    Username = delivery.DeliveryPerson.Username
                }
            };
        }

        public async Task<List<DeliveryDto>> GetDeliveriesByShipmentAsync(int shipmentId)
        {
            return await _context.Deliveries
                .Where(d => d.ShipmentId == shipmentId)
                .Select(d => new DeliveryDto
                {
                    DeliveryId = d.DeliveryId,
                    ShipmentId = d.ShipmentId,
                    DeliveryPersonId = d.DeliveryPersonId,
                    ScheduledDate = d.ScheduledDate,
                    EstimatedTimeWindow = d.EstimatedTimeWindow,
                    Status = d.Status,
                    ActualDeliveryTime = d.ActualDeliveryTime
                }).ToListAsync();
        }

        public async Task<DeliveryDto> CreateDeliveryAsync(DeliveryDto deliveryDto)
        {
            var delivery = new Delivery
            {
                ShipmentId = deliveryDto.ShipmentId,
                DeliveryPersonId = deliveryDto.DeliveryPersonId,
                ScheduledDate = deliveryDto.ScheduledDate,
                EstimatedTimeWindow = deliveryDto.EstimatedTimeWindow,
                Status = "Scheduled"
            };

            _context.Deliveries.Add(delivery);
            await _context.SaveChangesAsync();

            return await GetDeliveryByIdAsync(delivery.DeliveryId);
        }

        public async Task<DeliveryDto> UpdateDeliveryAsync(int deliveryId, DeliveryDto deliveryDto)
        {
            var delivery = await _context.Deliveries.FindAsync(deliveryId);
            if (delivery == null) return null;

            delivery.Status = deliveryDto.Status;
            delivery.ActualDeliveryTime = deliveryDto.ActualDeliveryTime;
            delivery.GeoLocation = deliveryDto.GeoLocation;
            delivery.Notes = deliveryDto.Notes;

            await _context.SaveChangesAsync();
            return await GetDeliveryByIdAsync(deliveryId);
        }
    }

    #endregion
}
