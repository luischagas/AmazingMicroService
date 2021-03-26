#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["AmazingMicroService.Worker/AmazingMicroService.Worker.csproj", "AmazingMicroService.Worker/"]
COPY ["AmazingMicroService.Infrastructure/AmazingMicroService.Infrastructure.csproj", "AmazingMicroService.Infrastructure/"]
COPY ["AmazingMicroService.Domain/AmazingMicroService.Domain.csproj", "AmazingMicroService.Domain/"]
COPY ["AmazingMicroService.DomainService/AmazingMicroService.DomainService.csproj", "AmazingMicroService.DomainService/"]
COPY ["AmazingMicroService.Application/AmazingMicroService.Application.csproj", "AmazingMicroService.Application/"]
COPY ["AmazingMicroService.Tests/AmazingMicroService.Tests.csproj", "AmazingMicroService.Tests/"]
RUN dotnet restore "AmazingMicroService.Worker/AmazingMicroService.Worker.csproj"
COPY . .
WORKDIR "/src/AmazingMicroService.Worker"
RUN dotnet build "AmazingMicroService.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AmazingMicroService.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AmazingMicroService.Worker.dll"]
