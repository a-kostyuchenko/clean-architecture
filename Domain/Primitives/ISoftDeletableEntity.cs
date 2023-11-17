namespace Domain.Primitives;

public interface ISoftDeletableEntity
{
    DateTime? DeletedOnUtc { get; set; }
    bool Deleted { get; set; }
}