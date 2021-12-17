using System.Collections;

namespace ReviewsCars.Entities;

public class PostCar
{
    public Guid Id { get; set; }
    public ApplicationUser? User { get; set; }
    public string Name { get; set; } = null!;
    public string Text { get; set; } = null!;
    public string? ImageUrl { get; set; }
    public DateTime CreateAt { get; set; }
}