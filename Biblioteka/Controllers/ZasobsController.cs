using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Biblioteka.Data;
using Biblioteka.Models;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.IdentityModel.Tokens;

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
        public async Task<IActionResult> Index(string? serachString, string? order,string? szukanePole)
        {
            var zasoby = _context.Zasob.Include(a => a.Autorzy).ToList();
            if (!szukanePole.IsNullOrEmpty())
            {
                if (szukanePole.Equals("1"))
                {
                    if (!serachString.IsNullOrEmpty())
                    {
                        zasoby = zasoby.Where(z => z.Tytul.Contains(serachString)).ToList();
                        
                    }
                    if (!order.IsNullOrEmpty())
                    {
                        if (order.Equals("asc"))
                        {
                            zasoby = zasoby.OrderBy(z => z.Tytul).ToList();
                        }
                        else if (order.Equals("dsc"))
                        {
                            zasoby = zasoby.OrderByDescending(z => z.Tytul).ToList();
                        }
                    }
                }
                else
                {
                    if (!serachString.IsNullOrEmpty())
                    {
                        zasoby = zasoby.Where(z => z.ISBN.Contains(serachString)).ToList();
                        
                    }
                    if (!order.IsNullOrEmpty())
                    {
                        if (order.Equals("asc"))
                        {
                            zasoby = zasoby.OrderBy(z => z.ISBN).ToList();
                        }
                        else if (order.Equals("dsc"))
                        {
                            zasoby = zasoby.OrderByDescending(z => z.ISBN).ToList();
                        }
                    }
                }
            }


            return View(zasoby);
        }

        // GET: Zasobs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Zasob == null)
            {
                return NotFound();
            }

            var zasob = await _context.Zasob.Include(z => z.Autorzy)
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

            return View(new AddEditZasobViewModel() { Autorzy=_context.Autor.ToList().OrderBy(a => a.Nazwa).ToList()});
        }

        // POST: Zasobs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Tytul,ISBN")] Zasob zasob, List<int> selectedAutorIds)
        {
            if (ModelState.IsValid)
            {
                // Detach the zasob from the context so it's treated as a new entity
                //_context.Entry(zasob).State = EntityState.Detached;

                // Create a new instance of Zasob
                var newZasob = new Zasob
                {
                    Tytul = zasob.Tytul,
                    ISBN = zasob.ISBN,
                    Autorzy = new List<Autor>()
                };

                foreach (var autorId in selectedAutorIds)
                {
                    var existingAutor = await _context.Autor.FindAsync(autorId);
                    if (existingAutor != null)
                    {
                        newZasob.Autorzy.Add(existingAutor);
                    }
                }
                _context.Zasob.Add(newZasob);
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
            var addEdit = new AddEditZasobViewModel() { Autorzy = _context.Autor.ToList(), Zasob = zasob };
            return View(addEdit);
        }

        // POST: Zasobs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Tytul,ISBN")] Zasob zasob, List<int> selectedAutorIds)
        {
            if (id != zasob.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    zasob.Autorzy = new List<Autor>();
                    foreach (var autorId in selectedAutorIds)
                    {
                        var existingAutor = await _context.Autor.FindAsync(autorId);
                        if (existingAutor != null)
                        {
                            zasob.Autorzy.Add(existingAutor);
                        }
                    }
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
