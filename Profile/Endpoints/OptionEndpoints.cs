using System.Globalization;

namespace Profile.Endpoints;

public static class OptionEndpoints
{
    public static void MapOptionEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/options");

        group.MapGet("/timeZones", () => {
            var zones = TimeZoneInfo.GetSystemTimeZones();
            return zones.Select(c => new { 
                c.Id,
                c.StandardName,
                c.DisplayName,
                c.SupportsDaylightSavingTime,
                c.BaseUtcOffset
            });
        });

        group.MapGet("/dateFormats", () => {
            var dateFormats = CultureInfo.GetCultures(CultureTypes.AllCultures);
            return dateFormats.Select(c => new {
                c.Name,
                c.DateTimeFormat.ShortDatePattern,
            }).DistinctBy(c => c.ShortDatePattern);
        });

        group.MapGet("/timeFormats", () => {
            var timeFormats = CultureInfo.GetCultures(CultureTypes.AllCultures);
            return timeFormats.Select(c => new {
                c.Name,
                c.DateTimeFormat.ShortTimePattern,
            }).DistinctBy(c => c.ShortTimePattern);
        });
    }
}
