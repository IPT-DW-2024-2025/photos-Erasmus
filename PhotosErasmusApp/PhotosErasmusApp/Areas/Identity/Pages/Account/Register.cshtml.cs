﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using PhotosErasmusApp.Data;
using PhotosErasmusApp.Models;

namespace PhotosErasmusApp.Areas.Identity.Pages.Account {
   public class RegisterModel: PageModel {
      private readonly SignInManager<IdentityUser> _signInManager;
      private readonly UserManager<IdentityUser> _userManager;
      private readonly IUserStore<IdentityUser> _userStore;
      private readonly IUserEmailStore<IdentityUser> _emailStore;
      private readonly ILogger<RegisterModel> _logger;
      private readonly IEmailSender _emailSender;
      private readonly ApplicationDbContext _context;

      public RegisterModel(
          UserManager<IdentityUser> userManager,
          IUserStore<IdentityUser> userStore,
          SignInManager<IdentityUser> signInManager,
          ILogger<RegisterModel> logger,
          IEmailSender emailSender,
          ApplicationDbContext context
          ) {
         _userManager = userManager;
         _userStore = userStore;
         _emailStore = GetEmailStore();
         _signInManager = signInManager;
         _logger = logger;
         _emailSender = emailSender;
         _context = context;
      }

      /// <summary>
      ///     This attribut is used to collect data from browser and send it
      ///     to the server to be precessed
      /// </summary>
      [BindProperty]
      public InputModel Input { get; set; }

      /// <summary>
      ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
      ///     directly from your code. This API may change or be removed in future releases.
      /// </summary>
      public string ReturnUrl { get; set; }

      /// <summary>
      ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
      ///     directly from your code. This API may change or be removed in future releases.
      /// </summary>
      public IList<AuthenticationScheme> ExternalLogins { get; set; }

      /// <summary>
      ///     This is a View Model class.
      ///     it is used to 'transport' data from/to browser
      /// </summary>
      public class InputModel {
         /// <summary>
         ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
         ///     directly from your code. This API may change or be removed in future releases.
         /// </summary>
         [Required]
         [EmailAddress]
         [Display(Name = "Email")]
         public string Email { get; set; }

         /// <summary>
         ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
         ///     directly from your code. This API may change or be removed in future releases.
         /// </summary>
         [Required]
         [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
         [DataType(DataType.Password)]
         [Display(Name = "Password")]
         public string Password { get; set; }

         /// <summary>
         ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
         ///     directly from your code. This API may change or be removed in future releases.
         /// </summary>
         [DataType(DataType.Password)]
         [Display(Name = "Confirm password")]
         [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
         public string ConfirmPassword { get; set; }

         /// <summary>
         /// this attribute is used to collects the data of MyUser, when someone
         /// registers to my system
         /// </summary>
         public MyUsers MyUser { get; set; }

      }


      // This function is 'connected' with the HTTP GET verb of HTTP protocol 
      public async Task OnGetAsync(string returnUrl = null) {
         ReturnUrl = returnUrl;
         ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
      }


      // This function is 'connected' with the HTTP POST verb of HTTP protocol 

      public async Task<IActionResult> OnPostAsync(string returnUrl = null) {
         // This variable is used to redirect the user to a new URL,
         // after autentication process finish
         returnUrl ??= Url.Content("~/");

         // if you are using 'external login' tools, you can use it
         ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();


         // if all data that you need is OK, you can continue
         if (ModelState.IsValid) {

            var user = CreateUser();

            // assign the email as 'UserName' and the email as 'email' to the new user
            await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
            // create the new user
            var result = await _userManager.CreateAsync(user, Input.Password);

            // if it is possible to create the user
            if (result.Succeeded) {
               _logger.LogInformation("User created a new account with password.");

               // *********************************************
               // save the personal user's data to the database
               // *********************************************

               try {
                  // assign the autentication UserName to the User's data
                  Input.MyUser.UserName = Input.Email;
                  //  Input.MyUser.UserName = user.UserName;   // if you want, you can use this line, also

                  _context.Add(Input.MyUser);
                  await _context.SaveChangesAsync();
               }
               catch (Exception) {
                  // YOU MUST DEAL WITH THE EXCEPTION
                  /*
                   * - write on the disk drive of your server a log with the error
                   * - send an email to your app administrator with the error
                   * - write to database, to ERROR table, the data related with the error
                   *     this can be tricky if the problem is with the database access
                   * - and,
                   *    - delete the autentication data - the user's data
                   * - send a message to user' screen, informing that you have a problem
                   */
                  // throw;
               }
               // *********************************************



               var userId = await _userManager.GetUserIdAsync(user);
               var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
               code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
               var callbackUrl = Url.Page(
                   "/Account/ConfirmEmail",
                   pageHandler: null,
                   values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                   protocol: Request.Scheme);

               await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                   $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

               if (_userManager.Options.SignIn.RequireConfirmedAccount) {
                  return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
               }
               else {
                  await _signInManager.SignInAsync(user, isPersistent: false);
                  return LocalRedirect(returnUrl);
               }
            }
            foreach (var error in result.Errors) {
               ModelState.AddModelError(string.Empty, error.Description);
            }
         }

         // If we got this far, something failed, redisplay form
         return Page();
      }

      private IdentityUser CreateUser() {
         try {
            return Activator.CreateInstance<IdentityUser>();
         }
         catch {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
         }
      }

      private IUserEmailStore<IdentityUser> GetEmailStore() {
         if (!_userManager.SupportsUserEmail) {
            throw new NotSupportedException("The default UI requires a user store with email support.");
         }
         return (IUserEmailStore<IdentityUser>)_userStore;
      }
   }
}
