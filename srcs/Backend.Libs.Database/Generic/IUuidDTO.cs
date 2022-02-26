namespace Backend.Libs.Database.Generic;

public interface IUuidDTO : IDTO
{
    public Guid Id { get; set; }
}