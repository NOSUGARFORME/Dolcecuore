using Dolcecuore.Application.Common.Commands;
using Dolcecuore.Application.Common.Query;

namespace Dolcecuore.Application.Common;

internal static class Utils
{
    public static bool IsHandlerInterface(Type type)
    {
        if (!type.IsGenericType)
        {
            return false;
        }

        var typeDefinition = type.GetGenericTypeDefinition();

        return typeDefinition == typeof(ICommandHandler<>)
               || typeDefinition == typeof(IQueryHandler<,>);
    }
}