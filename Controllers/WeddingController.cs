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


    [HttpGet("Dashboard")]
    public IActionResult Dashboard()
    {
    List<Wedding> weddings = db.Weddings.Include(v => v.Creator).Include(l => l.WeddingGuests).ToList();        
        //passing weddingss down to the view file
        return View("CREATE ALL PAGE", weddings);

    }




}