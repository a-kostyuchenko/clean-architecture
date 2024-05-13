using Domain.Roles;
using SharedKernel;

namespace Domain.Users;

public class User : Entity, IAuditable, IDeletable
{
    private string _passwordHash;
    private readonly List<Role> _roles = [];

    private User(FirstName firstName, LastName lastName, Email email, string passwordHash)
        : base(Guid.NewGuid())
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        _passwordHash = passwordHash;
    }

    private User()
    {
    }
    
    public FirstName FirstName { get; private set; }
    public LastName LastName { get; private set; }
    public string FullName => $"{FirstName} {LastName}";
    public Email Email { get; private set; }
    
    public DateTime CreatedOnUtc { get; set; }
    public DateTime? ModifiedOnUtc { get; set; }
    public DateTime? DeletedOnUtc { get; set; }
    public bool Deleted { get; set; }

    public List<Role> Roles => _roles.ToList();


    public static User Create(FirstName firstName, LastName lastName, Email email, string passwordHash)
    {
        var user = new User(firstName, lastName, email, passwordHash);
        
        return user;
    }
    
    public Result ChangePassword(string passwordHash)
    {
        if (passwordHash == _passwordHash)
        {
            return Result.Failure(UserErrors.CannotChangePassword);
        }

        _passwordHash = passwordHash;
        
        return Result.Success();
    }

    public bool VerifyPasswordHash(string password, IPasswordHashChecker checker) => 
        !string.IsNullOrWhiteSpace(password) && checker.HashesMatch(_passwordHash, password);
}