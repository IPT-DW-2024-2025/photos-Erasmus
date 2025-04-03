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
   public class PhotosController: Controller {

      private readonly ApplicationDbContext _context;

      public PhotosController(ApplicationDbContext context) {
         _context = context;
      }

      // GET: Photos
      public async Task<IActionResult> Index() {

         /* using LINQ
          * SELECT *
          * FROM Photos p INNER JOIN Categories c ON p.CategoryFK=c.Id
          *               INNER JOIN MyUser m ON p.OwnerFK=m.Id
          * ORDER BY p.Date DESC
          */
         var listOfPhotos = _context.Photos
                                    .Include(p => p.Category)
                                    .Include(p => p.Owner)
                                    .OrderByDescending(p => p.Date);
        
         return View(await listOfPhotos.ToListAsync());
      }

      // GET: Photos/Details/5
      public async Task<IActionResult> Details(int? id) {
         if (id == null) {
            return NotFound();
         }

         var photo = await _context.Photos
             .Include(p => p.Category)
             .Include(p => p.Owner)
             .FirstOrDefaultAsync(m => m.Id == id);
         if (photo == null) {
            return NotFound();
         }

         return View(photo);
      }

      // GET: Photos/Create
      public IActionResult Create() {
         ViewData["CategoryFK"] = new SelectList(_context.Categories, "Id", "Category");
         ViewData["OwnerFK"] = new SelectList(_context.MyUsers, "Id", "Id");
         return View();
      }

      // POST: Photos/Create
      // To protect from overposting attacks, enable the specific properties you want to bind to.
      // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Create([Bind("Id,Description,Date,FileName,Price,CategoryFK,OwnerFK")] Photos photo) {
         if (ModelState.IsValid) {
            _context.Add(photo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
         }
         ViewData["CategoryFK"] = new SelectList(_context.Categories, "Id", "Category", photo.CategoryFK);
         ViewData["OwnerFK"] = new SelectList(_context.MyUsers, "Id", "Id", photo.OwnerFK);
         return View(photo);
      }

      // GET: Photos/Edit/5
      public async Task<IActionResult> Edit(int? id) {
         if (id == null) {
            return NotFound();
         }

         var photo = await _context.Photos.FindAsync(id);
         if (photo == null) {
            return NotFound();
         }
         ViewData["CategoryFK"] = new SelectList(_context.Categories, "Id", "Category", photo.CategoryFK);
         ViewData["OwnerFK"] = new SelectList(_context.MyUsers, "Id", "Id", photo.OwnerFK);
         return View(photo);
      }

      // POST: Photos/Edit/5
      // To protect from overposting attacks, enable the specific properties you want to bind to.
      // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, [Bind("Id,Description,Date,FileName,Price,CategoryFK,OwnerFK")] Photos photo) {
         if (id != photo.Id) {
            return NotFound();
         }

         if (ModelState.IsValid) {
            try {
               _context.Update(photo);
               await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
               if (!PhotoExists(photo.Id)) {
                  return NotFound();
               }
               else {
                  throw;
               }
            }
            return RedirectToAction(nameof(Index));
         }
         ViewData["CategoryFK"] = new SelectList(_context.Categories, "Id", "Category", photo.CategoryFK);
         ViewData["OwnerFK"] = new SelectList(_context.MyUsers, "Id", "Id", photo.OwnerFK);
         return View(photo);
      }

      // GET: Photos/Delete/5
      public async Task<IActionResult> Delete(int? id) {
         if (id == null) {
            return NotFound();
         }

         var photo = await _context.Photos
             .Include(p => p.Category)
             .Include(p => p.Owner)
             .FirstOrDefaultAsync(m => m.Id == id);
         if (photo == null) {
            return NotFound();
         }

         return View(photo);
      }

      // POST: Photos/Delete/5
      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> DeleteConfirmed(int id) {
         var photo = await _context.Photos.FindAsync(id);
         if (photo != null) {
            _context.Photos.Remove(photo);
         }

         await _context.SaveChangesAsync();
         return RedirectToAction(nameof(Index));
      }

      private bool PhotoExists(int id) {
         return _context.Photos.Any(e => e.Id == id);
      }
   }
}
