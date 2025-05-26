using inventrack_backend_demo.DTOs;
using inventrack_backend_demo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace inventrack_backend_demo.Controllers
{
    #region Analytics Controllers

    [ApiController]
    [Route("api/[controller]")]
    public class SalesStatisticsController : ControllerBase
    {
        private readonly ISalesStatisticsRepository _salesStatisticsRepository;

        public SalesStatisticsController(ISalesStatisticsRepository salesStatisticsRepository)
        {
            _salesStatisticsRepository = salesStatisticsRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<SalesStatisticsDto>>> GetSalesStatistics(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] int? productId,
            [FromQuery] int? categoryId,
            [FromQuery] int? warehouseId)
        {
            var statistics = await _salesStatisticsRepository.GetSalesStatisticsAsync(
                startDate, endDate, productId, categoryId, warehouseId);
            return Ok(statistics);
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class InventoryStatisticsController : ControllerBase
    {
        private readonly IInventoryStatisticsRepository _inventoryStatisticsRepository;

        public InventoryStatisticsController(IInventoryStatisticsRepository inventoryStatisticsRepository)
        {
            _inventoryStatisticsRepository = inventoryStatisticsRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<InventoryStatisticsDto>>> GetInventoryStatistics(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] int? productId,
            [FromQuery] int? warehouseId)
        {
            var statistics = await _inventoryStatisticsRepository.GetInventoryStatisticsAsync(
                startDate, endDate, productId, warehouseId);
            return Ok(statistics);
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class DashboardWidgetController : ControllerBase
    {
        private readonly IDashboardWidgetRepository _dashboardWidgetRepository;

        public DashboardWidgetController(IDashboardWidgetRepository dashboardWidgetRepository)
        {
            _dashboardWidgetRepository = dashboardWidgetRepository;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<DashboardWidgetDto>>> GetWidgetsByUser(int userId)
        {
            var widgets = await _dashboardWidgetRepository.GetWidgetsByUserAsync(userId);
            return Ok(widgets);
        }

        [HttpGet("{widgetId}")]
        public async Task<ActionResult<DashboardWidgetDto>> GetWidgetById(int widgetId)
        {
            var widget = await _dashboardWidgetRepository.GetWidgetByIdAsync(widgetId);
            if (widget == null)
                return NotFound();

            return Ok(widget);
        }

        [HttpPost]
        public async Task<ActionResult<DashboardWidgetDto>> CreateWidget(DashboardWidgetDto widgetDto)
        {
            var widget = await _dashboardWidgetRepository.CreateWidgetAsync(widgetDto);
            return CreatedAtAction(nameof(GetWidgetById), new { widgetId = widget.WidgetId }, widget);
        }

        [HttpPut("{widgetId}")]
        public async Task<ActionResult<DashboardWidgetDto>> UpdateWidget(int widgetId, DashboardWidgetDto widgetDto)
        {
            var widget = await _dashboardWidgetRepository.UpdateWidgetAsync(widgetId, widgetDto);
            if (widget == null)
                return NotFound();

            return Ok(widget);
        }

        [HttpDelete("{widgetId}")]
        public async Task<ActionResult> DeleteWidget(int widgetId)
        {
            var result = await _dashboardWidgetRepository.DeleteWidgetAsync(widgetId);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly IReportRepository _reportRepository;

        public ReportController(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<ReportDto>>> GetAllReports()
        {
            var reports = await _reportRepository.GetAllReportsAsync();
            return Ok(reports);
        }

        [HttpGet("{reportId}")]
        public async Task<ActionResult<ReportDto>> GetReportById(int reportId)
        {
            var report = await _reportRepository.GetReportByIdAsync(reportId);
            if (report == null)
                return NotFound();

            return Ok(report);
        }

        [HttpPost]
        public async Task<ActionResult<ReportDto>> CreateReport(ReportDto reportDto)
        {
            var report = await _reportRepository.CreateReportAsync(reportDto);
            return CreatedAtAction(nameof(GetReportById), new { reportId = report.ReportId }, report);
        }

        [HttpPut("{reportId}")]
        public async Task<ActionResult<ReportDto>> UpdateReport(int reportId, ReportDto reportDto)
        {
            var report = await _reportRepository.UpdateReportAsync(reportId, reportDto);
            if (report == null)
                return NotFound();

            return Ok(report);
        }

        [HttpDelete("{reportId}")]
        public async Task<ActionResult> DeleteReport(int reportId)
        {
            var result = await _reportRepository.DeleteReportAsync(reportId);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }

    #endregion
}
