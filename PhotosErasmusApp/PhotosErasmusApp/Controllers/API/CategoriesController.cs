using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using PhotosErasmusApp.Data;
using PhotosErasmusApp.Models;

namespace PhotosErasmusApp.Controllers.API {

   [Route("api/[controller]")]
   [ApiController]
   public class CategoriesController:ControllerBase {

      private readonly ApplicationDbContext _context;

      public CategoriesController(ApplicationDbContext context) {
         _context=context;
      }

      /// <summary>
      /// GET: List of Categories
      /// </summary>
      /// <returns></returns>
      [HttpGet]
      public async Task<ActionResult<IEnumerable<Categories>>> GetCategories() {
         return await _context.Categories.ToListAsync();
      }

      /// <summary>
      /// GET: list of one Category
      /// </summary>
      /// <param name="id">identification of the category that you want</param>
      /// <returns></returns>
      [HttpGet("{id}")]
      public async Task<ActionResult<Categories>> GetCategory(int id) {
         var category = await _context.Categories.FindAsync(id);

         if (category==null) {
            return NotFound();
         }

         return category;
      }

      /// <summary>
      /// changing the data of one Category
      /// </summary>
      /// <param name="id">identification of the category that you want to change</param>
      /// <param name="category">the new data of your category</param>
      /// <returns></returns>
      // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
      [HttpPut("{id}")]
      public async Task<IActionResult> PutCategory(int id,Categories category) {
         if (id!=category.Id) {
            return BadRequest();
         }

         _context.Entry(category).State=EntityState.Modified;

         try {
            await _context.SaveChangesAsync();
         }
         catch (DbUpdateConcurrencyException) {
            if (!CategoryExists(id)) {
               return NotFound();
            }
            else {
               throw;
            }
         }

         return NoContent();
      }

      /// <summary>
      /// add a new Category
      /// </summary>
      /// <param name="category">the data of the new category</param>
      /// <returns></returns>
      // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
      [HttpPost]
      public async Task<ActionResult<Categories>> PostCategory(Categories category) {
         _context.Categories.Add(category);
         await _context.SaveChangesAsync();

         return CreatedAtAction("GetCategories",new { id = category.Id },category);
      }

      /// <summary>
      /// DELETE a category
      /// </summary>
      /// <param name="id">the identification of category to be deleted</param>
      /// <returns></returns>
      [HttpDelete("{id}")]
      public async Task<IActionResult> DeleteCategory(int id) {
         var category = await _context.Categories.FindAsync(id);
         if (category==null) {
            return NotFound();
         }

         _context.Categories.Remove(category);
         await _context.SaveChangesAsync();

         return NoContent();
      }

      /// <summary>
      /// search for a category
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
      private bool CategoryExists(int id) {
         return _context.Categories.Any(e => e.Id==id);
      }
   }
}
