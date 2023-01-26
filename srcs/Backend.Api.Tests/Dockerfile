FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["srcs/Backend.Api.Tests/Backend.Api.Tests.csproj", "srcs/Backend.Api.Tests/"]
RUN dotnet restore "srcs/Backend.Api.Tests/Backend.Api.Tests.csproj"
COPY . .
WORKDIR "/src/srcs/Backend.Api.Tests"
RUN dotnet build "Backend.Api.Tests.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Backend.Api.Tests.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Backend.Api.Tests.dll"]
