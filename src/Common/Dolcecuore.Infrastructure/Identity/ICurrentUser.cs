namespace Dolcecuore.Infrastructure.Identity;

public interface ICurrentUser
{
    Guid UserId { get; }
    bool IsAuthenticated { get; }
}