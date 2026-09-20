using ConferenceRooms.BLL.Services.Interfaces;
using ConferenceRooms.DAL.Entities;
using ConferenceRooms.DAL.Repositories.Interfaces;

namespace ConferenceRooms.BLL.Services;

/// <summary>
/// Сервіс для бізнес-операцій із конференц-залами (пошук, фільтрація, отримання деталей).
/// </summary>
public class RoomService : IRoomService
{
    private readonly IUnitOfWork _unitOfWork;

    public RoomService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // Отримує список усіх конференц-залів
    public async Task<IEnumerable<Room>> GetAllRoomsAsync()
    {
        return await _unitOfWork.Rooms.GetAllAsync();
    }

    // Отримує детальну інформацію про конкретний зал за його ідентифікатором, 
    // включно з усіма пов'язаними додатковими послугами.
    public async Task<Room?> GetRoomByIdAsync(int id)
    {
        return await _unitOfWork.Rooms.GetRoomWithServicesAsync(id);
    }

    // Здійснює пошук вільних конференц-залів на заданий часовий інтервал 
    // з урахуванням мінімальної необхідної місткості.
    public async Task<IEnumerable<Room>> GetAvailableRoomsAsync(DateTime startTime, DateTime endTime, int requiredCapacity)
    {
        // Перевірка валідності часового інтервалу
        if (startTime >= endTime)
        {
            throw new ArgumentException("Час початку бронювання має бути ранішим за час закінчення.");
        }

        return await _unitOfWork.Rooms.GetAvailableRoomsAsync(startTime, endTime, requiredCapacity);
    }
}