using ConferenceRooms.DAL.Entities;

namespace ConferenceRooms.DAL.Repositories.Interfaces;
/// <summary>
/// Інтерфейс репозиторію для роботи з конференц-залами.
/// </summary>
public interface IRoomRepository : IRepository<Room>
{
    Task<IEnumerable<Room>> GetAvailableRoomsAsync(DateTime startTime, DateTime endTime, int requiredCapacity);
    Task<Room?> GetRoomWithServicesAsync(int id);
}