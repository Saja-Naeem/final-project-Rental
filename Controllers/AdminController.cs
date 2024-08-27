using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;
using Rental.Context;
using Rental.Models;

namespace Rental.Controllers
{
    public class AdminController : Controller
    {
        private readonly MyContext _context;
        public AdminController(MyContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.FirstName = HttpContext.Session.GetString("FirstName");
            ViewBag.LastName = HttpContext.Session.GetString("LastName");
            ViewBag.UserID = HttpContext.Session.GetString("UserID");

            /*ViewBag.Image = HttpContext.Session.GetString("Image");*/
            ViewBag.NumOfUsers = _context.Users.Count();
            ViewBag.NumOfCars = _context.cars.Count();
            ViewBag.NumOfReservation = _context.Reservations.Count();
            return View();
        }
    }
}
