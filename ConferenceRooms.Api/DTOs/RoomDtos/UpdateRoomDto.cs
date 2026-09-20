namespace ConferenceRooms.Api.DTOs.RoomDtos;

/// <summary>
/// DTO для оновлення інформації про конференц-зал.
/// </summary>
public record UpdateRoomDto(
    string? Name, 
    int Capacity, 
    decimal BasePricePerHour,
    List<int>? ServiceIds
);