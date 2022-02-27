using Backend.Libs.Grpc.CustomTypes;

namespace Backend.Libs.gRPC.Extensions;

public static class ConversionExtensions
{
    public static Guid ToGuid(this GuidValue guidValue)
    {
        return Guid.TryParse(string.IsNullOrEmpty(guidValue.Value) ? Guid.Empty.ToString() : guidValue.Value, out Guid result) ? result : Guid.Empty;
    }

    public static GuidValue ToGuidValue(this Guid guid)
    {
        return new GuidValue(guid.ToString());
    }
    
}