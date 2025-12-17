using Microsoft.EntityFrameworkCore;
using UrlHealthMonitor.Data;
using UrlHealthMonitor.Services;

var builder = WebApplication.CreateBuilder(args);

// -------------------- SERVICES --------------------

// Database (SQLite)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Background URL Health Checker (REGISTER ONLY ONCE)
builder.Services.AddHostedService<UrlHealthCheckerService>();

// MVC
builder.Services.AddControllersWithViews();

// -------------------- APP PIPELINE --------------------

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


