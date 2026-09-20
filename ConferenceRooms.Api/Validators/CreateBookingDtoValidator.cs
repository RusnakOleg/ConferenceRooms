using ConferenceRooms.Api.DTOs.BookingDtos;
using FluentValidation;

namespace ConferenceRooms.Api.Validators;
/// <summary>
/// Валідатор для перевірки вхідних даних при створенні нового бронювання конференц-залу.
/// </summary>
public class CreateBookingDtoValidator : AbstractValidator<CreateBookingDto>
{
    public CreateBookingDtoValidator()
    {
        RuleFor(x => x.RoomId)
            .GreaterThan(0).WithMessage("ID залу має бути коректним.");

        RuleFor(x => x.StartTime)
            .NotEmpty().WithMessage("Час початку бронювання є обов'язковим.")
            .GreaterThan(DateTime.UtcNow).WithMessage("Не можна бронювати зал у минулому часі.");

        RuleFor(x => x.DurationHours)
            .GreaterThan(0).WithMessage("Тривалість оренди повинна бути більшою за 0 годин.")
            .LessThanOrEqualTo(24).WithMessage("Максимальна тривалість одного бронювання — 24 години.");
    }
}