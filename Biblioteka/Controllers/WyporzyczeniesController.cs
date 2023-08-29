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
    public class WyporzyczeniesController : Controller
    {
        private readonly BibliotekaContext _context;

        public WyporzyczeniesController(BibliotekaContext context)
        {
            _context = context;
        }

        // GET: Wyporzyczenies
        public async Task<IActionResult> Index()
        {
              return _context.Wyporzyczenie != null ? 
                          View(await _context.Wyporzyczenie.ToListAsync()) :
                          Problem("Entity set 'BibliotekaContext.Wyporzyczenie'  is null.");
        }

        // GET: Wyporzyczenies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Wyporzyczenie == null)
            {
                return NotFound();
            }

            var wyporzyczenie = await _context.Wyporzyczenie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wyporzyczenie == null)
            {
                return NotFound();
            }

            return View(wyporzyczenie);
        }

        // GET: Wyporzyczenies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Wyporzyczenies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DataWyporzyczenia,DataZwrotu")] Wyporzyczenie wyporzyczenie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(wyporzyczenie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(wyporzyczenie);
        }

        // GET: Wyporzyczenies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Wyporzyczenie == null)
            {
                return NotFound();
            }

            var wyporzyczenie = await _context.Wyporzyczenie.FindAsync(id);
            if (wyporzyczenie == null)
            {
                return NotFound();
            }
            return View(wyporzyczenie);
        }

        // POST: Wyporzyczenies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DataWyporzyczenia,DataZwrotu")] Wyporzyczenie wyporzyczenie)
        {
            if (id != wyporzyczenie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wyporzyczenie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WyporzyczenieExists(wyporzyczenie.Id))
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
            return View(wyporzyczenie);
        }

        // GET: Wyporzyczenies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Wyporzyczenie == null)
            {
                return NotFound();
            }

            var wyporzyczenie = await _context.Wyporzyczenie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wyporzyczenie == null)
            {
                return NotFound();
            }

            return View(wyporzyczenie);
        }

        // POST: Wyporzyczenies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Wyporzyczenie == null)
            {
                return Problem("Entity set 'BibliotekaContext.Wyporzyczenie'  is null.");
            }
            var wyporzyczenie = await _context.Wyporzyczenie.FindAsync(id);
            if (wyporzyczenie != null)
            {
                _context.Wyporzyczenie.Remove(wyporzyczenie);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WyporzyczenieExists(int id)
        {
          return (_context.Wyporzyczenie?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
