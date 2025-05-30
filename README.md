# Xunet.MiniApi

[![Nuget](https://img.shields.io/nuget/v/Xunet.MiniApi.svg?style=flat-square)](https://www.nuget.org/packages/Xunet.MiniApi)
[![Downloads](https://img.shields.io/nuget/dt/Xunet.MiniApi.svg?style=flat-square)](https://www.nuget.org/stats/packages/Xunet.MiniApi?groupby=Version)
[![License](https://img.shields.io/github/license/shelley-xl/Xunet.MiniApi.svg)](https://github.com/shelley-xl/Xunet.MiniApi/blob/master/LICENSE)
![Vistors](https://visitor-badge.laobi.icu/badge?page_id=https://github.com/shelley-xl/Xunet.MiniApi)

����΢�����.NET ��СAPI֧�֣��������԰�����

- ͳһ�Ľӿ���Ӧ��ʽ����
- �Զ����쳣��������ݳ־û�
- �Զ����������м��
- ����ǩ���м��
- ���ý������
- ����System.Text.Json�����л��ͷ����л�
- ֧��Sqlite��MySql��SqlServer���ݿ�
- Swagger�ӿ��ĵ�
- �����Զ���֤
- ��֤����Ȩ
- ���԰���������������Ȩ

Support .NET 8.0+

## ��װ

Xunet.MiniApi �� NuGet ������ʽ�ṩ��������ʹ�� NuGet ������̨���ڰ�װ����

```
PM> Install-Package Xunet.MiniApi
```

## ʹ��

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
            // ����֤����Ȩ
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
  }
}
```
