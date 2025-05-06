namespace inventtrack_backend.DTOs
{
    using System.ComponentModel.DataAnnotations;

    public class CustomerDto
    {
        public int CustomerId { get; set; }
        public int UserId { get; set; }
        public string CustomerType { get; set; }
        public string CompanyName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public bool TaxExempt { get; set; }
        public string TaxIdentificationNumber { get; set; }
        public DateTime JoinDate { get; set; }
        public DateTime? LastPurchaseDate { get; set; }
        public string Status { get; set; }
        public CustomerMembershipDto Membership { get; set; }
        public int OrderCount { get; set; }
        public decimal TotalSpent { get; set; }
        public int WishlistCount { get; set; }
        public decimal AverageOrderValue { get; set; }
    }

    public class CustomerCreateDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(20)]
        public string CustomerType { get; set; }

        [StringLength(200)]
        public string CompanyName { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }

        [Phone]
        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(50)]
        public string State { get; set; }

        [StringLength(20)]
        public string ZipCode { get; set; }

        [StringLength(50)]
        public string Country { get; set; }

        public bool TaxExempt { get; set; }

        [StringLength(50)]
        public string TaxIdentificationNumber { get; set; }
    }

    public class CustomerUpdateDto
    {
        [StringLength(20)]
        public string CustomerType { get; set; }

        [StringLength(200)]
        public string CompanyName { get; set; }

        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(100)]
        public string LastName { get; set; }

        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }

        [Phone]
        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(50)]
        public string State { get; set; }

        [StringLength(20)]
        public string ZipCode { get; set; }

        [StringLength(50)]
        public string Country { get; set; }

        public bool TaxExempt { get; set; }

        [StringLength(50)]
        public string TaxIdentificationNumber { get; set; }

        [StringLength(20)]
        public string Status { get; set; }
    }

    public class CustomerListItemDto
    {
        public int CustomerId { get; set; }
        public string CustomerType { get; set; }
        public string CompanyName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Status { get; set; }
        public DateTime JoinDate { get; set; }
        public DateTime? LastPurchaseDate { get; set; }
        public decimal TotalSpent { get; set; }
        public string MembershipLevel { get; set; }
    }

    public class CustomerMembershipDto
    {
        public int MembershipId { get; set; }
        public int CustomerId { get; set; }
        public string MembershipLevel { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int PointsBalance { get; set; }
        public int LifetimePoints { get; set; }
        public bool IsActive { get; set; }
    }

    public class CustomerMembershipCreateDto
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(20)]
        public string MembershipLevel { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public int PointsBalance { get; set; } = 0;

        public bool IsActive { get; set; } = true;
    }

    public class CustomerMembershipUpdateDto
    {
        [StringLength(20)]
        public string MembershipLevel { get; set; }

        public DateTime? EndDate { get; set; }

        public int? PointsBalance { get; set; }

        public bool? IsActive { get; set; }
    }

    public class WishlistDto
    {
        public int WishlistId { get; set; }
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public bool IsPublic { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<WishlistItemDto> Items { get; set; }
    }

    public class WishlistCreateDto
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public bool IsPublic { get; set; } = false;
    }

    public class WishlistItemDto
    {
        public int WishlistItemId { get; set; }
        public int WishlistId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string SKU { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public DateTime DateAdded { get; set; }
        public string Notes { get; set; }
    }

    public class WishlistItemCreateDto
    {
        [Required]
        public int WishlistId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }
    }

    public class CustomerFeedbackDto
    {
        public int FeedbackId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int? OrderId { get; set; }
        public string OrderNumber { get; set; }
        public int? ProductId { get; set; }
        public string ProductName { get; set; }
        public string FeedbackType { get; set; }
        public int Rating { get; set; }
        public string Comments { get; set; }
        public DateTime SubmissionDate { get; set; }
        public string Status { get; set; }
        public string Response { get; set; }
        public DateTime? ResponseDate { get; set; }
    }

    public class CustomerFeedbackCreateDto
    {
        [Required]
        public int CustomerId { get; set; }

        public int? OrderId { get; set; }

        public int? ProductId { get; set; }

        [Required]
        [StringLength(50)]
        public string FeedbackType { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        [StringLength(2000)]
        public string Comments { get; set; }
    }

    public class CustomerFeedbackUpdateDto
    {
        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(2000)]
        public string Response { get; set; }
    }
}
