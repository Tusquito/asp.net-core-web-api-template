using Backend.Libs.Domain.Enums;

namespace Backend.Plugins.Domain;

public class ServiceResult<T> : ServiceResult
{
    public T? Value { get; init; }

    public ServiceResult()
    {
        Value = default;
    }

    public ServiceResult(T? value, ServiceResultType type) : base(type)
    {
        Value = value;
    }

    public ServiceResult(ServiceResultType type) : base(type)
    {
        Value = default;
    }
    
    public ServiceResult(T? value)
    {
        Value = value;
    }
}

public class ServiceResult
{
    public ServiceResultType Type { get; init; }


    public ServiceResult()
    {
        Type = ServiceResultType.Success;
    }
    public ServiceResult(ServiceResultType type)
    {
        Type = type;
    }
}