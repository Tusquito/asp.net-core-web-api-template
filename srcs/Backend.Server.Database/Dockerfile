FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 7771

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["srcs/Backend.Server.Database/Backend.Server.Database.csproj", "srcs/Backend.Server.Database/"]
COPY ["srcs/Backend.Libs.Domain/Backend.Libs.Domain.csproj", "srcs/Backend.Libs.Domain/"]
COPY ["srcs/Backend.Libs.Cryptography/Backend.Libs.Cryptography.csproj", "srcs/Backend.Libs.Cryptography/"]
COPY ["srcs/Backend.Libs.Database/Backend.Libs.Database.csproj", "srcs/Backend.Libs.Database/"]
COPY ["srcs/Backend.Libs.gRPC/Backend.Libs.gRPC.csproj", "srcs/Backend.Libs.gRPC/"]
COPY ["srcs/Backend.Libs.Models/Backend.Libs.Models.csproj", "srcs/Backend.Libs.Models/"]
COPY ["srcs/Backend.Libs.Security/Backend.Libs.Security.csproj", "srcs/Backend.Libs.Security/"]
COPY ["srcs/Backend.Plugins.Database/Backend.Plugins.Database.csproj", "srcs/Backend.Plugins.Database/"]
COPY ["srcs/Backend.Plugins.gRPC/Backend.Plugins.gRPC.csproj", "srcs/Backend.Plugins.gRPC/"]
COPY ["srcs/Backend.Plugins.Domain/Backend.Plugins.Domain.csproj", "srcs/Backend.Plugins.Domain/"]
COPY ["srcs/Backend.Libs.Redis/Backend.Libs.Redis.csproj", "srcs/Backend.Libs.Redis/"]
RUN dotnet restore "srcs/Backend.Server.Database/Backend.Server.Database.csproj"
COPY . .
WORKDIR "/src/srcs/Backend.Server.Database"
RUN dotnet build "Backend.Server.Database.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Backend.Server.Database.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Backend.Server.Database.dll"]
