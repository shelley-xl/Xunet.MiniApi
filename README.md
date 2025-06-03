# Xunet.MiniApi

[![Nuget](https://img.shields.io/nuget/v/Xunet.MiniApi.svg?style=flat-square)](https://www.nuget.org/packages/Xunet.MiniApi)
[![Downloads](https://img.shields.io/nuget/dt/Xunet.MiniApi.svg?style=flat-square)](https://www.nuget.org/stats/packages/Xunet.MiniApi?groupby=Version)
[![License](https://img.shields.io/github/license/shelley-xl/Xunet.MiniApi.svg)](https://github.com/shelley-xl/Xunet.MiniApi/blob/master/LICENSE)
![Vistors](https://visitor-badge.laobi.icu/badge?page_id=https://github.com/shelley-xl/Xunet.MiniApi)

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

## 安装

Xunet.MiniApi 以 NuGet 包的形式提供。您可以使用 NuGet 包控制台窗口安装它：

```
PM> Install-Package Xunet.MiniApi
```

## 使用

**Program.cs**

```c#
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddXunetCache();
builder.Services.AddXunetJsonOptions();
builder.Services.AddXunetFluentValidation();
builder.Services.AddXunetHttpContextAccessor();
builder.Services.AddXunetHealthChecks();
builder.Services.AddXunetSwagger();
builder.Services.AddXunetSqliteStorage();
builder.Services.AddXunetJwtAuth();
builder.Services.AddXunetCors();
builder.Services.AddXunetRateLimiter();
builder.Services.AddXunetEventHandler();
builder.Services.AddXunetAuthorizationHandler();
builder.Services.AddXunetMapper();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseXunetCustomException();
app.UseXunetRequestHandler();
app.UseXunetHttpContextAccessor();
app.UseXunetHealthChecks();
app.UseXunetSwagger();
app.UseXunetStorage();
app.UseXunetAuthentication();
app.UseXunetCors();
app.UseRateLimiter();

// Configure the MiniApi request pipeline.

app.Run();
```

**PermissionHandler.cs**

```c#
public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (context.User.Identity != null && context.User.Identity.IsAuthenticated)
        {
            // 已认证，鉴权
            await Task.CompletedTask;
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
    }
}
```

**appsettings.Development.json**

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "Kestrel": {
    "EndPoints": {
      "Http": {
        "Url": "http://0.0.0.0:8000"
      },
      "Http2": {
        "Url": "http://0.0.0.0:8080",
        "Protocols": "Http2"
      }
    }
  },
  "CorsHosts": [],
  "ConnectionStrings": {
    "DefaultConnection": "server=127.0.0.1;uid=root;pwd=root;database=miniapi;max pool size=8000;Charset=utf8;SslMode=none;Allow User Variables=True;",
    "RedisConnection": "127.0.0.1:6379"
  },
  "JwtConfig": {
    "ValidateIssuer": true,
    "ValidIssuer": "miniapi",
    "ValidateIssuerSigningKey": true,
    "SymmetricSecurityKey": "294c66d31f8c4ec0b243bb7479cc38e0",
    "ValidateAudience": true,
    "ValidAudience": "manager",
    "ValidateLifetime": true,
    "RequireExpirationTime": true,
    "ClockSkew": 1,
    "RefreshTokenAudience": "manager",
    "Expire": 2592000,
    "RefreshTokenExpire": 10080
  },
  "StorageOptions": [
    {
      "ConfigId": 0,
      "ConnectionString": "DefaultConnection"
    }
  ],
  "SwaggerOptions": {
    "DocumentTitle": "测试接口服务",
    "Endpoints": [
      {
        "EndpointName": "测试接口",
        "Title": "测试标题",
        "Description": "测试描述",
        "Name": "test"
      }
    ]
  }
}
```

## 更新日志

[CHANGELOG](CHANGELOG.md)
