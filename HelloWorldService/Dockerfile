# builder
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

COPY HelloWorldService.csproj .
RUN dotnet restore
COPY . .

RUN dotnet publish -c release -o /app

# runner
FROM mcr.microsoft.com/dotnet/aspnet:9.0

WORKDIR /app

COPY --from=build /app .

ENTRYPOINT ["dotnet", "HelloWorldService.dll"]
