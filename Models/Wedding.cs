#pragma warning disable CS8618

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingPlanner2.Models;
public class Wedding
{        
    [Key]        
    public int WeddingId { get; set; }
    
    [Required] 
    [MinLength(2, ErrorMessage = "Wedder one must be at least 2 characters long.")]       
    public string WedderOne { get; set; }
    
    [Required]  
    [MinLength(2, ErrorMessage = "Wedder two must be at least 2 characters long.")]      
    public string WedderTwo { get; set; } 

    [Required]
    [FutureWeddingDate]
    public DateTime Date { get; set; } 

    [Required]
    public string Address { get; set; } 

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    
    //-----foreign key/one-to-many connection------
    public int UserId {get; set;}

    public User? Creator {get; set;}


    //! ====== adding many to many - user to weddings linking ========

    //-------many to many connection----------
    public List<WeddingGuest> Guests {get; set;} = new List<WeddingGuest>();

}


// CUSTOM VALIDATION FOR WEDDING DATE
public class FutureWeddingDateAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        DateTime Now = DateTime.Now;
        DateTime Input = (DateTime)value;


        if (Input < Now)
        {
            return new ValidationResult("Wedding Date Must Be In The Future.");
        } else {
            return ValidationResult.Success;
        }
    }
}