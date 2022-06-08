using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Dolcecuore.Infrastructure.Identity;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _context;

    public CurrentUser(IHttpContextAccessor context)
    {
        _context = context;
    }

    public Guid UserId
    {
        get
        {
            var userId = _context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                     _context.HttpContext.User.FindFirst("sub")?.Value;
            return Guid.Parse(userId);
        }
    }

    public bool IsAuthenticated => _context.HttpContext.User.Identity is {IsAuthenticated: true};
}