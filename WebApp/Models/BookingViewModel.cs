namespace WebApp.Models;

public class BookingViewModel
{
    public int Id { get; set; }
    
    

    public string AdminAccountId { get; set; }

    public DateTime DateOfSession { get; set; }
    
    public DateTime timeOfSession { get; set; }

    
    public bool Booked { get; set; }
    
    public string usernameOfUser { get; set; }
    
    public DateTime Created { get; set; }
    
    public DateTime? Completed { get; set; }
    
    public System.Collections.Generic.List<WebApp.Models.PotentialBookings> PotentialBookingsList { get; set; }
    
    public DateTime Date { get; set; }

    public static Booking Booking { get; set; }

    public List<CheckBoxListItem> checkboxes { get; set; }
    
    

}