namespace Messages;

class UserProfileChangedDto
{
    public Guid UserId { get; set; }
    public string TimeFormat { get; set; }
    public string DateFormat { get; set; }
    public string TimeZoneId { get; set; }
}