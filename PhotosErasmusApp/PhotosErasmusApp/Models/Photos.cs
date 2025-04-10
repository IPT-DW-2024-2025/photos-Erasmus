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
      [DataType(DataType.Date)]
      [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}",
         ApplyFormatInEditMode = true)]
      public DateTime Date { get; set; }

      /// <summary>
      /// The file name of file with the photo on disk drive
      /// </summary>
      public string FileName { get; set; } = string.Empty;

      /// <summary>
      /// The price to sell the photo
      /// </summary>
      public Decimal Price { get; set; }

      /// <summary>
      /// Auxilary atribute to collect the price of a photo
      /// </summary>
      [NotMapped] // this means, this attribute is to be used ONLY in C#
      [Display(Name ="Price")]
      [RegularExpression("[0-9]{1,5}([,.][0-9]{1,2})?",
         ErrorMessage ="please, write up to five numbers as price, " +
         "with up two decimal values")]
      [StringLength(8)]
      public string? PriceAux { get; set; }


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
