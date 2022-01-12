using System;

namespace Backend.Domain;

public interface IUuidDto : IDto
{
    public Guid Id { get; set; }
}