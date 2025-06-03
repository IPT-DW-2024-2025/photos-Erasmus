namespace PhotosErasmusApp.Models.ViewModels {

   /// <summary>
   /// photos' data with owner
   /// </summary>
   public class PhotoWithOwnerDTO : PhotosDTO {

      /// <summary>
      /// the nome of Owner's photo
      /// </summary>
      public string Owner { get; set; } = "";

   }
}
