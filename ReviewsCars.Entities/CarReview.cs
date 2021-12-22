using System.Collections;

namespace ReviewsCars.Entities;

public class CarReview
{
    public Guid Id { get; set; }
    public ApplicationUser? User { get; set; }
    public Car? Car { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Text { get; set; } = null!;
    public string? ImageUrl { get; set; }
    public DateTime CreateAt { get; set; }
}