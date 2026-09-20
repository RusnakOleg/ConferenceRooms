using AutoMapper;
using ConferenceRooms.Api.DTOs.BookingDtos;
using ConferenceRooms.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceRooms.Api.Controllers;
/// <summary>
/// Контролер для управління бронюваннями конференц-залів.
/// </summary
[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;
    private readonly IMapper _mapper;

    public BookingsController(IBookingService bookingService, IMapper mapper)
    {
        _bookingService = bookingService;
        _mapper = mapper;
    }
    
    // Отримання списку всіх бронювань.
    [HttpGet]
    public async Task<IActionResult> GetAllBookings()
    {
        var bookings = await _bookingService.GetAllBookingsAsync();
        var responseDtos = _mapper.Map<IEnumerable<BookingResponseDto>>(bookings);
        
        return Ok(responseDtos);
    }
    
    // Бронювання залу з розрахунком вартості.
    [HttpPost]
    public async Task<IActionResult> CreateBooking([FromBody] CreateBookingDto dto)
    {
        try
        {
            // Обчислюємо час закінчення на основі тривалості в годинах
            var endTime = dto.StartTime.AddHours(dto.DurationHours);

            var booking = await _bookingService.CreateBookingAsync(
                dto.RoomId, 
                dto.StartTime, 
                endTime, 
                dto.ServiceIds ?? new List<int>()
            );

            // Мапимо результат у BookingResponseDto через AutoMapper
            var responseDto = _mapper.Map<BookingResponseDto>(booking);

            return Ok(new
            {
                Message = "Бронювання успішно створено!",
                Data = responseDto
            });
        }
        catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException || ex is KeyNotFoundException)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
}