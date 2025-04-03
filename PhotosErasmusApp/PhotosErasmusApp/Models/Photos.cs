using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotosErasmusApp.Models {
   public class Photos {

      [Key] // Primary Key
      public int Id { get; set; }

      public string Description { get; set; } = string.Empty;

      public DateTime Date { get; set; }

      public string FileName { get; set; } = string.Empty;

      public Decimal Price { get; set; }

      // ************************************
      // Foreign Keys
      // ************************************


      [ForeignKey(nameof(Category))]
      public int CategoryFK { get; set; }
      public Categories Category { get; set; }=new Categories();


      [ForeignKey(nameof(Owner))]
      public int OwnerFK { get; set; }
      public MyUsers Owner { get; set; }=new MyUsers();


      public ICollection<Likes> ListOfLikes { get; set; } = [];

    }
}
