using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Profile.Endpoints.Dtos;
using Profile.Models;

namespace Profile.Endpoints;

public static class UserProfileEndpoints
{
    public static void MapUserProfileEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/profile");

        group.MapPost("/{user-id}", async ([FromRoute(Name = "user-id")] Guid userId, CreateProfileDto request, ProfileDbContext dbContext) =>
        {
            ArgumentNullException.ThrowIfNull(request.TimeZoneId);
            ArgumentNullException.ThrowIfNull(request.DateFormat);
            ArgumentNullException.ThrowIfNull(request.TimeFormat);

            var profile = Models.Profile.Create(userId, request.TimeZoneId, request.DateFormat, request.TimeFormat);
            dbContext.Profiles.Add(profile);
            await dbContext.SaveChangesAsync();

        });

        group.MapGet("/{user-id}", async ([FromRoute(Name = "user-id")] Guid userId, ProfileDbContext dbContext) =>
        {
            var profile = await dbContext.Profiles.FirstOrDefaultAsync(c => c.UserId == userId);
            if (profile is null)
                return Results.NotFound();

            return Results.Ok(new UserProfileDto
            {
                UserId = profile.UserId,
                TimeZoneId = profile.TimeZoneId,
                DateFormat = profile.DateFormat,
                TimeFormat = profile.TimeFormat
            });

        });

        group.MapPost("/address/{user-id}", async (

            [FromRoute(Name = "user-id")] Guid userId,
            AddProfileAddressDto request,
            ProfileDbContext dbContext) =>
        {

            var profile = await dbContext.Profiles.FirstOrDefaultAsync(c => c.UserId == userId);
            if (profile is null)
                return Results.NotFound();

            profile.AddAddress(new Address
            {
                Country = request.Country,
                City = request.City,
                Street = request.Street,
                PostalCode = request.PostalCode,
            });

            await dbContext.SaveChangesAsync();
            return Results.Ok();
        });

    }
}
