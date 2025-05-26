using System.ComponentModel.DataAnnotations;

namespace inventrack_backend_demo.Model
{
    #region Integration Domain

    public class IntegrationPartner
    {
        [Key]
        public int PartnerId { get; set; }

        [Required]
        [StringLength(100)]
        public string PartnerName { get; set; }

        [Required]
        [StringLength(50)]
        public string PartnerType { get; set; }

        [Required]
        [StringLength(255)]
        public string APIEndpoint { get; set; }

        [Required]
        [StringLength(50)]
        public string AuthType { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; }

        public DateTime? LastSyncDate { get; set; }

        // Navigation properties
        public virtual ICollection<IntegrationConfig> Configurations { get; set; }
        public virtual ICollection<SyncStatus> SyncStatuses { get; set; }
    }

    public class IntegrationConfig
    {
        [Key]
        public int ConfigId { get; set; }

        [Required]
        public int PartnerId { get; set; }

        [Required]
        [StringLength(100)]
        public string ConfigKey { get; set; }

        [Required]
        public string ConfigValue { get; set; }

        public bool IsEncrypted { get; set; }

        // Navigation properties
        public virtual IntegrationPartner Partner { get; set; }
    }

    public class SyncStatus
    {
        [Key]
        public int SyncId { get; set; }

        [Required]
        public int PartnerId { get; set; }

        [Required]
        [StringLength(50)]
        public string EntityType { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; }

        public int RecordsProcessed { get; set; }

        public int RecordsSucceeded { get; set; }

        public int RecordsFailed { get; set; }

        public string ErrorDetails { get; set; }

        // Navigation properties
        public virtual IntegrationPartner Partner { get; set; }
    }

    #endregion
}
