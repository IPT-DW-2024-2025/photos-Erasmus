using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using PhotosErasmusApp.Models;

namespace PhotosErasmusApp.Data {
   
   /// <summary>
   /// it express the data base of our projetc
   /// </summary>
   public class ApplicationDbContext: IdentityDbContext {
      // create schema ApplicationDbContext

      public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
          : base(options) {
      }

        // define the 'tables' names of our DB
        public DbSet<MyUsers> MyUsers { get; set; }
        public DbSet<Photos> Photos { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Likes> Likes { get; set; }

    }
}
