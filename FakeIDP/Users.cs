namespace FakeIDP;

public class Store
{
    public List<User> Users =
    [
        new User
        {
            UserId = Guid.Parse("280279E3-3B47-4E80-9460-5617EF757594"),
            UserName = "user1",
        },
        new User
        {
            UserId = Guid.Parse("6F445C28-E180-4CCA-AD60-7B1AB6CE3C8A"),
            UserName = "user2",
        }
    ];

    public void SetUserRawData(Guid userId, string key, string value)
    {
        var user = Users.FirstOrDefault(c => c.UserId == userId);
        if (user is null)
            return;

        user.UserData ??= [];
        if (user.UserData.ContainsKey(key))
            user.UserData.Remove(key);

        user.UserData.Add(key, value);
    }
}

public class User
{
    public Guid UserId { get; set; }
    public required string UserName { get; set; }
    public Dictionary<string, string> UserData { get; set; }    
}