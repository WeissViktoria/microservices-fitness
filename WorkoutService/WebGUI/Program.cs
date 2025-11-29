using Domain.Interfaces;
using Domain.Repositories;
using Domain.Services;
using Microsoft.EntityFrameworkCore;
using Model.Configuration;
using Model.Entities;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Use fixed MySQL version to avoid connection attempt during startup
var serverVersion = new MySqlServerVersion(new Version(8, 0, 36));

builder.Services.AddDbContext<WorkoutDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("WorkoutDB"),
        serverVersion,
        mySqlOptions => mySqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null)
    ));

builder.Services.AddDbContext<AnalyticsDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("AnalyticsDB"),
        serverVersion,
        mySqlOptions => mySqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null)
    ));

builder.Services.AddDbContext<RecommendationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("RecommendationDB"),
        serverVersion,
        mySqlOptions => mySqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null)
    ));

builder.Services.AddScoped<AnalyticService>(sp =>
{
    var analyticsContext = sp.GetRequiredService<AnalyticsDbContext>();
    var workoutContext = sp.GetRequiredService<WorkoutDbContext>();
    return new AnalyticService(analyticsContext, workoutContext);
});

builder.Services.AddScoped<IRepositoryAsync<Workout>>(sp =>
{
    var context = sp.GetRequiredService<WorkoutDbContext>();
    return new WorkoutRepository(context);
});
builder.Services.AddScoped<Domain.Services.WorkoutService>();

// Register RecommendationService
builder.Services.AddScoped<Domain.Services.RecommendationService>(sp =>
{
    var recommendationContext = sp.GetRequiredService<RecommendationDbContext>();
    var workoutContext = sp.GetRequiredService<WorkoutDbContext>();
    var analyticsContext = sp.GetRequiredService<AnalyticsDbContext>();
    return new Domain.Services.RecommendationService(recommendationContext, workoutContext, analyticsContext);
});

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