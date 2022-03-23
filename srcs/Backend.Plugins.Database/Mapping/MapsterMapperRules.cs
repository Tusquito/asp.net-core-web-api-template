using Backend.Libs.Database.Account;
using Backend.Libs.gRPC.Account;
using Backend.Libs.gRPC.Extensions;
using Mapster;

namespace Backend.Plugins.Database.Mapping;

public static class MapsterMapperRules
{
    public static void InitMappingRules()
    {
        TypeAdapterConfig<GrpcAccountDTO, AccountDTO>
            .NewConfig()
            .Map(dest => dest.Id, src => src.Id.ToGuid())
            .PreserveReference(true);
        TypeAdapterConfig<AccountDTO, GrpcAccountDTO>
            .NewConfig()
            .Map(dest => dest.Id, src => src.Id.ToGuidValue())
            .PreserveReference(true);
    }
}