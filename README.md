# Xunet.MiniApi

[![Nuget](https://img.shields.io/nuget/v/Xunet.MiniApi.svg?style=flat-square)](https://www.nuget.org/packages/Xunet.MiniApi)
[![Downloads](https://img.shields.io/nuget/dt/Xunet.MiniApi.svg?style=flat-square)](https://www.nuget.org/stats/packages/Xunet.MiniApi?groupby=Version)
[![License](https://img.shields.io/github/license/shelley-xl/Xunet.MiniApi.svg)](https://github.com/shelley-xl/Xunet.MiniApi/blob/master/LICENSE)
![Vistors](https://visitor-badge.laobi.icu/badge?page_id=https://github.com/shelley-xl/Xunet.MiniApi)

面向微服务的.NET 最小API支持，功能特性包括：

- 自定义请求处理中间件，统一的数据格式返回
- 自定义异常处理中间件，全局异常处理
- 自定义参数签名中间件，支持参数签名
- 基于System.Text.Json的序列化和反序列化
- 基于OpenIddict实现客户端向认证中心注册
- 内置健康检查、Swagger接口说明文档
- 内置SqlSugar ORM框架，支持Sqlite、MySql、SqlServer快捷使用
- 内置FluentValidation框架，实现请求参数验证器
- 内置Redis缓存、MemoryCache缓存
- 支持Jwt认证、自定义授权策略、跨域策略
- 支持限流、事件、授权、映射器等扩展功能
- 支持阿里云对象存储、阿里云短信服务
- 支持腾讯云对象存储、腾讯云短信服务
- 支持微信公众号开发，内置微信公众号相关接口

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
builder.Services.AddXunetJwtBearer();
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
app.UseXunetCors();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

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
