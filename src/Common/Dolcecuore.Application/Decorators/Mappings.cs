using System.Reflection;
using Dolcecuore.Application.Common.Commands;
using Dolcecuore.Application.Common.Query;
using Dolcecuore.CrossCuttingConcerns.ExtensionsMethods;

namespace Dolcecuore.Application.Decorators;

internal static class Mappings
{
    static Mappings()
    {
        var decorators = Assembly.GetExecutingAssembly().GetTypes();
        foreach (var type in decorators)
        {
            if (type.HasInterface(typeof(ICommandHandler<>)))
            {
                var decoratorAttribute = (MappingAttribute)type.GetCustomAttributes(false).FirstOrDefault(x => x.GetType() == typeof(MappingAttribute));

                if (decoratorAttribute != null)
                {
                    AttributeToCommandHandler[decoratorAttribute.Type] = type;
                }
            }
            else if (type.HasInterface(typeof(IQueryHandler<,>)))
            {
                var decoratorAttribute = (MappingAttribute)type.GetCustomAttributes(false).FirstOrDefault(x => x.GetType() == typeof(MappingAttribute));

                if (decoratorAttribute != null)
                {
                    AttributeToQueryHandler[decoratorAttribute.Type] = type;
                }
            }
        }
    }

    public static readonly Dictionary<Type, Type> AttributeToCommandHandler = new ();

    public static readonly Dictionary<Type, Type> AttributeToQueryHandler = new ();
}

[AttributeUsage(AttributeTargets.Class)]
public sealed class MappingAttribute : Attribute
{
    public Type Type { get; set; }
}