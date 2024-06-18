using Application.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Authentication;

public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public Guid UserId => httpContextAccessor.HttpContext?.User.GetUserId() ??
                          throw new NullReferenceException("User identifier is unavailable");
}
