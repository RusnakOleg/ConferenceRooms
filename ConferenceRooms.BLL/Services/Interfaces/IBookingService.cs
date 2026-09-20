using ConferenceRooms.DAL.Entities;

namespace ConferenceRooms.BLL.Services.Interfaces;
/// <summary>
/// Інтерфейс для сервісу бронювань
/// </summary>
public interface IBookingService
{
    Task<Booking> CreateBookingAsync(int roomId, DateTime startTime, DateTime endTime, List<int> serviceIds);
    Task<IEnumerable<Booking>> GetAllBookingsAsync();
}