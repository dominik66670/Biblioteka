using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Biblioteka.Data;
using Biblioteka.Models;

namespace Biblioteka.Controllers
{
    public class ZasobsController : Controller
    {
        private readonly BibliotekaContext _context;

        public ZasobsController(BibliotekaContext context)
        {
            _context = context;
        }

        // GET: Zasobs
        public async Task<IActionResult> Index()
        {
            return _context.Zasob != null ? 
                          View(await _context.Zasob
                          .Include(x => x.Autorzy)
                          .ToListAsync()) :
                          Problem("Entity set 'BibliotekaContext.Zasob'  is null.");
        }

        // GET: Zasobs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Zasob == null)
            {
                return NotFound();
            }

            var zasob = await _context.Zasob
                .FirstOrDefaultAsync(m => m.Id == id);
            if (zasob == null)
            {
                return NotFound();
            }

            return View(zasob);
        }

        // GET: Zasobs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Zasobs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Tytul,ISBN")] Zasob zasob)
        {
            if (ModelState.IsValid)
            {
                _context.Add(zasob);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(zasob);
        }

        // GET: Zasobs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Zasob == null)
            {
                return NotFound();
            }

            var zasob = await _context.Zasob.FindAsync(id);
            if (zasob == null)
            {
                return NotFound();
            }
            return View(zasob);
        }

        // POST: Zasobs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Tytul,ISBN")] Zasob zasob)
        {
            if (id != zasob.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(zasob);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZasobExists(zasob.Id))
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
            return View(zasob);
        }

        // GET: Zasobs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Zasob == null)
            {
                return NotFound();
            }

            var zasob = await _context.Zasob
                .FirstOrDefaultAsync(m => m.Id == id);
            if (zasob == null)
            {
                return NotFound();
            }

            return View(zasob);
        }

        // POST: Zasobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Zasob == null)
            {
                return Problem("Entity set 'BibliotekaContext.Zasob'  is null.");
            }
            var zasob = await _context.Zasob.FindAsync(id);
            if (zasob != null)
            {
                _context.Zasob.Remove(zasob);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ZasobExists(int id)
        {
          return (_context.Zasob?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
