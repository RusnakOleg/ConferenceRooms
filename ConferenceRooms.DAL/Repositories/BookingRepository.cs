using ConferenceRooms.DAL.Context;
using ConferenceRooms.DAL.Entities;
using ConferenceRooms.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ConferenceRooms.DAL.Repositories;
/// <summary>
/// Реалізація репозиторію бронювань для перевірки зайнятості та отримання зв'язаних даних.
/// </summary>
public class BookingRepository : Repository<Booking>, IBookingRepository
{
    public BookingRepository(AppDbContext context) : base(context)
    {
    }

    // Перевірка, чи зайнятий зал у вказаний проміжок часу
    public async Task<bool> IsRoomBookedAsync(int roomId, DateTime startTime, DateTime endTime)
    {
        return await Context.Bookings
            .AnyAsync(b => b.RoomId == roomId && b.StartTime < endTime && b.EndTime > startTime);
    }

    // Отримання всіх бронювань разом із деталями залу та обраними послугами 
    public async Task<IEnumerable<Booking>> GetAllWithDetailsAsync()
    {
        return await Context.Bookings
            .Include(b => b.Room)
            .Include(b => b.BookingServices)
            .ThenInclude(bs => bs.Service)
            .ToListAsync();
    }
    
    // Реалізація додавання проміжного зв'язку
    public async Task AddBookingServiceAsync(BookingService bookingService)
    {
        await Context.BookingServices.AddAsync(bookingService);
    }
}