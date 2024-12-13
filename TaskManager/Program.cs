using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Security.Claims;
using System.Text;
using TaskManger.Endpoints;
using TaskManger.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<Store>();
builder.Services.AddHttpContextAccessor();
// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = false,
                        ValidIssuer = "https://localhost:7124",
                        ValidAudience = "System-Design-FakeIDP",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("myFakeIdpService@SymmetricSecretKey")),
                    };
                });

builder.Services.AddAuthorization();

builder.Services.AddScoped(sp =>
{
    var context = sp.GetRequiredService<IHttpContextAccessor>();

    Guid.TryParse(context.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value, out Guid userId);
    var userName = context.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "";

    var timeZoneId = context.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "TimeZoneId")?.Value ?? "";
    var dateFormat = context.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "DateFormat")?.Value ?? "";
    var timeFormat = context.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "TimeFormat")?.Value ?? "";

    return new UserPrincipals
    {
        UserId = userId,
        UserName = userName,
        TimeZoneId = timeZoneId,
        DateFormat = dateFormat,
        TimeFormat = timeFormat,
    };
});

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
app.MapOpenApi();
app.MapScalarApiReference();
app.UseCors("AllowOrigins");

//app.UseHttpsRedirection();

app.MapTaskEndpoints();
app.Run();
