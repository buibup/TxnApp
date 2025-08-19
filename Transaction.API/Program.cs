using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Transaction.Infrastructure.Auth;
using Transaction.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Transaction.Domain.Entities;
using Transaction.Application.Common;
using Transaction.Infrastructure.Events;
using Transaction.Application.EventAdapter;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite(connectionString));

builder.Services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<TransactionCreatedHandler>());

var jwtSecret = builder.Configuration["Jwt:Secret"] ?? throw new InvalidOperationException("JWT Secret is not configured.");
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT Issuer is not configured.");

builder.Services.AddSingleton(new JwtService(jwtSecret, jwtIssuer));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapPost("/api/auth/login", (JwtService jwtService, LoginRequest request) =>
{
    // Mock user
    if (request.Username == "user" && request.Password == "1234")
    {
        var token = jwtService.GenerateToken("user-001");
        return Results.Ok(new { token });
    }

    return Results.Unauthorized();
});

app.MapGet("/api/transactions", [Authorize] async (HttpContext http, AppDbContext db) =>
{
    var userId = http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    var data = await db.Transactions.Where(t => t.UserId == userId).ToListAsync();

    return Results.Ok(data);
});

app.MapPost("/api/transactions", [Authorize] async (HttpContext http, AppDbContext db, TransactionInput input) =>
{
    var userId = http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    var txn = new TransactionEntity
    {
        Id = Guid.NewGuid(),
        Description = input.Description,
        Amount = input.Amount,
        Date = input.Date,
        Type = input.Type,
        UserId = userId!
    };

    db.Transactions.Add(txn);
    await db.SaveChangesAsync();

    return Results.Ok(txn);
});

app.MapPut("/api/transactions/{id}", [Authorize] async (HttpContext http, AppDbContext db, Guid id, TransactionInput input) =>
{
    var userId = http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    var txn = await db.Transactions.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

    if (txn is null) return Results.NotFound();

    txn.Description = input.Description;
    txn.Amount = input.Amount;
    txn.Date = input.Date;
    txn.Type = input.Type ?? txn.Type;

    await db.SaveChangesAsync();

    return Results.Ok(txn);
});

app.MapDelete("api/transactions/{id}", [Authorize] async (HttpContext http, AppDbContext db, Guid id) =>
{
    var userId = http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    var txn = await db.Transactions.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

    if (txn is null) return Results.NotFound();

    db.Transactions.Remove(txn);
    await db.SaveChangesAsync();

    return Results.Ok();
});

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
await dbContext.Database.EnsureCreatedAsync();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

record LoginRequest(string Username, string Password);

record TransactionInput(string Description, decimal Amount, DateTime Date, string Type);