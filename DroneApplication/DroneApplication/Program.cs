using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DroneApplication.FileUploadService;
using BussinesLayer;
using BussinesLayer.Interfaces;
using DataAccessLayer;
using DataAccessLayer.Repositories;
using DataAccessLayer.Repositories.Interfaces;
using BussinesLayer.Services;
using DataAccessLayer.Interfaces;
using System.Configuration;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DroneApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DroneApplicationContext") ?? throw new InvalidOperationException("Connection string 'DroneApplicationContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();


builder.Services.AddScoped<IFileUploadService, LocalFileUploadService>();



builder.Services.AddTransient<IDroneApplicationDbContext, DroneApplicationDbContext>();
builder.Services.AddTransient<IGeoCoordsService,GeoCoordsService>();
builder.Services.AddTransient<IMiddledEventRepository,MiddledEventRepository>();
builder.Services.AddTransient<IDroneModelService,DroneModelService>();
builder.Services.AddTransient<IDroneModelRepository,DroneModelRepository>();
builder.Services.AddTransient<IFileModelService, FileModelService>();
builder.Services.AddTransient<IFileModelRepository,FileModelRepository>();


builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = 104857600;
    options.MaxRequestBodyBufferSize = 104857600;
});

builder.Services.Configure<FormOptions>(options =>
{
    options.ValueLengthLimit = 104857600;
    options.MultipartBodyLengthLimit = 104857600; // if don't set default value is: 128 MB
    options.MultipartHeadersLengthLimit = 104857600;
    options.BufferBodyLengthLimit = 104857600;
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
