using ConferenceRooms.DAL.Entities;

namespace ConferenceRooms.BLL.Services.Interfaces;
/// <summary>
/// Інтерфейс для сервісу конференц-залів
/// </summary>
public interface IRoomService
{
    Task<IEnumerable<Room>> GetAllRoomsAsync();
    Task<Room?> GetRoomByIdAsync(int id);
    Task<IEnumerable<Room>> GetAvailableRoomsAsync(DateTime startTime, DateTime endTime, int requiredCapacity);
}