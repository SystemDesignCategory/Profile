namespace Profile.Endpoints.Dtos;

public class CreateProfileDto
{
    public string TimeFormat { get; set; }
    public string DateFormat { get; set; }
    public string TimeZoneId { get; set; }
}