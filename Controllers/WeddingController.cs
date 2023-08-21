using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WeddingPlanner2.Models;

//Added for Include:
using Microsoft.EntityFrameworkCore;

//ADDED for session check
using Microsoft.AspNetCore.Mvc.Filters;

namespace WeddingPlanner2.Controllers;

[SessionCheck]
public class WeddingController : Controller
{
    private readonly ILogger<WeddingController> _logger;

    // Add field - adding context into our class // "db" can eb any name
    private MyContext db;

    public WeddingController(ILogger<WeddingController> logger, MyContext context)
    {
        _logger = logger;
        db = context;
    }

    //Use RESTful routing 
    // -------Dashboard-------
    [HttpGet("weddings")]
    public IActionResult Index()
    {
        {
            //Many to many, add guests list and invited guests, to get 3 tables of information
            List<Wedding> weddings = db.Weddings.Include(g => g.Guests).ThenInclude(u => u.User).Include(c => c.Creator).ToList();
            return View("All");
        }
    }

    //-------Display Page For Wedding Form------
    [HttpGet("weddings/new")]
    public IActionResult New()
    {
        return View("New"); //<--- HTML page to see our new, displayed wedding
    }

    //-------Add a wedding into db--------
    [HttpPost("weddings/create")]
    public IActionResult Create(Wedding newWedding) //<----- Method to create a wedding and add in db
    {
        if (ModelState.IsValid) //<--- validation 
        {
            db.Add(newWedding);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        else
        {
            // call the method to render the new page
            return View("New");
        }
    }












[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public class SessionCheckAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Find the session, but remember it may be null so we need int?
            int? userId = context.HttpContext.Session.GetInt32("UUID");
            // Check to see if we got back null
            if(userId == null)
            {
                // Redirect to the Index page if there was nothing in session
                // "Home" here is referring to "HomeController", you can use any controller that is appropriate here
                context.Result = new RedirectToActionResult("Index", "User", null);
            }
        }
    }
}