using AboKamel.Api.Extensions;
using AboKamel.Api.SeedData;
using AboKamel.Application.Services.Hubs;
using Capsula.Application.ExtensionForServices;
using Capsula.Domain.Entities.Brands;
using Capsula.Domain.Entities.Categories;
using Services.Application.ExtensionForServices;
using Services.Infrastructure.DbContexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.ResolveServicesExtension(builder.Configuration);
builder.Services.AddDatabaseConnection(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddExceptionHandlers();
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
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
