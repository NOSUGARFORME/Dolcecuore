using FluentValidation;

namespace Dolcecuore.Services.Order.Application.Features.Orders.Commands.CheckoutOrder;

public class CheckoutOrderCommandValidator : AbstractValidator<CheckoutOrderCommand>
{
    public CheckoutOrderCommandValidator()
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