namespace Wakett.Infrastructure.Persistence;

public abstract class Entity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
}