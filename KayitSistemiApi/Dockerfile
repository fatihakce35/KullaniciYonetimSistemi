#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["KayitSistemiApi/KayitSistemiApi.csproj", "KayitSistemiApi/"]
RUN dotnet restore "KayitSistemiApi/KayitSistemiApi.csproj"
COPY . .
WORKDIR "/src/KayitSistemiApi"
RUN dotnet build "KayitSistemiApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KayitSistemiApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KayitSistemiApi.dll"]
