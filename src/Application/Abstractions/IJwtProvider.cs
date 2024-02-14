using Domain.Users;

namespace Application.Abstractions;

public interface IJwtProvider
{
    Task<string> GenerateAsync(User user);
}