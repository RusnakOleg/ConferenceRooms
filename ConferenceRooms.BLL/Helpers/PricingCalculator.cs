namespace ConferenceRooms.BLL.Helpers;

/// <summary>
/// Сервіс для розрахунку вартості оренди конференц-залу з урахуванням годинних знижок та націнок.
/// </summary>
public class PricingCalculator
{
    public decimal CalculateTotalPrice(decimal basePricePerHour, DateTime startTime, DateTime endTime, IEnumerable<decimal> servicePrices)
    {
        decimal totalRoomPrice = 0m;
        var currentHour = startTime;

        // Погодинний розрахунок для коректного застосування знижок і націнок, якщо бронювання перетинає різні часові зони
        while (currentHour < endTime)
        {
            decimal hourlyRate = basePricePerHour;
            int hour = currentHour.Hour;

            // Важливо: спочатку перевіряємо специфічні часові зони (пікові години), 
            // щоб вони не перекривалися загальним стандартним діапазоном.
            
            if (hour >= 12 && hour < 14)
            {
                // Пікові години (з 12:00 до 14:00): націнка 15%
                hourlyRate = basePricePerHour * 1.15m;
            }
            else if (hour >= 6 && hour < 9)
            {
                // Ранкові години (з 06:00 до 09:00): знижка 10%
                hourlyRate = basePricePerHour * 0.90m;
            }
            else if (hour >= 18 && hour < 23)
            {
                // Вечірні години (з 18:00 до 23:00): знижка 20%
                hourlyRate = basePricePerHour * 0.80m;
            }
            else if (hour >= 9 && hour < 18)
            {
                // Стандартні години (з 09:00 до 18:00): базова вартість залу (окрім пікових 12-14)
                hourlyRate = basePricePerHour;
            }
            else
            {
                // Усі інші години — залишаємо базову ціну 
                hourlyRate = basePricePerHour;
            }

            totalRoomPrice += hourlyRate;
            currentHour = currentHour.AddHours(1);
        }

        // Додаємо суму всіх обраних додаткових послуг
        decimal servicesTotal = servicePrices.Sum();

        return totalRoomPrice + servicesTotal;
    }
}