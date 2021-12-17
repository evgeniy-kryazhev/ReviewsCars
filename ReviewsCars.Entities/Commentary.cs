namespace ReviewsCars.Entities;

public abstract class Commentary
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}