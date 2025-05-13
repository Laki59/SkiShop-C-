using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using salalal.Models;
using salalal.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Database setup
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer("Server=DESKTOP-3BURIQ0\\MSSQLSERVER01;Database=project;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"));

// Authentication setup
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Redirekcija
        options.AccessDeniedPath = "/Home/Index"; // Redirekcija na kucu ako nije logovan
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

// Configure repositories (dependency injection)
builder.Services.AddScoped<ISkiRepository, SkiRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();

// Configure session middleware
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseRouting();         // Ensure routing is set first
app.UseStaticFiles();     // Enable serving static files (CSS, JS, images)

app.UseSession();         // Ensure session is enabled before authentication
app.UseAuthentication();  // Enable authentication
app.UseAuthorization();   // Enable authorization

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"); // Set default page to Home Index
});

app.Run();