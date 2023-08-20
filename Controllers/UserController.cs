using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WeddingPlanner2.Models;
using Microsoft.AspNetCore.Identity;
//Added for session check
using Microsoft.AspNetCore.Mvc.Filters;


namespace WeddingPlanner2.Controllers;


public class UserController : Controller
{
    private readonly ILogger<UserController> _logger;

    private readonly MyContext db;

    public UserController(ILogger<UserController> logger, MyContext context)
    {
        _logger = logger;
        db = context;
    }

    [HttpGet("")]
    public IActionResult Index()
    {

        return View();

    }

    // [HttpGet("Dashboard")]
    // public IActionResult Dashboard()
    // {
    //     return View("Dashboard");

    // }

    //-----REGISTER-----
    [HttpPost("/register")]

    public IActionResult Register(User newUser)
    {
        if (!ModelState.IsValid)
        {
            return View("Index");
        }
        PasswordHasher<User> hasher = new PasswordHasher<User>();

        newUser.Password = hasher.HashPassword(newUser, newUser.Password);

        db.Users.Add(newUser);
        db.SaveChanges();
        HttpContext.Session.SetInt32("UUID", newUser.UserId);

        return RedirectToAction("Dashboard");

    }
    //-----Login-----
    [HttpPost("/login")]
    public IActionResult Login(LoginUser userSubmission)
    {
        if (ModelState.IsValid)
        {
            User? userInDb = db.Users.FirstOrDefault(e => e.Email == userSubmission.LoginEmail);

            //Email Verification

            if (userInDb == null)
            {
                ModelState.AddModelError("LoginEmail", "Invalid Email");
                return View("Index");
            }

            //Password Verification
            PasswordHasher<LoginUser> hasher = new PasswordHasher<LoginUser>();
            var Result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.LoginPassword);
            if (Result == 0)
            {
                ModelState.AddModelError("LoginEmail", "Invalid Password");
                return View("Index");
            }
            else
            {
                HttpContext.Session.SetInt32("UUID", userInDb.UserId);
                return RedirectToAction("Dashboard");
            }

        }
        else
        {
            return View("Index");
        }
    }
    // ---logout----
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

// Name this anything you want with the word "Attribute" at the end
public class SessionCheckAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // Find the session, but remember it may be null so we need int?
        int? userId = context.HttpContext.Session.GetInt32("UUID");
        // Check to see if we got back null
        if (userId == null)
        {
            // Redirect to the Index page if there was nothing in session
            // "Home" here is referring to "HomeController", you can use any controller that is appropriate here
            context.Result = new RedirectToActionResult("Index", "Home", null);
        }
    }
}


