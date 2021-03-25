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
    public class DayController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DayController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Days
        public async Task<IActionResult> Index()
        {
            return View(await _context.Days.ToListAsync());
        }

        // GET: Days/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var day = _context.Days.Find(id);
            if (day == null)
            {
                return NotFound();
            }

            return View(day);
        }

        // GET: Days/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Days/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Day day)
        {
            if (ModelState.IsValid)
            {
                var applicationDbContext = _context.Travelers.Include(t => t.IdentityUser);
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var traveler = _context.Travelers.Where(t => t.IdentityUserID == userId).FirstOrDefault();
                _context.Days.Add(day);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Traveler");
            }
            return View(day);
        }

        // GET: Days/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var day = _context.Days.Find(id);
            if (day == null)
            {
                return NotFound();
            }
            return View(day);
        }

        // POST: Days/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Day day)
        {
            if (id != day.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var dayToEdit = _context.Days.Find(id);
                    dayToEdit.Date = day.Date;
                    dayToEdit.Name = day.Name;
                    _context.Update(dayToEdit);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DayExists(day.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Traveler");
            }
            return View(day);
        }

        // GET: Days/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var day = await _context.Days
                .FirstOrDefaultAsync(m => m.Id == id);
            if (day == null)
            {
                return NotFound();
            }

            return View(day);
        }

        // POST: Days/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var day = await _context.Days.FindAsync(id);
            _context.Days.Remove(day);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DayExists(int id)
        {
            return _context.Days.Any(e => e.Id == id);
        }
    }
}
