using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations.Schema;

namespace PhotosErasmusApp.Models {


   [PrimaryKey(nameof(PhotoFK), nameof(PersonFK))]
   public class Likes {

      public DateTime Date { get; set; }

      // ************************************
      // Foreign Keys
      // ************************************

      [ForeignKey(nameof(Photo))]
      public int PhotoFK { get; set; }
      public Photos Photo { get; set; } = new Photos();


      [ForeignKey(nameof(Person))]
      public int PersonFK { get; set; }
      public MyUsers Person { get; set; } = new MyUsers();

   }
}
