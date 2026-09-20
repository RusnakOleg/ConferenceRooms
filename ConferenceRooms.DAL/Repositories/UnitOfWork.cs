using ConferenceRooms.DAL.Context;
using ConferenceRooms.DAL.Entities;
using ConferenceRooms.DAL.Repositories.Interfaces;

namespace ConferenceRooms.DAL.Repositories;
/// <summary>
/// Реалізація Unit of Work для керування транзакціями та репозиторіями DAL шару.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public IRoomRepository Rooms { get; }
    public IBookingRepository Bookings { get; }
    public IRepository<Service> Services { get; }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Rooms = new RoomRepository(_context);
        Bookings = new BookingRepository(_context);
        Services = new Repository<Service>(_context);
    }

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}