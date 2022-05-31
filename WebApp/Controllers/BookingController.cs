using System.Collections;
using System.Collections.Immutable;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WebApp.Models;

namespace WebApp.Controllers;

public class BookingController : Controller
{

    public CheckBoxListItem GetPickHoursViewModel(DateTime date)
    {
        List<PotentialBookings> potentialBookings = new List<PotentialBookings>();
        for(int i = 9; i < 18; i++)
        {
            potentialBookings.Add(new PotentialBookings()
            {
                TimeOfSession = i + ":00"
            });

        }
        var bookingViewModel = new CheckBoxListItem()
        {
            booking = new BookingViewModel()
            {
                PotentialBookingsList = potentialBookings,
                Date = date
            }
        };
        
        return bookingViewModel;
    }
    
    
    
    [HttpPost]
    public IActionResult PickHours(DateTime date)
    {
        var bookingViewModel = GetPickHoursViewModel(date);

        return View(bookingViewModel);
    }
    
    [HttpPost]
    public IActionResult Create(List<string> checkbox, DateTime dateOfSession)
    {

            var client = GetHttpClient();
            

            foreach (var time in checkbox)
            {
                var dateAndTimeOfSession = formatTime(time, dateOfSession);
                
                var bookingForCreation = new bookingDto()
                {
                    AdminAccountId = User.Identity.Name,
                    DateOfSession = dateAndTimeOfSession,
                    Booked = false,
                    usernameOfUser = "unbooked",
                    Created = DateTime.Now
                };
                
                postToApi(bookingForCreation, client);
            }
            
            return RedirectToAction("ChooseSessions");
    }

    private async void postToApi(bookingDto bookingForCreation, HttpClient client)
    {
        var booking = JsonSerializer.Serialize(bookingForCreation);

        var requestContent = new StringContent(booking, Encoding.UTF8, "application/json");

        var response = await client.PostAsync("api/Booking", requestContent);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        var createdCompany = JsonSerializer.Deserialize<bookingDto>(content);
    }

    private DateTime formatTime(string time, DateTime date)
    {
        string formattedDate = date.ToString("yyyy-M-d") + " " + time + ":00";

        DateTime formattedDateTime = Convert.ToDateTime(formattedDate, System.Globalization.CultureInfo.InvariantCulture);

        return formattedDateTime;
    }

    private HttpClient GetHttpClient()
    {
        HttpClientHandler clientHandler = new HttpClientHandler();
        clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
        {
            return true;
        };

        HttpClient client = new HttpClient(clientHandler);
        
        client.BaseAddress = new Uri("https://localhost:7252");
        
        client.BaseAddress = new Uri("https://localhost:7252");
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));





        return client;
    }
    
    [HttpGet]
    public async Task<IActionResult> ViewBookedSessions(string sortOrder, string searchString)
    {
        ViewData["DateSortParm"] = sortOrder == "Date" ? "dateMostRecent" : "Date";
        ViewData["CurrentFilter"] = searchString;
        
        List<bookingDto> Bookings = new List<bookingDto>();

        var client = GetHttpClient();

        var test = "jameshall8@outlook.com";
        
        HttpResponseMessage response = await client.GetAsync($"api/Booking/jameshall8@outlook.com");
        response.EnsureSuccessStatusCode();

        
        
        if (response.IsSuccessStatusCode)
        {
            var bookings = await response.Content.ReadFromJsonAsync<IEnumerable<bookingDto>>();
            
            foreach (var booking in bookings)
            {
                Bookings.Add(booking);
            }
            
            if (!String.IsNullOrEmpty(searchString))
            {
                Bookings = Bookings.Where(s => s.usernameOfUser.Contains(searchString)
                                               || s.AdminAccountId.Contains(searchString)).ToList();
            }
            
            switch (sortOrder)
            {
                case "Date":
                    Bookings = Bookings.OrderBy(s => s.DateOfSession).ToList();
                    break;
                case "dateMostRecent":
                    Bookings = Bookings.OrderByDescending(s => s.DateOfSession).ToList();
                    break;

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