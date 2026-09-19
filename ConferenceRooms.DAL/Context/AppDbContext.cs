using ConferenceRooms.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConferenceRooms.DAL.Context;
/// <summary>
/// Головний контекст бази даних (Entity Framework Core). 
/// Відповідає за підключення до БД, управління таблицями та конфігурацію зв'язків між сутностями.
/// </summary>
public class AppDbContext : DbContext
{
    // Таблиця конференц-залів
    public DbSet<Room> Rooms => Set<Room>();
    // Таблиця додаткових послуг
    public DbSet<Service> Services => Set<Service>();
    // Проміжна таблиця зв'язку багато-до-багато між залами та послугами
    public DbSet<RoomService> RoomServices => Set<RoomService>();
    // Таблиця бронювань залів
    public DbSet<Booking> Bookings => Set<Booking>();
    // Проміжна таблиця зв'язку багато-до-багато між конкретними бронюваннями та обраними послугами
    public DbSet<BookingService> BookingServices => Set<BookingService>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Налаштування складеного ключа для RoomService
        modelBuilder.Entity<RoomService>()
            .HasKey(rs => new { rs.RoomId, rs.ServiceId });

        modelBuilder.Entity<RoomService>()
            .HasOne(rs => rs.Room)
            .WithMany(r => r.RoomServices)
            .HasForeignKey(rs => rs.RoomId);

        modelBuilder.Entity<RoomService>()
            .HasOne(rs => rs.Service)
            .WithMany(s => s.RoomServices)
            .HasForeignKey(rs => rs.ServiceId);

        // Налаштування складеного ключа для BookingService
        modelBuilder.Entity<BookingService>()
            .HasKey(bs => new { bs.BookingId, bs.ServiceId });

        modelBuilder.Entity<BookingService>()
            .HasOne(bs => bs.Booking)
            .WithMany(b => b.BookingServices)
            .HasForeignKey(bs => bs.BookingId);

        modelBuilder.Entity<BookingService>()
            .HasOne(bs => bs.Service)
            .WithMany()
            .HasForeignKey(bs => bs.ServiceId);

        //Налаштування точності та типу даних для грошових полів (decimal), 
        // щоб уникнути втрати точності чи помилок при збереженні в SQL Server
        
        // Базова вартість години оренди залу
        modelBuilder.Entity<Room>()
            .Property(r => r.BasePricePerHour)
            .HasColumnType("decimal(18,2)");

        // Вартість окремої послуги
        modelBuilder.Entity<Service>()
            .Property(s => s.Price)
            .HasColumnType("decimal(18,2)");

        // Загальна вартість бронювання
        modelBuilder.Entity<Booking>()
            .Property(b => b.TotalPrice)
            .HasColumnType("decimal(18,2)");
    }
}