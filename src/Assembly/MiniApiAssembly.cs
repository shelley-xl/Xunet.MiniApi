#pragma warning disable IDE0130

global using Swashbuckle.AspNetCore.SwaggerGen;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Caching.Memory;
global using Microsoft.Extensions.Configuration;
global using Microsoft.AspNetCore.Diagnostics.HealthChecks;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Http.Features;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.RateLimiting;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.OpenApi.Models;
global using Microsoft.IdentityModel.Tokens;
global using System.Collections.Concurrent;
global using System.Configuration;
global using System.Diagnostics;
global using System.Globalization;
global using System.Threading.RateLimiting;
global using System.Security.Cryptography;
global using System.Reflection;
global using System.ComponentModel;
global using System.Text;
global using System.Text.Encodings.Web;
global using System.Text.Json;
global using System.Text.Json.Serialization;
global using System.Text.Unicode;
global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;
global using Xunet.MiniApi.Attributes;
global using Xunet.MiniApi.Authentication;
global using Xunet.MiniApi.Authorization;
global using Xunet.MiniApi.Dtos;
global using Xunet.MiniApi.Dtos.Requests;
global using Xunet.MiniApi.Entities;
global using Xunet.MiniApi.Enums;
global using Xunet.MiniApi.Filters;
global using Xunet.MiniApi.Http;
global using Xunet.MiniApi.Http.Json;
global using Xunet.MiniApi.Middlewares;
global using Xunet.MiniApi.Policies;
global using Xunet.MiniApi.SqlSugar;
global using FluentValidation;
global using FluentValidation.AspNetCore;
global using SqlSugar;

namespace Xunet.MiniApi;

/// <summary>
/// MiniApi程序集
/// </summary>
public class MiniApiAssembly
{
    /// <summary>
    /// 程序集对象
    /// </summary>
    public static readonly Assembly Assembly = Assembly.GetExecutingAssembly();
}
