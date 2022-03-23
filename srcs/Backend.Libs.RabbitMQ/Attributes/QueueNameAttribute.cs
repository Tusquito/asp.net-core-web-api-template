namespace Backend.Libs.RabbitMQ.Attributes;

public class QueueNameAttribute : Attribute
{
    public string Name { get; init; }
    public QueueNameAttribute(string name)
    {
        Name = name;
    }
}