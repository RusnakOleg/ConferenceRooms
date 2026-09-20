namespace ConferenceRooms.Api.DTOs.BookingDtos;

/// <summary>
/// DTO для оформлення нового бронювання залу.
/// </summary>
public record CreateBookingDto(
    int RoomId, 
    DateTime StartTime, 
    double DurationHours, 
    List<int> ServiceIds
);