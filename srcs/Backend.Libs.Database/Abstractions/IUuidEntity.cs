namespace Backend.Libs.Database.Abstractions;

public interface IUuidEntity : IEntity
{
    public Guid Id { get; set; }
}