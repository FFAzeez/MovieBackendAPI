#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["InsureService/InsureService.API.csproj", "InsureService/"]
COPY ["InsureService.Business/InsureService.Business.csproj", "InsureService.Business/"]
COPY ["InsureService.Domain/InsureService.Domain.csproj", "InsureService.Domain/"]
COPY ["InsureService.Infrastructure/InsureService.Infrastructure.csproj", "InsureService.Infrastructure/"]

RUN dotnet restore "InsureService/InsureService.API.csproj"
COPY . .
WORKDIR "/src/InsureService"
RUN dotnet build "InsureService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "InsureService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InsureService.API.dll"]