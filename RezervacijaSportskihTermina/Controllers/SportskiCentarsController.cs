using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace RezervacijaSportskihTermina.Controllers
{
    public class SportskiCentarsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SportskiCentarsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SportskiCentars
        public async Task<IActionResult> Index()
        {
            return View(await _context.SportskiCentri.ToListAsync());
        }

        // GET: SportskiCentars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sportskiCentar = await _context.SportskiCentri
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sportskiCentar == null)
            {
                return NotFound();
            }

            return View(sportskiCentar);
        }

        // GET: SportskiCentars/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SportskiCentars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Naziv,Lokacija")] SportskiCentar sportskiCentar)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sportskiCentar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sportskiCentar);
        }

        // GET: SportskiCentars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sportskiCentar = await _context.SportskiCentri.FindAsync(id);
            if (sportskiCentar == null)
            {
                return NotFound();
            }
            return View(sportskiCentar);
        }

        // POST: SportskiCentars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naziv,Lokacija")] SportskiCentar sportskiCentar)
        {
            if (id != sportskiCentar.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sportskiCentar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SportskiCentarExists(sportskiCentar.Id))
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
            return View(sportskiCentar);
        }

        // GET: SportskiCentars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sportskiCentar = await _context.SportskiCentri
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sportskiCentar == null)
            {
                return NotFound();
            }

            return View(sportskiCentar);
        }

        // POST: SportskiCentars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sportskiCentar = await _context.SportskiCentri.FindAsync(id);
            if (sportskiCentar != null)
            {
                _context.SportskiCentri.Remove(sportskiCentar);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SportskiCentarExists(int id)
        {
            return _context.SportskiCentri.Any(e => e.Id == id);
        }
    }
}
