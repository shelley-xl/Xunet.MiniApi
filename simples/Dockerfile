# 请参阅 https://aka.ms/customizecontainer 以了解如何自定义调试容器，以及 Visual Studio 如何使用此 Dockerfile 生成映像以更快地进行调试。

# 此阶段用于在快速模式(默认为调试配置)下从 VS 运行时
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8000


# 此阶段用于生成服务项目
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["simples/Xunet.MiniApi.Simples.csproj", "simples/"]
COPY ["src/Xunet.MiniApi.csproj", "src/"]
RUN dotnet restore "./simples/Xunet.MiniApi.Simples.csproj"
COPY . .
WORKDIR "/src/simples"
RUN dotnet build "./Xunet.MiniApi.Simples.csproj" -c $BUILD_CONFIGURATION -o /app/build

# 此阶段用于发布要复制到最终阶段的服务项目
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Xunet.MiniApi.Simples.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 此阶段在生产中使用，或在常规模式下从 VS 运行时使用(在不使用调试配置时为默认值)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# 设置环境变量
ENV ASPNETCORE_ENVIRONMENT=Test
ENV TZ=Asia/Shanghai

ENTRYPOINT ["dotnet", "Xunet.MiniApi.Simples.dll"]
