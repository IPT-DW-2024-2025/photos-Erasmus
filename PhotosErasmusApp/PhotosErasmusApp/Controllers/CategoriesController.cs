using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using PhotosErasmusApp.Data;
using PhotosErasmusApp.Models;

namespace PhotosErasmusApp.Controllers {
   public class CategoriesController: Controller {

      private readonly ApplicationDbContext _context;

      public CategoriesController(ApplicationDbContext context) {
         _context = context;
      }

      // GET: Categories
      public async Task<IActionResult> Index() {

         // we are making a shearch for Categories in our database
         // We are using LINQ
         // please see: https://learn.microsoft.com/en-us/dotnet/csharp/linq/
         // SELECT *
         // FROM Categories c
         // ORDER BY c.Category

         return View(await _context.Categories.OrderBy(c=>c.Category).ToListAsync());
      }

      // GET: Categories/Details/5
      public async Task<IActionResult> Details(int? id) {
         if (id == null) {
            return NotFound();
         }

         // SELECT *
         // FROM Categories m
         // WHERE m.Id = id
         var category = await _context.Categories
                                      .FirstOrDefaultAsync(m => m.Id == id);
         if (category == null) {
            return NotFound();
         }

         return View(category);
      }

      // GET: Categories/Create
      public IActionResult Create() {
         return View();
      }

      // POST: Categories/Create
      // To protect from overposting attacks, enable the specific properties you want to bind to.
      // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Create([Bind("Id,Category")] Categories newCategory) {
         if (ModelState.IsValid) {
            _context.Add(newCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
         }
         return View(newCategory);
      }

      // GET: Categories/Edit/5
      public async Task<IActionResult> Edit(int? id) {
         if (id == null) {
            return NotFound();
         }

         var category = await _context.Categories.FindAsync(id);
         if (category == null) {
            return NotFound();
         }
         return View(category);
      }

      // POST: Categories/Edit/5
      // To protect from overposting attacks, enable the specific properties you want to bind to.
      // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, [Bind("Id,Category")] Categories newCategory) {
         if (id != newCategory.Id) {
            return NotFound();
         }

         if (ModelState.IsValid) {
            try {
               _context.Update(newCategory);
               await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
               if (!CategoriesExists(newCategory.Id)) {
                  return NotFound();
               }
               else {
                  throw;
               }
            }
            return RedirectToAction(nameof(Index));
         }
         return View(newCategory);
      }

      // GET: Categories/Delete/5
      public async Task<IActionResult> Delete(int? id) {
         if (id == null) {
            return NotFound();
         }

         var category = await _context.Categories
             .FirstOrDefaultAsync(m => m.Id == id);
         if (category == null) {
            return NotFound();
         }

         return View(category);
      }

      // POST: Categories/Delete/5
      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> DeleteConfirmed(int id) {
         var category = await _context.Categories.FindAsync(id);
         if (category != null) {
            _context.Categories.Remove(category);
         }

         await _context.SaveChangesAsync();
         return RedirectToAction(nameof(Index));
      }

      private bool CategoriesExists(int id) {
         return _context.Categories.Any(e => e.Id == id);
      }
   }
}
