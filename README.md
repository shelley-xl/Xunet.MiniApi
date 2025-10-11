# Xunet.MiniApi

[![Nuget](https://img.shields.io/nuget/v/Xunet.MiniApi.svg?style=flat-square)](https://www.nuget.org/packages/Xunet.MiniApi)
[![Downloads](https://img.shields.io/nuget/dt/Xunet.MiniApi.svg?style=flat-square)](https://www.nuget.org/stats/packages/Xunet.MiniApi?groupby=Version)
[![License](https://img.shields.io/github/license/shelley-xl/Xunet.MiniApi.svg)](https://github.com/shelley-xl/Xunet.MiniApi/blob/master/LICENSE)

面向微服务的.NET 最小API支持，功能特性包括：

- 自定义请求处理中间件，统一的数据格式返回，全局异常处理
- 自定义参数签名中间件/过滤器，支持参数签名
- 基于System.Text.Json的序列化和反序列化
- 基于OpenIddict实现客户端管理功能
- 内置健康检查、Swagger接口说明文档
- 内置SqlSugar ORM框架，支持Sqlite、MySql、SqlServer等主流数据库
- 内置FluentValidation框架，支持参数验证，支持自定义验证器
- 内置Redis缓存、MemoryCache缓存，支持分布式缓存
- 支持Jwt认证、自定义授权策略、跨域策略
- 支持限流、事件、授权、映射器等扩展功能
- 支持阿里云对象存储、阿里云短信服务
- 支持腾讯云对象存储、腾讯云短信服务
- 支持微信公众号开发，内置微信公众号/小程序相关接口

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

builder.Services.AddXunetCore();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseXunetCore();

// Map endpoints.

app.MapHelloEndpoint();

app.Run();
```

**AppDbContext.cs**

```c#
public class AppDbContext : SugarDbContext<AppDbContext>
{
    
}
```

**HelloRequest.cs**

```c#
internal class HelloRequest
{
    [FromParameter("name", "姓名")]
    public string? Name { get; set; }
}
```

**IHelloService.cs**

```c#
internal interface IHelloService
{
    Task<IResult> SayHelloAsync(HelloRequest request);
}
```

**HelloService.cs**

```c#
internal class HelloService : MiniService<AppDbContext>, IHelloService
{
    public async Task<IResult> SayHelloAsync(HelloRequest request)
    {
        await Task.CompletedTask;

        return XunetResults.Ok($"Hello,{request.Name ?? "world"}!");
    }
}
```

**HelloEndpoint.cs**

```c#
internal static class HelloEndpoint
{
    internal static void MapHelloEndpoint(this WebApplication app)
    {
        var group = app.MapGroup("/api/v1", "test", false, "你好");

        group.MapGet<IHelloService, HelloRequest>("/hello", "你好", (service, [AsParameters] request) =>
        {
            return service.SayHelloAsync(request);
        });
    }
}
```

## 更新日志

[CHANGELOG](CHANGELOG.md)
