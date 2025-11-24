using Microsoft.AspNetCore.SignalR;
using QuikPic.Web;
using QuikPic.Web.Hubs;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using QuikPic.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// 👉 Force invariant culture (fixes decimal parsing)
var culture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
builder.Services.AddScoped<IPresetService, PresetService>();

builder.Services.AddDbContext<QuikPicContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("Default")
    )
);

var app = builder.Build(); 

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<ImageHub>("/ImageHub");
app.Run();
