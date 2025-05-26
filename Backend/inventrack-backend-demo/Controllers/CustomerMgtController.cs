using inventrack_backend_demo.DTOs;
using inventrack_backend_demo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace inventrack_backend_demo.Controllers
{
    #region Customer Management Controllers

    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet("{customerId}")]
        public async Task<ActionResult<CustomerDto>> GetCustomerById(int customerId)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(customerId);
            if (customer == null)
                return NotFound();

            return Ok(customer);
        }

        [HttpGet]
        public async Task<ActionResult<List<CustomerDto>>> GetAllCustomers()
        {
            var customers = await _customerRepository.GetAllCustomersAsync();
            return Ok(customers);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDto>> CreateCustomer(CustomerCreateDto customerCreateDto)
        {
            var customer = await _customerRepository.CreateCustomerAsync(customerCreateDto);
            return CreatedAtAction(nameof(GetCustomerById), new { customerId = customer.CustomerId }, customer);
        }

        [HttpPut("{customerId}")]
        public async Task<ActionResult<CustomerDto>> UpdateCustomer(int customerId, CustomerDto customerDto)
        {
            var customer = await _customerRepository.UpdateCustomerAsync(customerId, customerDto);
            if (customer == null)
                return NotFound();

            return Ok(customer);
        }

        [HttpDelete("{customerId}")]
        public async Task<ActionResult> DeleteCustomer(int customerId)
        {
            var result = await _customerRepository.DeleteCustomerAsync(customerId);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class CustomerMembershipController : ControllerBase
    {
        private readonly ICustomerMembershipRepository _membershipRepository;

        public CustomerMembershipController(ICustomerMembershipRepository membershipRepository)
        {
            _membershipRepository = membershipRepository;
        }

        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<CustomerMembershipDto>> GetMembershipByCustomer(int customerId)
        {
            var membership = await _membershipRepository.GetMembershipByCustomerAsync(customerId);
            if (membership == null)
                return NotFound();

            return Ok(membership);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerMembershipDto>> CreateOrUpdateMembership(CustomerMembershipDto membershipDto)
        {
            var membership = await _membershipRepository.CreateOrUpdateMembershipAsync(membershipDto);
            return Ok(membership);
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistRepository _wishlistRepository;

        public WishlistController(IWishlistRepository wishlistRepository)
        {
            _wishlistRepository = wishlistRepository;
        }

        [HttpGet("{wishlistId}")]
        public async Task<ActionResult<WishlistDto>> GetWishlistById(int wishlistId)
        {
            var wishlist = await _wishlistRepository.GetWishlistByIdAsync(wishlistId);
            if (wishlist == null)
                return NotFound();

            return Ok(wishlist);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<List<WishlistDto>>> GetWishlistsByCustomer(int customerId)
        {
            var wishlists = await _wishlistRepository.GetWishlistsByCustomerAsync(customerId);
            return Ok(wishlists);
        }

        [HttpPost]
        public async Task<ActionResult<WishlistDto>> CreateWishlist(WishlistCreateDto wishlistCreateDto)
        {
            var wishlist = await _wishlistRepository.CreateWishlistAsync(wishlistCreateDto);
            return CreatedAtAction(nameof(GetWishlistById), new { wishlistId = wishlist.WishlistId }, wishlist);
        }

        [HttpDelete("{wishlistId}")]
        public async Task<ActionResult> DeleteWishlist(int wishlistId)
        {
            var result = await _wishlistRepository.DeleteWishlistAsync(wishlistId);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class WishlistItemController : ControllerBase
    {
        private readonly IWishlistItemRepository _wishlistItemRepository;

        public WishlistItemController(IWishlistItemRepository wishlistItemRepository)
        {
            _wishlistItemRepository = wishlistItemRepository;
        }

        [HttpGet("{wishlistItemId}")]
        public async Task<ActionResult<WishlistItemDto>> GetWishlistItemById(int wishlistItemId)
        {
            var item = await _wishlistItemRepository.GetWishlistItemByIdAsync(wishlistItemId);
            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<WishlistItemDto>> AddItemToWishlist(WishlistItemCreateDto wishlistItemCreateDto)
        {
            var item = await _wishlistItemRepository.AddItemToWishlistAsync(wishlistItemCreateDto);
            return CreatedAtAction(nameof(GetWishlistItemById), new { wishlistItemId = item.WishlistItemId }, item);
        }

        [HttpDelete("{wishlistItemId}")]
        public async Task<ActionResult> RemoveItemFromWishlist(int wishlistItemId)
        {
            var result = await _wishlistItemRepository.RemoveItemFromWishlistAsync(wishlistItemId);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class CustomerFeedbackController : ControllerBase
    {
        private readonly ICustomerFeedbackRepository _feedbackRepository;

        public CustomerFeedbackController(ICustomerFeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }

        [HttpGet("{feedbackId}")]
        public async Task<ActionResult<CustomerFeedbackDto>> GetFeedbackById(int feedbackId)
        {
            var feedback = await _feedbackRepository.GetFeedbackByIdAsync(feedbackId);
            if (feedback == null)
                return NotFound();

            return Ok(feedback);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<List<CustomerFeedbackDto>>> GetFeedbackByCustomer(int customerId)
        {
            var feedback = await _feedbackRepository.GetFeedbackByCustomerAsync(customerId);
            return Ok(feedback);
        }

        [HttpGet("product/{productId}")]
        public async Task<ActionResult<List<CustomerFeedbackDto>>> GetFeedbackByProduct(int productId)
        {
            var feedback = await _feedbackRepository.GetFeedbackByProductAsync(productId);
            return Ok(feedback);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerFeedbackDto>> CreateFeedback(CustomerFeedbackDto feedbackDto)
        {
            var feedback = await _feedbackRepository.CreateFeedbackAsync(feedbackDto);
            return CreatedAtAction(nameof(GetFeedbackById), new { feedbackId = feedback.FeedbackId }, feedback);
        }

        [HttpPut("{feedbackId}/response")]
        public async Task<ActionResult<CustomerFeedbackDto>> UpdateFeedbackResponse(int feedbackId, [FromBody] string response)
        {
            var feedback = await _feedbackRepository.UpdateFeedbackResponseAsync(feedbackId, response);
            if (feedback == null)
                return NotFound();

            return Ok(feedback);
        }
    }

    #endregion
}
