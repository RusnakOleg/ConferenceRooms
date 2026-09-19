namespace ConferenceRooms.DAL.Entities;
/// <summary>
/// Сутність конференц-залу. Містить базові характеристики залу: назву, місткість та базову вартість оренди за годину.
/// </summary>
public class Room
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public decimal BasePricePerHour { get; set; }
    
    // Зв'язок "багато-до-багато": Один зал може мати багато доступних послуг (проєктор, Wi-Fi тощо)
    public ICollection<RoomService> RoomServices { get; set; } = new List<RoomService>();
    // Зв'язок "один-до-багатьох": Один і той самий зал може мати багато бронювань у різний час
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}