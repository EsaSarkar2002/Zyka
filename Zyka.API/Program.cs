using Microsoft.EntityFrameworkCore;
using Zyka.API.Data;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// 1. Add Services with Native JSON configuration
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // This ensures Enums (like UserRole) are sent as strings or handled correctly
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. Database Connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ZykaDbContext>(options =>
    options.UseSqlServer(connectionString));

// 3. CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMvcApp", policy =>
        policy.WithOrigins("https://localhost:7001") // MVC URL
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowMvcApp");
app.UseAuthorization();
app.MapControllers();
app.Run();