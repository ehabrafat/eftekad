using System.Security.Claims;
using Eftekad.Shared.Abstractions;
using Eftekad.Utils;

namespace Eftekad.Shared.Infrastructure;

public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    public string Id => httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                        ?? throw new UnauthorizedAccessException(ErrorMessage.UnauthenticatedUser);
}