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
    public class TestimonialsController : Controller
    {
        private readonly MyContext _context;

        public TestimonialsController(MyContext context)
        {
            _context = context;
        }

        // GET: Testimonials
        public async Task<IActionResult> Index()
        {
            ViewBag.FirstName = HttpContext.Session.GetString("FirstName");

            /*var testimonials = await _context.Testimonials.Include(t => t.User).ToListAsync();
            return View(testimonials);*/

            var myContext = _context.Testimonials.Include(t => t.User);
            return View(await myContext.ToListAsync());
        }

        // GET: Testimonials/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.FirstName = HttpContext.Session.GetString("FirstName");

            if (id == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonials
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.TestimonialID == id);
            if (testimonial == null)
            {
                return NotFound();
            }

            return View(testimonial);
        }

        // GET: Testimonials/Create
        public IActionResult Create()
        {

            ViewBag.FirstName = HttpContext.Session.GetString("FirstName");
            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "UserID");
           /* ViewData["UserID"] = new SelectList(_context.Users, "UserID", "FirstName");*/
            return View();
            /*ViewData["UserID"] = new SelectList(_context.Users, "UserID", "FirstName");
            return View(new Testimonial()); // Pass a new Testimonial object to the view*/
        }

        // POST: Testimonials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TestimonialID,UserID,Message,Date,IsDeleted,TestimonialStatus")] Testimonial testimonial)
        {
            if (ModelState.IsValid)
            {
                _context.Add(testimonial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "UserID", testimonial.UserID);
            return View(testimonial);
        }

        // GET: Testimonials/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.FirstName = HttpContext.Session.GetString("FirstName");

            if (id == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonials.FindAsync(id);
            if (testimonial == null)
            {
                return NotFound();
            }


/*            ViewBag.StatusList = new SelectList(Enum.GetValues(typeof(TestimonialStatus))
    .Cast<TestimonialStatus>()
    .Select(e => new SelectListItem
    {
        Value = e.ToString(),
        Text = e.ToString()
    }), "Value", "Text", testimonial.TestimonialStatus.ToString());
*/


            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "UserID", testimonial.UserID);
            return View(testimonial);
        }

        // POST: Testimonials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TestimonialID,UserID,Message,Date,IsDeleted,TestimonialStatus")] Testimonial testimonial)
        {
            if (id != testimonial.TestimonialID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(testimonial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestimonialExists(testimonial.TestimonialID))
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



/*            ViewBag.StatusList = new SelectList(Enum.GetValues(typeof(TestimonialStatus))
        .Cast<TestimonialStatus>()
        .Select(e => new SelectListItem
        {
            Value = e.ToString(),
            Text = e.ToString()
        }), "Value", "Text", testimonial.TestimonialStatus.ToString());
*/


            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "UserID", testimonial.UserID);
            return View(testimonial);
        }

        // GET: Testimonials/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.FirstName = HttpContext.Session.GetString("FirstName");

            if (id == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonials
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.TestimonialID == id);
            if (testimonial == null)
            {
                return NotFound();
            }

            return View(testimonial);
        }

        // POST: Testimonials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var testimonial = await _context.Testimonials.FindAsync(id);
            if (testimonial != null)
            {
                _context.Testimonials.Remove(testimonial);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestimonialExists(int id)
        {
            return _context.Testimonials.Any(e => e.TestimonialID == id);
        }

        public async Task<IActionResult> Accept(int? id)
        {
            var testimonial = await _context.Testimonials.FindAsync(id);
            testimonial.TestimonialStatus = TestimonialStatus.Accepted;
            _context.Update(testimonial);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Reject(int? id)
        {
            var testimonial = await _context.Testimonials.FindAsync(id);
            testimonial.TestimonialStatus = TestimonialStatus.Rejected;
            _context.Update(testimonial);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }
    }
}
