namespace Backend.Libs.Grpc.CustomTypes;

public partial class GuidValue
{
    public GuidValue(string value)
    {
        Value = value;
    }

    public static implicit operator Guid(GuidValue guidValue)
    {
        return Guid.Parse(guidValue.Value);
    }

    public static implicit operator GuidValue(Guid guid)
    {
        return new GuidValue(guid.ToString());
    }
    
    public static implicit operator string(GuidValue guidValue)
    {
        return Guid.Parse(guidValue.Value).ToString();
    }
}