using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;
using WebApplication2.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Swagger'ı ekliyoruz
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Veritabanı bağlantısını yapıyoruz
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 32))
    ));

// CORS yapılandırması ekliyoruz
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        // Geliştirme ortamında sadece localhost:3000'e izin veriyoruz
        policy.WithOrigins("http://localhost:3000")  // Frontend'inizin portu
            .AllowAnyMethod()  // Herhangi bir HTTP metoduna izin veriyoruz
            .AllowAnyHeader(); // Herhangi bir header'a izin veriyoruz
    });
});

// UserService servisinin bağımlılığı
builder.Services.AddScoped<UserService>();

var app = builder.Build();

// Swagger'ı geliştirme ortamında aktif ediyoruz
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Swagger middleware'ini ekliyoruz
    app.UseSwaggerUI(); // Swagger UI'yi aktif ediyoruz
}

// CORS politikasını uyguluyoruz
app.UseCors("AllowAll");  // CORS middleware'ini burada ekliyoruz

// app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
