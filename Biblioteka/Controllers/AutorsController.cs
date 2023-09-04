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
using System.Runtime.Intrinsics.X86;
using X.PagedList;

namespace Biblioteka.Controllers
{
    public class AutorsController : Controller
    {
        private readonly BibliotekaContext _context;

        public AutorsController(BibliotekaContext context)
        {
            _context = context;
        }

        // GET: Autors
        public async Task<IActionResult> Index(int? page,string? serachString, string? order)
        {
            int pageNumber = page ?? 1;
            int pageSize = 10;
            var autorzy = _context.Autor.ToList();
            if (!serachString.IsNullOrEmpty())
            {
                autorzy = _context.Autor.ToList().FindAll(x => x.Nazwa.Contains(serachString));
                if (!order.IsNullOrEmpty())
                {
                    if (order.Equals("asc"))
                    {
                        autorzy = autorzy.OrderBy(a => a.Nazwa).ToList();
                    }else if (order.Equals("dsc"))
                    {
                        autorzy = autorzy.OrderByDescending(a => a.Nazwa).ToList();
                    }
                }
                IPagedList<Autor> pagedAutors = autorzy.ToPagedList(pageNumber, pageSize);
                return View(pagedAutors);
            }
            else
            {
                if (!order.IsNullOrEmpty())
                {
                    if (order.Equals("asc"))
                    {
                        autorzy = autorzy.OrderBy(a => a.Nazwa).ToList();
                    }
                    else if (order.Equals("dsc"))
                    {
                        autorzy = autorzy.OrderByDescending(a => a.Nazwa).ToList();
                    }
                }
                IPagedList<Autor> pagedAutors = autorzy.ToPagedList(pageNumber, pageSize);
                return View(pagedAutors);
            }
              
        }

        // GET: Autors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Autor == null)
            {
                return NotFound();
            }
            var avm = new AutorViewModel()
            {
                Autor = await _context.Autor.FirstOrDefaultAsync(m => m.Id == id),
                Dziela = _context.Zasob.Include(z => z.Autorzy).ToList().Where(zasob=> zasob.Autorzy.Any(autor => autor.Id==id)).ToList()
            };
            if (avm == null)
            {
                return NotFound();
            }

            return View(avm);
        }


        // GET: Autors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Autors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nazwa,Description,DataUrodzenia,DataSmierci")] Autor autor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(autor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(autor);
        }

        // GET: Autors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Autor == null)
            {
                return NotFound();
            }

            var autor = await _context.Autor.FindAsync(id);
            if (autor == null)
            {
                return NotFound();
            }
            return View(autor);
        }

        // POST: Autors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nazwa,Description,DataUrodzenia,DataSmierci")] Autor autor)
        {
            if (id != autor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(autor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AutorExists(autor.Id))
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
            return View(autor);
        }

        // GET: Autors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Autor == null)
            {
                return NotFound();
            }

            var autor = await _context.Autor
                .FirstOrDefaultAsync(m => m.Id == id);
            if (autor == null)
            {
                return NotFound();
            }

            return View(autor);
        }

        // POST: Autors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Autor == null)
            {
                return Problem("Entity set 'BibliotekaContext.Autor'  is null.");
            }
            var autor = await _context.Autor.FindAsync(id);
            _context.Zasob.Include(z => z.Autorzy).ToList().ForEach(z =>
            {
                if (z.Autorzy.Contains(autor))
                {
                    z.Autorzy.Remove(autor);
                }
            });
            if (autor != null)
            {
                _context.Autor.Remove(autor);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AutorExists(int id)
        {
          return (_context.Autor?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
