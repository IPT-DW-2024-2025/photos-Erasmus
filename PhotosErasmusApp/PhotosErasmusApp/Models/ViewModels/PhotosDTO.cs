namespace PhotosErasmusApp.Models.ViewModels {

   /// <summary>
   /// View Model to expose the photos data to API
   /// </summary>
   public class PhotosDTO {

      /// <summary>
      /// description of the photo, made by author's photo
      /// </summary>
      public string PhotoDescription { get; set; } = "";

      /// <summary>
      /// the name of file that has the photo
      /// </summary>
      public string PhotoFile { get; set; } = "";

      /// <summary>
      /// the category of photo
      /// </summary>
      public string Category { get; set; } = "";

      /// <summary>
      /// Date when photo was taken
      /// </summary>
      public DateTime Date { get; set; }

   }
}
