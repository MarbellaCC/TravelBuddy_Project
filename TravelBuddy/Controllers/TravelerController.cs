using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TravelBuddy.Data;
using TravelBuddy.Models;
using TravelBuddy.Services;

namespace TravelBuddy.Controllers
{
    public class TravelerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly GeocodingService _geocodingSerice;
        private readonly PlacesService _googlePlacesService;

        public TravelerController(ApplicationDbContext context, GeocodingService geocodingService, PlacesService googlePlacesService)
        {
            _context = context;
            _geocodingSerice = geocodingService;
            _googlePlacesService = googlePlacesService;
        }

        // GET: Travelers
        public ActionResult Index()
        {
            var applicationDbContext = _context.Travelers.Include(t => t.IdentityUser);
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var traveler = _context.Travelers.Where(t => t.IdentityUserID == userId).FirstOrDefault();
            if(traveler == null)
            {
                return RedirectToAction("Create");
            }
            var travelerDays = _context.Days.Where(d => d.TravelerId == traveler.Id);
            return View(travelerDays);
        }
        public ActionResult DayDetails() 
        {
            var applicationDbContext = _context.Travelers.Include(t => t.IdentityUser);
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var traveler = _context.Travelers.Where(t => t.IdentityUserID == userId).FirstOrDefault();
            var day = _context.Days.Where(d => d.TravelerId == traveler.Id).FirstOrDefault();
            var activitiesList = _context.Activities.Where(a => a.DayId == day.Id);
            return View(activitiesList);
        }
        public ActionResult InterestList()
        {
            var applicationDbContext = _context.Travelers.Include(t => t.IdentityUser);
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var traveler = _context.Travelers.Where(t => t.IdentityUserID == userId).FirstOrDefault();
            var travelerInterests = _context.Interests.Where(i => i.TravelerId == traveler.Id);
            return View(travelerInterests);
        }

        // GET: Travelers/Details/5
        public ActionResult Details()
        {
            //var traveler = await _context.Travelers.Include(t => t.IdentityUser).FirstOrDefaultAsync(m => m.Id == id);
            var applicationDbContext = _context.Travelers.Include(t => t.IdentityUser);
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var traveler = _context.Travelers.Where(t => t.IdentityUserID == userId).SingleOrDefault();
            if (traveler == null)
            {
                return NotFound();
            }

            return View(traveler);
        }

        // GET: Travelers/Create
        public IActionResult Create()
        {
            ViewData["IdentityUserID"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Travelers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Traveler traveler, Locations location)
        {
            if (ModelState.IsValid)
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                traveler.IdentityUserID = userId;
                var travelerLatLong = await _geocodingSerice.GetGeocoding(traveler);
                _context.Add(travelerLatLong);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdentityUserID"] = new SelectList(_context.Users, "Id", "Id", traveler.IdentityUserID);
            return View(traveler);
        }
        public ActionResult CreateInterest()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateInterest(Interest interest)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var traveler = _context.Travelers.Where(t => t.IdentityUserID == userId).FirstOrDefault();
            if (ModelState.IsValid)
            {
                interest.TravelerId = traveler.Id;
                _context.Interests.Add(interest);
                _context.SaveChanges();
                return RedirectToAction("InterestList");
            }
            return View(interest);
        }
        public ActionResult CreateDay()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDay(Day day)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var traveler = _context.Travelers.Where(t => t.IdentityUserID == userId).FirstOrDefault();
            if (ModelState.IsValid)
            {
                day.TravelerId = traveler.Id;
                _context.Days.Add(day);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(day);
        }
        public ActionResult CreateActivity()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateActivity(Activity activity)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var traveler = _context.Travelers.Where(t => t.IdentityUserID == userId).FirstOrDefault();
            var day = _context.Days.Where(d => d.TravelerId == traveler.Id).FirstOrDefault();
            if (ModelState.IsValid)
            {
                activity.DayId = day.Id;
                _context.Activities.Add(activity);
                _context.SaveChanges();
                return RedirectToAction("DayDetails");
            }
            return View(activity);
        }

        // GET: Travelers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var traveler = await _context.Travelers.FindAsync(id);
            if (traveler == null)
            {
                return NotFound();
            }
            ViewData["IdentityUserID"] = new SelectList(_context.Users, "Id", "Id", traveler.IdentityUserID);
            return View(traveler);
        }

        // POST: Travelers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Traveler traveler)
        {
            if (id != traveler.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Traveler travelerToEdit = _context.Travelers.Find(id);
                    travelerToEdit.FirstName = traveler.FirstName;
                    travelerToEdit.LastName = traveler.LastName;
                    travelerToEdit.DestinationCity = traveler.DestinationCity;
                    travelerToEdit.DestinationState = traveler.DestinationState;
                    travelerToEdit.DestinationCountry = traveler.DestinationCountry;
                    travelerToEdit.ZipCode = traveler.ZipCode;
                    travelerToEdit.Latitude = traveler.Latitude;
                    travelerToEdit.Longitude = traveler.Longitude;
                    await _geocodingSerice.GetGeocoding(travelerToEdit);
                    _context.Update(travelerToEdit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TravelerExists(traveler.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdentityUserID"] = new SelectList(_context.Users, "Id", "Id", traveler.IdentityUserID);
            return View(traveler);
        }
        public ActionResult EditDay(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var day = _context.Days.Find(id);
            if(day == null)
            {
                return NotFound();
            }
            return View(day);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDay(int id, Day day)
        {
            if(id != day.Id)
            {
                return NotFound();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var dayToEdit = _context.Days.Find(id);
                    dayToEdit.Date = day.Date;
                    dayToEdit.Name = day.Name;
                    _context.Update(dayToEdit);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(day);
        }
        public ActionResult EditActivity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var activity = _context.Activities.Find(id);
            if (activity == null)
            {
                return NotFound();
            }
            return View(activity);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditActivity(int id, Activity activity)
        {
            if (id != activity.Id)
            {
                return NotFound();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var dayToEdit = _context.Activities.Find(id);
                    dayToEdit.Time = activity.Time;
                    dayToEdit.PlaceName = activity.PlaceName;
                    dayToEdit.Rating = activity.Rating;
                    _context.Update(dayToEdit);
                    _context.SaveChanges();
                    return RedirectToAction("DayDetails");
                }
            }
            return View(activity);
        }
        public async Task<IActionResult> DeleteDay(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var day = await _context.Days
                .Include(d => d.Traveler)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (day == null)
            {
                return NotFound();
            }

            return View(day);
        }

        // POST: Days/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDay(int id)
        {
            var day = await _context.Days.FindAsync(id);
            _context.Days.Remove(day);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> DeleteActivity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.Activities
                .Include(a => a.Day)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        // POST: Days/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteActivity(int id)
        {
            var activity = await _context.Activities.FindAsync(id);
            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(DayDetails));
        }
        public async Task<IActionResult> DeleteInterest(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var interest = await _context.Interests
                .Include(i => i.Traveler)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (interest == null)
            {
                return NotFound();
            }

            return View(interest);
        }

        // POST: Days/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteInterest(int id)
        {
            var interest = await _context.Interests.FindAsync(id);
            _context.Interests.Remove(interest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(InterestList));
        }
        private bool TravelerExists(int id)
        {
            return _context.Travelers.Any(e => e.Id == id);
        }

        public ActionResult CreateHotel()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateHotel(Hotel hotel)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var traveler = _context.Travelers.Where(t => t.IdentityUserID == userId).FirstOrDefault();
            if (ModelState.IsValid)
            {
                hotel.TravelerId = traveler.Id;
                Locations locations = await _googlePlacesService.GetHotels(traveler, hotel);
                hotel.HotelName = locations.results[3].name;
                hotel.HotelAddress = locations.results[3].vicinity;
                hotel.HotelPhotos = locations.results[3].photos[0].html_attributions[0];
                hotel.HotelGoogleRating = locations.results[3].rating;
                hotel.HotelLat = locations.results[3].geometry.location.lat;
                hotel.HotelLng = locations.results[3].geometry.location.lng;
                _context.Hotels.Add(hotel);
                _context.SaveChanges();
                return RedirectToAction("HotelList");
            }
            return View(hotel);
        }
        public ActionResult HotelList()
        {
            var applicationDbContext = _context.Travelers.Include(t => t.IdentityUser);
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var traveler = _context.Travelers.Where(t => t.IdentityUserID == userId).FirstOrDefault();
            var travelerHotel = _context.Hotels.Where(h => h.TravelerId == traveler.Id);
            return View(travelerHotel);
        }
        public ActionResult EditHotel(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var hotel = _context.Hotels.Find(id);
            if (hotel == null)
            {
                return NotFound();
            }
            return View(hotel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditHotel(int id)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var traveler = _context.Travelers.Where(t => t.IdentityUserID == userId).FirstOrDefault();
            var hotelToEdit = _context.Hotels.Find(id);
            Locations locations = await _googlePlacesService.GetHotels(traveler, hotelToEdit);
            hotelToEdit.HotelName = locations.results[3].name;
            hotelToEdit.HotelAddress = locations.results[3].vicinity;
            hotelToEdit.HotelPhotos = locations.results[3].photos[0].html_attributions[0];
            hotelToEdit.HotelGoogleRating = locations.results[3].rating;
            hotelToEdit.HotelLat = locations.results[3].geometry.location.lat;
            hotelToEdit.HotelLng = locations.results[3].geometry.location.lng;
            _context.Update(hotelToEdit);
            await _context.SaveChangesAsync();
            return RedirectToAction("HotelList");
        }
        public ActionResult HotelDetails()
        {
            var applicationDbContext = _context.Travelers.Include(t => t.IdentityUser);
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var traveler = _context.Travelers.Where(t => t.IdentityUserID == userId).SingleOrDefault();
            var hotel = _context.Hotels.Where(h => h.TravelerId == traveler.Id).SingleOrDefault();
            ViewData["APIKeys"] = APIKeys.GOOGLE_API_KEY;
            if (hotel == null)
            {
                return NotFound();
            }

            return View(hotel);
        }
    }
}
