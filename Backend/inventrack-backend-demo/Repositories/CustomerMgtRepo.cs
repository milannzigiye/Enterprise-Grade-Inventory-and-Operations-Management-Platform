using inventrack_backend_demo.DTOs;
using inventrack_backend_demo.Model;
using inventrack_backend_demo.Data;
using Microsoft.EntityFrameworkCore;

namespace inventrack_backend_demo.Repositories
{
    #region Customer Management Repositories

    public interface ICustomerRepository
    {
        Task<CustomerDto> GetCustomerByIdAsync(int customerId);
        Task<List<CustomerDto>> GetAllCustomersAsync();
        Task<CustomerDto> CreateCustomerAsync(CustomerCreateDto customerCreateDto);
        Task<CustomerDto> UpdateCustomerAsync(int customerId, CustomerDto customerDto);
        Task<bool> DeleteCustomerAsync(int customerId);
    }

    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CustomerDto> GetCustomerByIdAsync(int customerId)
        {
            var customer = await _context.Customers
                .Include(c => c.User)
                .Include(c => c.Membership)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);

            if (customer == null) return null;

            return new CustomerDto
            {
                CustomerId = customer.CustomerId,
                UserId = customer.UserId,
                CustomerType = customer.CustomerType,
                CompanyName = customer.CompanyName,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                Phone = customer.Phone,
                Address = customer.Address,
                City = customer.City,
                State = customer.State,
                ZipCode = customer.ZipCode,
                Country = customer.Country,
                TaxExempt = customer.TaxExempt,
                TaxIdentificationNumber = customer.TaxIdentificationNumber,
                JoinDate = customer.JoinDate,
                LastPurchaseDate = customer.LastPurchaseDate,
                Status = customer.Status,
                Membership = customer.Membership != null ? new CustomerMembershipDto
                {
                    MembershipId = customer.Membership.MembershipId,
                    CustomerId = customer.Membership.CustomerId,
                    MembershipLevel = customer.Membership.MembershipLevel,
                    StartDate = customer.Membership.StartDate,
                    EndDate = customer.Membership.EndDate,
                    PointsBalance = customer.Membership.PointsBalance,
                    LifetimePoints = customer.Membership.LifetimePoints,
                    IsActive = customer.Membership.IsActive
                } : null,
                User = new UserDto
                {
                    UserId = customer.User.UserId,
                    Username = customer.User.Username,
                    Email = customer.User.Email
                }
            };
        }

        public async Task<List<CustomerDto>> GetAllCustomersAsync()
        {
            return await _context.Customers
                .Include(c => c.User)
                .Select(c => new CustomerDto
                {
                    CustomerId = c.CustomerId,
                    UserId = c.UserId,
                    CustomerType = c.CustomerType,
                    CompanyName = c.CompanyName,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    Phone = c.Phone,
                    JoinDate = c.JoinDate,
                    Status = c.Status,
                    User = new UserDto
                    {
                        UserId = c.User.UserId,
                        Username = c.User.Username,
                        Email = c.User.Email
                    }
                }).ToListAsync();
        }

        public async Task<CustomerDto> CreateCustomerAsync(CustomerCreateDto customerCreateDto)
        {
            var customer = new Customer
            {
                UserId = customerCreateDto.UserId,
                CustomerType = customerCreateDto.CustomerType,
                CompanyName = customerCreateDto.CompanyName,
                FirstName = customerCreateDto.FirstName,
                LastName = customerCreateDto.LastName,
                Email = customerCreateDto.Email,
                Phone = customerCreateDto.Phone,
                Address = customerCreateDto.Address,
                City = customerCreateDto.City,
                State = customerCreateDto.State,
                ZipCode = customerCreateDto.ZipCode,
                Country = customerCreateDto.Country,
                TaxExempt = customerCreateDto.TaxExempt,
                TaxIdentificationNumber = customerCreateDto.TaxIdentificationNumber,
                JoinDate = DateTime.UtcNow,
                Status = "Active"
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return await GetCustomerByIdAsync(customer.CustomerId);
        }

        public async Task<CustomerDto> UpdateCustomerAsync(int customerId, CustomerDto customerDto)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null) return null;

            customer.CustomerType = customerDto.CustomerType;
            customer.CompanyName = customerDto.CompanyName;
            customer.FirstName = customerDto.FirstName;
            customer.LastName = customerDto.LastName;
            customer.Email = customerDto.Email;
            customer.Phone = customerDto.Phone;
            customer.Address = customerDto.Address;
            customer.City = customerDto.City;
            customer.State = customerDto.State;
            customer.ZipCode = customerDto.ZipCode;
            customer.Country = customerDto.Country;
            customer.TaxExempt = customerDto.TaxExempt;
            customer.TaxIdentificationNumber = customerDto.TaxIdentificationNumber;
            customer.Status = customerDto.Status;

            await _context.SaveChangesAsync();
            return await GetCustomerByIdAsync(customerId);
        }

        public async Task<bool> DeleteCustomerAsync(int customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null) return false;

            customer.Status = "Inactive";
            await _context.SaveChangesAsync();
            return true;
        }
    }

    public interface ICustomerMembershipRepository
    {
        Task<CustomerMembershipDto> GetMembershipByCustomerAsync(int customerId);
        Task<CustomerMembershipDto> CreateOrUpdateMembershipAsync(CustomerMembershipDto membershipDto);
    }

    public class CustomerMembershipRepository : ICustomerMembershipRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerMembershipRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CustomerMembershipDto> GetMembershipByCustomerAsync(int customerId)
        {
            var membership = await _context.CustomerMemberships
                .FirstOrDefaultAsync(m => m.CustomerId == customerId);

            if (membership == null) return null;

            return new CustomerMembershipDto
            {
                MembershipId = membership.MembershipId,
                CustomerId = membership.CustomerId,
                MembershipLevel = membership.MembershipLevel,
                StartDate = membership.StartDate,
                EndDate = membership.EndDate,
                PointsBalance = membership.PointsBalance,
                LifetimePoints = membership.LifetimePoints,
                IsActive = membership.IsActive
            };
        }

        public async Task<CustomerMembershipDto> CreateOrUpdateMembershipAsync(CustomerMembershipDto membershipDto)
        {
            var membership = await _context.CustomerMemberships
                .FirstOrDefaultAsync(m => m.CustomerId == membershipDto.CustomerId);

            if (membership == null)
            {
                membership = new CustomerMembership
                {
                    CustomerId = membershipDto.CustomerId,
                    MembershipLevel = membershipDto.MembershipLevel,
                    StartDate = DateTime.UtcNow,
                    PointsBalance = membershipDto.PointsBalance,
                    LifetimePoints = membershipDto.LifetimePoints,
                    IsActive = true
                };
                _context.CustomerMemberships.Add(membership);
            }
            else
            {
                membership.MembershipLevel = membershipDto.MembershipLevel;
                membership.PointsBalance = membershipDto.PointsBalance;
                membership.LifetimePoints = membershipDto.LifetimePoints;
                membership.IsActive = membershipDto.IsActive;
            }

            await _context.SaveChangesAsync();
            return await GetMembershipByCustomerAsync(membershipDto.CustomerId);
        }
    }

    public interface IWishlistRepository
    {
        Task<WishlistDto> GetWishlistByIdAsync(int wishlistId);
        Task<List<WishlistDto>> GetWishlistsByCustomerAsync(int customerId);
        Task<WishlistDto> CreateWishlistAsync(WishlistCreateDto wishlistCreateDto);
        Task<bool> DeleteWishlistAsync(int wishlistId);
    }

    public class WishlistRepository : IWishlistRepository
    {
        private readonly ApplicationDbContext _context;

        public WishlistRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<WishlistDto> GetWishlistByIdAsync(int wishlistId)
        {
            var wishlist = await _context.Wishlists
                .Include(w => w.Items)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(w => w.WishlistId == wishlistId);

            if (wishlist == null) return null;

            return new WishlistDto
            {
                WishlistId = wishlist.WishlistId,
                CustomerId = wishlist.CustomerId,
                Name = wishlist.Name,
                IsPublic = wishlist.IsPublic,
                CreatedDate = wishlist.CreatedDate,
                Items = wishlist.Items?.Select(i => new WishlistItemDto
                {
                    WishlistItemId = i.WishlistItemId,
                    WishlistId = i.WishlistId,
                    ProductId = i.ProductId,
                    DateAdded = i.DateAdded,
                    Notes = i.Notes,
                    Product = new ProductDto
                    {
                        ProductId = i.Product.ProductId,
                        SKU = i.Product.SKU,
                        Name = i.Product.Name,
                        ImageUrl = i.Product.ImageUrl,
                        ListPrice = i.Product.ListPrice
                    }
                }).ToList()
            };
        }

        public async Task<List<WishlistDto>> GetWishlistsByCustomerAsync(int customerId)
        {
            return await _context.Wishlists
                .Where(w => w.CustomerId == customerId)
                .Select(w => new WishlistDto
                {
                    WishlistId = w.WishlistId,
                    CustomerId = w.CustomerId,
                    Name = w.Name,
                    IsPublic = w.IsPublic,
                    CreatedDate = w.CreatedDate
                }).ToListAsync();
        }

        public async Task<WishlistDto> CreateWishlistAsync(WishlistCreateDto wishlistCreateDto)
        {
            var wishlist = new Wishlist
            {
                CustomerId = wishlistCreateDto.CustomerId,
                Name = wishlistCreateDto.Name,
                IsPublic = wishlistCreateDto.IsPublic,
                CreatedDate = DateTime.UtcNow
            };

            _context.Wishlists.Add(wishlist);
            await _context.SaveChangesAsync();

            return await GetWishlistByIdAsync(wishlist.WishlistId);
        }

        public async Task<bool> DeleteWishlistAsync(int wishlistId)
        {
            var wishlist = await _context.Wishlists.FindAsync(wishlistId);
            if (wishlist == null) return false;

            _context.Wishlists.Remove(wishlist);
            await _context.SaveChangesAsync();
            return true;
        }
    }

    public interface IWishlistItemRepository
    {
        Task<WishlistItemDto> GetWishlistItemByIdAsync(int wishlistItemId);
        Task<WishlistItemDto> AddItemToWishlistAsync(WishlistItemCreateDto wishlistItemCreateDto);
        Task<bool> RemoveItemFromWishlistAsync(int wishlistItemId);
    }

    public class WishlistItemRepository : IWishlistItemRepository
    {
        private readonly ApplicationDbContext _context;

        public WishlistItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<WishlistItemDto> GetWishlistItemByIdAsync(int wishlistItemId)
        {
            var item = await _context.WishlistItems
                .Include(i => i.Product)
                .FirstOrDefaultAsync(i => i.WishlistItemId == wishlistItemId);

            if (item == null) return null;

            return new WishlistItemDto
            {
                WishlistItemId = item.WishlistItemId,
                WishlistId = item.WishlistId,
                ProductId = item.ProductId,
                DateAdded = item.DateAdded,
                Notes = item.Notes,
                Product = new ProductDto
                {
                    ProductId = item.Product.ProductId,
                    SKU = item.Product.SKU,
                    Name = item.Product.Name
                }
            };
        }

        public async Task<WishlistItemDto> AddItemToWishlistAsync(WishlistItemCreateDto wishlistItemCreateDto)
        {
            var item = new WishlistItem
            {
                WishlistId = wishlistItemCreateDto.WishlistId,
                ProductId = wishlistItemCreateDto.ProductId,
                DateAdded = DateTime.UtcNow,
                Notes = wishlistItemCreateDto.Notes
            };

            _context.WishlistItems.Add(item);
            await _context.SaveChangesAsync();

            return await GetWishlistItemByIdAsync(item.WishlistItemId);
        }

        public async Task<bool> RemoveItemFromWishlistAsync(int wishlistItemId)
        {
            var item = await _context.WishlistItems.FindAsync(wishlistItemId);
            if (item == null) return false;

            _context.WishlistItems.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }
    }

    public interface ICustomerFeedbackRepository
    {
        Task<CustomerFeedbackDto> GetFeedbackByIdAsync(int feedbackId);
        Task<List<CustomerFeedbackDto>> GetFeedbackByCustomerAsync(int customerId);
        Task<List<CustomerFeedbackDto>> GetFeedbackByProductAsync(int productId);
        Task<CustomerFeedbackDto> CreateFeedbackAsync(CustomerFeedbackDto feedbackDto);
        Task<CustomerFeedbackDto> UpdateFeedbackResponseAsync(int feedbackId, string response);
    }

    public class CustomerFeedbackRepository : ICustomerFeedbackRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerFeedbackRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CustomerFeedbackDto> GetFeedbackByIdAsync(int feedbackId)
        {
            var feedback = await _context.CustomerFeedbacks
                .Include(f => f.Customer)
                .Include(f => f.Order)
                .Include(f => f.Product)
                .FirstOrDefaultAsync(f => f.FeedbackId == feedbackId);

            if (feedback == null) return null;

            return new CustomerFeedbackDto
            {
                FeedbackId = feedback.FeedbackId,
                CustomerId = feedback.CustomerId,
                OrderId = feedback.OrderId,
                ProductId = feedback.ProductId,
                FeedbackType = feedback.FeedbackType,
                Rating = feedback.Rating,
                Comments = feedback.Comments,
                SubmissionDate = feedback.SubmissionDate,
                Status = feedback.Status,
                Response = feedback.Response,
                ResponseDate = feedback.ResponseDate,
                Customer = new CustomerDto
                {
                    CustomerId = feedback.Customer.CustomerId,
                    FirstName = feedback.Customer.FirstName,
                    LastName = feedback.Customer.LastName
                },
                Product = feedback.Product != null ? new ProductDto
                {
                    ProductId = feedback.Product.ProductId,
                    Name = feedback.Product.Name
                } : null
            };
        }

        public async Task<List<CustomerFeedbackDto>> GetFeedbackByCustomerAsync(int customerId)
        {
            return await _context.CustomerFeedbacks
                .Where(f => f.CustomerId == customerId)
                .Include(f => f.Product)
                .Select(f => new CustomerFeedbackDto
                {
                    FeedbackId = f.FeedbackId,
                    CustomerId = f.CustomerId,
                    OrderId = f.OrderId,
                    ProductId = f.ProductId,
                    FeedbackType = f.FeedbackType,
                    Rating = f.Rating,
                    Comments = f.Comments,
                    SubmissionDate = f.SubmissionDate,
                    Status = f.Status,
                    Product = f.Product != null ? new ProductDto
                    {
                        ProductId = f.Product.ProductId,
                        Name = f.Product.Name
                    } : null
                }).ToListAsync();
        }

        public async Task<List<CustomerFeedbackDto>> GetFeedbackByProductAsync(int productId)
        {
            return await _context.CustomerFeedbacks
                .Where(f => f.ProductId == productId)
                .Include(f => f.Customer)
                .Select(f => new CustomerFeedbackDto
                {
                    FeedbackId = f.FeedbackId,
                    CustomerId = f.CustomerId,
                    OrderId = f.OrderId,
                    ProductId = f.ProductId,
                    FeedbackType = f.FeedbackType,
                    Rating = f.Rating,
                    Comments = f.Comments,
                    SubmissionDate = f.SubmissionDate,
                    Status = f.Status,
                    Customer = new CustomerDto
                    {
                        CustomerId = f.Customer.CustomerId,
                        FirstName = f.Customer.FirstName,
                        LastName = f.Customer.LastName
                    }
                }).ToListAsync();
        }

        public async Task<CustomerFeedbackDto> CreateFeedbackAsync(CustomerFeedbackDto feedbackDto)
        {
            var feedback = new CustomerFeedback
            {
                CustomerId = feedbackDto.CustomerId,
                OrderId = feedbackDto.OrderId,
                ProductId = feedbackDto.ProductId,
                FeedbackType = feedbackDto.FeedbackType,
                Rating = (int)feedbackDto.Rating,
                Comments = feedbackDto.Comments,
                SubmissionDate = DateTime.UtcNow,
                Status = "Pending"
            };

            _context.CustomerFeedbacks.Add(feedback);
            await _context.SaveChangesAsync();

            return await GetFeedbackByIdAsync(feedback.FeedbackId);
        }

        public async Task<CustomerFeedbackDto> UpdateFeedbackResponseAsync(int feedbackId, string response)
        {
            var feedback = await _context.CustomerFeedbacks.FindAsync(feedbackId);
            if (feedback == null) return null;

            feedback.Response = response;
            feedback.ResponseDate = DateTime.UtcNow;
            feedback.Status = "Responded";

            await _context.SaveChangesAsync();
            return await GetFeedbackByIdAsync(feedbackId);
        }
    }

    #endregion
}
