FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 6663

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["srcs/Backend.Api.Authentication/Backend.Api.Authentication.csproj", "srcs/Backend.Api.Authentication/"]
COPY ["srcs/Backend.Libs.Domain/Backend.Libs.Domain.csproj", "srcs/Backend.Libs.Domain/"]
COPY ["srcs/Backend.Libs.Cryptography/Backend.Libs.Cryptography.csproj", "srcs/Backend.Libs.Cryptography/"]
COPY ["srcs/Backend.Libs.Database/Backend.Libs.Database.csproj", "srcs/Backend.Libs.Database/"]
COPY ["srcs/Backend.Libs.gRPC/Backend.Libs.gRPC.csproj", "srcs/Backend.Libs.gRPC/"]
COPY ["srcs/Backend.Libs.Models/Backend.Libs.Models.csproj", "srcs/Backend.Libs.Models/"]
COPY ["srcs/Backend.Libs.Security/Backend.Libs.Security.csproj", "srcs/Backend.Libs.Security/"]
COPY ["srcs/Backend.Plugins.Domain/Backend.Plugins.Domain.csproj", "srcs/Backend.Plugins.Domain/"]
COPY ["srcs/Backend.Libs.Redis/Backend.Libs.Redis.csproj", "srcs/Backend.Libs.Redis/"]
COPY ["srcs/Backend.Plugins.gRPC/Backend.Plugins.gRPC.csproj", "srcs/Backend.Plugins.gRPC/"]
RUN dotnet restore "srcs/Backend.Api.Authentication/Backend.Api.Authentication.csproj"
COPY . .
WORKDIR "/src/srcs/Backend.Api.Authentication"
RUN dotnet build "Backend.Api.Authentication.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Backend.Api.Authentication.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Backend.Api.Authentication.dll"]
