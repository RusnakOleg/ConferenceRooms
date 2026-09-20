namespace ConferenceRooms.Api.DTOs.RoomDtos;

/// <summary>
/// DTO для створення нового конференц-залу.
/// </summary>
public record CreateRoomDto(
    string Name, 
    int Capacity, 
    decimal BasePricePerHour, 
    List<int>? ServiceIds
);