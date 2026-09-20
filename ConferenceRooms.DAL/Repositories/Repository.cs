using ConferenceRooms.DAL.Context;
using ConferenceRooms.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ConferenceRooms.DAL.Repositories;
/// <summary>
/// Загальна реалізація універсального репозиторію на основі Entity Framework Core.
/// </summary>
public class Repository<T> : IRepository<T> where T : class
{
    protected readonly AppDbContext Context;
    protected readonly DbSet<T> DbSet;

    public Repository(AppDbContext context)
    {
        Context = context;
        DbSet = context.Set<T>();
    }

    // Отримання всіх записів сутності з бази даних
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await DbSet.ToListAsync();
    }

    // Пошук конкретного запису за первинним ключем (ID)
    public async Task<T?> GetByIdAsync(int id)
    {
        return await DbSet.FindAsync(id);
    }

    // Додавання нового запису в контекст (збереження відбудеться через Unit of Work)
    public async Task AddAsync(T entity)
    {
        await DbSet.AddAsync(entity);
    }

    // Оновлення існуючого запису
    public void Update(T entity)
    {
        DbSet.Update(entity);
    }

    // Видалення запису
    public void Delete(T entity)
    {
        DbSet.Remove(entity);
    }
}