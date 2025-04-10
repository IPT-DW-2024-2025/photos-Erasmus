using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotosErasmusApp.Models {
   public class Photos {

      [Key] // Primary Key
      public int Id { get; set; }

      public string Description { get; set; } = string.Empty;

      /// <summary>
      /// the date when we took the photo
      /// </summary>
      public DateTime Date { get; set; }

      /// <summary>
      /// The file name of file with the photo on disk drive
      /// </summary>
      public string FileName { get; set; } = string.Empty;

      /// <summary>
      /// The price to sell the photo
      /// </summary>
      public Decimal Price { get; set; }

      // ************************************
      // Foreign Keys
      // ************************************


      [ForeignKey(nameof(Category))]
      [Display(Name = "Category")]
      public int CategoryFK { get; set; }
      public Categories Category { get; set; } = new Categories();


      [ForeignKey(nameof(Owner))]
      [Display(Name = "Owner of photo")]
      public int OwnerFK { get; set; }
      public MyUsers Owner { get; set; } = new MyUsers();


      public ICollection<Likes> ListOfLikes { get; set; } = [];

   }
}
