using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Rental.Context;
using Rental.Models;
using System.Security.Cryptography;
using System.Text;

namespace Rental.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly MyContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;
        public AuthenticationController(MyContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this.webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }


        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("UserID,FirstName,LastName,ImagePath,IsActive,IsDeleted,ImageFile")] User user, string email, string password)

        {
            /*if (ModelState.IsValid)
            {*/
            /*user.Address = "Default Address";
            user.PhoneNumber = "000-000-0000";*/

            if (user.ImageFile != null)
            {
                string wwwRootPath = webHostEnvironment.WebRootPath;

                string fileName = Guid.NewGuid().ToString() + user.ImageFile.FileName;

                string path = Path.Combine(wwwRootPath + "/Images/" + fileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await user.ImageFile.CopyToAsync(fileStream);
                }

                user.ImagePath = fileName;
            }
            var userr = _context.UserLogins.Where(x => x.UserName == email).FirstOrDefault();


            if (userr == null)
            {

                _context.Add(user);
                await _context.SaveChangesAsync();


                var role = _context.Roles.FirstOrDefault(r => r.Rolename == "User");

                if (role == null)
                {
                    // Handle case where the role doesn't exist
                    ModelState.AddModelError("", "Role not found.");
                    /*return View(user);*/
                }



                UserLogin userLogin = new UserLogin();
                userLogin.UserName = email;
                userLogin.Password = HashPassword(password);
                userLogin.RoleId = 2;
                userLogin.UserId = user.UserID;
                _context.Add(userLogin);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = "Email is already used, please try another one. ";
            }
            /*}*/
            return View(user);
        }

        /* [HttpPost]
         public async Task<IActionResult> Login(string UserName, string Password)
         {

         }*/

        [HttpPost]
        public async Task<IActionResult> Login([Bind("Id,UserName,Password")] UserLogin userLogin)
        {
            var hashedPassword = HashPassword(userLogin.Password);
            var auth = _context.UserLogins.Where(x => x.UserName == userLogin.UserName && x.Password == hashedPassword).FirstOrDefault();

            if (auth != null)
            {
                var user = _context.Users.Where(x => x.UserID == auth.UserId).FirstOrDefault();
                switch (auth.RoleId)
                {
                    case 1:
                        HttpContext.Session.SetString("UserID", user.UserID.ToString());
                        HttpContext.Session.SetString("FirstName",user.FirstName);
                        HttpContext.Session.SetString("LastName", user.LastName);
                        HttpContext.Session.SetString("UserName", auth.UserName);
                        return RedirectToAction("Index", "Admin");
                    case 2:
                        HttpContext.Session.SetString("UserID", user.UserID.ToString());
                        HttpContext.Session.SetString("FirstName", user.FirstName);
                        HttpContext.Session.SetString("LastName", user.LastName);
                        HttpContext.Session.SetString("UserName", auth.UserName);
                        HttpContext.Session.SetString("Image", user.ImagePath);



                        return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ViewBag.Error = "Wrong credentials";
            }
            return View(userLogin);
        }


        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
