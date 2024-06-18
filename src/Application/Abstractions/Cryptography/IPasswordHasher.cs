using Domain.Users;
using Domain.Users.ValueObjects;

namespace Application.Abstractions.Cryptography;

public interface IPasswordHasher
{
    string HashPassword(Password password);
}