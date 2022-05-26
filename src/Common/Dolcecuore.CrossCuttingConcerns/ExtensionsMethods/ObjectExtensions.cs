using System.Text.Json;

namespace Dolcecuore.CrossCuttingConcerns.ExtensionsMethods;

public static class ObjectExtensions
{
    public static string AsJsonString(this object obj)
        => JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true });
}