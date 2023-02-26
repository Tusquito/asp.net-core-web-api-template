namespace Backend.Libs.Database.Abstractions;

public interface IUuidDto : IDto
{
    public Guid Id { get; set; }
}