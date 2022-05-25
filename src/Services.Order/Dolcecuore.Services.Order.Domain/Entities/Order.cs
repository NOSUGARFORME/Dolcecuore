using Dolcecuore.Services.Order.Domain.Common;

namespace Dolcecuore.Services.Order.Domain.Entities;

public class Order : EntityBase
{
    public string Username { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public string AddressLine { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    
    public int PaymentMethod { get; set; }
}