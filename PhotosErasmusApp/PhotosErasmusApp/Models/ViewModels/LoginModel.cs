namespace PhotosErasmusApp.Models.ViewModels {


   /// <summary>
   /// data relatated with the API Authentication process
   /// </summary>
   public class LoginModel {

      /// <summary>
      /// the user's Username
      /// </summary>
      public string Username { get; set; } = "";

      /// <summary>
      /// the user's Password
      /// </summary>
      public string Password { get; set; } = "";

   }
}
