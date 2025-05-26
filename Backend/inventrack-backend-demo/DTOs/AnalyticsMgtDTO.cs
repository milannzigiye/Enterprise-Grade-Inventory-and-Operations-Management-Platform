namespace inventrack_backend_demo.DTOs
{
    #region Analytics DTOs

    public class SalesStatisticsDto
    {
        public int StatId { get; set; }
        public DateTime Date { get; set; }
        public int? ProductId { get; set; }
        public int? CategoryId { get; set; }
        public int? WarehouseId { get; set; }
        public int SalesQuantity { get; set; }
        public decimal SalesAmount { get; set; }
        public int ReturnQuantity { get; set; }
        public decimal ReturnAmount { get; set; }
        public decimal NetSales { get; set; }
        public decimal ProfitMargin { get; set; }
    }

    public class InventoryStatisticsDto
    {
        public int StatId { get; set; }
        public DateTime Date { get; set; }
        public int ProductId { get; set; }
        public int WarehouseId { get; set; }
        public decimal AvgStockLevel { get; set; }
        public decimal StockTurnoverRate { get; set; }
        public int DaysOfSupply { get; set; }
        public int StockOutEvents { get; set; }
        public decimal InventoryValue { get; set; }
    }

    public class DashboardWidgetDto
    {
        public int WidgetId { get; set; }
        public int UserId { get; set; }
        public string WidgetType { get; set; }
        public string Name { get; set; }
        public string Configuration { get; set; }
        public int Position { get; set; }
        public string Size { get; set; }
        public bool IsActive { get; set; }
    }

    public class ReportDto
    {
        public int ReportId { get; set; }
        public string ReportName { get; set; }
        public string Description { get; set; }
        public string ReportType { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? LastRunDate { get; set; }
        public string ScheduleType { get; set; }
        public DateTime? NextScheduledRun { get; set; }
        public string Parameters { get; set; }
        public string OutputFormat { get; set; }

        // Navigation properties
        public UserDto CreatedByUser { get; set; }
    }

    #endregion
}
