using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PhotosErasmusApp.Data;
using PhotosErasmusApp.Models;

namespace PhotosErasmusApp.Controllers
{
    public class MyUsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MyUsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MyUsers
        public async Task<IActionResult> Index()
        {
            return View(await _context.MyUsers.ToListAsync());
        }

        // GET: MyUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var myUser = await _context.MyUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (myUser == null)
            {
                return NotFound();
            }

            return View(myUser);
        }

        // GET: MyUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MyUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,CellPhone,Street,PostalCode,Country")] MyUsers myUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(myUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(myUser);
        }

        // GET: MyUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var myUser = await _context.MyUsers.FindAsync(id);
            if (myUser == null)
            {
                return NotFound();
            }
            return View(myUser);
        }

        // POST: MyUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CellPhone,Street,PostalCode,Country")] MyUsers myUser)
        {
            if (id != myUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(myUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MyUserExists(myUser.Id))
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
            return View(myUser);
        }

        // GET: MyUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var myUser = await _context.MyUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (myUser == null)
            {
                return NotFound();
            }

            return View(myUser);
        }

        // POST: MyUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var myUser = await _context.MyUsers.FindAsync(id);
            if (myUser != null)
            {
                _context.MyUsers.Remove(myUser);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MyUserExists(int id)
        {
            return _context.MyUsers.Any(e => e.Id == id);
        }
    }
}
