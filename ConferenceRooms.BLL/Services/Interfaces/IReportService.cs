namespace ConferenceRooms.BLL.Services.Interfaces;
/// <summary>
/// Інтерфейс сервісу для генерації бізнес-звітів та аналітики.
/// </summary>
public interface IReportService
{
    Task<object> GetRoomUtilizationReportAsync(DateTime startDate, DateTime endDate);
    Task<object> GetRevenueReportAsync(DateTime startDate, DateTime endDate);
}