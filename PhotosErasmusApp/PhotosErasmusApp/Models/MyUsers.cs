using System.ComponentModel.DataAnnotations;

namespace PhotosErasmusApp.Models {
   public class MyUsers {

      [Key]
      public int Id { get; set; }

      public string Name { get; set; } = string.Empty;

      public string CellPhone { get; set; } = string.Empty;

      public string Street { get; set; } = string.Empty;

      public string PostalCode { get; set; } = string.Empty;

      public string Country { get; set; } = string.Empty;


      // *************************************
      // Foreign Key relations
      // *************************************

      public ICollection<Photos> PhotosList { get; set; } = [];

      public ICollection<Likes> ListOfLikes { get; set; } = [];

   }
}
