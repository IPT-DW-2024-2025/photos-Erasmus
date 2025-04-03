using System.ComponentModel.DataAnnotations;

namespace PhotosErasmusApp.Models {
   /// <summary>
   /// table to describe the category that a photo must have
   /// </summary>
   public class Categories {

      [Key]
      public int Id { get; set; }

      /// <summary>
      /// The name of the category
      /// </summary>
      public string Category { get; set; } = string.Empty;

      // *************************************
      // Foreign Key relations
      // *************************************

      /// <summary>
      /// The list of photos that has a category
      /// </summary>
      public ICollection<Photos> PhotosList { get; set; } = [];






    }
}
