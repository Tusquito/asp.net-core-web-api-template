namespace Backend.Libs.Database.Generic;

public interface IEntity
{
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
}