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
    public class ZbiorsController : Controller
    {
        private readonly BibliotekaContext _context;

        public ZbiorsController(BibliotekaContext context)
        {
            _context = context;
        }

        // GET: Zbiors
        public async Task<IActionResult> Index()
        {
              return _context.Zbior != null ? 
                          View(await _context.Zbior.ToListAsync()) :
                          Problem("Entity set 'BibliotekaContext.Zbior'  is null.");
        }

        // GET: Zbiors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Zbior == null)
            {
                return NotFound();
            }

            var zbior = await _context.Zbior
                .FirstOrDefaultAsync(m => m.Id == id);
            if (zbior == null)
            {
                return NotFound();
            }

            return View(zbior);
        }

        // GET: Zbiors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Zbiors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CzyDostepny")] Zbior zbior)
        {
            if (ModelState.IsValid)
            {
                _context.Add(zbior);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(zbior);
        }

        // GET: Zbiors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Zbior == null)
            {
                return NotFound();
            }

            var zbior = await _context.Zbior.FindAsync(id);
            if (zbior == null)
            {
                return NotFound();
            }
            return View(zbior);
        }

        // POST: Zbiors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CzyDostepny")] Zbior zbior)
        {
            if (id != zbior.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(zbior);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZbiorExists(zbior.Id))
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
            return View(zbior);
        }

        // GET: Zbiors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Zbior == null)
            {
                return NotFound();
            }

            var zbior = await _context.Zbior
                .FirstOrDefaultAsync(m => m.Id == id);
            if (zbior == null)
            {
                return NotFound();
            }

            return View(zbior);
        }

        // POST: Zbiors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Zbior == null)
            {
                return Problem("Entity set 'BibliotekaContext.Zbior'  is null.");
            }
            var zbior = await _context.Zbior.FindAsync(id);
            if (zbior != null)
            {
                _context.Zbior.Remove(zbior);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ZbiorExists(int id)
        {
          return (_context.Zbior?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
