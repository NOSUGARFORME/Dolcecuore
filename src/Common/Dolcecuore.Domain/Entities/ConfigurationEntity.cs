namespace Dolcecuore.Domain.Entities;

public class ConfigurationEntity : Entity<Guid>
{
    public string Key { get; set; }
    
    public string Value { get; set; }
}