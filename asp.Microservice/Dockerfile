FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["asp.Microservice/asp.Microservice.csproj", "asp.Microservice/"]
RUN dotnet restore "asp.Microservice/asp.Microservice.csproj"
COPY . .
WORKDIR "/src/asp.Microservice"
RUN dotnet build "asp.Microservice.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "asp.Microservice.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "asp.Microservice.dll"]
