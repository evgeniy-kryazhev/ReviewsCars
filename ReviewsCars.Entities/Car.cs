namespace ReviewsCars.Entities;

public class Car
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public ICollection<CarImage>? Images { get; set; }
}