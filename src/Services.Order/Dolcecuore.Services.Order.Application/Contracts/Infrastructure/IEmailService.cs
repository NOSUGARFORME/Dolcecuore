using Dolcecuore.Services.Order.Application.Models;

namespace Dolcecuore.Services.Order.Application.Contracts.Infrastructure;

public interface IEmailService
{
    Task<bool> SendEmail(Email email);
}