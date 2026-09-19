using ConferenceRooms.DAL.Entities;

namespace ConferenceRooms.DAL.Context;
/// <summary>
/// Ініціалізатор бази даних (Data Seeder). 
/// Відповідає за автоматичне заповнення бази початковими даними (зали, послуги та їх зв'язки) під час першого запуску застосунку.
/// </summary>
public static class DbInitializer
{
    public static void Seed(AppDbContext context)
    {
        // Перевіряємо, чи є вже дані в базі, щоб не дублювати при кожному запуску
        if (context.Rooms.Any())
        {
            return; // База вже заповнена
        }

        // Створення базових послуг
        var projector = new Service { Name = "Проєктор", Price = 500m };
        var wifi = new Service { Name = "Wi-Fi", Price = 300m };
        var sound = new Service { Name = "Звук", Price = 700m };

        context.Services.AddRange(projector, wifi, sound);

        // Створення конференц-залів
        var roomA = new Room { Name = "Зал А", Capacity = 50, BasePricePerHour = 2000m };
        var roomB = new Room { Name = "Зал B", Capacity = 100, BasePricePerHour = 3500m };
        var roomC = new Room { Name = "Зал C", Capacity = 30, BasePricePerHour = 1500m };

        context.Rooms.AddRange(roomA, roomB, roomC);
        
        context.SaveChanges(); // Зберігаємо, щоб отримати ID для зв'язків

        // Зв'язок залів із послугами (усім залам додаємо доступні послуги)
        var rooms = new[] { roomA, roomB, roomC };
        var services = new[] { projector, wifi, sound };

        foreach (var room in rooms)
        {
            foreach (var service in services)
            {
                context.RoomServices.Add(new RoomService
                {
                    RoomId = room.Id,
                    ServiceId = service.Id
                });
            }
        }

        // Остаточне збереження зв'язків у базі даних
        context.SaveChanges();
    }
}