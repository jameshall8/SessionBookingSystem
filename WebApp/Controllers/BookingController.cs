using System.Collections;
using System.Collections.Immutable;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers;

public class BookingController : Controller
{


    [HttpPost]
    public IActionResult PickHours(DateTime date)
    {
     List<PotentialBookings> _potentialBookings = new List<PotentialBookings>();
     for(int i = 9; i < 18; i++)
     {
         _potentialBookings.Add(new PotentialBookings()
         {
             TimeOfSession = i + ":00"
         });

     }
     var bookingViewModel = new BookingViewModel()
     {
         PotentialBookingsList = _potentialBookings,
         Date = date
     };
     return View(bookingViewModel);
    }
    
    public ActionResult create()
    {
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> ViewBookedSessions()
    {
        List<string> Bookings = new List<string>();
        HttpClient client = new();
        client.BaseAddress = new Uri("https://localhost:7252");
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        HttpResponseMessage response = await client.GetAsync("api/BookingApi");
        response.EnsureSuccessStatusCode();

        if (response.IsSuccessStatusCode)
        {
            var bookings = await response.Content.ReadFromJsonAsync<IEnumerable<bookingDto>>();

            foreach (var booking in bookings)
            {
                Bookings.Add(booking.DateOfSession.ToString());
            }
        }

        return View(Bookings);


    }
    
    



    // GET
    public IActionResult ChooseSessions()
    {
        

        DateTime dateTime = new DateTime();

        ViewData["Title"] = "Open Sessions";
        
        var todaysDate = DateTime.Now.ToString("yyyy-M-d");
        var DateNow = DateTime.Now;
        var dateInThreeMonths = DateNow.AddMonths(3).ToString("yyyy-M-d");

        ViewData["TodaysDate"] = todaysDate;

        ViewData["DateInThreeMonths"] = dateInThreeMonths;
        


        return View();
    }
    
    
    // [HttpPost]
    // public ActionResult AddBooking(Booking booking) { 
    //     //your code 
    //     //do stuff
    //
    //     return RedirectToAction("ChooseSessions");
    // }

    
    
}