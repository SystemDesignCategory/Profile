using Task = TaskManger.Model.Task;
namespace TaskManger.Persistence;
public class Store
{
    List<Task> Tasks { get; set; }
    public List<Task> GetTasks()
    {
        return Tasks;
    }
    public void AddTask(Task task)
    {
        Tasks ??= [];
        Tasks.Add(task);
    }
}