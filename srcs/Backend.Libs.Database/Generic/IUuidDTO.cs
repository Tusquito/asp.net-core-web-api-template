namespace Backend.Libs.Database.Generic;

public interface IUuidDto : IDto
{
    public Guid Id { get; set; }
}