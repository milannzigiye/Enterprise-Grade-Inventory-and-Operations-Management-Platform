using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace inventtrack_backend.Model
{
    #region Analytics Domain

    public class SalesStatistics
    {
        [Key]
        public int StatId { get; set; }  // Changed from Guid to int

        [Required]
        public DateTime Date { get; set; }

        public int? ProductId { get; set; }  // Changed from Guid? to int?

        public int? CategoryId { get; set; }  // Changed from Guid? to int?

        public int? WarehouseId { get; set; }  // Changed from Guid? to int?

        public int SalesQuantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal SalesAmount { get; set; }

        public int ReturnQuantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ReturnAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal NetSales { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ProfitMargin { get; set; }

        // Navigation properties
        public virtual Product Product { get; set; }
        public virtual ProductCategory Category { get; set; }
        public virtual Warehouse Warehouse { get; set; }
    }

    public class InventoryStatistics
    {
        [Key]
        public int StatId { get; set; }  // Changed from Guid to int

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int ProductId { get; set; }  // Changed from Guid to int

        [Required]
        public int WarehouseId { get; set; }  // Changed from Guid to int

        public decimal AvgStockLevel { get; set; }

        public decimal StockTurnoverRate { get; set; }

        public int DaysOfSupply { get; set; }

        public int StockOutEvents { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal InventoryValue { get; set; }

        // Navigation properties
        public virtual Product Product { get; set; }
        public virtual Warehouse Warehouse { get; set; }
    }

    public class DashboardWidget
    {
        [Key]
        public int WidgetId { get; set; }  // Changed from Guid to int

        [Required]
        public int UserId { get; set; }  // Changed from Guid to int

        [Required]
        [StringLength(50)]
        public string WidgetType { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string Configuration { get; set; }

        public int Position { get; set; }

        public string Size { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual User User { get; set; }
    }

    public class Report
    {
        [Key]
        public int ReportId { get; set; }

        [Required]
        [StringLength(100)]
        public string ReportName { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [StringLength(50)]
        public string ReportType { get; set; }

        [Required]
        public int CreatedByUserId { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.UtcNow;

        public DateTime? LastRunDate { get; set; }

        [StringLength(20)]
        public string ScheduleType { get; set; }

        public DateTime? NextScheduledRun { get; set; }

        public string Parameters { get; set; }

        [StringLength(20)]
        public string OutputFormat { get; set; }

        // Navigation properties
        public virtual User CreatedByUser { get; set; }
    }

    #endregion
}
