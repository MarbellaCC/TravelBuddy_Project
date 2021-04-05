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
            var travelerDays = _context.Days.Where(d => d.TravelerId == traveler.Id).OrderBy(d => d.Date);
            return View(travelerDays);
        }
        public ActionResult DayDetails() 
        {
            var applicationDbContext = _context.Travelers.Include(t => t.IdentityUser);
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var traveler = _context.Travelers.Where(t => t.IdentityUserID == userId).FirstOrDefault();
            var day = _context.Days.Where(d => d.TravelerId == traveler.Id).FirstOrDefault();
            var activitiesList = _context.Activities.Where(a => a.DayId == day.Id).OrderBy(a => a.Time);
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
            var applicationDbContext = _context.Travelers.Include(t => t.IdentityUser);
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var traveler = _context.Travelers.Where(t => t.IdentityUserID == userId).SingleOrDefault();
            if (traveler == null)
            {
                return NotFound();
            }

            return View(traveler);
        }
        public ActionResult ActivityDetails(int? id)
        {
            var activity = _context.Activities.Find(id);
            ViewData["APIKeys"] = APIKeys.GOOGLE_API_KEY;
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
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
                return RedirectToAction("ResultList");
            }
            return View(activity);
        }
        public async Task<IActionResult> ResultList()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var traveler = _context.Travelers.Where(t => t.IdentityUserID == userId).FirstOrDefault();
            var day = _context.Days.Where(d => d.TravelerId == traveler.Id).FirstOrDefault();
            var activity = _context.Activities.Where(a => a.DayId == day.Id && a.PlaceName == null).FirstOrDefault();
            if (ModelState.IsValid)
            {
                var locations = await _googlePlacesService.GetActivity(traveler, activity);
                ActivityResult result = new ActivityResult();
                ActivityResult result1 = new ActivityResult();
                ActivityResult result2 = new ActivityResult();
                ActivityResult result3 = new ActivityResult();
                ActivityResult result4 = new ActivityResult();
                for (int i = 0; i < locations.results.Length; i++)
                {
                    if (i == 0)
                    {
                        result.ActivityId = activity.Id;
                        result.PlaceName = locations.results[i].name;
                        result.GoogleRating = locations.results[i].rating;
                        result.ActivityLat = locations.results[i].geometry.location.lat;
                        result.ActivityLng = locations.results[i].geometry.location.lng;
                        result.Address = locations.results[i].vicinity;
                        _context.ActivityResults.Add(result);
                        _context.SaveChanges();
                        
                    }
                    else if(i == 1)
                    {
                        result1.ActivityId = activity.Id;
                        result1.PlaceName = locations.results[i].name;
                        result1.GoogleRating = locations.results[i].rating;
                        result1.ActivityLat = locations.results[i].geometry.location.lat;
                        result1.ActivityLng = locations.results[i].geometry.location.lng;
                        result1.Address = locations.results[i].vicinity;
                        _context.ActivityResults.Add(result1);
                        _context.SaveChanges();
                    }
                    else if (i == 2)
                    {
                        result2.ActivityId = activity.Id;
                        result2.PlaceName = locations.results[i].name;
                        result2.GoogleRating = locations.results[i].rating;
                        result2.ActivityLat = locations.results[i].geometry.location.lat;
                        result2.ActivityLng = locations.results[i].geometry.location.lng;
                        result2.Address = locations.results[i].vicinity;
                        _context.ActivityResults.Add(result2);
                        _context.SaveChanges();
                    }
                    else if (i == 2)
                    {
                        result2.ActivityId = activity.Id;
                        result2.PlaceName = locations.results[i].name;
                        result2.GoogleRating = locations.results[i].rating;
                        result2.ActivityLat = locations.results[i].geometry.location.lat;
                        result2.ActivityLng = locations.results[i].geometry.location.lng;
                        result2.Address = locations.results[i].vicinity;
                        _context.ActivityResults.Add(result2);
                        _context.SaveChanges();
                    }
                    else if (i == 3)
                    {
                        result3.ActivityId = activity.Id;
                        result3.PlaceName = locations.results[i].name;
                        result3.GoogleRating = locations.results[i].rating;
                        result3.ActivityLat = locations.results[i].geometry.location.lat;
                        result3.ActivityLng = locations.results[i].geometry.location.lng;
                        result3.Address = locations.results[i].vicinity;
                        _context.ActivityResults.Add(result3);
                        _context.SaveChanges();
                    }
                    else if (i == 4)
                    {
                        result4.ActivityId = activity.Id;
                        result4.PlaceName = locations.results[i].name;
                        result4.GoogleRating = locations.results[i].rating;
                        result4.ActivityLat = locations.results[i].geometry.location.lat;
                        result4.ActivityLng = locations.results[i].geometry.location.lng;
                        result4.Address = locations.results[i].vicinity;
                        _context.ActivityResults.Add(result4);
                        _context.SaveChanges();
                    }
                }
                var results = _context.ActivityResults.Where(r => r.ActivityId == activity.Id);
                return View(results);
            }
            return View();
        }
        public ActionResult AddActivity(int id, ActivityResult result)
        {
            ActivityResult activityResult = _context.ActivityResults.Find(id);
            Activity activity = _context.Activities.Find(activityResult.ActivityId);
            activity.Address = activityResult.Address;
            activity.GoogleRating = activityResult.GoogleRating;
            activity.PlaceName = activityResult.PlaceName;
            activity.ActivityLat = activityResult.ActivityLat;
            activity.ActivityLng = activityResult.ActivityLng;
            _context.Update(activity);
            _context.SaveChanges();
            return RedirectToAction("DayDetails");
        }
        public ActionResult ResultDetails(int? id)
        {
            var result = _context.ActivityResults.Find(id);
            ViewData["APIKeys"] = APIKeys.GOOGLE_API_KEY;
            if (result == null)
            {
                return NotFound();
            }

            return View(result);
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
        public async Task<IActionResult> EditActivity(int id, Activity activity)
        {
            if (id != activity.Id)
            {
                return NotFound();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var traveler = _context.Travelers.Where(t => t.IdentityUserID == userId).FirstOrDefault();
                    var activityToEdit = _context.Activities.Find(id);
                    activityToEdit.MaxDistance = activity.MaxDistance;
                    activityToEdit.Time = activity.Time;
                    activityToEdit.TypeOfActivity = activity.TypeOfActivity;
                    activityToEdit.TypeOfAdventureOrRestaurant = activity.TypeOfAdventureOrRestaurant;
                    Locations locations = await _googlePlacesService.GetActivity(traveler, activityToEdit);
                    activityToEdit.PlaceName = locations.results[0].name;
                    activityToEdit.GoogleRating = locations.results[0].rating;
                    activityToEdit.ActivityLat = locations.results[0].geometry.location.lat;
                    activityToEdit.ActivityLng = locations.results[0].geometry.location.lng;
                    activityToEdit.Address = locations.results[0].vicinity;
                    _context.Update(activityToEdit);
                    _context.SaveChanges();
                    return RedirectToAction("DayDetails");
                }
            }
            return View(activity);
        }
        public ActionResult RateActivity(int? id)
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
        public ActionResult RateActivity(int id, Activity activity)
        {
            if (id != activity.Id)
            {
                return NotFound();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var traveler = _context.Travelers.Where(t => t.IdentityUserID == userId).FirstOrDefault();
                    var day = _context.Days.Where(d => d.TravelerId == traveler.Id).FirstOrDefault();
                    var activityToEdit = _context.Activities.Find(id);
                    activityToEdit.RatingAdded = activity.RatingAdded;
                    activityToEdit.RatingList.Add(activity.RatingAdded);
                    double ratingTotaled = 0;
                    foreach (var rate in activityToEdit.RatingList){
                        ratingTotaled =+ rate;
                    }
                    activityToEdit.Rating = ratingTotaled / activityToEdit.RatingList.Count;
                    activityToEdit.Review = activity.Review;

                    _context.Update(activityToEdit);
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

        public ActionResult CreateLodging()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateLodging(Lodging hotel)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var traveler = _context.Travelers.Where(t => t.IdentityUserID == userId).FirstOrDefault();
            if (ModelState.IsValid)
            {
                hotel.TravelerId = traveler.Id;
                _context.Lodgings.Add(hotel);
                _context.SaveChanges();
                return RedirectToAction("HotelList");
            }
            return View(hotel);
        }
        public async Task<IActionResult> LodgingResults()
        {
            var applicationDbContext = _context.Travelers.Include(t => t.IdentityUser);
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var traveler = _context.Travelers.Where(t => t.IdentityUserID == userId).FirstOrDefault();
            var lodging = _context.Lodgings.Where(h => h.TravelerId == traveler.Id && h.Name == null).FirstOrDefault();
            if (ModelState.IsValid)
            {
                var locations = await _googlePlacesService.GetLodging(traveler, lodging);
                LodgingResult result = new LodgingResult();
                LodgingResult result1 = new LodgingResult();
                LodgingResult result2 = new LodgingResult();
                LodgingResult result3 = new LodgingResult();
                LodgingResult result4 = new LodgingResult();
                for (int i = 0; i < locations.results.Length; i++)
                {
                    if (i == 0)
                    {
                        result.LodgingId = lodging.Id;
                        result.Name = locations.results[i].name;
                        result.GoogleRating = locations.results[i].rating;
                        result.LodgingLat = locations.results[i].geometry.location.lat;
                        result.LodgingLng = locations.results[i].geometry.location.lng;
                        result.Address = locations.results[i].vicinity;
                        _context.LodgingResults.Add(result);
                        _context.SaveChanges();

                    }
                    else if (i == 1)
                    {
                        result1.LodgingId = lodging.Id;
                        result1.Name = locations.results[i].name;
                        result1.GoogleRating = locations.results[i].rating;
                        result1.LodgingLat = locations.results[i].geometry.location.lat;
                        result1.LodgingLng = locations.results[i].geometry.location.lng;
                        result1.Address = locations.results[i].vicinity;
                        _context.LodgingResults.Add(result1);
                        _context.SaveChanges();
                    }
                    else if (i == 2)
                    {
                        result2.LodgingId = lodging.Id;
                        result2.Name = locations.results[i].name;
                        result2.GoogleRating = locations.results[i].rating;
                        result2.LodgingLat = locations.results[i].geometry.location.lat;
                        result2.LodgingLng = locations.results[i].geometry.location.lng;
                        result2.Address = locations.results[i].vicinity;
                        _context.LodgingResults.Add(result2);
                        _context.SaveChanges();
                    }
                    else if (i == 2)
                    {
                        result2.LodgingId = lodging.Id;
                        result2.Name = locations.results[i].name;
                        result2.GoogleRating = locations.results[i].rating;
                        result2.LodgingLat = locations.results[i].geometry.location.lat;
                        result2.LodgingLng = locations.results[i].geometry.location.lng;
                        result2.Address = locations.results[i].vicinity;
                        _context.LodgingResults.Add(result2);
                        _context.SaveChanges();
                    }
                    else if (i == 3)
                    {
                        result3.LodgingId = lodging.Id;
                        result3.Name = locations.results[i].name;
                        result3.GoogleRating = locations.results[i].rating;
                        result3.LodgingLat = locations.results[i].geometry.location.lat;
                        result3.LodgingLng = locations.results[i].geometry.location.lng;
                        result3.Address = locations.results[i].vicinity;
                        _context.LodgingResults.Add(result3);
                        _context.SaveChanges();
                    }
                    else if (i == 4)
                    {
                        result4.LodgingId = lodging.Id;
                        result4.Name = locations.results[i].name;
                        result4.GoogleRating = locations.results[i].rating;
                        result4.LodgingLat = locations.results[i].geometry.location.lat;
                        result4.LodgingLng = locations.results[i].geometry.location.lng;
                        result4.Address = locations.results[i].vicinity;
                        _context.LodgingResults.Add(result4);
                        _context.SaveChanges();
                    }
                }
                var results = _context.LodgingResults.Where(r => r.LodgingId == lodging.Id);
                return View(results);
            }
            return View();
        }
        public ActionResult AddLodging(int id, LodgingResult result)
        {
            LodgingResult lodgingResult = _context.LodgingResults.Find(id);
            Lodging lodging = _context.Lodgings.Find(lodgingResult.LodgingId);
            lodging.Address = lodgingResult.Address;
            lodging.GoogleRating = lodgingResult.GoogleRating;
            lodging.Name = lodgingResult.Name;
            lodging.LodgingLat = lodgingResult.LodgingLat;
            lodging.LodgingLng = lodgingResult.LodgingLng;
            _context.Update(lodging);
            _context.SaveChanges();
            return RedirectToAction("Lodging");
        }

        public ActionResult LodgingDetails()
        {
            var applicationDbContext = _context.Travelers.Include(t => t.IdentityUser);
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var traveler = _context.Travelers.Where(t => t.IdentityUserID == userId).SingleOrDefault();
            var lodging = _context.Lodgings.Where(h => h.TravelerId == traveler.Id).SingleOrDefault();
            ViewData["APIKeys"] = APIKeys.GOOGLE_API_KEY;
            if (lodging == null)
            {
                return NotFound();
            }

            return View(lodging);
        }
    }
}
