using ConferenceRooms.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceRooms.Api.Controllers;
/// <summary>
/// Тестовий контролер для перевірки репозиторіїв.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public TestController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet("check-db")]
    public async Task<IActionResult> CheckDatabase()
    {
        //  Отримуємо всі зали разом із послугами (перевірка зв'язків)
        var rooms = await _unitOfWork.Rooms.GetAllAsync();
        
        //  Отримуємо список послуг через загальний репозиторій
        var services = await _unitOfWork.Services.GetAllAsync();

        //  Тестуємо метод пошуку доступних залів (наприклад, на завтра на 2 години)
        var startTime = DateTime.Today.AddDays(1).AddHours(10); // Завтра о 10:00
        var endTime = startTime.AddHours(2);                    // до 12:00
        var availableRooms = await _unitOfWork.Rooms.GetAvailableRoomsAsync(startTime, endTime, requiredCapacity: 40);

        return Ok(new
        {
            Message = "База даних та репозиторії працюють успішно!",
            RoomsCount = rooms.Count(),
            ServicesCount = services.Count(),
            AvailableRoomsForTomorrow = availableRooms.Select(r => new { r.Name, r.Capacity })
        });
    }
}