namespace Profile;

public class AppSetting
{
    public required ConnectionStrings ConnectionStrings { get; set; }
    public required Encryption Encryption  { get; set; }
}

public class ConnectionStrings
{
    public required string defaultConnection { get; set; }
}
public class Encryption
{
    public required string Key { get; set; }
}
