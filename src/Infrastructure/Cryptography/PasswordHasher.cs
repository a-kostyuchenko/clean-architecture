using System.Security.Cryptography;
using Application.Abstractions.Cryptography;
using Domain.Users;
using Domain.Users.ValueObjects;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Infrastructure.Cryptography;

public class PasswordHasher : IPasswordHasher, IPasswordHashChecker, IDisposable
{
    private const KeyDerivationPrf Prf = KeyDerivationPrf.HMACSHA256;
    private const int IterationCount = 100000;
    private const int NumberOfBytesRequested = 256 / 8;
    private const int SaltSize = 128 / 8;
    private readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();

    public string HashPassword(Password password)
    {
        ArgumentNullException.ThrowIfNull(password);

        string hashedPassword = Convert.ToBase64String(HashPassword(password.Value));

        return hashedPassword;
    }

    public bool HashesMatch(string passwordHash, string providedPassword)
    {
        ArgumentNullException.ThrowIfNull(passwordHash);
        
        ArgumentNullException.ThrowIfNull(providedPassword);

        byte[] decodedHashedPassword = Convert.FromBase64String(passwordHash);

        if (decodedHashedPassword.Length == 0)
        {
            return false;
        }

        return VerifyPasswordHash(decodedHashedPassword, providedPassword);
    }

    public void Dispose()
    {
        _rng.Dispose();
    }

    private byte[] HashPassword(string password)
    {
        byte[] salt = GetRandomSalt();

        byte[] subKey = KeyDerivation.Pbkdf2(password, salt, Prf, IterationCount, NumberOfBytesRequested);

        byte[] outputBytes = new byte[salt.Length + subKey.Length];
        
        Buffer.BlockCopy(salt, 0, outputBytes, 0, salt.Length);

        Buffer.BlockCopy(subKey, 0, outputBytes, salt.Length, subKey.Length);

        return outputBytes;
    }
    

    private byte[] GetRandomSalt()
    {
        byte[] salt = new byte[SaltSize];
        
        _rng.GetBytes(salt);

        return salt;
    }

    private static bool VerifyPasswordHash(byte[] hashedPassword, string password)
    {
        try
        {
            byte[] salt = new byte[SaltSize];
            
            Buffer.BlockCopy(hashedPassword, 0, salt, 0, salt.Length);

            int subKeyLength = hashedPassword.Length - salt.Length;

            if (subKeyLength < SaltSize)
            {
                return false;
            }

            byte[] expectedSubKey = new byte[subKeyLength];

            Buffer.BlockCopy(hashedPassword, salt.Length, expectedSubKey, 0, expectedSubKey.Length);

            byte[] actualSubKey = KeyDerivation.Pbkdf2(password, salt, Prf, IterationCount, subKeyLength);

            return ByteArraysEqual(actualSubKey, expectedSubKey);
        }
        catch
        {
            return false;
        }
    }
    
    private static bool ByteArraysEqual(byte[]? a, byte[]? b)
    {
        if (a == null && b == null)
        {
            return true;
        }

        if (a == null || b == null || a.Length != b.Length)
        {
            return false;
        }

        bool areSame = true;

        for (int i = 0; i < a.Length; i++)
        {
            areSame &= a[i] == b[i];
        }

        return areSame;
    }
}
