using System.Collections.Frozen;
using System.Reflection.Metadata.Ecma335;

namespace Profile.Models;

public class Profile
{
    public Guid UserId { get; set; }
    public required string TimeZoneId { get; set; }
    public required string DateFormat { get; set; }
    public required string TimeFormat { get; set; }
    public ICollection<Address> Addresses { get; set; }
    public static Profile Create(Guid userId, string timeZoneId, string dateFormat, string timeFormat)
    => new Profile
    {
        UserId = userId,
        TimeZoneId = timeZoneId,
        DateFormat = dateFormat,
        TimeFormat = timeFormat
    };
    public void AddAddress(Address address)
    {
        Addresses ??= [];

        if(!Addresses.Contains(address))
            Addresses.Add(address);
    }
}
