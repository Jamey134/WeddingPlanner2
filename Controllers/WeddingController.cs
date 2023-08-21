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
    private readonly MyContext db;

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
            return View("All", weddings);
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
            newWedding.UserId = (int)HttpContext.Session.GetInt32("UUID");
            db.Weddings.Add(newWedding);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        else
        {
            // call the method to render the new page
            return View("New");
        }
    }
//---------View One (READ) Wedding----------
    [HttpGet("weddings/{weddingId}")]
    public IActionResult Details(int weddingId)
    {
        //Include many to many guests list
        Wedding? wedding = db.Weddings.Include(g => g.Guests).ThenInclude(u => u.User).FirstOrDefault(w => w.WeddingId == weddingId);

        if (wedding == null)
        {
            return RedirectToAction("Index");
        }
        return View("Details", wedding);
    }

// ---------Update a wedding---------------
    [HttpGet("wedding/{id}/edit")]

    //add id in parameter*
    public IActionResult Edit(int id)
    {
        // confirm it matches the id we're passing in above*
    Wedding? weddings = db.Weddings.Include(v => v.Creator).FirstOrDefault(p => p.WeddingId == id);

    //confirming the creator of the wedding is editing 
    if (weddings == null || weddings.UserId != HttpContext.Session.GetInt32("UUID")) //<--- (Session check)
    {
        return RedirectToAction("Index");
    }
        //passing weddings data down to view
        return View("Edit", weddings);
    }

    //---------Delete a wedding---------
    [HttpPost("wedding/{id}/delete")]
    public IActionResult Delete(int id)

    
    {
        Wedding? weddings = db.Weddings.FirstOrDefault(d => d.WeddingId == id);

        //Tostop from deleting other users' data
        if(weddings == null || weddings.UserId != HttpContext.Session.GetInt32("UUID")) 
        {
            return RedirectToAction("Index");
        }

        db.Weddings.Remove(weddings);
        db.SaveChanges();
        return RedirectToAction("Index");
    }

    //--------RSVP For Wedding-------
    [HttpPost("weddings/{id}/rsvp")]
    public IActionResult RSVP(int id)
    {
        int? userId = HttpContext.Session.GetInt32("UUID");

        if (userId == null) 
        {
            return RedirectToAction("Index");
        }
        
        //must equal for session check
        WeddingGuest? guestRSVP = db.WeddingGuests.FirstOrDefault(u => u.UserId == userId.Value && u.WeddingId == id);

        if(guestRSVP != null)
        {
            db.WeddingGuests.Remove(guestRSVP);
        }
        else
        {
            WeddingGuest newRSVP = new WeddingGuest()
            {
                WeddingId = id,
                UserId = userId.Value 
            };
            Console.WriteLine(newRSVP);
            db.WeddingGuests.Add(newRSVP);
        }
        db.SaveChanges();
        return RedirectToAction("Index", "Wedding");

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
            if (userId == null)
            {
                // Redirect to the Index page if there was nothing in session
                // "Home" here is referring to "HomeController", you can use any controller that is appropriate here
                context.Result = new RedirectToActionResult("Index", "User", null);
            }
        }
    }
}