#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WeddingPlanner2.Models;

public class WeddingGuest
{
    [Key]
    public int WeddingGuestId { get; set; }

    //------User Foreign Key-------
    public int UserId { get; set; }
    public User? User { get; set; }


    //------Wedding Foreign Key-------
    public int WeddingId { get; set; }
    public Wedding? Wedding { get; set; }

    public DateTime CreatedAt {get;set;} = DateTime.Now;        
    public DateTime UpdatedAt {get;set;} = DateTime.Now;

}