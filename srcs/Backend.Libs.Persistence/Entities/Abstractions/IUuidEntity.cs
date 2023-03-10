namespace Backend.Libs.Persistence.Entities.Abstractions;

public interface IUuidEntity : IEntity
{
    public Guid Id { get; set; }
}