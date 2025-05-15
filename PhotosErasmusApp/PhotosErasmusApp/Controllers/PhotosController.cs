using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using PhotosErasmusApp.Data;
using PhotosErasmusApp.Models;

namespace PhotosErasmusApp.Controllers {



   [Authorize]  // this means that only Autenticated people can access these data
   public class PhotosController:Controller {

      /// <summary>
      /// reference to the project's database
      /// </summary>
      private readonly ApplicationDbContext _context;

      /// <summary>
      /// the data related with the web server
      /// </summary>
      private readonly IWebHostEnvironment _webHostEnvironment;

      public PhotosController(ApplicationDbContext context,
         IWebHostEnvironment webHostEnvironment) {
         _context=context;
         _webHostEnvironment=webHostEnvironment;
      }

      // GET: Photos
      [AllowAnonymous]  // Everyone can access the Photos list
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
         if (id==null) {
            return NotFound();
         }

         var photo = await _context.Photos
             .Include(p => p.Category)
             .Include(p => p.Owner)
             .FirstOrDefaultAsync(m => m.Id==id);
         if (photo==null) {
            return NotFound();
         }

         return View(photo);
      }





      // GET: Photos/Create
      public IActionResult Create() {
         // SELECT c.Id, c.Category
         // FROM Categories c
         // ORDER BY c.Category
         ViewData["CategoryFK"]=new SelectList(_context.Categories.OrderBy(c => c.Category),"Id","Category");


         /* at this moment, we do not need this code anymore
          * if a user is authenticated, we will use its data
          */
         //// SELECT u.Id, u.Name
         //// FROM MyUsers u
         //// ORDER BY u.Name
         //ViewData["OwnerFK"] = new SelectList(_context.MyUsers.OrderBy(u => u.Name), "Id", "Name");


         return View();
      }





      // POST: Photos/Create
      // To protect from overposting attacks, enable the specific properties you want to bind to.
      // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Create([Bind("Description,Date,FileName,PriceAux,CategoryFK,OwnerFK")] Photos photo,IFormFile PhotoFile) {

         // aux. vars.
         bool isError = false;
         string imageName = "";

         /* Algorithm to deal with Photos
          * 1- ensure that we receive a file
          *    if is null, notify user, and send control to user
          * 2- ensure that you receive a Photo (JPEG or PNG)
          *    if not, notify user, and resend control to he
          * 3- save the image to disk drive and add its data to database
          *    3.1- define the file name
          *    3.2- assign that name to database
          *    3.3- save the image to disk drive 
          */

         // 1-
         if (PhotoFile is null) {
            ModelState.AddModelError("","Please, you must add an image.");
            // return View(photo);
            isError=true;
         }

         // 2-
         // https://developer.mozilla.org/en-US/docs/Web/HTTP/Guides/MIME_types
         if (!(PhotoFile.ContentType=="image/jpeg"||PhotoFile.ContentType=="image/png")) {
            ModelState.AddModelError("","Please, you are uploading a file, but you must upload an image.");
            isError=true;
         }

         // 3-
         // we have an image :-)
         // 3.1-
         imageName=Guid.NewGuid().ToString();
         string extension = Path.GetExtension(PhotoFile.FileName).ToLower();
         imageName+=extension;

         // 3.2-
         photo.FileName=imageName;

         // 3.3-
         // this will be done only after the data was saved to database



         if (!isError&&ModelState.IsValid) {
            try {
               // Lets assign the PriceAux to Price
               if (!photo.PriceAux.IsNullOrEmpty()) {
                  photo.Price=Convert.ToDecimal(photo.PriceAux.Replace('.',','),
                     new CultureInfo("tr-TR")
                     );
               }

               // we need to assign the 'owner' to the photo
               string userName = User.Identity.Name; // this is the login name

               // we use this code to figure out the Owner's ID, or
               // instead the code commented bellow
               //var idOwner = await _context.MyUsers
               //                .Where(u => u.UserName==userName)
               //                .Select(u => u.Id)
               //                .FirstOrDefaultAsync();
               //photo.OwnerFK=idOwner;

               var owner = await _context.MyUsers
                                           .Where(u => u.UserName==userName)
                                           .FirstOrDefaultAsync();
               photo.Owner=owner;


               _context.Add(photo);
               await _context.SaveChangesAsync();
            }
            catch (Exception) {
               // please, remember that you need to deal with the exception!!!!
               // because, otherwise is the same of not having the Try-Catch
               throw;
            }

            // 3.3-
            // if we arrive here, the data of your photo is saved on database
            // we will save photo file to diskdrive
            // where to store the file?
            string whereToStoreTheImage = _webHostEnvironment.WebRootPath;
            whereToStoreTheImage=Path.Combine(whereToStoreTheImage,"images");
            if (!Directory.Exists(whereToStoreTheImage)) {
               Directory.CreateDirectory(whereToStoreTheImage);
            }
            // join the location of your file to its name 
            imageName=Path.Combine(whereToStoreTheImage,imageName);
            // write the file to disk drive
            using var stream = new FileStream(
                imageName,FileMode.Create
             );
            await PhotoFile.CopyToAsync(stream);




            return RedirectToAction(nameof(Index));
         }

         ViewData["CategoryFK"]=new SelectList(_context.Categories,"Id","Category",photo.CategoryFK);
         ViewData["OwnerFK"]=new SelectList(_context.MyUsers,"Id","Name",photo.OwnerFK);

         // if we arrive here, something went wrong...
         return View(photo);
      }







      // GET: Photos/Edit/5
      public async Task<IActionResult> Edit(int? id) {
         if (id==null) {
            return NotFound();
         }

         var photo = await _context.Photos.FindAsync(id);
         if (photo==null) {
            return NotFound();
         }
         ViewData["CategoryFK"]=new SelectList(_context.Categories.OrderBy(c => c.Category),"Id","Category",photo.CategoryFK);
         ViewData["OwnerFK"]=new SelectList(_context.MyUsers.OrderBy(u => u.Name),"Id","Name",photo.OwnerFK);

         return View(photo);
      }

      // POST: Photos/Edit/5
      // To protect from overposting attacks, enable the specific properties you want to bind to.
      // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id,[Bind("Id,Description,Date,FileName,Price,CategoryFK,OwnerFK")] Photos photo) {
         if (id!=photo.Id) {
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
         ViewData["CategoryFK"]=new SelectList(_context.Categories,"Id","Category",photo.CategoryFK);
         ViewData["OwnerFK"]=new SelectList(_context.MyUsers,"Id","Id",photo.OwnerFK);
         return View(photo);
      }

      // GET: Photos/Delete/5
      public async Task<IActionResult> Delete(int? id) {
         if (id==null) {
            return NotFound();
         }

         var photo = await _context.Photos
             .Include(p => p.Category)
             .Include(p => p.Owner)
             .FirstOrDefaultAsync(m => m.Id==id);
         if (photo==null) {
            return NotFound();
         }

         return View(photo);
      }

      // POST: Photos/Delete/5
      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> DeleteConfirmed(int id) {
         var photo = await _context.Photos.FindAsync(id);
         if (photo!=null) {
            _context.Photos.Remove(photo);
         }

         await _context.SaveChangesAsync();
         return RedirectToAction(nameof(Index));
      }

      private bool PhotoExists(int id) {
         return _context.Photos.Any(e => e.Id==id);
      }
   }
}
