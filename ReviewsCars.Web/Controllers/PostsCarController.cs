using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReviewsCars.Data;
using ReviewsCars.Entities;

namespace ReviewsCars.Web.Controllers;

public class PostsCarController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public PostsCarController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var posts = await _context.PostCars.ToListAsync();
        return View(posts);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var post = await _context.PostCars
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (post != null)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            bool isPostBelongsUser = user != null && user.Id == post.User?.Id;
        
            return View((post, isPostBelongsUser));
        }

        return RedirectToAction("Index");
    }
    
    
    public IActionResult Create()
    {
        return View();
    }
    
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(PostCar postCar)
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        postCar.CreateAt = DateTime.Now;
        postCar.User = user;
        _context.PostCars.Add(postCar);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        var post = await _context.PostCars.FirstOrDefaultAsync(p => p.Id == id);
        if (post != null)
        {
            _context.PostCars.Remove(post);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Index");
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var post = await _context.PostCars.FirstOrDefaultAsync(p => p.Id == id);
        if (post != null)
        {
            return View(post);
        }

        return RedirectToAction("Index");
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Edit(PostCar post)
    {
        _context.PostCars.Update(post);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index", "PostsCar", new { id = post.Id });
    }
}