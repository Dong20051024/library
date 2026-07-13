using LibrarySeatReservation.Web.Data;
using LibrarySeatReservation.Web.Models.SeedData;
using LibrarySeatReservation.Web.Services;
using LibrarySeatReservation.Web.Services.Implementations;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.IsEssential = true;
});

// EF Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// DI — Services
builder.Services.AddScoped<ISeatService, SeatService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IStatisticsService, StatisticsService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseSession();

app.MapAreaControllerRoute("Admin", "Admin", "Admin/{controller=Login}/{action=Index}/{id?}");
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

// 种子数据（每次启动检查，已存在则跳过）
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    SeedData.Initialize(dbContext);
}

app.Run();
