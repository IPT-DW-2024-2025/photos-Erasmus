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
      [Display(Name ="Category")]
      [Required(ErrorMessage ="the {0} is mandatory. You must write up to 10 characters.")]
      [StringLength(10)]
      [RegularExpression("[A-Za-z ]{3,10}", 
         ErrorMessage ="Please, write only english letters for {0}")]
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
