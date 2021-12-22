using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReviewsCars.Data;
using ReviewsCars.Entities;
using ReviewsCars.Web.Models;

namespace ReviewsCars.Web.Controllers;

public class CarReviewController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public CarReviewController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var reviews = await _context.CarReviews.ToListAsync();
        return View(reviews);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var review = await _context.CarReviews
            .Include(p => p.User)
            .Include(p => p.Car)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (review != null)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            bool isPostBelongsUser = user != null && user.Id == review.User?.Id;

            return View((review, isPostBelongsUser));
        }

        return RedirectToAction("Index");
    }


    public async Task<IActionResult> Create()
    {
        var cars = await _context.Cars.ToListAsync();
        ViewData["Cars"] = new SelectList(cars, "Id", "Name");
        return View();
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(CreateCarReviewViewModel review)
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        var car = await _context.Cars.FirstOrDefaultAsync(c => c.Id == review.CarId);
        CarReview carReview = new CarReview
        {
            Id = Guid.NewGuid(),
            User = user,
            Car = car,
            Name = review.Name,
            Text = review.Text,
            ImageUrl = review.ImageUrl,
            CreateAt = DateTime.Now
        };
        _context.CarReviews.Add(carReview);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        var review = await _context.CarReviews.FirstOrDefaultAsync(p => p.Id == id);
        if (review != null)
        {
            _context.CarReviews.Remove(review);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Index");
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var review = await _context.CarReviews.FirstOrDefaultAsync(p => p.Id == id);
        if (review != null)
        {
            return View(review);
        }

        return RedirectToAction("Index");
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Edit(CarReview review)
    {
        _context.CarReviews.Update(review);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index", "CarReview", new {id = review.Id});
    }
}