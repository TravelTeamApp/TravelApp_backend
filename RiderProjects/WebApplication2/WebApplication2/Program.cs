using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;
using WebApplication2.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Add Swagger support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Set up the database connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 32))
    ));

// Add CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        // Allow only localhost:3000 during development for frontend (React/Vue etc.)
        policy.WithOrigins("http://localhost:3000")
            .AllowAnyMethod()  // Allow all HTTP methods (GET, POST, PUT, DELETE etc.)
            .AllowAnyHeader(); // Allow all headers
    });
});

// Add the UserService dependency
builder.Services.AddScoped<UserService>();

// Add session support
builder.Services.AddDistributedMemoryCache(); // In-memory cache for sessions
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout duration
    options.Cookie.HttpOnly = true; // Makes cookies accessible only by the server
    options.Cookie.IsEssential = true; // Essential cookie required by GDPR
});

var app = builder.Build();

// Enable Swagger in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Enable Swagger middleware
    app.UseSwaggerUI(); // Enable Swagger UI
}

// Apply CORS policy
app.UseCors("AllowAll"); // CORS middleware

// Enable session support
app.UseSession(); // Make sure session middleware is added before routing

// Enable authorization
app.UseAuthorization();

// Map controllers
app.MapControllers();

// Run the app
app.Run();