// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddXunetCore();
builder.Services.AddXunetHealthChecks();
builder.Services.AddXunetSqliteStorage();
builder.Services.AddXunetCaptcha();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseXunetCore();
app.UseXunetHealthChecks();
app.UseXunetStorage();

// Map endpoints.

app.MapAuthEndpoint();
app.MapAccountsEndpoint();

app.Run();
