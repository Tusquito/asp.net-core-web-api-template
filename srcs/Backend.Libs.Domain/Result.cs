using System;
using System.Linq;
using System.Reflection;
using Backend.Libs.Domain.Attributes;
using Backend.Libs.Domain.Enums;

namespace Backend.Libs.Domain;

public class Result<T> : Result
{
    public T? Value { get; init; }

    private Result(T value)
    {
        Value = value;
    }

    private Result(T value, ResultType type, ResultMessageKey message) : base(type, message)
    {
        Value = value;
    }
    public Result(ResultType type, ResultMessageKey message) : base(type, message) { }

    public static Result<T> Ok(T value, ResultMessageKey message = ResultMessageKey.Ok) =>
        new(value, ResultType.Ok, message);
    public static Result<T> Created(T value, ResultMessageKey message = ResultMessageKey.Created) =>
        new(value, ResultType.Created, message);
    public new static Result<T> Failed(ResultMessageKey message = ResultMessageKey.InternalServerError) => new(ResultType.Failure, message);
    public new static Result<T> Failed(ResultType type, ResultMessageKey message) => new(type, message);
    public new static Result<T> NotFound(ResultMessageKey message = ResultMessageKey.NotFound) =>
        new(ResultType.NotFound, message);
    public new static Result<T> BadRequest(ResultMessageKey message = ResultMessageKey.BadRequest) =>
        new(ResultType.BadRequest, message);
    public new static Result<T> Maintenance(ResultMessageKey message = ResultMessageKey.ServiceUnavailable) =>
        new(ResultType.Maintenance, message);

}

public class Result
{
    public ResultType Type { get; init; } = ResultType.Ok;
    public ResultMessageKey Message { get; init; } = ResultMessageKey.Ok;
    public bool Successful => (ResultType.Success & Type) == Type;
    
    protected Result(){}

    protected Result(ResultType type, ResultMessageKey message)
    {
        Type messageKeyType = typeof(ResultMessageKey);
        MemberInfo[] memberInfos = messageKeyType.GetMember(message.ToString());

        if (!memberInfos.Any() || memberInfos.Length > 1)
        {
            throw new ArgumentException("Can't retrieve ResultMessageKey member infos");
        }

        MemberInfo currentMsgMbrInfo = memberInfos.FirstOrDefault(m => m.DeclaringType == messageKeyType) ??
                                       throw new ArgumentException();

        ResultType? resultType = currentMsgMbrInfo.GetCustomAttribute<ResultMessageTypeAttribute>()?.Type;
        
        if (!resultType.HasValue || !type.HasFlag(resultType.Value) || resultType.Value != type)
        {
            throw new ArgumentException("Message type does not match result type");
        }
        
        Type = type;
        Message = message;
    }

    public static Result Success() => new();
    public static Result Ok(ResultMessageKey message) => new(ResultType.Ok, message);
    public static Result NoContent(ResultMessageKey message = ResultMessageKey.NoContent) =>
        new(ResultType.NoContent, message);
    public static Result Failure(ResultMessageKey message = ResultMessageKey.InternalServerError) =>
        new(ResultType.Failure, message);
    public static Result Failure(ResultType type, ResultMessageKey message) => new(type, message);
    public static Result NotFound(ResultMessageKey message = ResultMessageKey.NotFound) =>
        new(ResultType.NotFound, message);
    public static Result BadRequest(ResultMessageKey message = ResultMessageKey.BadRequest) =>
        new(ResultType.BadRequest, message);
    public static Result Maintenance(ResultMessageKey message = ResultMessageKey.ServiceUnavailable) =>
        new(ResultType.Maintenance, message);
}