using Microsoft.EntityFrameworkCore;
using Profile;
using Profile.Endpoints;
using Profile.Models;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<AppSetting>(builder.Configuration);
var config = builder.Configuration.Get<AppSetting>();
builder.Services.AddOptions<Encryption>();

if (config is null)
    throw new Exception();

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
