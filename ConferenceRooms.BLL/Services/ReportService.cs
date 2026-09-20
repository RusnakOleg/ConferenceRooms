using ConferenceRooms.BLL.Services.Interfaces;
using ConferenceRooms.DAL.Repositories.Interfaces;

namespace ConferenceRooms.BLL.Services;
/// <summary>
/// Сервіс для генерації бізнес-звітів та аналітики.
/// </summary>
public class ReportService : IReportService
{
    private readonly IUnitOfWork _unitOfWork;

    public ReportService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // Формує звіт про завантаженість конференц-залів за період.
    public async Task<object> GetRoomUtilizationReportAsync(DateTime startDate, DateTime endDate)
    {
        var bookings = await _unitOfWork.Bookings.GetAllAsync();
        var rooms = await _unitOfWork.Rooms.GetAllAsync();

        // Фільтруємо бронювання за вказаним періодом
        var filteredBookings = bookings
            .Where(b => b.StartTime >= startDate && b.EndTime <= endDate)
            .ToList();

        var utilizationReport = rooms.Select(room =>
        {
            var roomBookings = filteredBookings.Where(b => b.RoomId == room.Id).ToList();
            
            // Рахуємо загальну кількість заброньованих годин для кожного залу
            double totalHoursBooked = roomBookings.Sum(b => (b.EndTime - b.StartTime).TotalHours);

            return new
            {
                RoomId = room.Id,
                RoomName = room.Name,
                TotalBookingsCount = roomBookings.Count,
                TotalHoursBooked = totalHoursBooked
            };
        });

        return new
        {
            Period = new { Start = startDate, End = endDate },
            ReportData = utilizationReport
        };
    }

    // Формує фінансовий звіт за період.
    public async Task<object> GetRevenueReportAsync(DateTime startDate, DateTime endDate)
    {
        var bookings = await _unitOfWork.Bookings.GetAllAsync();

        var filteredBookings = bookings
            .Where(b => b.StartTime >= startDate && b.EndTime <= endDate)
            .ToList();

        var totalRevenue = filteredBookings.Sum(b => b.TotalPrice);
        var totalBookingsCount = filteredBookings.Count;

        // Групуємо дохід по днях для деталізації
        var revenueByDay = filteredBookings
            .GroupBy(b => b.StartTime.Date)
            .Select(g => new
            {
                Date = g.Key.ToString("yyyy-MM-dd"),
                DailyRevenue = g.Sum(b => b.TotalPrice),
                BookingsCount = g.Count()
            })
            .OrderBy(x => x.Date)
            .ToList();

        return new
        {
            Period = new { Start = startDate, End = endDate },
            TotalBookings = totalBookingsCount,
            TotalRevenue = totalRevenue,
            DailyBreakdown = revenueByDay
        };
    }
}