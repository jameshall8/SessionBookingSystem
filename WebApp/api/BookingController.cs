using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly BookingDbContext _context;

        public BookingController(BookingDbContext context)
        {
            _context = context;
        }

        // GET: api/Booking
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
          if (_context.Bookings == null)
          {
              return NotFound();
          }
            return await _context.Bookings.ToListAsync();
        }
        
        [HttpGet("{AdminAccountId}")]
        [ActionName("GetBookingByEmail")]
        public async Task<List<Booking>> GetBookingByEmail(string AdminAccountId)
        {
            // if (_context.Bookings == null)
            // {
            //     return NotFound();
            // }
            
            List<Booking> booking = await _context.Bookings.Where(x => x.AdminAccountId == AdminAccountId).ToListAsync();
            
            

            // if (booking == null)
            // {
            //     return NotFound();
            // }

            return booking;
        }
        
        [HttpGet("{searchString, email}")]
        [ActionName("GetBookingBySearch")]
        [Route("api/[controller]/[action]/{searchString}/{email}")]
        public async Task<List<Booking>> GetBookingBySearch(string searchString, string email)
        {
             if (_context.Bookings == null)
             {
                 return new List<Booking>();
             }
            
             List<Booking> booking = await _context.Bookings.Where(x => x.DateOfSession.ToString().Substring(0, 10).Contains(searchString) && x.AdminAccountId == email).ToListAsync();
            
            

             if (booking == null) 
             {
                 return new List<Booking>();
             }

             return booking;
        }
        
        [HttpGet("{date}")]
        [ActionName("GetBookingByDate")]
        [Route("api/[controller]/[action]/{date}")]
        public async Task<List<Booking>> GetBookingByDate(DateTime date)
        {
            if (_context.Bookings == null)
            {
                return new List<Booking>();
            }
            
            
            List<Booking> booking = await _context.Bookings.Where(x => x.DateOfSession == date).ToListAsync();
            
            

            if (booking == null) 
            {
                return new List<Booking>();
            }

            return booking;
        }
        
        
        

        // GET: api/Booking/5
        [HttpGet("{Id}")]
        [ActionName("GetBookingById")]
        public async Task<ActionResult<Booking>> GetBookingById(int Id)
        {
          if (_context.Bookings == null)
          {
              return NotFound();
          }
          
            var booking = await _context.Bookings.FindAsync(Id);
        
        
            if (booking == null)
            {
                return NotFound();
            }
        
            return booking;
        }

        // PUT: api/Booking/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooking(int id, Booking booking)
        {
            if (id != booking.Id)
            {
                return BadRequest();
            }

            _context.Entry(booking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Booking
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Booking>> PostBooking(Booking booking)
        {
            
            // string AdminAccountId, DateTime DateOfSession, bool Booked, string usernameOfUser

          if (_context.Bookings == null)
          {
              return Problem("Entity set 'BookingDbContext.Bookings'  is null.");
          }
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBookingById", new { id = booking.Id }, booking);
        }

        // DELETE: api/Booking/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            if (_context.Bookings == null)
            {
                return NotFound();
            }
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookingExists(int id)
        {
            return (_context.Bookings?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
