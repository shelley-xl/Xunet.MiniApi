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

static async Task<IResult> Test()
{
    await Task.CompletedTask;
    throw new Exception("“Ï≥£≤‚ ‘");
}

var group = app.MapGroup("/api");

group.MapGet("/test", Test).WithTags("≤‚ ‘").WithOpenApi(x => new(x)
{
    Summary = "≤‚ ‘“ªœ¬",
    Description = "≤‚ ‘“ªœ¬∞°",
});

group.AddEndpointFilter<AutoValidationFilter>();
group.RequireRateLimiting(RateLimiterPolicy.Fixed);
group.RequireAuthorization(AuthorizePolicy.Default);

app.Run();
