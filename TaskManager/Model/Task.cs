namespace TaskManger.Model;
public class Task
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Owner { get; set; }
    public DateTime CreatedOn { get; set; }
    public string CreatedOnFormated { get; set; }
}