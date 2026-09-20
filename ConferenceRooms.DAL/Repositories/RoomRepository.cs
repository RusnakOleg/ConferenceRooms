using ConferenceRooms.DAL.Context;
using ConferenceRooms.DAL.Entities;
using ConferenceRooms.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ConferenceRooms.DAL.Repositories;
/// <summary>
/// Реалізація репозиторію конференц-залів із додатковою логікою фільтрації за часом і місткістю.
/// </summary>
public class RoomRepository : Repository<Room>, IRoomRepository
{
    public RoomRepository(AppDbContext context) : base(context)
    {
    }

    // Отримання залу разом із його доступними послугами
    public async Task<Room?> GetRoomWithServicesAsync(int id)
    {
        return await Context.Rooms
            .Include(r => r.RoomServices)
            .ThenInclude(rs => rs.Service)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    // Пошук доступних залів на конкретний час і місткість
    public async Task<IEnumerable<Room>> GetAvailableRoomsAsync(DateTime startTime, DateTime endTime, int requiredCapacity)
    {
        // Знаходимо ID залів, які вже заброньовані на цей проміжок часу
        var bookedRoomIds = await Context.Bookings
            .Where(b => b.StartTime < endTime && b.EndTime > startTime)
            .Select(b => b.RoomId)
            .ToListAsync();

        // Повертаємо зали, які підходять за місткістю і НЕ входять у список зайнятих
        return await Context.Rooms
            .Include(r => r.RoomServices)
            .ThenInclude(rs => rs.Service)
            .Where(r => r.Capacity >= requiredCapacity && !bookedRoomIds.Contains(r.Id))
            .ToListAsync();
    }
}