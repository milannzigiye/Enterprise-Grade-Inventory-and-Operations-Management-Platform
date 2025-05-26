using System.ComponentModel.DataAnnotations;

namespace inventrack_backend_demo.Model
{
    #region Authentication Models

    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        public DateTime ExpiresAt { get; set; }

        public bool IsRevoked { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public int UserId { get; set; }

        // Navigation property
        public virtual User User { get; set; }
    }

    public class PasswordResetToken
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        public DateTime ExpiresAt { get; set; }

        public bool IsUsed { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public int UserId { get; set; }

        // Navigation property
        public virtual User User { get; set; }
    }

    public class TwoFactorSecret
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string SecretKey { get; set; }

        public bool IsActive { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public int UserId { get; set; }

        // Navigation property
        public virtual User User { get; set; }
    }

    #endregion
}
