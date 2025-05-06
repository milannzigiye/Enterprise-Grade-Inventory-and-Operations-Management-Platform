using Microsoft.AspNetCore.Mvc;
using inventtrack_backend.Model;
using inventtrack_backend.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using inventtrack_backend.DTOs;

namespace omniflow_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerListItemDto>>> GetAllCustomers()
        {
            var customers = await _customerRepository.GetAllCustomersAsync();
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetCustomerById(int id)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(id);
            if (customer == null) return NotFound();
            return Ok(customer);
        }

        [HttpPost]
        public async Task<ActionResult<Customer>> CreateCustomer(CustomerCreateDto customerCreateDto)
        {
            try
            {
                var customer = await _customerRepository.CreateCustomerAsync(customerCreateDto);
                return CreatedAtAction(nameof(GetCustomerById), new { id = customer.CustomerId }, customer);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Customer>> UpdateCustomer(int id, CustomerUpdateDto customerUpdateDto)
        {
            try
            {
                var customer = await _customerRepository.UpdateCustomerAsync(id, customerUpdateDto);
                if (customer == null) return NotFound();
                return Ok(customer);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var result = await _customerRepository.DeleteCustomerAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPost("membership")]
        public async Task<ActionResult<CustomerMembership>> CreateCustomerMembership(CustomerMembershipCreateDto membershipCreateDto)
        {
            try
            {
                var membership = await _customerRepository.CreateCustomerMembershipAsync(membershipCreateDto);
                return CreatedAtAction(nameof(GetCustomerById), new { id = membership.CustomerId }, membership);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("membership/{customerId}")]
        public async Task<ActionResult<CustomerMembership>> UpdateCustomerMembership(int customerId, CustomerMembershipUpdateDto membershipUpdateDto)
        {
            try
            {
                var membership = await _customerRepository.UpdateCustomerMembershipAsync(customerId, membershipUpdateDto);
                if (membership == null) return NotFound();
                return Ok(membership);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("wishlist")]
        public async Task<ActionResult<Wishlist>> CreateWishlist(WishlistCreateDto wishlistCreateDto)
        {
            try
            {
                var wishlist = await _customerRepository.CreateWishlistAsync(wishlistCreateDto);
                return CreatedAtAction(nameof(GetCustomerById), new { id = wishlist.CustomerId }, wishlist);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("wishlist/item")]
        public async Task<ActionResult<Wishlist>> AddWishlistItem(WishlistItemCreateDto wishlistItemCreateDto)
        {
            try
            {
                var wishlist = await _customerRepository.AddWishlistItemAsync(wishlistItemCreateDto);
                return Ok(wishlist);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("wishlist/item/{wishlistItemId}")]
        public async Task<IActionResult> RemoveWishlistItem(int wishlistItemId)
        {
            var result = await _customerRepository.RemoveWishlistItemAsync(wishlistItemId);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPost("feedback")]
        public async Task<ActionResult<CustomerFeedback>> CreateCustomerFeedback(CustomerFeedbackCreateDto feedbackCreateDto)
        {
            try
            {
                var feedback = await _customerRepository.CreateCustomerFeedbackAsync(feedbackCreateDto);
                return CreatedAtAction(nameof(GetCustomerById), new { id = feedback.CustomerId }, feedback);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("feedback/{feedbackId}")]
        public async Task<ActionResult<CustomerFeedback>> UpdateCustomerFeedback(int feedbackId, CustomerFeedbackUpdateDto feedbackUpdateDto)
        {
            try
            {
                var feedback = await _customerRepository.UpdateCustomerFeedbackAsync(feedbackId, feedbackUpdateDto);
                if (feedback == null) return NotFound();
                return Ok(feedback);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}