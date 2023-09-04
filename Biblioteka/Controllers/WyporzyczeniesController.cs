using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Biblioteka.Data;
using Biblioteka.Models;
using Microsoft.IdentityModel.Tokens;

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
        public async Task<IActionResult> Index(string? searchString, List<int>? szukanyStatus, string order, int searchField)
        {
            await _context.Wyporzyczenie.Include(w => w.ObecnyStatus).ForEachAsync(w =>
            {
                if (w.ObecnyStatus.Nazwa.Equals("Trwa") && w.DataZwrotu<DateTime.Now)
                {
                    w.ObecnyStatus = _context.Status.ToList().Find(s => s.Nazwa.Equals("Spóżnienie"));
                }
            });
            _context.SaveChanges();
            var Wyporzyczenies = await _context.Wyporzyczenie
                    .Include(w => w.Wyporzyczajacy)
                    .Include(w => w.WyporzyczanyZbior)
                    .Include(w => w.ObecnyStatus)
                    .Include(w => w.WyporzyczanyZbior.Zasob)
                    .ToListAsync();
            var szukane = new List<Wyporzyczenie>();
            
            if (!szukanyStatus.IsNullOrEmpty())
            {
                foreach (var id in szukanyStatus)
                {
                    szukane.AddRange
                        (
                            Wyporzyczenies.FindAll(w => w.ObecnyStatus.Id == id)
                        );
                }
            }
            else
            {
                szukane.AddRange( Wyporzyczenies );
            }
            if (!searchString.IsNullOrEmpty())
            {
                if(searchField == 1)
                {
                    szukane = szukane.FindAll(w => w.Wyporzyczajacy.Imie.Contains(searchString) || w.Wyporzyczajacy.Nazwisko.Contains(searchString));
                }
                else
                {
                    try
                    {
                        szukane = szukane.FindAll(w => w.WyporzyczanyZbior.Id == Int32.Parse(searchString));
                    }catch (Exception ex) { }
                }
            }
            if (!order.IsNullOrEmpty())
            {
                if (order.Equals("asc"))
                {
                    szukane = szukane.OrderBy(w => w.DataZwrotu).ToList();
                }
                else
                {
                    szukane = szukane.OrderByDescending(w => w.DataZwrotu).ToList();
                }
            }
            
            var viewModel = new WyporzyczeniaViewModel()
            {
                wyporzyczenies = szukane,
                statuses = await _context.Status.ToListAsync()
            };

            return View(viewModel);
        }

        // GET: Wyporzyczenies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Wyporzyczenie == null)
            {
                return NotFound();
            }

            var wyporzyczenie = await _context.Wyporzyczenie.Include(w => w.Wyporzyczajacy).Include(w => w.WyporzyczanyZbior).Include(w => w.WyporzyczanyZbior.Zasob).Include(w => w.ObecnyStatus)
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
            var addView = new AddWyporzyczenieViewModel()
            {
                Czytelnicy = _context.LibraryUser.ToList().OrderBy(c => c.Nazwisko),
                Zasoby = _context.Zasob.ToList().OrderBy(z => z.Tytul)
            };
            return View(addView);
        }

        // POST: Wyporzyczenies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CzytelnikId,ZasobId")] AddWyporzyczenieViewModel addWyporzyczenie)
        {
            var zbior = _context.Zbior.Include(c => c.Zasob).ToList().Find(c => c.Zasob.Id == addWyporzyczenie.ZasobId && c.CzyDostepny);
            if (zbior == null)
            {
                addWyporzyczenie.Czytelnicy = _context.LibraryUser.ToList().OrderBy(c => c.Nazwisko);
                addWyporzyczenie.Zasoby = _context.Zasob.ToList().OrderBy(z => z.Tytul);
                ModelState.AddModelError("BrakZbioru", "Obecnie nie ma dostępnych egzemplarzy wybranej książki");
                return View(addWyporzyczenie);
            }
            zbior.CzyDostepny = false;
            _context.Zbior.Update(zbior);
            var wyporzyczenie = new Wyporzyczenie()
            {
                Wyporzyczajacy = _context.LibraryUser.ToList().Find(c => c.Id == addWyporzyczenie.CzytelnikId),
                WyporzyczanyZbior = zbior,
                DataWyporzyczenia = DateTime.Now,
                DataZwrotu = DateTime.Now.AddDays(30),
                ObecnyStatus = _context.Status.ToList().Find(s => s.Nazwa.Equals("Trwa"))
            };
            _context.Add(wyporzyczenie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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
        public async Task<IActionResult> Zwrot(int? id)
        {
            if (id == null || _context.Wyporzyczenie == null)
            {
                return NotFound();
            }
            var wyporzyczenie = _context.Wyporzyczenie.Include(e => e.ObecnyStatus).Include(e => e.WyporzyczanyZbior).ToList().Find(e => e.Id==id);
            wyporzyczenie.ObecnyStatus = _context.Status.ToList().Find(s => s.Nazwa.Equals("Zakończone"));
            wyporzyczenie.WyporzyczanyZbior.CzyDostepny = true;
            _context.Update(wyporzyczenie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
