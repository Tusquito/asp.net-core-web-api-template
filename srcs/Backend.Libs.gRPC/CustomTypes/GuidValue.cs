using Backend.Libs.gRPC.Extensions;

namespace Backend.Libs.gRPC.CustomTypes;

public partial class GuidValue
{
    public GuidValue(string value)
    {
        Value = value;
    }

    public static implicit operator Guid(GuidValue guidValue)
    {
        return guidValue.ToGuid();
    }

    public static implicit operator GuidValue(Guid guid)
    {
        return guid.ToGuidValue();
    }
    
    public static implicit operator string(GuidValue guidValue)
    {
        return Guid.TryParse((string?)guidValue.Value, out Guid result) ? result.ToString() : string.Empty;
    }
}