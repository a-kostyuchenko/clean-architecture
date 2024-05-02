namespace SharedKernel;

public abstract class Permission(int id, string name, string description) 
    : Enumeration<Permission>(id, name)
{
    public string Description { get; private init; } = description;

    public abstract string Key { get; }
    
    public override string ToString()
    {
        return $"{Key}_{Name}";
    }

    public static implicit operator string(Permission permission) => 
        $"{permission.Key}_{permission.Name}";
}