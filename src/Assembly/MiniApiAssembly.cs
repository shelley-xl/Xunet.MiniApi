// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Diagnostics.HealthChecks;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Http.Json;
global using Microsoft.AspNetCore.Http.Features;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Caching.Memory;
global using Microsoft.Extensions.Caching.Distributed;
global using Microsoft.Extensions.Caching.Redis;
global using Microsoft.Extensions.Configuration;
global using Microsoft.OpenApi.Models;
global using Swashbuckle.AspNetCore.SwaggerGen;
global using System.Collections.Concurrent;
global using System.Diagnostics;
global using System.Globalization;
global using System.Threading.RateLimiting;
global using System.Reflection;
global using System.ComponentModel;
global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Cryptography;
global using System.Security.Claims;
global using System.Text;
global using System.Text.Unicode;
global using System.Text.Encodings.Web;
global using System.Text.Json;
global using System.Text.Json.Serialization;
global using System.Text.Json.Nodes;
global using System.Net;
global using System.Net.Http.Headers;
global using System.Net.Http.Json;
global using AutoMapper;
global using CSRedis;
global using FluentScheduler;
global using FluentValidation;
global using SqlSugar;
global using SkiaSharp;
global using Microsoft.IdentityModel.Tokens;
global using OpenIddict.Client;
global using OpenIddict.Validation.AspNetCore;
global using RabbitMQ.Client;
global using Xunet.MiniApi.Aliyun;
global using Xunet.MiniApi.Aliyun.Oss;
global using Xunet.MiniApi.Aliyun.Sms;
global using Xunet.MiniApi.Attributes;
global using Xunet.MiniApi.AutoMapper;
global using Xunet.MiniApi.Authentication;
global using Xunet.MiniApi.Authorization;
global using Xunet.MiniApi.Caches;
global using Xunet.MiniApi.Constants;
global using Xunet.MiniApi.Dtos;
global using Xunet.MiniApi.Dtos.Requests;
global using Xunet.MiniApi.Enums;
global using Xunet.MiniApi.Exceptions;
global using Xunet.MiniApi.Http;
global using Xunet.MiniApi.Http.Json;
global using Xunet.MiniApi.Middlewares;
global using Xunet.MiniApi.Middlewares.IEventHandlers;
global using Xunet.MiniApi.SqlSugar;
global using Xunet.MiniApi.Swagger;
global using Xunet.MiniApi.Tencent;
global using Xunet.MiniApi.Tencent.Cos;
global using Xunet.MiniApi.Tencent.Sms;
global using Xunet.MiniApi.Tencent.WeixinMp;
global using Xunet.MiniApi.Tencent.MiniProgram;

#pragma warning disable IDE0130 

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

    /// <summary>
    /// 程序集对象（入口程序集）
    /// </summary>
    public static readonly Assembly EntryAssembly = Assembly.GetEntryAssembly() ?? Assembly;

    /// <summary>
    /// 程序集名称
    /// </summary>
    public static readonly string AssemblyName = Assembly.GetName().Name ?? "Xunet.MiniApi";

    /// <summary>
    /// 程序集名称（入口程序集）
    /// </summary>
    public static readonly string EntryAssemblyName = EntryAssembly.GetName().Name ?? "Xunet.MiniApi";
}
