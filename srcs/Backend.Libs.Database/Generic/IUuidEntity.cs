namespace Backend.Libs.Database.Generic;

public interface IUuidEntity : IEntity
{
    public Guid Id { get; set; }
}