FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SeleniumExample.Employees/SeleniumExample.Employees.csproj", "SeleniumExample.Employees/"]
RUN dotnet restore "SeleniumExample.Employees/SeleniumExample.Employees.csproj"
COPY . .
WORKDIR "/src/SeleniumExample.Employees"
RUN dotnet build "SeleniumExample.Employees.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SeleniumExample.Employees.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SeleniumExample.Employees.dll"]
