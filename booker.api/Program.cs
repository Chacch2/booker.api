using Microsoft.EntityFrameworkCore;
using booker.api.Data;
using Microsoft.AspNetCore.Identity;
using booker.api.Models;
using booker.api.DIExtension;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Booker API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
