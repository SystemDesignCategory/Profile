using TaskManger.Persistence;

namespace TaskManger.Endpoints;

public static class TaskManagerEndpoints
{
    public static void MapTaskEndpoints(this IEndpointRouteBuilder builder)
    {        
        var mapGroup = builder.MapGroup("/Task");

        mapGroup.MapPost("/", (string EngineerName, string Name, Store store) =>
        {
            var task = new Model.Task
            {
                Id = Guid.NewGuid(),
                Name = Name,
                Owner = EngineerName,
                CreatedOn = DateTime.UtcNow,
            };
            store.AddTask(task);

            return Results.Ok(task);

        }).RequireAuthorization();

        mapGroup.MapGet("/", (Store store, UserPrincipals userPrincipals) =>
        {
            var tasks = store.GetTasks();
            var taskList = tasks.Select(c => new Model.Task
            {
                Id = c.Id,
                Name = c.Name,
                Owner = c.Owner,
                
                CreatedOn = TimeZoneInfo.ConvertTimeFromUtc(c.CreatedOn, userPrincipals.TimeZoneInfo),
                CreatedOnFormated = TimeZoneInfo.ConvertTimeFromUtc(c.CreatedOn, userPrincipals.TimeZoneInfo)
                                        .ToString($"{userPrincipals.DateFormat} {userPrincipals.TimeFormat}"),

            });

            return Results.Ok(taskList);
        }).RequireAuthorization();

    }
}