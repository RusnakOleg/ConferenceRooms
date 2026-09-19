namespace ConferenceRooms.DAL.Entities;
/// <summary>
/// Проміжна сутність для реалізації зв'язку "багато-до-багато" між залами та послугами
/// (які послуги доступні в якому залі).
/// </summary>
public class RoomService
{
    public int RoomId { get; set; }
    public Room Room { get; set; } = null!;

    public int ServiceId { get; set; }
    public Service Service { get; set; } = null!;
}