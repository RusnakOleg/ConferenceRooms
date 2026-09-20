using ConferenceRooms.Api.Middleware;
using ConferenceRooms.BLL.Helpers;
using ConferenceRooms.BLL.Services;
using ConferenceRooms.BLL.Services.Interfaces;
using ConferenceRooms.DAL.Context;
using ConferenceRooms.DAL.Repositories;
using ConferenceRooms.DAL.Repositories.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Додавання сервісів до контейнера
builder.Services.AddControllers(); 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Реєстрація AppDbContext з SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Реєстрація Unit Of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Реєстрація сервісів
builder.Services.AddScoped<PricingCalculator>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IExtraServiceService, ExtraServiceService>();
builder.Services.AddScoped<IReportService, ReportService>();

// Реєстрація AutoMapper
builder.Services.AddAutoMapper(config => { }, AppDomain.CurrentDomain.GetAssemblies());

// Реєстрація валідаторів та увімкнення автоматичної валідації для API
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddFluentValidationAutoValidation();

var app = builder.Build();

// Автоматичне створення бази даних та заповнення початкових даних
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.Migrate(); // Застосовує міграції автоматично
        DbInitializer.Seed(context);  // Заповнює початкові дані
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Сталася помилка під час міграції або ініціалізації бази даних.");
    }
}

// Налаштування конвеєра HTTP-запитів

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();