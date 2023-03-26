#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Heus.Web/Heus.Web.csproj", "src/Heus.Web/"]
COPY ["src/Heus.Core/Heus.Core.csproj", "src/Heus.Core/"]
COPY ["src/Heus.AspNetCore/Heus.AspNetCore.csproj", "src/Heus.AspNetCore/"]
COPY ["Directory.Build.props", "."]
RUN dotnet restore "src/Heus.Web/Heus.Web.csproj"
COPY . .
RUN dotnet build "src/Heus.Web/Heus.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/Heus.Web/Heus.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Heus.Web.dll"]
