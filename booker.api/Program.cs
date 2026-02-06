using booker.api.Data;
using booker.api.DIExtension;
using booker.api.Models;
using booker.api.Services;
using booker.api.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddMemoryCache();

builder.Services.AddDbContext<BookerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BookerDb")));

builder.Services.AddDbContext<BookerIdentityDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BookerIdentityDb")));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<BookerIdentityDbContext>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.WithOrigins("http://localhost:5173", "http://localhost:5174")
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
}).AddRoles<IdentityRole>()
  .AddEntityFrameworkStores<BookerIdentityDbContext>()
  .AddDefaultTokenProviders();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapGet("/", () => "Hello world!");


app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
