using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using PhotosErasmusApp.Data;
using PhotosErasmusApp.Models;
using PhotosErasmusApp.Models.ViewModels;

namespace PhotosErasmusApp.Controllers.API {

   /// <summary>
   /// this class will ask for the generation of JWToken
   /// for this, it will provide the 'login' and 'password' of an user
   /// </summary>
   [Route("api/[controller]")]
   [ApiController]
   public class AuthController:ControllerBase {
      private readonly ApplicationDbContext _context;
      private readonly UserManager<IdentityUser> _userManager;
      private readonly SignInManager<IdentityUser> _signInManager;
      private readonly IConfiguration _config;

      public AuthController(ApplicationDbContext context,
         UserManager<IdentityUser> userManager,
         SignInManager<IdentityUser> signInManager,
         IConfiguration config) {
         _context=context;
         _userManager=userManager;
         _signInManager=signInManager;
         _config=config;
      }


      [AllowAnonymous]
      [HttpPost("login")]
      public async Task<IActionResult> Login([FromBody] LoginModel login) {

         // search if the provided USER exists in the database
         var user = await _userManager.FindByEmailAsync(login.Username);
         if (user==null) return Unauthorized();

         // if you arrive here, the user EXISTS
         // but, the 'password' is correct?
         var result = await _signInManager.CheckPasswordSignInAsync(user,login.Password,false);
         if (!result.Succeeded) return Unauthorized();

         // the user EXISTS, and is AUTHENTICATED
         var token = GenerateJwtToken(login.Username);

         return Ok(new { token });
      }


      private string GenerateJwtToken(string username) {
         var claims = new[] {
         new Claim(ClaimTypes.Name, username)
     };

         var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(s: _config["Jwt:Key"]));
         var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

         var token = new JwtSecurityToken(
             issuer: _config["Jwt:Issuer"],
             audience: _config["Jwt:Audience"],
             claims: claims,
             expires: DateTime.Now.AddHours(Convert.ToDouble(_config["Jwt:ExpireHours"])),
             signingCredentials: creds);

         return new JwtSecurityTokenHandler().WriteToken(token);
      }

   }




}
