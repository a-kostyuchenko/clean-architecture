namespace SharedKernel;

public abstract class ApplicationPermission(int id, string name, string description) 
    : Enumeration<ApplicationPermission>(id, name)
{
    public string Description { get; private init; } = description;

    public abstract string Key { get; }
    
    public override string ToString()
    {
        return $"{Key}_{Name}";
    }

    public static implicit operator string(ApplicationPermission permission) => 
        $"{permission.Key}_{permission.Name}";
}
