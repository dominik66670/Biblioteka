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
    public class LibraryUsersController : Controller
    {
        private readonly BibliotekaContext _context;

        public LibraryUsersController(BibliotekaContext context)
        {
            _context = context;
        }

        // GET: LibraryUsers
        public async Task<IActionResult> Index()
        {
            var viewModel = new LibraryUserViewModel()
            {
                Czytelnicy= await _context.LibraryUser.ToListAsync(),
                Wyporzyczenia = await _context.Wyporzyczenie.Include(w => w.Wyporzyczajacy).ToListAsync()
            };
              return View(viewModel);
        }

        // GET: LibraryUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LibraryUser == null)
            {
                return NotFound();
            }

            var libraryUser = await _context.LibraryUser
                .FirstOrDefaultAsync(m => m.Id == id);
            if (libraryUser == null)
            {
                return NotFound();
            }

            return View(libraryUser);
        }

        // GET: LibraryUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LibraryUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Imie,Nazwisko,Adres")] LibraryUser libraryUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(libraryUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(libraryUser);
        }

        // GET: LibraryUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LibraryUser == null)
            {
                return NotFound();
            }

            var libraryUser = await _context.LibraryUser.FindAsync(id);
            if (libraryUser == null)
            {
                return NotFound();
            }
            return View(libraryUser);
        }

        // POST: LibraryUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Imie,Nazwisko,Adres")] LibraryUser libraryUser)
        {
            if (id != libraryUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(libraryUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LibraryUserExists(libraryUser.Id))
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
            return View(libraryUser);
        }

        // GET: LibraryUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LibraryUser == null)
            {
                return NotFound();
            }

            var libraryUser = await _context.LibraryUser
                .FirstOrDefaultAsync(m => m.Id == id);
            if (libraryUser == null)
            {
                return NotFound();
            }

            return View(libraryUser);
        }

        // POST: LibraryUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LibraryUser == null)
            {
                return Problem("Entity set 'BibliotekaContext.LibraryUser'  is null.");
            }
            var libraryUser = await _context.LibraryUser.FindAsync(id);
            if (libraryUser != null)
            {
                _context.LibraryUser.Remove(libraryUser);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LibraryUserExists(int id)
        {
          return (_context.LibraryUser?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
