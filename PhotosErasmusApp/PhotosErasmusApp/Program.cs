using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using PhotosErasmusApp.Data;

using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")??throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();



builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount=true)
   .AddRoles<IdentityRole>() // we need authorization to read the 'roles' data
   .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();



// allows the use of cookies on our project
builder.Services.AddSession(
   options => {
      options.IdleTimeout=TimeSpan.FromSeconds(30);
      options.Cookie.HttpOnly=true;
      options.Cookie.IsEssential=true;
   } );
builder.Services.AddDistributedMemoryCache();



// *******************************************************************
// Install the package
// Microsoft.AspNetCore.Authentication.JwtBearer
//
// using Microsoft.IdentityModel.Tokens;
// *******************************************************************
// JWT Settings
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

builder.Services.AddAuthentication(options => { })
   .AddCookie("Cookies",options => {
      options.LoginPath="/Identity/Account/Login";
      options.AccessDeniedPath="/Identity/Account/AccessDenied";
   })
   .AddJwtBearer("Bearer",options => {
      options.TokenValidationParameters=new TokenValidationParameters {
         ValidateIssuer=true,
         ValidateAudience=true,
         ValidateLifetime=true,
         ValidateIssuerSigningKey=true,
         ValidIssuer=jwtSettings["Issuer"],
         ValidAudience=jwtSettings["Audience"],
         IssuerSigningKey=new SymmetricSecurityKey(key)
      };
   });


// configuração do JWT
builder.Services.AddScoped<TokenService>();







var app = builder.Build();




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
   app.UseMigrationsEndPoint();
}
else {
   app.UseExceptionHandler("/Home/Error");
   // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
   app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// starting to use the cookies
app.UseSession();


app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
