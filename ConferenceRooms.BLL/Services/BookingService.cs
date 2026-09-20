using ConferenceRooms.BLL.Helpers;
using ConferenceRooms.BLL.Services.Interfaces;
using ConferenceRooms.DAL.Entities;
using ConferenceRooms.DAL.Repositories.Interfaces;

namespace ConferenceRooms.BLL.Services;

/// <summary>
/// Сервіс для створення бронювань, перевірки конфліктів за розкладом та розрахунку фінальної вартості.
/// </summary>
public class BookingService : IBookingService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly PricingCalculator _pricingCalculator;

    public BookingService(IUnitOfWork unitOfWork, PricingCalculator pricingCalculator)
    {
        _unitOfWork = unitOfWork;
        _pricingCalculator = pricingCalculator;
    }

    // Отримання всіх бронювань разом із деталями залу та послуг
    public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
    {
        return await _unitOfWork.Bookings.GetAllWithDetailsAsync();
    }

    // Створення нового бронювання
    public async Task<Booking> CreateBookingAsync(int roomId, DateTime startTime, DateTime endTime, List<int> serviceIds)
    {
        // 1. Валідація часу
        if (startTime >= endTime)
        {
            throw new ArgumentException("Час початку бронювання має бути меншим за час закінчення.");
        }

        if (startTime < DateTime.Now)
        {
            throw new ArgumentException("Неможливо створити бронювання в минулому часі.");
        }

        // 2. Перевірка існування залу
        var room = await _unitOfWork.Rooms.GetByIdAsync(roomId);
        if (room == null)
        {
            throw new KeyNotFoundException("Конференц-зал із таким ID не знайдено.");
        }

        // 3. Перевірка чи зал уже зайнятий на цей час
        bool isBooked = await _unitOfWork.Bookings.IsRoomBookedAsync(roomId, startTime, endTime);
        if (isBooked)
        {
            throw new InvalidOperationException("Цей зал уже заброньований на обраний проміжок часу.");
        }

        // 4. Отримання цін обраних послуг
        var servicePrices = new List<decimal>();
        var validServices = new List<Service>();

        foreach (var serviceId in serviceIds)
        {
            var service = await _unitOfWork.Services.GetByIdAsync(serviceId);
            if (service != null)
            {
                servicePrices.Add(service.Price);
                validServices.Add(service);
            }
        }

        // 5. Розрахунок загальної вартості через калькулятор з урахуванням годинних знижок/націнок
        decimal totalPrice = _pricingCalculator.CalculateTotalPrice(room.BasePricePerHour, startTime, endTime, servicePrices);

        // 6. Створення об'єкта бронювання
        var booking = new Booking
        {
            RoomId = roomId,
            StartTime = startTime,
            EndTime = endTime,
            TotalPrice = totalPrice
        };

        await _unitOfWork.Bookings.AddAsync(booking);
        await _unitOfWork.CompleteAsync(); // Зберігаємо, щоб згенерувався Booking.Id

        // 7. Додавання зв'язків із послугами через репозиторій бронювань
        foreach (var service in validServices)
        {
            var bookingServiceLink = new ConferenceRooms.DAL.Entities.BookingService
            {
                BookingId = booking.Id,
                ServiceId = service.Id
            };

            await _unitOfWork.Bookings.AddBookingServiceAsync(bookingServiceLink);
        }

        await _unitOfWork.CompleteAsync(); // Фінальне збереження зв'язків

        return booking;
    }
}