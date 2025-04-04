using System.ComponentModel.DataAnnotations;

namespace PhotosErasmusApp.Models {
   public class MyUsers {

      [Key]
      public int Id { get; set; }

      /// <summary>
      /// Name of user
      /// </summary>
      [StringLength(50)]
      [Required(ErrorMessage = "{0} is mandatory")]
      public string Name { get; set; } = string.Empty;

      /// <summary>
      /// Cell phone of user
      /// </summary>
      [Required(ErrorMessage = "{0} is mandatory")]
      [StringLength(20)]
      [RegularExpression("([+]|00)?[0-9]{5-18}")]
      public string CellPhone { get; set; } = string.Empty;

      /// <summary>
      /// Street of address of user
      /// </summary>
      [StringLength(50)]
      public string? Street { get; set; } = string.Empty;

      /// <summary>
      /// Postal code of address of user
      /// </summary>
      [StringLength(50)]
      [RegularExpression("[1-9][0-9]{3,6}-[0-9]{3,5}( [a-zA-Z ]+)?", ErrorMessage = "use only english letters")]
      public string? PostalCode { get; set; } = string.Empty;

      /// <summary>
      /// Country of address of user
      /// </summary>
      [StringLength(50)]
      public string? Country { get; set; } = string.Empty;


      // *************************************
      // Foreign Key relations
      // *************************************

      /// <summary>
      /// list of photos that the user owns
      /// </summary>
      public ICollection<Photos> PhotosList { get; set; } = [];

      /// <summary>
      /// list of photos that user likes 
      /// </summary>
      public ICollection<Likes> ListOfLikes { get; set; } = [];

   }
}
