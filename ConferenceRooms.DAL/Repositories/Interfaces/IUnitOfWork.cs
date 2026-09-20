using ConferenceRooms.DAL.Entities;

namespace ConferenceRooms.DAL.Repositories.Interfaces;
/// <summary>
/// Інтерфейс паттерну Unit of Work. Об'єднує всі репозиторії в єдиний контекст 
/// та забезпечує збереження всіх змін у базі даних за один транзакційний запит.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    IRoomRepository Rooms { get; }
    IBookingRepository Bookings { get; }
    IRepository<Service> Services { get; }
    Task<int> CompleteAsync();
}