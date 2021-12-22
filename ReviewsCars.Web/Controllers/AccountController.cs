using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReviewsCars.Data;
using ReviewsCars.Entities;
using ReviewsCars.Web.Models;

namespace ReviewsCars.Web.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ApplicationDbContext _context;

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
        ApplicationDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
    }

    [Authorize]
    public async Task<ActionResult<UserViewModel>> Index()
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        var posts = await _context.CarReviews
            .Include(p => p.User)
            .Where(p => p.User!.Id == user.Id)
            .ToListAsync();

        var userViewModel = new UserViewModel
        {
            User = user,
            PostCars = posts,
        };
        return View(userViewModel);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<UserViewModel>> Index(UserViewModel userViewModel)
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);

        user.UserName = userViewModel.User.UserName;

        await _userManager.UpdateAsync(user);
        return RedirectToAction("Index");
    }

    public async Task<ActionResult<UserViewModel>> Profile(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        var posts = await _context.CarReviews
            .Include(p => p.User)
            .Where(p => p.User!.Id == user.Id)
            .ToListAsync();

        var userViewModel = new UserViewModel
        {
            User = user,
            PostCars = posts,
        };
        return View(userViewModel);
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult<LoginViewModel>> Login(LoginViewModel loginViewModel)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(loginViewModel.Email);
            var checkPassword = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
            if (user != null && checkPassword)
            {
                await _signInManager.SignInAsync(user, false);
                if (!string.IsNullOrEmpty(loginViewModel.ReturnUrl) && Url.IsLocalUrl(loginViewModel.ReturnUrl))
                {
                    return Redirect(loginViewModel.ReturnUrl);
                }

                return RedirectToAction("Index", "CarReview");
            }

            if (user == null)
            {
                ModelState.AddModelError("", "Пользватель не зарегистрирован");
            }

            if (!checkPassword)
            {
                ModelState.AddModelError("", "Неправильный пароль");
            }
            
            return View(loginViewModel);
        }

        return View(loginViewModel);
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult<RegisterViewModel>> Register(RegisterViewModel registerViewModel)
    {
        if (ModelState.IsValid)
        {
            ApplicationUser user = new ApplicationUser
            {
                Email = registerViewModel.Email,
                UserName = registerViewModel.Email,
            };

            var result = await _userManager.CreateAsync(user, registerViewModel.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(registerViewModel);
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}