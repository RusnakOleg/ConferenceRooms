using ConferenceRooms.BLL.Services.Interfaces;
using ConferenceRooms.DAL.Entities;
using ConferenceRooms.DAL.Repositories.Interfaces;

namespace ConferenceRooms.BLL.Services;

/// <summary>
/// Сервіс для управління та отримання інформації про додаткові послуги.
/// </summary>
public class ExtraServiceService : IExtraServiceService
{
    private readonly IUnitOfWork _unitOfWork;

    public ExtraServiceService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // Отримує список усіх доступних додаткових послуг (наприклад, проєктор, Wi-Fi, звук).
    public async Task<IEnumerable<Service>> GetAllServicesAsync()
    {
        return await _unitOfWork.Services.GetAllAsync();
    }

    // Отримує додаткову послугу за її унікальним ідентифікатором.
    public async Task<Service?> GetServiceByIdAsync(int id)
    {
        return await _unitOfWork.Services.GetByIdAsync(id);
    }
}