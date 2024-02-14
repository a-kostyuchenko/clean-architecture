namespace Domain.Users;

public interface IPasswordHashChecker
{
    bool HashesMatch(string passwordHash, string providedPassword);
}