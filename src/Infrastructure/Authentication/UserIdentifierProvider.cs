using System.Security.Claims;
using Application.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Authentication;

public class UserIdentifierProvider : IUserIdentifierProvider
{
    public UserIdentifierProvider(IHttpContextAccessor httpContextAccessor)
    {
        string userIdClaim = httpContextAccessor.HttpContext?.User?.Claims
                                 .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value
                             ?? throw new ArgumentException("The user identifier claim is required.", nameof(httpContextAccessor));

        UserId = new Guid(userIdClaim);
    }
    public Guid UserId { get; }
}