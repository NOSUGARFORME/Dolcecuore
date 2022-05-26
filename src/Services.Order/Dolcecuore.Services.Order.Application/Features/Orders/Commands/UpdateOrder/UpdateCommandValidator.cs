using FluentValidation;

namespace Dolcecuore.Services.Order.Application.Features.Orders.Commands.UpdateOrder;

public class UpdateCommandValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateCommandValidator()
    {
        RuleFor(c => c.Username)
            .NotEmpty().WithMessage("{Username} is required.");

        RuleFor(c => c.EmailAddress)
            .NotEmpty().WithMessage("{EmailAddress} is required.");

        RuleFor(c => c.Price)
            .NotEmpty().WithMessage("{Price} is required.")
            .GreaterThan(0).WithMessage("{Price} should be greater than zero.");
    }
}