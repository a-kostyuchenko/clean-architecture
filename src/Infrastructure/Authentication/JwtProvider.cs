using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Abstractions;
using Domain.Users;
using Infrastructure.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Authentication;

internal sealed class JwtProvider(IOptions<JwtOptions> jwtOptions, IPermissionService permissionService) 
    : IJwtProvider
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    public async Task<string> GenerateAsync(User user)
    {
        var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new (JwtRegisteredClaimNames.Email, user.Email)
        };
        
        HashSet<string> permissions = await permissionService
            .GetPermissionsAsync(user.Id);

        claims.AddRange(permissions.Select(permission =>
            new Claim(CustomClaims.Permissions, permission)));

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            claims,
            null,
            DateTime.UtcNow.AddHours(_jwtOptions.LifeTime),
            signingCredentials);

        string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenValue;
    }
}