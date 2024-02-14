using Domain.Users;

namespace Application.Abstractions.Cryptography;

public interface IPasswordHasher
{
    string HashPassword(Password password);
}