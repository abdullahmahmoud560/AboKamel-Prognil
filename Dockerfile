FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /app

COPY *.sln ./
COPY src/AboKamel.Api/AboKamel.Api.csproj ./src/AboKamel.Api/
COPY src/AboKamel.Application/AboKamel.Application.csproj ./src/AboKamel.Application/
COPY src/AboKamel.Infrastructure/AboKamel.Infrastructure.csproj ./src/AboKamel.Infrastructure/
COPY src/AboKamel.Domain/AboKamel.Domain.csproj ./src/AboKamel.Domain/
COPY src/AboKamel.Core/AboKamel.Core.csproj ./src/AboKamel.Core/
COPY test/AboKamel.Test/AboKamel.Test.csproj ./test/AboKamel.Test/

RUN dotnet restore

COPY . ./
RUN dotnet publish src/AboKamel.Api/AboKamel.Api.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build-env /app/out .
COPY src/AboKamel.Api/wwwroot ./wwwroot
COPY src/AboKamel.Api/SeedData ./SeedData

ENTRYPOINT ["dotnet", "AboKamel.Api.dll"]
EXPOSE 8080