namespace Domain.Roles;

public class Permission
{
    public Guid Id { get; private init; }
    public string Key { get; private init; }
    public string Name { get; private init; }
    public string Description { get; private init; }

    private Permission()
    {
    }

    private Permission(string key, string name, string description)
    {
        Id = Guid.NewGuid();
        Key = key;
        Name = name;
        Description = description;
    }
    
    public static Permission Create(string key, string name, string description)
    {
        var permission = new Permission(key, name, description);

        return permission;
    }

    public override string ToString() => $"{Key}_{Name}";
}