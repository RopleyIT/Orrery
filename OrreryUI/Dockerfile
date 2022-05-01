#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["OrreryUI/OrreryUI.csproj", "OrreryUI/"]
RUN dotnet restore "OrreryUI/OrreryUI.csproj"
COPY . .
WORKDIR "/src/OrreryUI"
RUN dotnet build "OrreryUI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OrreryUI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrreryUI.dll"]