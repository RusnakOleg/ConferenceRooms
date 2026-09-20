using ConferenceRooms.Api.DTOs.RoomDtos;
using FluentValidation;

namespace ConferenceRooms.Api.Validators;
/// <summary>
/// Валідатор для перевірки вхідних даних при створенні нового конференц-залу.
/// </summary>
public class CreateRoomValidator : AbstractValidator<CreateRoomDto>
{
    public CreateRoomValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Назва конференц-залу є обов'язковою.")
            .MaximumLength(100).WithMessage("Назва не повинна перевищувати 100 символів.");

        RuleFor(x => x.Capacity)
            .GreaterThan(0).WithMessage("Місткість залу має бути більшою за 0.");

        RuleFor(x => x.BasePricePerHour)
            .GreaterThan(0).WithMessage("Базова вартість за годину має бути більшою за 0.");
    }
}