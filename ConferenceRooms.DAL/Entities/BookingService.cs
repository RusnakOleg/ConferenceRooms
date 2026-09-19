namespace ConferenceRooms.DAL.Entities;
/// <summary>
/// Проміжна сутність для реалізації зв'язку "багато-до-багато" між бронюваннями та послугами
/// (які послуги прив'язані до конкретного замовлення).
/// </summary>
public class BookingService
{
    public int BookingId { get; set; }
    public Booking Booking { get; set; } = null!;

    public int ServiceId { get; set; }
    public Service Service { get; set; } = null!;
}