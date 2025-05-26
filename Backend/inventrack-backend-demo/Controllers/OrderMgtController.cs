using inventrack_backend_demo.DTOs;
using inventrack_backend_demo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace inventrack_backend_demo.Controllers
{
    #region Order Management Controllers

    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult<OrderDto>> GetOrderById(int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<List<OrderDto>>> GetOrdersByCustomer(int customerId)
        {
            var orders = await _orderRepository.GetOrdersByCustomerAsync(customerId);
            return Ok(orders);
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<List<OrderDto>>> GetOrdersByStatus(string status)
        {
            var orders = await _orderRepository.GetOrdersByStatusAsync(status);
            return Ok(orders);
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> CreateOrder(OrderCreateDto orderCreateDto)
        {
            var order = await _orderRepository.CreateOrderAsync(orderCreateDto);
            return CreatedAtAction(nameof(GetOrderById), new { orderId = order.OrderId }, order);
        }

        [HttpPut("{orderId}")]
        public async Task<ActionResult<OrderDto>> UpdateOrder(int orderId, OrderDto orderDto)
        {
            var order = await _orderRepository.UpdateOrderAsync(orderId, orderDto);
            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpPost("{orderId}/cancel")]
        public async Task<ActionResult> CancelOrder(int orderId)
        {
            var result = await _orderRepository.CancelOrderAsync(orderId);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemRepository _orderItemRepository;

        public OrderItemController(IOrderItemRepository orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }

        [HttpGet("{orderItemId}")]
        public async Task<ActionResult<OrderItemDto>> GetOrderItemById(int orderItemId)
        {
            var item = await _orderItemRepository.GetOrderItemByIdAsync(orderItemId);
            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpGet("order/{orderId}")]
        public async Task<ActionResult<List<OrderItemDto>>> GetOrderItemsByOrder(int orderId)
        {
            var items = await _orderItemRepository.GetOrderItemsByOrderAsync(orderId);
            return Ok(items);
        }

        [HttpPut("{orderItemId}/status")]
        public async Task<ActionResult<OrderItemDto>> UpdateOrderItemStatus(int orderItemId, [FromBody] string status)
        {
            var item = await _orderItemRepository.UpdateOrderItemStatusAsync(orderItemId, status);
            if (item == null)
                return NotFound();

            return Ok(item);
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class OrderStatusController : ControllerBase
    {
        private readonly IOrderStatusRepository _orderStatusRepository;

        public OrderStatusController(IOrderStatusRepository orderStatusRepository)
        {
            _orderStatusRepository = orderStatusRepository;
        }

        [HttpGet("order/{orderId}")]
        public async Task<ActionResult<List<OrderStatusDto>>> GetStatusHistoryByOrder(int orderId)
        {
            var statuses = await _orderStatusRepository.GetStatusHistoryByOrderAsync(orderId);
            return Ok(statuses);
        }

        [HttpPost]
        public async Task<ActionResult<OrderStatusDto>> AddOrderStatus(OrderStatusCreateDto statusCreateDto)
        {
            var status = await _orderStatusRepository.AddOrderStatusAsync(
                statusCreateDto.OrderId,
                statusCreateDto.Status,
                statusCreateDto.Notes);
            return Ok(status);
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class OrderShipmentController : ControllerBase
    {
        private readonly IOrderShipmentRepository _orderShipmentRepository;

        public OrderShipmentController(IOrderShipmentRepository orderShipmentRepository)
        {
            _orderShipmentRepository = orderShipmentRepository;
        }

        [HttpGet("{shipmentId}")]
        public async Task<ActionResult<OrderShipmentDto>> GetShipmentById(int shipmentId)
        {
            var shipment = await _orderShipmentRepository.GetShipmentByIdAsync(shipmentId);
            if (shipment == null)
                return NotFound();

            return Ok(shipment);
        }

        [HttpGet("order/{orderId}")]
        public async Task<ActionResult<List<OrderShipmentDto>>> GetShipmentsByOrder(int orderId)
        {
            var shipments = await _orderShipmentRepository.GetShipmentsByOrderAsync(orderId);
            return Ok(shipments);
        }

        [HttpPost]
        public async Task<ActionResult<OrderShipmentDto>> CreateShipment(OrderShipmentDto shipmentDto)
        {
            var shipment = await _orderShipmentRepository.CreateShipmentAsync(shipmentDto);
            return CreatedAtAction(nameof(GetShipmentById), new { shipmentId = shipment.ShipmentId }, shipment);
        }

        [HttpPut("{shipmentId}")]
        public async Task<ActionResult<OrderShipmentDto>> UpdateShipment(int shipmentId, OrderShipmentDto shipmentDto)
        {
            var shipment = await _orderShipmentRepository.UpdateShipmentAsync(shipmentId, shipmentDto);
            if (shipment == null)
                return NotFound();

            return Ok(shipment);
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentController(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        [HttpGet("{paymentId}")]
        public async Task<ActionResult<PaymentDto>> GetPaymentById(int paymentId)
        {
            var payment = await _paymentRepository.GetPaymentByIdAsync(paymentId);
            if (payment == null)
                return NotFound();

            return Ok(payment);
        }

        [HttpGet("order/{orderId}")]
        public async Task<ActionResult<List<PaymentDto>>> GetPaymentsByOrder(int orderId)
        {
            var payments = await _paymentRepository.GetPaymentsByOrderAsync(orderId);
            return Ok(payments);
        }

        [HttpPost]
        public async Task<ActionResult<PaymentDto>> CreatePayment(PaymentDto paymentDto)
        {
            var payment = await _paymentRepository.CreatePaymentAsync(paymentDto);
            return CreatedAtAction(nameof(GetPaymentById), new { paymentId = payment.PaymentId }, payment);
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class ReturnController : ControllerBase
    {
        private readonly IReturnRepository _returnRepository;

        public ReturnController(IReturnRepository returnRepository)
        {
            _returnRepository = returnRepository;
        }

        [HttpGet("{returnId}")]
        public async Task<ActionResult<ReturnDto>> GetReturnById(int returnId)
        {
            var returnOrder = await _returnRepository.GetReturnByIdAsync(returnId);
            if (returnOrder == null)
                return NotFound();

            return Ok(returnOrder);
        }

        [HttpGet("order/{orderId}")]
        public async Task<ActionResult<List<ReturnDto>>> GetReturnsByOrder(int orderId)
        {
            var returns = await _returnRepository.GetReturnsByOrderAsync(orderId);
            return Ok(returns);
        }

        [HttpPost]
        public async Task<ActionResult<ReturnDto>> CreateReturn(ReturnDto returnDto)
        {
            var returnOrder = await _returnRepository.CreateReturnAsync(returnDto);
            return CreatedAtAction(nameof(GetReturnById), new { returnId = returnOrder.ReturnId }, returnOrder);
        }

        [HttpPut("{returnId}")]
        public async Task<ActionResult<ReturnDto>> UpdateReturn(int returnId, ReturnDto returnDto)
        {
            var returnOrder = await _returnRepository.UpdateReturnAsync(returnId, returnDto);
            if (returnOrder == null)
                return NotFound();

            return Ok(returnOrder);
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class DeliveryController : ControllerBase
    {
        private readonly IDeliveryRepository _deliveryRepository;

        public DeliveryController(IDeliveryRepository deliveryRepository)
        {
            _deliveryRepository = deliveryRepository;
        }

        [HttpGet("{deliveryId}")]
        public async Task<ActionResult<DeliveryDto>> GetDeliveryById(int deliveryId)
        {
            var delivery = await _deliveryRepository.GetDeliveryByIdAsync(deliveryId);
            if (delivery == null)
                return NotFound();

            return Ok(delivery);
        }

        [HttpGet("shipment/{shipmentId}")]
        public async Task<ActionResult<List<DeliveryDto>>> GetDeliveriesByShipment(int shipmentId)
        {
            var deliveries = await _deliveryRepository.GetDeliveriesByShipmentAsync(shipmentId);
            return Ok(deliveries);
        }

        [HttpPost]
        public async Task<ActionResult<DeliveryDto>> CreateDelivery(DeliveryDto deliveryDto)
        {
            var delivery = await _deliveryRepository.CreateDeliveryAsync(deliveryDto);
            return CreatedAtAction(nameof(GetDeliveryById), new { deliveryId = delivery.DeliveryId }, delivery);
        }

        [HttpPut("{deliveryId}")]
        public async Task<ActionResult<DeliveryDto>> UpdateDelivery(int deliveryId, DeliveryDto deliveryDto)
        {
            var delivery = await _deliveryRepository.UpdateDeliveryAsync(deliveryId, deliveryDto);
            if (delivery == null)
                return NotFound();

            return Ok(delivery);
        }
    }

    // Helper class for OrderStatusController
    public class OrderStatusCreateDto
    {
        public int OrderId { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
    }

    #endregion
}
