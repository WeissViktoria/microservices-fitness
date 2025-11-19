using Microsoft.EntityFrameworkCore;
using Model.Configuration;
using Model.Entities;
using Domain.Interfaces;   

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

builder.Services.AddDbContextFactory<AnalyticsContext>(
    options => options.UseMySql(
        builder.Configuration
            .GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 27)))
        );

//builder.Services.AddTransient<IRepositoryAsync<Workout>, WorkoutRepository>();


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