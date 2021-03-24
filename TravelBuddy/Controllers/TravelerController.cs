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

namespace TravelBuddy.Controllers
{
    public class TravelerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TravelerController(ApplicationDbContext context)
        {
            _context = context;
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
            var travelerDays = _context.Days.Where(d => d.Id == traveler.Id);
            return View(travelerDays);
        }

        // GET: Travelers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
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
        public async Task<IActionResult> Create(Traveler traveler)
        {
            if (ModelState.IsValid)
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                traveler.IdentityUserID = userId;
                _context.Add(traveler);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdentityUserID"] = new SelectList(_context.Users, "Id", "Id", traveler.IdentityUserID);
            return View(traveler);
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
                    travelerToEdit.Interests = traveler.Interests;
                    travelerToEdit.DestinationCity = traveler.DestinationCity;
                    travelerToEdit.DestinationState = traveler.DestinationState;
                    travelerToEdit.DestinationCountry = traveler.DestinationCountry;
                    travelerToEdit.ZipCode = traveler.ZipCode;
                    travelerToEdit.Lodging = traveler.Lodging;
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

        //// GET: Travelers/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var traveler = await _context.Travelers
        //        .Include(t => t.IdentityUser)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (traveler == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(traveler);
        //}

        //// POST: Travelers/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var traveler = await _context.Travelers.FindAsync(id);
        //    _context.Travelers.Remove(traveler);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool TravelerExists(int id)
        {
            return _context.Travelers.Any(e => e.Id == id);
        }
    }
}
