# Xunet.MiniApi

面向微服务的.NET 最小API支持，功能特性包括：

- 统一的接口响应格式返回
- 自定义异常处理和数据持久化
- 自定义请求处理中间件
- 参数签名中间件
- 内置健康检查
- 基于System.Text.Json的序列化和反序列化
- 支持Sqlite、MySql、SqlServer数据库
- Swagger接口文档
- 参数自动验证
- 认证和授权
- 策略包含跨域、限流、授权

Support .NET 8.0+

[![Nuget](https://img.shields.io/nuget/v/Xunet.MiniApi.svg?style=flat-square)](https://www.nuget.org/packages/Xunet.MiniApi)
[![Downloads](https://img.shields.io/nuget/dt/Xunet.MiniApi.svg?style=flat-square)](https://www.nuget.org/stats/packages/Xunet.MiniApi?groupby=Version)
[![License](https://img.shields.io/github/license/shelley-xl/Xunet.MiniApi.svg)](https://github.com/shelley-xl/Xunet.MiniApi/blob/master/LICENSE)
![Vistors](https://visitor-badge.laobi.icu/badge?page_id=https://github.com/shelley-xl/Xunet.MiniApi)

## 安装

Xunet.MiniApi 以 NuGet 包的形式提供。您可以使用 NuGet 包控制台窗口安装它：

```
PM> Install-Package Xunet.MiniApi
```

## 使用

Program.cs

```c#
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAntiforgery();
builder.Services.AddXunetHttpContextAccessor();
builder.Services.AddXunetJsonOptions();
builder.Services.AddXunetHealthChecks();
builder.Services.AddXunetSwagger();
builder.Services.AddXunetSqliteStorage();
builder.Services.AddXunetFluentValidation();
builder.Services.AddXunetRateLimiter();
builder.Services.AddXunetAuthentication<PermissionHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseAntiforgery();
app.UseRateLimiter();
app.UseXunetCustomException();
app.UseXunetRequestHandler();
app.UseXunetHttpContextAccessor();
app.UseXunetHealthChecks();
app.UseXunetSwagger();
app.UseXunetStorage();
app.UseXunetAuthentication();

app.Run();
```
