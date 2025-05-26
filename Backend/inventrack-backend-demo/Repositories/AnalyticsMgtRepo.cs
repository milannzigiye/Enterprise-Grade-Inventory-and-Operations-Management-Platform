using inventrack_backend_demo.DTOs;
using inventrack_backend_demo.Model;
using inventrack_backend_demo.Data;
using Microsoft.EntityFrameworkCore;

namespace inventrack_backend_demo.Repositories
{
    #region Analytics Repositories

    public interface ISalesStatisticsRepository
    {
        Task<List<SalesStatisticsDto>> GetSalesStatisticsAsync(DateTime startDate, DateTime endDate, int? productId, int? categoryId, int? warehouseId);
    }

    public class SalesStatisticsRepository : ISalesStatisticsRepository
    {
        private readonly ApplicationDbContext _context;

        public SalesStatisticsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<SalesStatisticsDto>> GetSalesStatisticsAsync(DateTime startDate, DateTime endDate, int? productId, int? categoryId, int? warehouseId)
        {
            var query = _context.SalesStatistics.AsQueryable();

            query = query.Where(s => s.Date >= startDate && s.Date <= endDate);

            if (productId.HasValue)
            {
                query = query.Where(s => s.ProductId == productId.Value);
            }

            if (categoryId.HasValue)
            {
                query = query.Where(s => s.CategoryId == categoryId.Value);
            }

            if (warehouseId.HasValue)
            {
                query = query.Where(s => s.WarehouseId == warehouseId.Value);
            }

            return await query
                .OrderBy(s => s.Date)
                .Select(s => new SalesStatisticsDto
                {
                    StatId = s.StatId,
                    Date = s.Date,
                    ProductId = s.ProductId,
                    CategoryId = s.CategoryId,
                    WarehouseId = s.WarehouseId,
                    SalesQuantity = s.SalesQuantity,
                    SalesAmount = s.SalesAmount,
                    ReturnQuantity = s.ReturnQuantity,
                    ReturnAmount = s.ReturnAmount,
                    NetSales = s.NetSales,
                    ProfitMargin = s.ProfitMargin
                }).ToListAsync();
        }
    }

    public interface IInventoryStatisticsRepository
    {
        Task<List<InventoryStatisticsDto>> GetInventoryStatisticsAsync(DateTime startDate, DateTime endDate, int? productId, int? warehouseId);
    }

    public class InventoryStatisticsRepository : IInventoryStatisticsRepository
    {
        private readonly ApplicationDbContext _context;

        public InventoryStatisticsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<InventoryStatisticsDto>> GetInventoryStatisticsAsync(DateTime startDate, DateTime endDate, int? productId, int? warehouseId)
        {
            var query = _context.InventoryStatistics.AsQueryable();

            query = query.Where(s => s.Date >= startDate && s.Date <= endDate);

            if (productId.HasValue)
            {
                query = query.Where(s => s.ProductId == productId.Value);
            }

            if (warehouseId.HasValue)
            {
                query = query.Where(s => s.WarehouseId == warehouseId.Value);
            }

            return await query
                .OrderBy(s => s.Date)
                .Select(s => new InventoryStatisticsDto
                {
                    StatId = s.StatId,
                    Date = s.Date,
                    ProductId = s.ProductId,
                    WarehouseId = s.WarehouseId,
                    AvgStockLevel = s.AvgStockLevel,
                    StockTurnoverRate = s.StockTurnoverRate,
                    DaysOfSupply = s.DaysOfSupply,
                    StockOutEvents = s.StockOutEvents,
                    InventoryValue = s.InventoryValue
                }).ToListAsync();
        }
    }

    public interface IDashboardWidgetRepository
    {
        Task<List<DashboardWidgetDto>> GetWidgetsByUserAsync(int userId);
        Task<DashboardWidgetDto> GetWidgetByIdAsync(int widgetId);
        Task<DashboardWidgetDto> CreateWidgetAsync(DashboardWidgetDto widgetDto);
        Task<DashboardWidgetDto> UpdateWidgetAsync(int widgetId, DashboardWidgetDto widgetDto);
        Task<bool> DeleteWidgetAsync(int widgetId);
    }

    public class DashboardWidgetRepository : IDashboardWidgetRepository
    {
        private readonly ApplicationDbContext _context;

        public DashboardWidgetRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<DashboardWidgetDto>> GetWidgetsByUserAsync(int userId)
        {
            return await _context.DashboardWidgets
                .Where(w => w.UserId == userId && w.IsActive)
                .OrderBy(w => w.Position)
                .Select(w => new DashboardWidgetDto
                {
                    WidgetId = w.WidgetId,
                    UserId = w.UserId,
                    WidgetType = w.WidgetType,
                    Name = w.Name,
                    Configuration = w.Configuration,
                    Position = w.Position,
                    Size = w.Size,
                    IsActive = w.IsActive
                }).ToListAsync();
        }

        public async Task<DashboardWidgetDto> GetWidgetByIdAsync(int widgetId)
        {
            var widget = await _context.DashboardWidgets.FindAsync(widgetId);
            if (widget == null) return null;

            return new DashboardWidgetDto
            {
                WidgetId = widget.WidgetId,
                UserId = widget.UserId,
                WidgetType = widget.WidgetType,
                Name = widget.Name,
                Configuration = widget.Configuration,
                Position = widget.Position,
                Size = widget.Size,
                IsActive = widget.IsActive
            };
        }

        public async Task<DashboardWidgetDto> CreateWidgetAsync(DashboardWidgetDto widgetDto)
        {
            // Get the next position for the user's widgets
            var maxPosition = await _context.DashboardWidgets
                .Where(w => w.UserId == widgetDto.UserId)
                .MaxAsync(w => (int?)w.Position) ?? 0;

            var widget = new DashboardWidget
            {
                UserId = widgetDto.UserId,
                WidgetType = widgetDto.WidgetType,
                Name = widgetDto.Name,
                Configuration = widgetDto.Configuration,
                Position = maxPosition + 1,
                Size = widgetDto.Size,
                IsActive = true
            };

            _context.DashboardWidgets.Add(widget);
            await _context.SaveChangesAsync();

            return await GetWidgetByIdAsync(widget.WidgetId);
        }

        public async Task<DashboardWidgetDto> UpdateWidgetAsync(int widgetId, DashboardWidgetDto widgetDto)
        {
            var widget = await _context.DashboardWidgets.FindAsync(widgetId);
            if (widget == null) return null;

            widget.WidgetType = widgetDto.WidgetType;
            widget.Name = widgetDto.Name;
            widget.Configuration = widgetDto.Configuration;
            widget.Position = widgetDto.Position;
            widget.Size = widgetDto.Size;
            widget.IsActive = widgetDto.IsActive;

            await _context.SaveChangesAsync();
            return await GetWidgetByIdAsync(widgetId);
        }

        public async Task<bool> DeleteWidgetAsync(int widgetId)
        {
            var widget = await _context.DashboardWidgets.FindAsync(widgetId);
            if (widget == null) return false;

            widget.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }

    public interface IReportRepository
    {
        Task<List<ReportDto>> GetAllReportsAsync();
        Task<ReportDto> GetReportByIdAsync(int reportId);
        Task<ReportDto> CreateReportAsync(ReportDto reportDto);
        Task<ReportDto> UpdateReportAsync(int reportId, ReportDto reportDto);
        Task<bool> DeleteReportAsync(int reportId);
    }

    public class ReportRepository : IReportRepository
    {
        private readonly ApplicationDbContext _context;

        public ReportRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ReportDto>> GetAllReportsAsync()
        {
            return await _context.Reports
                .Include(r => r.CreatedByUser)
                .Select(r => new ReportDto
                {
                    ReportId = r.ReportId,
                    ReportName = r.ReportName,
                    Description = r.Description,
                    ReportType = r.ReportType,
                    CreatedByUserId = r.CreatedByUserId,
                    CreationDate = r.CreationDate,
                    LastRunDate = r.LastRunDate,
                    ScheduleType = r.ScheduleType,
                    NextScheduledRun = r.NextScheduledRun,
                    OutputFormat = r.OutputFormat,
                    CreatedByUser = new UserDto
                    {
                        UserId = r.CreatedByUser.UserId,
                        Username = r.CreatedByUser.Username
                    }
                }).ToListAsync();
        }

        public async Task<ReportDto> GetReportByIdAsync(int reportId)
        {
            var report = await _context.Reports
                .Include(r => r.CreatedByUser)
                .FirstOrDefaultAsync(r => r.ReportId == reportId);

            if (report == null) return null;

            return new ReportDto
            {
                ReportId = report.ReportId,
                ReportName = report.ReportName,
                Description = report.Description,
                ReportType = report.ReportType,
                CreatedByUserId = report.CreatedByUserId,
                CreationDate = report.CreationDate,
                LastRunDate = report.LastRunDate,
                ScheduleType = report.ScheduleType,
                NextScheduledRun = report.NextScheduledRun,
                Parameters = report.Parameters,
                OutputFormat = report.OutputFormat,
                CreatedByUser = new UserDto
                {
                    UserId = report.CreatedByUser.UserId,
                    Username = report.CreatedByUser.Username
                }
            };
        }

        public async Task<ReportDto> CreateReportAsync(ReportDto reportDto)
        {
            var report = new Report
            {
                ReportName = reportDto.ReportName,
                Description = reportDto.Description,
                ReportType = reportDto.ReportType,
                CreatedByUserId = reportDto.CreatedByUserId,
                CreationDate = DateTime.UtcNow,
                ScheduleType = reportDto.ScheduleType,
                NextScheduledRun = CalculateNextRunDate(reportDto.ScheduleType),
                Parameters = reportDto.Parameters,
                OutputFormat = reportDto.OutputFormat
            };

            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            return await GetReportByIdAsync(report.ReportId);
        }

        public async Task<ReportDto> UpdateReportAsync(int reportId, ReportDto reportDto)
        {
            var report = await _context.Reports.FindAsync(reportId);
            if (report == null) return null;

            report.ReportName = reportDto.ReportName;
            report.Description = reportDto.Description;
            report.ReportType = reportDto.ReportType;
            report.ScheduleType = reportDto.ScheduleType;
            report.NextScheduledRun = CalculateNextRunDate(reportDto.ScheduleType);
            report.Parameters = reportDto.Parameters;
            report.OutputFormat = reportDto.OutputFormat;

            await _context.SaveChangesAsync();
            return await GetReportByIdAsync(reportId);
        }

        public async Task<bool> DeleteReportAsync(int reportId)
        {
            var report = await _context.Reports.FindAsync(reportId);
            if (report == null) return false;

            _context.Reports.Remove(report);
            await _context.SaveChangesAsync();
            return true;
        }

        private DateTime? CalculateNextRunDate(string scheduleType)
        {
            if (string.IsNullOrEmpty(scheduleType)) return null;

            return scheduleType switch
            {
                "Daily" => DateTime.UtcNow.AddDays(1),
                "Weekly" => DateTime.UtcNow.AddDays(7),
                "Monthly" => DateTime.UtcNow.AddMonths(1),
                _ => null,
            };
        }
    }

    #endregion
}
