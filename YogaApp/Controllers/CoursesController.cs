using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using YogaApp.Models;

namespace YogaApp.Controllers
{
    public class CoursesController : Controller
    {
        private readonly YogaSystemContext _context;

        public CoursesController(YogaSystemContext context)
        {
            _context = context;
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            var yogaSystemContext = _context.Courses.Include(c => c.Location).Include(c => c.Status);
            return View(await yogaSystemContext.ToListAsync());
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Location)
                .Include(c => c.Status)
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
/*        public IActionResult Create()
        {
            ViewData["LocationId"] = new SelectList(_context.Locations, "LocationId", "LocationId");
            ViewData["StatusId"] = new SelectList(_context.CourseStatusLkps, "StatusId", "StatusId");
            return View();
        }*/


        public IActionResult Create(int? id)
        {
            ViewData["selectedLocationId"] = id;
            

            IEnumerable<LocationInstructorLink> availableInstrucors =  _context.LocationInstructorLinks.Include(y => y.Instructor)
                .Where(x => x.LocationId == id) ;

      

            List<SelectListItem> insList = new List<SelectListItem>();

            foreach(LocationInstructorLink lil in availableInstrucors)
            {
                insList.Add(new SelectListItem { Value = lil.InstructorId.ToString(), Text = lil.Instructor.Name.ToString() });
            }
            
            ViewData["AvailableInstructors"] = insList;
            ViewData["StatusId"] = new SelectList(_context.CourseStatusLkps, "StatusId", "Status");

            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseId,Title,Description,RequiredExpertise,StartDate,EndDate,StartTime,EndTime,SessionCount,PricePerSession,Capacity,LocationId,InstructorId,StatusId,PreviousOfferingCourseId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate")] Course course)
        {
            if (ModelState.IsValid)
            {

                course.CreatedDate = DateTime.Now;

                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Locations");
            }

            return RedirectToAction("Index", "Locations");

        }
        


        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);

                
            if (course == null)
            {
                return NotFound();
            }


            ViewData["StatusId"] = new SelectList(_context.CourseStatusLkps, "StatusId", "Status", course.StatusId);
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseId,Title,Description,RequiredExpertise,StartDate,EndDate,StartTime,EndTime,SessionCount,PricePerSession,Capacity,LocationId,InstructorId,StatusId")] Course course)
        {
            if (id != course.CourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //check the new status of the course


                    //updat eflags
                    course.LastModifiedDate = DateTime.Now;

                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.CourseId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Locations");
            }

            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Location)
                .Include(c => c.Status)
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Courses == null)
            {
                return Problem("Entity set 'YogaSystemContext.Courses'  is null.");
            }
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
          return (_context.Courses?.Any(e => e.CourseId == id)).GetValueOrDefault();
        }
    }
}
