// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) –Ï¿¥ ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

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
app.UseAuthMiniApi();
app.UseUserMiniApi();

app.Run();
