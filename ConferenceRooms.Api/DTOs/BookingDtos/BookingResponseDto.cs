namespace ConferenceRooms.Api.DTOs.BookingDtos;
/// <summary>
/// DTO для передачі інформації про створене бронювання у відповіді клієнту.
/// </summary>
public record BookingResponseDto(
    int Id,
    int RoomId,
    DateTime StartTime,
    DateTime EndTime,
    decimal TotalPrice
);