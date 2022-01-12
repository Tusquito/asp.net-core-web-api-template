using System;

namespace Backend.Api.Database.Generic;

public interface IUuidEntity : IEntity
{
    public Guid Id { get; set; }
}