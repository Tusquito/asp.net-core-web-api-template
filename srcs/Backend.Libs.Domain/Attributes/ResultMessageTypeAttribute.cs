using System;
using Backend.Libs.Domain.Enums;

namespace Backend.Libs.Domain.Attributes;

public class ResultMessageTypeAttribute : Attribute
{
    public ResultType Type { get; init; }
    public ResultMessageTypeAttribute(ResultType resultType)
    {
        Type = resultType;
    }
}