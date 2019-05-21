FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["nettest.csproj", "nettest/"]
RUN dotnet restore "nettest/nettest.csproj"
COPY . .
RUN dotnet build "nettest.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "nettest.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "nettest.dll"]