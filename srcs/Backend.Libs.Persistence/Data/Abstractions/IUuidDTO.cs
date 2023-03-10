namespace Backend.Libs.Persistence.Data.Abstractions;

public interface IUuidDto : IDto
{
    public Guid Id { get; set; }
}