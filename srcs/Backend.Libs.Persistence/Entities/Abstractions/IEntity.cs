namespace Backend.Libs.Persistence.Entities.Abstractions;

public interface IEntity
{
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
}