using System.ComponentModel.DataAnnotations;

namespace PhotosErasmusApp.Models {
   public class Categories {

      [Key]
      public int Id { get; set; }

      public string Category { get; set; } = string.Empty;

      // *************************************
      // Foreign Key relations
      // *************************************

      public ICollection<Photos> PhotosList { get; set; } = [];






    }
}
