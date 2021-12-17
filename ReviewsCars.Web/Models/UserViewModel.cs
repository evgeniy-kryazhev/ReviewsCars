using ReviewsCars.Entities;

namespace ReviewsCars.Web.Models;

public class UserViewModel
{
    public ApplicationUser User { get; set; } = null!;
    public IEnumerable<PostCar>? PostCars { get; set; }
}