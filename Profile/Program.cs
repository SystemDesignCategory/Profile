using Microsoft.EntityFrameworkCore;
using Profile;
using Profile.Endpoints;
using Profile.Models;
using Scalar.AspNetCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<AppSetting>(builder.Configuration);
var config = builder.Configuration.Get<AppSetting>();
if (config is null)
    throw new Exception("AppSetting is empty.");

builder.Services.AddOptions<Encryption>();
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(config.Redis.Connection)); 

builder.Services.AddDbContext<ProfileDbContext>(
    options => options.UseSqlServer(config.ConnectionStrings.defaultConnection)
    );

builder.Services.AddOpenApi();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigins",
                      policy =>
                      {
                          policy.AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});
var app = builder.Build();

// Configure the HTTP request pipeline.

//app.UseHttpsRedirection();
app.MapOpenApi();
app.MapScalarApiReference();

app.MapOptionEndpoints();
app.MapUserProfileEndpoints();

app.UseCors("AllowOrigins");
app.Run();
