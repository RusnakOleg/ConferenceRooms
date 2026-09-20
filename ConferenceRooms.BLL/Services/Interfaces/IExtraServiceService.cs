using ConferenceRooms.DAL.Entities;

namespace ConferenceRooms.BLL.Services.Interfaces;
/// <summary>
/// Інтерфейс для сервісу додаткових послуг
/// </summary>
public interface IExtraServiceService
{
    Task<IEnumerable<Service>> GetAllServicesAsync();
    Task<Service?> GetServiceByIdAsync(int id);
}