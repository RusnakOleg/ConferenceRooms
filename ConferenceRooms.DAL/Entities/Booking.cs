namespace ConferenceRooms.DAL.Entities;
/// <summary>
/// Сутність бронювання. Зберігає інформацію про замовлений зал, час оренди, загальну вартість та обрані додаткові послуги.
/// </summary>
public class Booking
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    public Room Room { get; set; } = null!;

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public decimal TotalPrice { get; set; }

    // Зв'язок "багато-до-багато": Одне конкретне бронювання може включати кілька послуг (наприклад, і проєктор, і Wi-Fi)
    public ICollection<BookingService> BookingServices { get; set; } = new List<BookingService>();
}