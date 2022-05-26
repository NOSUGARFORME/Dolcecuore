using System.Net;
using Dolcecuore.Services.Order.Application.Contracts.Infrastructure;
using Dolcecuore.Services.Order.Application.Models;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Dolcecuore.Services.Order.Infrastructure.Services.Mail;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<bool> SendEmail(Email email)
    {
        var client = new SendGridClient("api-key");
        var subject = email.Subject;
        var to = new EmailAddress(email.To);
        var emailBody = email.Body;

        var from = new EmailAddress
        {
            Email = "email",
            Name = "name"
        };

        var sendMessage = MailHelper.CreateSingleEmail(from, to, subject, emailBody, emailBody);
        var response = await client.SendEmailAsync(sendMessage);
        
        _logger.LogInformation("Sending email.");
        
        if (response.StatusCode is HttpStatusCode.Accepted or HttpStatusCode.OK)
            return true;
        
        _logger.LogError("Sending an email failed.");
        return false;
    }
}
