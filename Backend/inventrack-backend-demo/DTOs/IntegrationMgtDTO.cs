namespace inventrack_backend_demo.DTOs
{
    #region Integration DTOs

    public class IntegrationPartnerDto
    {
        public int PartnerId { get; set; }
        public string PartnerName { get; set; }
        public string PartnerType { get; set; }
        public string APIEndpoint { get; set; }
        public string AuthType { get; set; }
        public string Status { get; set; }
        public DateTime? LastSyncDate { get; set; }

        // Navigation properties
        public List<IntegrationConfigDto> Configurations { get; set; }
    }

    public class IntegrationConfigDto
    {
        public int ConfigId { get; set; }
        public int PartnerId { get; set; }
        public string ConfigKey { get; set; } = string.Empty;
        public string? ConfigValue { get; set; }
        public bool IsEncrypted { get; set; }
    }

    public class SyncStatusDto
    {
        public int SyncId { get; set; }
        public int PartnerId { get; set; }
        public string EntityType { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Status { get; set; } = string.Empty;
        public int RecordsProcessed { get; set; }
        public int RecordsSucceeded { get; set; }
        public int RecordsFailed { get; set; }
        public string? ErrorDetails { get; set; }
    }

    #endregion
}
