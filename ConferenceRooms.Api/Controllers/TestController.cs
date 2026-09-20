using ConferenceRooms.BLL.Helpers;
using ConferenceRooms.BLL.Services.Interfaces;
using ConferenceRooms.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceRooms.Api.Controllers;
/// <summary>
/// Тестовий контролер для перевірки шарів.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly IRoomService _roomService;
    private readonly IExtraServiceService _extraServiceService;
    private readonly IBookingService _bookingService;
    private readonly PricingCalculator _pricingCalculator;

    public TestController(
        IRoomService roomService, 
        IExtraServiceService extraServiceService, 
        IBookingService bookingService, 
        PricingCalculator pricingCalculator)
    {
        _roomService = roomService;
        _extraServiceService = extraServiceService;
        _bookingService = bookingService;
        _pricingCalculator = pricingCalculator;
    }

    // 1. Перевірка RoomService та ExtraServiceService
    [HttpGet("check-services")]
    public async Task<IActionResult> CheckServices()
    {
        var rooms = await _roomService.GetAllRoomsAsync();
        var services = await _extraServiceService.GetAllServicesAsync();

        return Ok(new
        {
            Message = "RoomService та ExtraServiceService працюють успішно!",
            TotalRooms = rooms.Count(),
            TotalServices = services.Count(),
            Rooms = rooms.Select(r => new { r.Id, r.Name, r.Capacity, r.BasePricePerHour }),
            Services = services.Select(s => new { s.Id, s.Name, s.Price })
        });
    }

    // 2. Перевірка PricingCalculator (тестуємо годинні знижки та націнки)
    [HttpGet("test-calculator")]
    public IActionResult TestCalculator()
    {
        decimal basePrice = 100m; // Базова ціна 100 грн/год
        
        // Тест 1: Пікові години (з 12:00 до 14:00) -> націнка 15% (очікується 115 * 2 = 230)
        var peakStart = new DateTime(2026, 6, 1, 12, 0, 0);
        var peakEnd = peakStart.AddHours(2);
        decimal peakPrice = _pricingCalculator.CalculateTotalPrice(basePrice, peakStart, peakEnd, new List<decimal>());

        // Тест 2: Ранкові години (з 07:00 до 09:00) -> знижка 10% (очікується 90 * 2 = 180)
        var morningStart = new DateTime(2026, 6, 1, 7, 0, 0);
        var morningEnd = morningStart.AddHours(2);
        decimal morningPrice = _pricingCalculator.CalculateTotalPrice(basePrice, morningStart, morningEnd, new List<decimal>());

        // Тест 3: Вечірні години (з 18:00 до 20:00) -> знижка 20% (очікується 80 * 2 = 160)
        var eveningStart = new DateTime(2026, 6, 1, 18, 0, 0);
        var eveningEnd = eveningStart.AddHours(2);
        decimal eveningPrice = _pricingCalculator.CalculateTotalPrice(basePrice, eveningStart, eveningEnd, new List<decimal>());

        return Ok(new
        {
            Message = "PricingCalculator працює успішно!",
            PeakHoursPrice_12_14 = peakPrice,       // Має бути 230
            MorningHoursPrice_07_09 = morningPrice,   // Має бути 180
            EveningHoursPrice_18_20 = eveningPrice    // Має бути 160
        });
    }

    // 3. Перевірка створення бронювання через BookingService
    [HttpPost("test-create-booking")]
    public async Task<IActionResult> TestCreateBooking()
    {
        try
        {
            // Бронюємо на завтра з 10:00 до 12:00 (стандартні години)
            var startTime = DateTime.Today.AddDays(1).AddHours(10);
            var endTime = startTime.AddHours(2);
            
            // Припускаємо, що у нас є зал з ID = 1 та послуги з ID = 1, 2
            var booking = await _bookingService.CreateBookingAsync(roomId: 1, startTime, endTime, serviceIds: new List<int> { 1, 2 });

            return Ok(new
            {
                Message = "BookingService успішно створив бронювання та розрахував ціну!",
                BookingId = booking.Id,
                booking.RoomId,
                booking.StartTime,
                booking.EndTime,
                booking.TotalPrice
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
}