using Microsoft.Build.Framework;

namespace WebApp.Models;

public class Booking
{
    
    public int Id { get; set; }
    
    

    public string AdminAccountId { get; set; }
    [Required]

    public DateTime DateOfSession { get; set; }
    [Required]
    
    public bool Booked { get; set; }
    [Required]
    
    public string usernameOfUser { get; set; }
    
    public DateTime Created { get; set; }
    
    public DateTime? Completed { get; set; }

}