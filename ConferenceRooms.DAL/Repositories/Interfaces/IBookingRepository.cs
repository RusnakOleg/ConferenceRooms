using ConferenceRooms.DAL.Entities;

namespace ConferenceRooms.DAL.Repositories.Interfaces;
/// <summary>
/// Інтерфейс репозиторію для управління бронюваннями залів.
/// </summary>
public interface IBookingRepository : IRepository<Booking>
{
    Task<bool> IsRoomBookedAsync(int roomId, DateTime startTime, DateTime endTime);
    Task<IEnumerable<Booking>> GetAllWithDetailsAsync();
}