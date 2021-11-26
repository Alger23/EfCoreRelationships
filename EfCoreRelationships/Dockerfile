FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["EfCoreRelationships/EfCoreRelationships.csproj", "EfCoreRelationships/"]
RUN dotnet restore "EfCoreRelationships/EfCoreRelationships.csproj"
COPY . .
WORKDIR "/src/EfCoreRelationships"
RUN dotnet build "EfCoreRelationships.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EfCoreRelationships.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EfCoreRelationships.dll"]
