using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rental.Context;
using Rental.Models;

namespace Rental.Controllers
{
    public class CarsController : Controller
    {
        private readonly MyContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public CarsController(MyContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this.webHostEnvironment = webHostEnvironment;

        }
       
        // GET: Cars
        public async Task<IActionResult> Index()
        {
            ViewBag.FirstName = HttpContext.Session.GetString("FirstName");
           
            var myContext = _context.cars.Include(c => c.Category).Include(c => c.Transmission);
            return View(await myContext.ToListAsync());
        }

        // GET: Cars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.FirstName = HttpContext.Session.GetString("FirstName");
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.cars
                .Include(c => c.Category)
                .Include(c => c.Transmission)
                .FirstOrDefaultAsync(m => m.CarID == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // GET: Cars/Create
        public IActionResult Create()
        {
            ViewBag.FirstName = HttpContext.Session.GetString("FirstName");

            ViewData["CategoryID"] = new SelectList(_context.categories, "CategoryID", "CategoryName");
            ViewData["TransmissionID"] = new SelectList(_context.Transmissions, "TransmissionID", "TransmissionType");
            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CarID,CategoryID,TransmissionID,Name,Model,Price,NumberOfSeats,AvailabilityStatus,FuelType,Description,ImagePath,IsDeleted,CreatedAt,UpdatedAt,ImageFile")] Car car)
        {
            if (ModelState.IsValid)
            {

                if (car.ImageFile != null)
                {
                    string wwwRootPath = webHostEnvironment.WebRootPath;

                    string fileName = Guid.NewGuid().ToString() + car.ImageFile.FileName;

                    string path = Path.Combine(wwwRootPath + "/Images/" + fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await car.ImageFile.CopyToAsync(fileStream);
                    }

                    car.ImagePath = fileName;
                }


                _context.Add(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryID"] = new SelectList(_context.categories, "CategoryID", "CategoryName", car.CategoryID);
            ViewData["TransmissionID"] = new SelectList(_context.Transmissions, "TransmissionID", "TransmissionType", car.TransmissionID);
            return View(car);
        }

        // GET: Cars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.FirstName = HttpContext.Session.GetString("FirstName");

            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            ViewData["CategoryID"] = new SelectList(_context.categories, "CategoryID", "CategoryName", car.CategoryID);
            ViewData["TransmissionID"] = new SelectList(_context.Transmissions, "TransmissionID", "TransmissionType", car.TransmissionID);
            return View(car);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CarID,CategoryID,TransmissionID,Name,Model,Price,NumberOfSeats,AvailabilityStatus,FuelType,Description,ImagePath,IsDeleted,CreatedAt,UpdatedAt,ImageFile")] Car car)
        {
            if (id != car.CarID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (car.ImageFile != null)
                    {
                        string wwwRootPath = webHostEnvironment.WebRootPath;

                        string fileName = Guid.NewGuid().ToString() + car.ImageFile.FileName;

                        string path = Path.Combine(wwwRootPath + "/Images/" + fileName);

                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await car.ImageFile.CopyToAsync(fileStream);
                        }

                        car.ImagePath = fileName;
                    }
                    _context.Update(car);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExists(car.CarID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryID"] = new SelectList(_context.categories, "CategoryID", "CategoryName", car.CategoryID);
            ViewData["TransmissionID"] = new SelectList(_context.Transmissions, "TransmissionID", "TransmissionType", car.TransmissionID);
            return View(car);
        }

        // GET: Cars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.FirstName = HttpContext.Session.GetString("FirstName");

            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.cars
                .Include(c => c.Category)
                .Include(c => c.Transmission)
                .FirstOrDefaultAsync(m => m.CarID == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var car = await _context.cars.FindAsync(id);
            if (car != null)
            {
                _context.cars.Remove(car);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarExists(int id)
        {
            return _context.cars.Any(e => e.CarID == id);
        }
    }
}
