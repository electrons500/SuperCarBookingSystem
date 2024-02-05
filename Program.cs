using Microsoft.EntityFrameworkCore;
using SuperCarBookingSystem;
using SuperCarBookingSystem.Models;
using SuperCarBookingSystem.Models.Repos;
using SuperCarBookingSystem.Models.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var mongoDBSettings = builder.Configuration.GetSection("MongoDBSettings").Get<MongoDBSettings>();
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDBSettings"));

builder.Services.AddDbContext<CarBookingDbContext>(options =>
options.UseMongoDB(mongoDBSettings.MongodbUrl ?? "", mongoDBSettings.DatabaseName ?? ""));

builder.Services.AddScoped<ICar, CarService>();
builder.Services.AddScoped<IBooking, BookingService>();

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

app.Run();
