namespace inventtrack_backend.DTOs
{
    #region Customer Management DTOs

    public class CustomerDto
    {
        public int CustomerId { get; set; }
        public int UserId { get; set; }
        public string CustomerType { get; set; }
        public string CompanyName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
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

        // Navigation properties
        public UserDto User { get; set; }
        public CustomerMembershipDto Membership { get; set; }
    }

    public class CustomerCreateDto
    {
        public int UserId { get; set; }
        public string CustomerType { get; set; }
        public string CompanyName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public bool TaxExempt { get; set; }
        public string TaxIdentificationNumber { get; set; }
    }

    public class CustomerMembershipDto
    {
        public int MembershipId { get; set; }
        public int CustomerId { get; set; }
        public string MembershipLevel { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int PointsBalance { get; set; }
        public int LifetimePoints { get; set; }
        public bool IsActive { get; set; }
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
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public bool IsPublic { get; set; }
    }

    public class WishlistItemDto
    {
        public int WishlistItemId { get; set; }
        public int WishlistId { get; set; }
        public int ProductId { get; set; }
        public DateTime DateAdded { get; set; }
        public string Notes { get; set; }

        // Navigation properties
        public ProductDto Product { get; set; }
    }

    public class WishlistItemCreateDto
    {
        public int WishlistId { get; set; }
        public int ProductId { get; set; }
        public string Notes { get; set; }
    }

    public class CustomerFeedbackDto
    {
        public int FeedbackId { get; set; }
        public int CustomerId { get; set; }
        public int? OrderId { get; set; }
        public int? ProductId { get; set; }
        public string FeedbackType { get; set; }
        public int? Rating { get; set; }
        public string Comments { get; set; }
        public DateTime SubmissionDate { get; set; }
        public string Status { get; set; }
        public string Response { get; set; }
        public DateTime? ResponseDate { get; set; }

        // Navigation properties
        public CustomerDto Customer { get; set; }
        public ProductDto Product { get; set; }
    }

    #endregion
}
