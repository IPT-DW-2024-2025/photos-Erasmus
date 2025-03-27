using System.ComponentModel.DataAnnotations;

namespace PhotosErasmusApp.Models {
   public class Photos {

      [Key]
      public int Id { get; set; }

      public string Description { get; set; } = string.Empty;

      public DateTime Date { get; set; }

      public string FileName { get; set; } = string.Empty;

      public Decimal Price { get; set; }

   }
}
