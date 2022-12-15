namespace SeaweedChat.Domain.Aggregates;
public class Entity : IEquatable<Entity>
{
    public Guid Id { get; protected set; } = new Guid();

    public bool Equals(Entity? obj) => 
        obj != null && obj.Id == Id;
    public override bool Equals(object? obj) => 
        obj != null && GetType() == obj.GetType() && base.Equals((Entity)obj);
    public override int GetHashCode() => Id.GetHashCode();
}