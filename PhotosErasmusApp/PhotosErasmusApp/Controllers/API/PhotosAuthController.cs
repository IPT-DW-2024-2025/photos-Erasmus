using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using PhotosErasmusApp.Data;
using PhotosErasmusApp.Models;
using PhotosErasmusApp.Models.ViewModels;

namespace PhotosErasmusApp.Controllers.API {

   [Route("api/[controller]")]
   [ApiController]
   [Authorize(AuthenticationSchemes ="Bearer")]
   public class PhotosAuthController:ControllerBase {

      private readonly ApplicationDbContext _context;

      public PhotosAuthController(ApplicationDbContext context) {
         _context=context;
      }

      // GET: api/Photos
      [HttpGet]
      public async Task<ActionResult<IEnumerable<PhotoWithOwnerDTO>>> GetPhotos() {

         // we know that the user is authenticated
         // but, what is he/she username????
         // let look for it...
         string username = User.Identity.Name;



         /*
         // SELECT *
         // FROM Photos
         var list = await _context.Photos.ToListAsync();
         */

         // SELECT p.Descrition, p.FileName, c.Category, p.Date,
         // FROM Photos p INNER JOIN Categories c ON p.CategoryFY=c.Id
         //               INNER JOIN Owner o ON f.OwnerFK=o.Id
         // WHERE o.Id=someId
         // ORDER BY p.Date DESC
         var list = await _context.Photos
                                  .OrderByDescending(p => p.Date)
                                  .Where(p=>p.Owner.UserName==username)
                                  .Select(p => new PhotoWithOwnerDTO {
                                     PhotoDescription=p.Description,
                                     PhotoFile=p.FileName,
                                     Date=p.Date,
                                     Category=p.Category.Category,
                                     Owner=p.Owner.Name
                                  })
                                  .ToListAsync();
         return list;
      }

      // GET: api/Photos/5
      [HttpGet("{id}")]
      public async Task<ActionResult<PhotoWithOwnerDTO>> GetPhoto(int id) {
         //    var photo = await _context.Photos.FindAsync(id);
         var photo = await _context.Photos
                                   .Where(p => p.Id==id)
                                   .Select(p => new PhotoWithOwnerDTO {
                                       PhotoDescription=p.Description,
                                       PhotoFile=p.FileName,
                                       Date=p.Date,
                                       Category=p.Category.Category,
                                       Owner=p.Owner.Name
                                   })
                                   .FirstOrDefaultAsync();

         if (photo==null) {
            return NotFound();
         }

         return photo;
      }

      // PUT: api/Photos/5
      // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
      [HttpPut("{id}")]
      public async Task<IActionResult> PutPhoto(int id,Photos photo) {
         if (id!=photo.Id) {
            return BadRequest();
         }

         _context.Entry(photo).State=EntityState.Modified;

         try {
            await _context.SaveChangesAsync();
         }
         catch (DbUpdateConcurrencyException) {
            if (!PhotoExists(id)) {
               return NotFound();
            }
            else {
               throw;
            }
         }

         return NoContent();
      }

      // POST: api/Photos
      // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
      [HttpPost]
      public async Task<ActionResult<Photos>> PostPhoto(Photos photo) {
         _context.Photos.Add(photo);
         await _context.SaveChangesAsync();

         return CreatedAtAction("GetPhotos",new { id = photo.Id },photo);
      }

      // DELETE: api/Photos/5
      [HttpDelete("{id}")]
      public async Task<IActionResult> DeletePhoto(int id) {
         var photo = await _context.Photos.FindAsync(id);
         if (photo==null) {
            return NotFound();
         }

         _context.Photos.Remove(photo);
         await _context.SaveChangesAsync();

         return NoContent();
      }

      private bool PhotoExists(int id) {
         return _context.Photos.Any(e => e.Id==id);
      }
   }
}
