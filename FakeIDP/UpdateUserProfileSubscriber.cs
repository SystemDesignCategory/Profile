
using Messages;
using StackExchange.Redis;
using System.Text.Json;

namespace FakeIDP;

public class UpdateUserProfileSubscriber(IConnectionMultiplexer connectionMultiplexer, Store store) : IHostedLifecycleService
{
    private readonly IConnectionMultiplexer _connectionMultiplexer = connectionMultiplexer;
    private readonly Store _store = store;

    public Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public Task StartedAsync(CancellationToken cancellationToken)
    {
        var subscriber = _connectionMultiplexer.GetSubscriber();
        var redisChannel = new RedisChannel("User:Profile:Channel:ProfileChanged", RedisChannel.PatternMode.Literal);
        subscriber.Subscribe(redisChannel, (channel, message) =>
        {
            var data = JsonSerializer.Deserialize<UserProfileChangedDto>(message);
            if (data is null)
            {
                return;
            }

            var user = _store.Users.FirstOrDefault(c => c.UserId == data.UserId);
            if (user is null)
            {
                return;
            }

            store.SetUserRawData(user.UserId, nameof(data.TimeZoneId), data.TimeZoneId);
            store.SetUserRawData(user.UserId, nameof(data.DateFormat), data.DateFormat);
            store.SetUserRawData(user.UserId, nameof(data.TimeFormat), data.TimeFormat);
        });

        return Task.CompletedTask;
    }

    public Task StartingAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    public Task StoppedAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    public Task StoppingAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
