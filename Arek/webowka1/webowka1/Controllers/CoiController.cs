using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using webowka1.Data;
using webowka1.Models;

namespace webowka1.Controllers
{
    public class CoiController : Controller
    {
        private readonly webowka1Context _context;

        public CoiController(webowka1Context context)
        {
            _context = context;
        }

        // GET: Coi
        public async Task<IActionResult> Index()
        {
            return View(await _context.OsadzeniTablica.ToListAsync());
        }

        // GET: Coi/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var co = await _context.OsadzeniTablica
                .FirstOrDefaultAsync(m => m.Id == id);
            if (co == null)
            {
                return NotFound();
            }

            return View(co);
        }

        // GET: Coi/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Coi/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Pseudonim,Imie,Nazwisko,Wyrok")] Models.Co co)
        {
            if (ModelState.IsValid)
            {
                _context.Add(co);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(co);
        }

        // GET: Coi/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var co = await _context.OsadzeniTablica.FindAsync(id);
            if (co == null)
            {
                return NotFound();
            }
            return View(co);
        }

        // POST: Coi/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Pseudonim,Imie,Nazwisko,Wyrok")] Models.Co co)
        {
            if (id != co.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(co);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoExists(co.Id))
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
            return View(co);
        }

        // GET: Coi/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var co = await _context.OsadzeniTablica
                .FirstOrDefaultAsync(m => m.Id == id);
            if (co == null)
            {
                return NotFound();
            }

            return View(co);
        }

        // POST: Coi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var co = await _context.OsadzeniTablica.FindAsync(id);
            if (co != null)
            {
                _context.OsadzeniTablica.Remove(co);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CoExists(int id)
        {
            return _context.OsadzeniTablica.Any(e => e.Id == id);
        }
    }
}
