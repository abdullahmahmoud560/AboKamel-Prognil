using AboKamel.Api.Extensions;
using AboKamel.Api.SeedData;
using AboKamel.Application.Services.Hubs;
using Capsula.Application.ExtensionForServices;
using dotenv.net;
using Services.Application.ExtensionForServices;
using Services.Infrastructure.DbContexts;

var possibleEnvPaths = new[] { ".env", "../../.env", "../../../.env" };
DotEnv.Load(options: new DotEnvOptions(envFilePaths: possibleEnvPaths, trimValues: true, ignoreExceptions: true));

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.ResolveServicesExtension(builder.Configuration);
builder.Services.AddDatabaseConnection(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorizationPolicies();
builder.Services.AddExceptionHandlers();
builder.Services.AddSignalR();
builder.Services.AddMemoryCache();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

await app.SeedDatabaseAsync();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.UseExceptionHandler();
app.MapControllers();
app.MapHub<NotificationHub>("/hubs/notifications");

app.Run();