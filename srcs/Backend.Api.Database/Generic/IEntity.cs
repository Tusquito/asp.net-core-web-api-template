using System;

namespace Backend.Api.Database.Generic;

public interface IEntity
{
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
}