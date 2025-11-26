using Microsoft.EntityFrameworkCore;
using Model.Configuration;
using Model.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<WorkoutDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("WorkoutDb"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("WorkoutDb"))
    ));

builder.Services.AddDbContext<AnalyticsDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("AnalyticsDb"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("AnalyticsDb"))
    ));

builder.Services.AddDbContext<RecommendationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("RecommendationDb"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("RecommendationDb"))
    ));

//builder.Services.AddTransient<IRepositoryAsync<Workout>, WorkoutRepository>();

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