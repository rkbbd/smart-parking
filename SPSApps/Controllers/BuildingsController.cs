﻿using System;
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
    public class BuildingsController : Controller
    {
        private readonly DatabaseEntity _context;

        public BuildingsController(DatabaseEntity context)
        {
            _context = context;
        }

        // GET: Buildings
        public async Task<IActionResult> Index()
        {
              return _context.Buildings != null ? 
                          View(await _context.Buildings.ToListAsync()) :
                          Problem("Entity set 'DatabaseEntity.Buildings'  is null.");
        }

        // GET: Buildings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Buildings == null)
            {
                return NotFound();
            }

            var building = await _context.Buildings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (building == null)
            {
                return NotFound();
            }

            return View(building);
        }

        // GET: Buildings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Buildings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Address,Latitude,Longitude,TotalAvailableParking,FairPerParking")] Building building)
        {
            if (ModelState.IsValid)
            {
                _context.Add(building);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(building);
        }

        // GET: Buildings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Buildings == null)
            {
                return NotFound();
            }

            var building = await _context.Buildings.FindAsync(id);
            if (building == null)
            {
                return NotFound();
            }
            return View(building);
        }

        // POST: Buildings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Address,Latitude,Longitude,TotalAvailableParking,FairPerParking,Id,Status,CreatedDate,CreatedBy")] Building building)
        {
            if (id != building.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(building);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BuildingExists(building.Id))
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
            return View(building);
        }

        // GET: Buildings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Buildings == null)
            {
                return NotFound();
            }

            var building = await _context.Buildings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (building == null)
            {
                return NotFound();
            }

            return View(building);
        }

        // POST: Buildings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Buildings == null)
            {
                return Problem("Entity set 'DatabaseEntity.Buildings'  is null.");
            }
            var building = await _context.Buildings.FindAsync(id);
            if (building != null)
            {
                _context.Buildings.Remove(building);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BuildingExists(int id)
        {
          return (_context.Buildings?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
