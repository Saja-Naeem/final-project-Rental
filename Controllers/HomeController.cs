using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rental.Context;
using Rental.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace Rental.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MyContext _myContext;
        public HomeController(ILogger<HomeController> logger, MyContext _myContext)
        {
            _logger = logger;
            this._myContext = _myContext;

        }

        public IActionResult Index()
        {
            /* var testimonials = _myContext.Testimonials
                 .Include(t => t.User) // Include related user data if needed
                 .ToList();
             return View(testimonials);*/
            ViewBag.Image = HttpContext.Session.GetString("Image");
            var testimonials = _myContext.Testimonials
         .Include(t => t.User) // Include related user data if needed
        /* .Where(t => t.TestimonialStatus == TestimonialStatus.Accepted)*/ // Filter to show only approved testimonials
         .OrderByDescending(t => t.Date) // Order by date, most recent first
         .ToList();

            return View(testimonials);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Cars(int categoryId)
        {
            var categories = _myContext.categories.ToList();

            /*var cars = _myContext.cars.Where(c => c.CategoryID == categoryId).ToList();*/
            var cars = _myContext.cars
                .Include(c => c.Category)         
                .Include(c => c.Transmission)    
                .Where(c => c.CategoryID == categoryId)
                .ToList();

            var viewModel = new CarsViewModel
            {
                Categories = categories,
                Cars = cars
            };
            return View(viewModel);
        }
        public IActionResult Testimonial()
        {
            /*var testimonial = _myContext.Testimonials
                .Include(t => t.User)
                .ToList();
            return View(testimonial);*/
            return View();
        }

        /*[Authorize(Roles = SD.Role_Customer)]*/
        /*[Authorize]*/
        public IActionResult AddTestimonial(Testimonial testimonial)
        {
            /*if (ModelState.IsValid)
            {*/
            ViewBag.UserID = HttpContext.Session.GetString("UserID");
            testimonial.UserID = ViewBag.UserID;
            testimonial.Date = DateTime.Now;
                testimonial.TestimonialStatus = TestimonialStatus.Pending; // Or any other default status
               
                _myContext.Testimonials.Add(testimonial);
                _myContext.SaveChanges();

                return RedirectToAction("Index", "Home"); // Redirect to the index page after saving
            /*} */

            return View("Testimonial");
        }
        public IActionResult AboutUs()
        {
            return View();
        }
        public IActionResult ContactUs()
        {
            return View();
        }
        public IActionResult Payment()
        {
            return View();
        }


        public IActionResult DetailsCar(int id)
        {
            var car = _myContext.cars
        .Include(c => c.Category)
        .Include(c => c.Transmission) // Include related entities if needed
        .FirstOrDefault(c => c.CarID == id);



            return View(car);
        }
        [HttpPost]
        public IActionResult AddReservation( Reservation reservation)
        {
            var userId = HttpContext.Session.GetString("UserID");

            if (userId == null)
            {
                // Handle the case where the user is not logged in
                return RedirectToAction("Login", "Authentication");
            }
            reservation.ReservationStatus = ReservationStatus.Approved;
            reservation.UserID = userId;
           /* reservation.CarID = carId;*/
            _myContext.Reservations.Add(reservation);
            _myContext.SaveChanges();

            return RedirectToAction("Index", "Home", new { id = reservation.CarID });
        }

    }
}
