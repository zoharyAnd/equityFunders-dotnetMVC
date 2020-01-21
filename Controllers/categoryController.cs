using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using cFunding.Data;
using cFunding.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace cFunding.Controllers
{
    public class categoryController : Controller
    {
        private readonly AppliContext _context;

        public categoryController(AppliContext context)
        {
            _context = context;
        }

        // GET: category
        public async Task<IActionResult> Index()
        {
            return View(await _context.categories.ToListAsync());
        }

        // GET: category/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("customErrorPage", "user");
            }

            var category = await _context.categories
                .FirstOrDefaultAsync(m => m.id == id);
            if (category == null)
            {
                return RedirectToAction("customErrorPage", "user");
            }

            return View(category);
        }

        // GET: category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: category/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,categoryname")] category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("customErrorPage", "user");
            }

            var category = await _context.categories.FindAsync(id);
            if (category == null)
            {
                return RedirectToAction("customErrorPage", "user");
            }
            return View(category);
        }

        // POST: category/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,categoryname")] category category)
        {
            if (id != category.id)
            {
                return RedirectToAction("customErrorPage", "user");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!categoryExists(category.id))
                    {
                        return RedirectToAction("customErrorPage", "user");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("customErrorPage", "user");
            }

            var category = await _context.categories
                .FirstOrDefaultAsync(m => m.id == id);
            if (category == null)
            {
                return RedirectToAction("customErrorPage", "user");
            }

            return View(category);
        }

        // POST: category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.categories.FindAsync(id);
            _context.categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool categoryExists(int id)
        {
            return _context.categories.Any(e => e.id == id);
        }
    }
}
