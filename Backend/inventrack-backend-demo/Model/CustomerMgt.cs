using System.ComponentModel.DataAnnotations;

namespace inventrack_backend_demo.Model
{
    #region Customer Management Domain

    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        public int UserId { get; set; }

        [Required]
        [StringLength(20)]
        public string CustomerType { get; set; }

        [StringLength(100)]
        public string CompanyName { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
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

        public DateTime JoinDate { get; set; } = DateTime.UtcNow;

        public DateTime? LastPurchaseDate { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Active";

        // Navigation properties
        public virtual User User { get; set; }
        public virtual CustomerMembership Membership { get; set; }
        public virtual ICollection<Wishlist> Wishlists { get; set; }
        public virtual ICollection<CustomerFeedback> Feedbacks { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }

    public class CustomerMembership
    {
        [Key]
        public int MembershipId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(20)]
        public string MembershipLevel { get; set; }

        public DateTime StartDate { get; set; } = DateTime.UtcNow;

        public DateTime? EndDate { get; set; }

        public int PointsBalance { get; set; }

        public int LifetimePoints { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual Customer Customer { get; set; }
    }

    public class Wishlist
    {
        [Key]
        public int WishlistId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public bool IsPublic { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual Customer Customer { get; set; }
        public virtual ICollection<WishlistItem> Items { get; set; }
    }

    public class WishlistItem
    {
        [Key]
        public int WishlistItemId { get; set; }

        [Required]
        public int WishlistId { get; set; }

        [Required]
        public int ProductId { get; set; }

        public DateTime DateAdded { get; set; } = DateTime.UtcNow;

        [StringLength(255)]
        public string Notes { get; set; }

        // Navigation properties
        public virtual Wishlist Wishlist { get; set; }
        public virtual Product Product { get; set; }
    }

    public class CustomerFeedback
    {
        [Key]
        public int FeedbackId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        public int? OrderId { get; set; }

        public int? ProductId { get; set; }

        [Required]
        [StringLength(20)]
        public string FeedbackType { get; set; }

        public int Rating { get; set; }

        [StringLength(1000)]
        public string Comments { get; set; }

        public DateTime SubmissionDate { get; set; } = DateTime.UtcNow;

        [StringLength(20)]
        public string Status { get; set; } = "Pending";

        [StringLength(500)]
        public string Response { get; set; }

        public DateTime? ResponseDate { get; set; }

        // Navigation properties
        public virtual Customer Customer { get; set; }
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }

    #endregion
}
