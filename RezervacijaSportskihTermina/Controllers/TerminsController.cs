using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RezervacijaSportskihTermina.Controllers
{
    

    public class TerminsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public TerminsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Termins
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Termini.Include(t => t.Sport).Include(t => t.SportskiCentar);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Termins/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var termin = await _context.Termini
                .Include(t => t.Sport)
                .Include(t => t.SportskiCentar)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (termin == null)
            {
                return NotFound();
            }

            return View(termin);
        }

        // GET: Termins/Create
        public IActionResult Create()
        {
            ViewData["SportId"] = new SelectList(_context.Sportovi, "Id", "Naziv");
            ViewData["SportskiCentarId"] = new SelectList(_context.SportskiCentri, "Id", "Naziv");
            return View();
        }

        // POST: Termins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DatumVreme,Cena,SportId,SportskiCentarId")] Termin termin)
        {
            if (ModelState.IsValid)
            {
                _context.Add(termin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SportId"] = new SelectList(_context.Sportovi, "Id", "Naziv", termin.SportId);
            ViewData["SportskiCentarId"] = new SelectList(_context.SportskiCentri, "Id", "Naziv", termin.SportskiCentarId);
            return View(termin);
        }

        // GET: Termins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var termin = await _context.Termini.FindAsync(id);
            if (termin == null)
            {
                return NotFound();
            }
            ViewData["SportId"] = new SelectList(_context.Sportovi, "Id", "Naziv", termin.SportId);
            ViewData["SportskiCentarId"] = new SelectList(_context.SportskiCentri, "Id", "Naziv", termin.SportskiCentarId);
            return View(termin);
        }

        // POST: Termins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DatumVreme,Cena,SportId,SportskiCentarId")] Termin termin)
        {
            if (id != termin.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(termin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TerminExists(termin.Id))
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
            ViewData["SportId"] = new SelectList(_context.Sportovi, "Id", "Naziv", termin.SportId);
            ViewData["SportskiCentarId"] = new SelectList(_context.SportskiCentri, "Id", "Naziv", termin.SportskiCentarId);
            return View(termin);
        }

        // GET: Termins/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var termin = await _context.Termini
                .Include(t => t.Sport)
                .Include(t => t.SportskiCentar)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (termin == null)
            {
                return NotFound();
            }

            return View(termin);
        }

        // POST: Termins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var termin = await _context.Termini.FindAsync(id);
            if (termin != null)
            {
                _context.Termini.Remove(termin);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TerminExists(int id)
        {
            return _context.Termini.Any(e => e.Id == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Rezervisi(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Challenge(); // Ako nije ulogovan

            var postoji = await _context.KorisnikTermini
                .AnyAsync(kt => kt.TerminId == id && kt.ApplicationUserId == user.Id);

            if (!postoji)
            {
                var rezervacija = new KorisnikTermin
                {
                    ApplicationUserId = user.Id,
                    TerminId = id
                };
                _context.KorisnikTermini.Add(rezervacija);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> MojeRezervacije()
        {
            var user = await _userManager.GetUserAsync(User);

            var termini = await _context.KorisnikTermini
                .Where(kt => kt.ApplicationUserId == user.Id)
                .Include(kt => kt.Termin)
                    .ThenInclude(t => t.Sport)
                .Include(kt => kt.Termin)
                    .ThenInclude(t => t.SportskiCentar)
                .Select(kt => kt.Termin)
                .ToListAsync();

            return View(termini);
        }


    }
}
