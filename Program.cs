using Microsoft.EntityFrameworkCore;
using CrudApi.Data;
using CrudApi.Services;
using CrudApi.Middleware;
using Microsoft.AspNetCore.Authentication.Cookies;
using AutoMapper; // Tambahkan ini
using CrudApi.Mappings; // Pastikan Anda memiliki namespace untuk Mapping Profile


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// Add services to the container
builder.Services.AddControllers();

// Konfigurasi CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder
            .AllowAnyOrigin() // Untuk testing dengan Postman
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Konfigurasi Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.None; // Penting untuk Postman
        options.Cookie.SecurePolicy = CookieSecurePolicy.None; // Set ke None untuk testing
    });

// Konfigurasi DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IKaryawanService, KaryawanService>();

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Gunakan CORS sebelum routing
app.UseCors("AllowAll");


// Tambahkan Authentication middleware sebelum Authorization
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<JwtMiddleware>();

app.MapControllers();

app.Run();