using Foundatio.Serializer;
using ProtoBuf;

namespace Backend.Libs.Redis.Serializers;

public class ProtoSerializer : ISerializer
{
    public object Deserialize(Stream data, Type objectType)
    {
        return Serializer.Deserialize(objectType, data);
    }

    public void Serialize(object value, Stream output)
    {
        Serializer.Serialize(output, value);
    }
}