namespace Backend.Libs.Infrastructure.Attributes;

public class QueueNameAttribute : Attribute
{
    public string Name { get; init; }
    public QueueNameAttribute(string name)
    {
        Name = name;
    }
}