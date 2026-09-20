namespace ConferenceRooms.Api.DTOs.RoomDtos;
/// <summary>
/// DTO для передачі детальної інформації про конференц-зал у відповіді клієнту.
/// </summary>
public record RoomResponseDto(
    int Id,
    string Name,
    int Capacity,
    decimal BasePricePerHour
);