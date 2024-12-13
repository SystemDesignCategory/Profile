using FakeIDP;
using Microsoft.AspNetCore.Mvc;
using Scalar.AspNetCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<Store>();
builder.Services.AddScoped<AuthService>();

if (builder.Configuration["Redis:Connection"] is null)
    throw new Exception("Redis Connection is invalid.");

builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(builder.Configuration["Redis:Connection"]));
builder.Services.AddHostedService<UpdateUserProfileSubscriber>();
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

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
var app = builder.Build();

// Configure the HTTP request pipeline.

//app.UseHttpsRedirection();
app.MapOpenApi();
app.MapScalarApiReference();
app.UseCors("AllowOrigins");

app.MapGet("/users", (Store store) =>
{
    return Results.Ok(store.Users);
});

app.MapGet("/users/{user-id}", ([FromRoute(Name = "user-id")] Guid userId, Store store) =>
{
    var user = store.Users.Find(c => c.UserId == userId);
    if (user is null)
        return Results.NotFound();
    
    return Results.Ok(user);
});

app.MapGet("/users/token/{user-id}", ([FromRoute(Name = "user-id")] Guid userId, Store store, AuthService authService) =>
{
    var user = store.Users.Find(c => c.UserId == userId);
    if (user is null)
        return Results.NotFound();

    var token = authService.CreateToken(user);
    return Results.Ok(token);
});

app.Run();
