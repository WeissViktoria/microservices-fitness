using Domain.Interfaces;
using Domain.Repositories;
using Domain.Services;
using Microsoft.EntityFrameworkCore;
using Model.Configuration;
using Model.Entities;
using RestApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<WorkoutDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("WorkoutDB"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("WorkoutDB"))
    ));

builder.Services.AddDbContext<AnalyticsDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("AnalyticsDB"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("AnalyticsDB"))
    ));

builder.Services.AddDbContext<RecommendationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("RecommendationDB"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("RecommendationDB"))
    ));

builder.Services.AddHttpClient<IParticipantClient, ParticipantClient>(client =>
{
    // URL of your ParticipantService
    client.BaseAddress = new Uri("http://10.0.28.72:5092/");
});


// Register repositories
builder.Services.AddScoped<IRepositoryAsync<Workout>>(sp =>
{
    var context = sp.GetRequiredService<WorkoutDbContext>();
    return new WorkoutRepository(context);
});

builder.Services.AddScoped<IRepositoryAsync<History>>(sp =>
{
    var context = sp.GetRequiredService<AnalyticsDbContext>();
    return new AnalyticRepository(context);
});

// Register services
builder.Services.AddScoped<AnalyticService>(sp =>
{
    var analyticsContext = sp.GetRequiredService<AnalyticsDbContext>();
    var workoutContext = sp.GetRequiredService<WorkoutDbContext>();
    return new AnalyticService(analyticsContext, workoutContext);
});

builder.Services.AddScoped<Domain.Services.WorkoutService>();


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