FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["CodexAPI/CodexAPI.csproj", "CodexAPI/"]
RUN dotnet restore "CodexAPI/CodexAPI.csproj"

COPY . .
RUN dotnet build "CodexAPI/CodexAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CodexAPI/CodexAPI.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:5000

EXPOSE 5000

ENTRYPOINT ["dotnet", "CodexAPI.dll"]
