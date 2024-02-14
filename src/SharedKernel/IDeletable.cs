namespace SharedKernel;

public interface IDeletable
{
    DateTime? DeletedOnUtc { get; set; }
    bool Deleted { get; set; }
}