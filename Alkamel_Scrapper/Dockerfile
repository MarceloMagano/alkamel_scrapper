FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5154

ENV ASPNETCORE_URLS=http://+:5154

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["Alkamel_Scrapper/Alkamel_Scrapper.csproj", "Alkamel_Scrapper/"]
RUN dotnet restore "Alkamel_Scrapper/Alkamel_Scrapper.csproj"
COPY . .
WORKDIR "/src/Alkamel_Scrapper"
RUN dotnet build "Alkamel_Scrapper.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Alkamel_Scrapper.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Alkamel_Scrapper.dll"]
