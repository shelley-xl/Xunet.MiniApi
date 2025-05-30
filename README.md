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

**PermissionHandler.cs**

```c#
public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        await Task.CompletedTask;

        context.Succeed(requirement);
    }
}
```

**appsettings.json**

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
      }
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "server=127.0.0.1;uid=root;pwd=root;database=miniapi;max pool size=8000; charset=utf8;"
  },
  "JwtConfig": {
    "ValidateIssuer": true,
    "ValidIssuer": "cloudstorage",
    "ValidateIssuerSigningKey": true,
    "SymmetricSecurityKey": "2aaa5f2eabcb4a519751bc4f009eaabe",
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
