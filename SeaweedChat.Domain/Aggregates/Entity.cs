using System;

namespace SeaweedChat.Domain.Aggregates;
public class BaseEntity : IEquatable<BaseEntity>
{
    public Guid Id { get; protected set; }

    public bool Equals(BaseEntity? obj) => 
        obj != null && obj.Id == Id;
    public override bool Equals(object? obj) => 
        obj != null && GetType() == obj.GetType() && base.Equals((BaseEntity)obj);
    public override int GetHashCode() => Id.GetHashCode();
}