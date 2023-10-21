#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Services/Custumer/Custumer.Api/Custumer.Api.csproj", "src/Services/Custumer/Custumer.Api/"]
RUN dotnet restore "src/Services/Custumer/Custumer.Api/Custumer.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/Custumer/Custumer.Api"
RUN dotnet build "Custumer.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Custumer.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Custumer.Api.dll"]