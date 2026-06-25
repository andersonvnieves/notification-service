FROM mcr.microsoft.com/dotnet/runtime:10.0 AS base
USER $APP_UID
WORKDIR /app


FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["br.com.fiap.cloudgames.Notification.WorkerService/br.com.fiap.cloudgames.Notification.WorkerService.csproj", "br.com.fiap.cloudgames.Notification.WorkerService/"]
RUN dotnet restore "./br.com.fiap.cloudgames.Notification.WorkerService/br.com.fiap.cloudgames.Notification.WorkerService.csproj"
COPY . .
WORKDIR "/src/br.com.fiap.cloudgames.Notification.WorkerService"
RUN dotnet build "./br.com.fiap.cloudgames.Notification.WorkerService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./br.com.fiap.cloudgames.Notification.WorkerService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "br.com.fiap.cloudgames.Notification.WorkerService.dll"]