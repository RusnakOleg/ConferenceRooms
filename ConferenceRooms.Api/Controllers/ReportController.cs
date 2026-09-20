using ConferenceRooms.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceRooms.Api.Controllers;
/// <summary>
/// Контролер для формування аналітичних звітів та бізнес-статистики.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;
    
    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }
    
    // Отримує звіт про завантаженість залів за вказаний період.
    [HttpGet("utilization")]
    public async Task<IActionResult> GetRoomUtilization(
        [FromQuery] DateTime startDate, 
        [FromQuery] DateTime endDate)
    {
        if (startDate > endDate)
        {
            throw new ArgumentException("Початкова дата не може бути більшою за кінцеву.");
        }

        var report = await _reportService.GetRoomUtilizationReportAsync(startDate, endDate);
        return Ok(report);
    }
    
    // Формує фінансовий звіт (доходи) за вказаний період.

    [HttpGet("revenue")]
    public async Task<IActionResult> GetRevenueReport(
        [FromQuery] DateTime startDate, 
        [FromQuery] DateTime endDate)
    {
        if (startDate > endDate)
        {
            throw new ArgumentException("Початкова дата не може бути більшою за кінцеву.");
        }

        var report = await _reportService.GetRevenueReportAsync(startDate, endDate);
        return Ok(report);
    }
}