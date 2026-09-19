namespace ConferenceRooms.DAL.Entities;
/// <summary>
/// Сутність додаткової послуги (наприклад, проєктор, Wi-Fi..), яку можна додати до оренди залу.
/// </summary>
public class Service
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }

    // Зв'язок "багато-до-багато": Одна і та сама послуга (наприклад, "Wi-Fi" чи "Проєктор") 
    // може бути доступна в багатьох різних залах
    public ICollection<RoomService> RoomServices { get; set; } = new List<RoomService>();
}