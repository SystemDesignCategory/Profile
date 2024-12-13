public class UserPrincipals
{
    public required string UserName { get; set; }
    public required Guid UserId { get; set; }
    public string TimeFormat { get; set; }
    public string DateFormat { get; set; }
    public string TimeZoneId { get; set; }
    public TimeZoneInfo TimeZoneInfo => TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId);
}