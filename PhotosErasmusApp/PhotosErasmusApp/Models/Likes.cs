using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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
      [ValidateNever]
      public Photos Photo { get; set; } = null!;


      [ForeignKey(nameof(Person))]
      public int PersonFK { get; set; }
      [ValidateNever]
      public MyUsers Person { get; set; } = null!;

   }
}
