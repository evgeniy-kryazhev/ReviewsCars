namespace ReviewsCars.Web.Models;

public class CreateCarReviewViewModel
{
    public string Name { get; set; } = null!;
    public string? ImageUrl { get; set; }
    public Guid CarId { get; set; }
    public string Text { get; set; } = null!;
}