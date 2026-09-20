#  Conference Room Booking API

Надійний, масштабований RESTful API для управління конференц-залами, бронюваннями та додатковими послугами. Проєкт розроблений за принципами **3-tier Architecture**.

---

##  Технологічний стек

* **Platform:** .NET (ASP.NET Core Web API)
* **Architecture:** 3-tier Architecture (API, BLL, DAL)
* **Database & ORM:** Entity Framework Core, MS SQL Server
* **Design Patterns:** Unit of Work, Repository Pattern
* **Validation:** FluentValidation (з автоматичною валідацією)
* **Mappers:** AutoMapper
* **Documentation:** Swagger 

---

##  Архітектура проєкту

Проєкт розділений на ізольовані шари для забезпечення максимальної масштабованості та тестованості:

1. **`ConferenceRooms.Api` (Presentation Layer):** Тонкі контролери, глобальний обробник помилок (`Middleware`), валідатори та конфігурація конвеєра.
2. **`ConferenceRooms.BLL` (Business Logic Layer):** Бізнес-правила, сервіси (у т.ч. сервіси звітів та розрахунків).
3. **`ConferenceRooms.DAL` (Data Access Layer):** Сутності бази даних, контекст EF Core, репозиторії та патерн *Unit of Work*.

---

##  Архітектурні особливості та чистота коду

* **Глобальна обробка помилок (Global Exception Handling Middleware):** Усі винятки (`KeyNotFoundException`, `ArgumentException` тощо) перехоплюються на рівні middleware та конвертуються у стандартизований JSON-формат відповіді без захаращення коду контролерів блоками `try-catch`.
* **Автоматична валідація (FluentValidation):** Перевірка вхідних даних винесена у спеціальні класи-валідатори. Запити з невалідними даними відхиляються на вході зі статусом `400 Bad Request`.
* **Тонкі контролери (Thin Controllers):** Контролери відповідають виключно за маршрутизацію та виклик відповідних сервісів.

---

##  Бізнес-звіти та аналітика

API містить окремий модуль аналітики для власників бізнесу (`ReportsController`):

* **Звіт про завантаженість залів (`GET /api/reports/utilization`):** 
  Вираховує загальну кількість заброньованих годин та сеансів для кожного залу за вказаний період (`startDate`, `endDate`).
* **Фінансовий звіт (`GET /api/reports/revenue`):** 
  Надає загальний дохід компанії, кількість бронювань та деталізовану розбивку по днях (`DailyBreakdown`).

---

##  Початок роботи та запуск

1. Клонуйте репозиторій
2. Налаштуйте рядок підключення до бази даних (`ConnectionStrings`) у файлі `appsettings.json` проєкту `API`.
3. Застосуйте міграції бази даних:
   ```bash
   dotnet ef database update --project ConferenceRooms.DAL --startup-project ConferenceRooms.Api
4. Запустіть проєкт:
   ```bash
   dotnet run --project ConferenceRooms.Api
5. Перейдіть у браузері за адресою Swagger-документації:
   ```bash
   https://localhost:ВАШ ПОРТ/swagger/index.html
