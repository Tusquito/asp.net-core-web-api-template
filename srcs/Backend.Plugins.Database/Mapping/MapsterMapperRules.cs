using Backend.Libs.Database.Account;
using Backend.Libs.Grpc.Account;
using Mapster;

namespace Backend.Plugins.Database.Mapping;

public static class MapsterMapperRules
{
    public static void InitMappingRules()
    {
        TypeAdapterConfig<GrpcAccountDTO, AccountDTO>
            .NewConfig()
            .Map(dest => dest.Id, src => (Guid)src.Id);
    }
}