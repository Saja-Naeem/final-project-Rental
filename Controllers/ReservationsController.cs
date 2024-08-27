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
    public class ReservationsController : Controller
    {
        private readonly MyContext _context;

        public ReservationsController(MyContext context)
        {
            _context = context;
        }

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            ViewBag.FirstName = HttpContext.Session.GetString("FirstName");

            var myContext = _context.Reservations.Include(r => r.Car).Include(r => r.User);
            return View(await myContext.ToListAsync());
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.FirstName = HttpContext.Session.GetString("FirstName");

            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Car)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.ReservationID == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservations/Create
        public IActionResult Create()
        {
            ViewBag.FirstName = HttpContext.Session.GetString("FirstName");
            ViewBag.UserId=HttpContext.Session.GetString("UserID");
            ViewData["CarID"] = new SelectList(_context.cars, "CarID", "CarID");
            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "UserID");
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReservationID,UserID,CarID,PickupLocation,DropoffLocation,PickupDate,DropoffDate,ReservationStatus")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                reservation.ReservationStatus = ReservationStatus.Pending;


                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Reservations");
               /* return RedirectToAction(nameof(Index));*/
            }
            ViewData["CarID"] = new SelectList(_context.cars, "CarID", "CarID", reservation.CarID);
            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "UserID", reservation.UserID);
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.FirstName = HttpContext.Session.GetString("FirstName");

            ViewBag.FirstName = HttpContext.Session.GetString("FirstName");

            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewData["CarID"] = new SelectList(_context.cars, "CarID", "CarID", reservation.CarID);
            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "UserID", reservation.UserID);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReservationID,UserID,CarID,PickupLocation,DropoffLocation,PickupDate,DropoffDate,ReservationStatus")] Reservation reservation)
        {
            if (id != reservation.ReservationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.ReservationID))
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
            ViewData["CarID"] = new SelectList(_context.cars, "CarID", "CarID", reservation.CarID);
            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "UserID", reservation.UserID);
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.FirstName = HttpContext.Session.GetString("FirstName");

            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Car)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.ReservationID == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.ReservationID == id);
        }
    }
}
