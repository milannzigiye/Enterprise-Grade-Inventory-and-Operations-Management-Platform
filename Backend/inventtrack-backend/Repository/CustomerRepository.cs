using inventtrack_backend.Database;
using inventtrack_backend.DTOs;
using inventtrack_backend.Model;

namespace inventtrack_backend.Repository
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<CustomerListItemDto>> GetAllCustomersAsync();
        Task<CustomerDto> GetCustomerByIdAsync(int id);
        Task<Customer> CreateCustomerAsync(CustomerCreateDto customerCreateDto);
        Task<Customer> UpdateCustomerAsync(int id, CustomerUpdateDto customerUpdateDto);
        Task<bool> DeleteCustomerAsync(int id);
        Task<CustomerMembership> CreateCustomerMembershipAsync(CustomerMembershipCreateDto membershipCreateDto);
        Task<CustomerMembership> UpdateCustomerMembershipAsync(int customerId, CustomerMembershipUpdateDto membershipUpdateDto);
        Task<Wishlist> CreateWishlistAsync(WishlistCreateDto wishlistCreateDto);
        Task<Wishlist> AddWishlistItemAsync(WishlistItemCreateDto wishlistItemCreateDto);
        Task<bool> RemoveWishlistItemAsync(int wishlistItemId);
        Task<CustomerFeedback> CreateCustomerFeedbackAsync(CustomerFeedbackCreateDto feedbackCreateDto);
        Task<CustomerFeedback> UpdateCustomerFeedbackAsync(int feedbackId, CustomerFeedbackUpdateDto feedbackUpdateDto);
    }

    public class CustomerRepository : ICustomerRepository
    {
        private readonly DbOmniflow _context;

        public CustomerRepository(DbOmniflow context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CustomerListItemDto>> GetAllCustomersAsync()
        {
            return await _context.Customers
                .Include(c => c.Membership)
                .Select(c => new CustomerListItemDto
                {
                    CustomerId = c.CustomerId,
                    CustomerType = c.CustomerType,
                    CompanyName = c.CompanyName,
                    FullName = $"{c.FirstName} {c.LastName}",
                    Email = c.Email,
                    Phone = c.Phone,
                    Status = c.Status,
                    JoinDate = c.JoinDate,
                    LastPurchaseDate = c.LastPurchaseDate,
                    TotalSpent = c.Orders.Sum(o => o.TotalAmount),
                    MembershipLevel = c.Membership != null ? c.Membership.MembershipLevel : "None"
                })
                .ToListAsync();
        }

        public async Task<CustomerDto> GetCustomerByIdAsync(int id)
        {
            var customer = await _context.Customers
                .Include(c => c.Membership)
                .Include(c => c.Wishlists)
                    .ThenInclude(w => w.Items)
                        .ThenInclude(i => i.Product)
                .Include(c => c.Feedbacks)
                .FirstOrDefaultAsync(c => c.CustomerId == id);

            if (customer == null) return null;

            var orderCount = await _context.Orders.CountAsync(o => o.CustomerId == id);
            var totalSpent = await _context.Orders
                .Where(o => o.CustomerId == id)
                .SumAsync(o => o.TotalAmount);
            var wishlistCount = await _context.Wishlists.CountAsync(w => w.CustomerId == id);
            var averageOrderValue = orderCount > 0 ? totalSpent / orderCount : 0;

            return new CustomerDto
            {
                CustomerId = customer.CustomerId,
                UserId = customer.UserId,
                CustomerType = customer.CustomerType,
                CompanyName = customer.CompanyName,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                FullName = $"{customer.FirstName} {customer.LastName}",
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
                    EndDate = (DateTime)customer.Membership.EndDate,
                    PointsBalance = customer.Membership.PointsBalance,
                    LifetimePoints = customer.Membership.LifetimePoints,
                    IsActive = customer.Membership.IsActive
                } : null,
                OrderCount = orderCount,
                TotalSpent = totalSpent,
                WishlistCount = wishlistCount,
                AverageOrderValue = averageOrderValue
            };
        }

        public async Task<Customer> CreateCustomerAsync(CustomerCreateDto customerCreateDto)
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
            return customer;
        }

        public async Task<Customer> UpdateCustomerAsync(int id, CustomerUpdateDto customerUpdateDto)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return null;

            if (!string.IsNullOrEmpty(customerUpdateDto.CustomerType))
                customer.CustomerType = customerUpdateDto.CustomerType;

            if (!string.IsNullOrEmpty(customerUpdateDto.CompanyName))
                customer.CompanyName = customerUpdateDto.CompanyName;

            if (!string.IsNullOrEmpty(customerUpdateDto.FirstName))
                customer.FirstName = customerUpdateDto.FirstName;

            if (!string.IsNullOrEmpty(customerUpdateDto.LastName))
                customer.LastName = customerUpdateDto.LastName;

            if (!string.IsNullOrEmpty(customerUpdateDto.Email))
                customer.Email = customerUpdateDto.Email;

            if (!string.IsNullOrEmpty(customerUpdateDto.Phone))
                customer.Phone = customerUpdateDto.Phone;

            if (!string.IsNullOrEmpty(customerUpdateDto.Address))
                customer.Address = customerUpdateDto.Address;

            if (!string.IsNullOrEmpty(customerUpdateDto.City))
                customer.City = customerUpdateDto.City;

            if (!string.IsNullOrEmpty(customerUpdateDto.State))
                customer.State = customerUpdateDto.State;

            if (!string.IsNullOrEmpty(customerUpdateDto.ZipCode))
                customer.ZipCode = customerUpdateDto.ZipCode;

            if (!string.IsNullOrEmpty(customerUpdateDto.Country))
                customer.Country = customerUpdateDto.Country;

            customer.TaxExempt = customerUpdateDto.TaxExempt;

            if (!string.IsNullOrEmpty(customerUpdateDto.TaxIdentificationNumber))
                customer.TaxIdentificationNumber = customerUpdateDto.TaxIdentificationNumber;

            if (!string.IsNullOrEmpty(customerUpdateDto.Status))
                customer.Status = customerUpdateDto.Status;

            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<bool> DeleteCustomerAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return false;

            // Soft delete by setting status to Inactive
            customer.Status = "Inactive";
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<CustomerMembership> CreateCustomerMembershipAsync(CustomerMembershipCreateDto membershipCreateDto)
        {
            var membership = new CustomerMembership
            {
                CustomerId = membershipCreateDto.CustomerId,
                MembershipLevel = membershipCreateDto.MembershipLevel,
                StartDate = membershipCreateDto.StartDate,
                EndDate = membershipCreateDto.EndDate,
                PointsBalance = membershipCreateDto.PointsBalance,
                LifetimePoints = membershipCreateDto.PointsBalance,
                IsActive = membershipCreateDto.IsActive
            };

            _context.CustomerMemberships.Add(membership);
            await _context.SaveChangesAsync();
            return membership;
        }

        public async Task<CustomerMembership> UpdateCustomerMembershipAsync(int customerId, CustomerMembershipUpdateDto membershipUpdateDto)
        {
            var membership = await _context.CustomerMemberships
                .FirstOrDefaultAsync(m => m.CustomerId == customerId);

            if (membership == null) return null;

            if (!string.IsNullOrEmpty(membershipUpdateDto.MembershipLevel))
                membership.MembershipLevel = membershipUpdateDto.MembershipLevel;

            if (membershipUpdateDto.EndDate.HasValue)
                membership.EndDate = membershipUpdateDto.EndDate.Value;

            if (membershipUpdateDto.PointsBalance.HasValue)
            {
                var pointsDifference = membershipUpdateDto.PointsBalance.Value - membership.PointsBalance;
                membership.PointsBalance = membershipUpdateDto.PointsBalance.Value;
                membership.LifetimePoints += pointsDifference;
            }

            if (membershipUpdateDto.IsActive.HasValue)
                membership.IsActive = membershipUpdateDto.IsActive.Value;

            await _context.SaveChangesAsync();
            return membership;
        }

        public async Task<Wishlist> CreateWishlistAsync(WishlistCreateDto wishlistCreateDto)
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
            return wishlist;
        }

        public async Task<Wishlist> AddWishlistItemAsync(WishlistItemCreateDto wishlistItemCreateDto)
        {
            var wishlistItem = new WishlistItem
            {
                WishlistId = wishlistItemCreateDto.WishlistId,
                ProductId = wishlistItemCreateDto.ProductId,
                DateAdded = DateTime.UtcNow,
                Notes = wishlistItemCreateDto.Notes
            };

            _context.WishlistItems.Add(wishlistItem);
            await _context.SaveChangesAsync();

            return await _context.Wishlists
                .Include(w => w.Items)
                .FirstOrDefaultAsync(w => w.WishlistId == wishlistItemCreateDto.WishlistId);
        }

        public async Task<bool> RemoveWishlistItemAsync(int wishlistItemId)
        {
            var wishlistItem = await _context.WishlistItems.FindAsync(wishlistItemId);
            if (wishlistItem == null) return false;

            _context.WishlistItems.Remove(wishlistItem);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<CustomerFeedback> CreateCustomerFeedbackAsync(CustomerFeedbackCreateDto feedbackCreateDto)
        {
            var feedback = new CustomerFeedback
            {
                CustomerId = feedbackCreateDto.CustomerId,
                OrderId = feedbackCreateDto.OrderId,
                ProductId = feedbackCreateDto.ProductId,
                FeedbackType = feedbackCreateDto.FeedbackType,
                Rating = feedbackCreateDto.Rating,
                Comments = feedbackCreateDto.Comments,
                SubmissionDate = DateTime.UtcNow,
                Status = "Pending"
            };

            _context.CustomerFeedbacks.Add(feedback);
            await _context.SaveChangesAsync();
            return feedback;
        }

        public async Task<CustomerFeedback> UpdateCustomerFeedbackAsync(int feedbackId, CustomerFeedbackUpdateDto feedbackUpdateDto)
        {
            var feedback = await _context.CustomerFeedbacks.FindAsync(feedbackId);
            if (feedback == null) return null;

            if (!string.IsNullOrEmpty(feedbackUpdateDto.Status))
                feedback.Status = feedbackUpdateDto.Status;

            if (!string.IsNullOrEmpty(feedbackUpdateDto.Response))
            {
                feedback.Response = feedbackUpdateDto.Response;
                feedback.ResponseDate = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return feedback;
        }
    }
}
