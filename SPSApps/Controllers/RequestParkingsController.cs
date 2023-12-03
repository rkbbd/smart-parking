using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SPSApps.Models;
using SPSApps.Models.Parking;

namespace SPSApps.Controllers
{
    public class RequestParkingsController : Controller
    {
        private readonly DatabaseEntity _context;

        public RequestParkingsController(DatabaseEntity context)
        {
            _context = context;
        }

        // GET: RequestParkings
        public async Task<IActionResult> Index()
        {
            var databaseEntity = _context.RequestParkings.Include(r => r.Building);
            return View(await databaseEntity.ToListAsync());
        }

        // GET: RequestParkings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RequestParkings == null)
            {
                return NotFound();
            }

            var requestParking = await _context.RequestParkings
                .Include(r => r.Building)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (requestParking == null)
            {
                return NotFound();
            }

            return View(requestParking);
        }

        // GET: RequestParkings/Create
        public IActionResult Create()
        {
            ViewData["BuildingId"] = new SelectList(_context.Buildings, "Id", "Id");
            return View();
        }

        // POST: RequestParkings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BuildingId,Fair,IsActive,AccessTime,Id,Status,CreatedDate,CreatedBy")] RequestParking requestParking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(requestParking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BuildingId"] = new SelectList(_context.Buildings, "Id", "Id", requestParking.BuildingId);
            return View(requestParking);
        }

        // GET: RequestParkings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RequestParkings == null)
            {
                return NotFound();
            }

            var requestParking = await _context.RequestParkings.FindAsync(id);
            if (requestParking == null)
            {
                return NotFound();
            }
            ViewData["BuildingId"] = new SelectList(_context.Buildings, "Id", "Id", requestParking.BuildingId);
            return View(requestParking);
        }

        // POST: RequestParkings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BuildingId,Fair,IsActive,AccessTime,Id,Status,CreatedDate,CreatedBy")] RequestParking requestParking)
        {
            if (id != requestParking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(requestParking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequestParkingExists(requestParking.Id))
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
            ViewData["BuildingId"] = new SelectList(_context.Buildings, "Id", "Id", requestParking.BuildingId);
            return View(requestParking);
        }

        // GET: RequestParkings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RequestParkings == null)
            {
                return NotFound();
            }

            var requestParking = await _context.RequestParkings
                .Include(r => r.Building)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (requestParking == null)
            {
                return NotFound();
            }

            return View(requestParking);
        }

        // POST: RequestParkings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RequestParkings == null)
            {
                return Problem("Entity set 'DatabaseEntity.RequestParkings'  is null.");
            }
            var requestParking = await _context.RequestParkings.FindAsync(id);
            if (requestParking != null)
            {
                _context.RequestParkings.Remove(requestParking);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RequestParkingExists(int id)
        {
          return (_context.RequestParkings?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
