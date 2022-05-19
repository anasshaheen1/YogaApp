using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using YogaApp.Areas.Identity.Data;
using YogaApp.Models;

namespace YogaApp.Controllers
{
    public class LocationsController : Controller
    {
        private readonly YogaSystemContext _context;
        private readonly UserManager<YogaAppUser> _userManager;


        public LocationsController(YogaSystemContext context, UserManager<YogaAppUser> userManager    )
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Locations
        public async Task<IActionResult> Index()
        {

            YogaAppUser yogaAppUser = _userManager.GetUserAsync(User).Result;

            Location? thisLocation = GetLocationByYogaUserId(yogaAppUser.YogaUserID);

            if (null == thisLocation)
            {
                //redirect to create page
                return RedirectToAction("Create");
            }
            else
            {
                //redirect to details page with the correct id
                return RedirectToAction("Details", new { id = thisLocation.LocationId});

            }


        }

        // GET: Locations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Locations == null)
            {
                return NotFound();
            }

            var location = await _context.Locations
                .Include(x => x.Reviews)
                .Include(x => x.Courses).ThenInclude(y=> y.Status)
                .Include(x => x.Courses).ThenInclude(y => y.ClassParticipantLinks)
                .Include(x => x.LocationInstructorLinks).ThenInclude(y=> y.Instructor)
                .FirstOrDefaultAsync(m => m.LocationId == id);
            if (location == null)
            {
                return NotFound();
            }




            var transactions = _context.Transactions
                .Include(x => x.Tstatus)
                .Include(x => x.Type)
                .Include(x => x.Instructor)
                .Include(x => x.Participant)
                .Where(x => x.LocationId == location.LocationId);

            ViewModel vm = new ViewModel();

            
            vm.thisLocation = location;
            vm.transactions = transactions;
            
            return View(vm);
        }

        // GET: Locations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Locations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LocationId,Description,OpeningTimes,ContactInfo,AddressPostcode,AddressFull,LastModifiedBy,LastModifiedDate")] Location location)
        {

            YogaAppUser yogaAppUser = _userManager.GetUserAsync(User).Result;

            if (ModelState.IsValid)
            {

                location.YogaUserId = yogaAppUser.YogaUserID;
                location.LastModifiedDate = DateTime.Now;
                location.LastModifiedBy = yogaAppUser.YogaUserID;

                _context.Add(location);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(location);
        }

        // GET: Locations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Locations == null)
            {
                return NotFound();
            }

            var location = await _context.Locations.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }
            return View(location);
        }

        // POST: Locations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LocationId,Description,OpeningTimes,ContactInfo,AddressPostcode,AddressFull,LastModifiedBy,LastModifiedDate")] Location location)
        {
            if (id != location.LocationId)
            {
                return NotFound();
            }

            YogaAppUser yogaAppUser = _userManager.GetUserAsync(User).Result;

            if (ModelState.IsValid)
            {

                try
                {
                    location.YogaUserId = yogaAppUser.YogaUserID;
                    location.LastModifiedDate = DateTime.Now;
                    location.LastModifiedBy = yogaAppUser.YogaUserID;

                    _context.Update(location);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocationExists(location.LocationId))
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
            return View(location);
        }

        // GET: Locations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Locations == null)
            {
                return NotFound();
            }

            var location = await _context.Locations
                .FirstOrDefaultAsync(m => m.LocationId == id);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        // POST: Locations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Locations == null)
            {
                return Problem("Entity set 'YogaSystemContext.Locations'  is null.");
            }
            var location = await _context.Locations.FindAsync(id);
            if (location != null)
            {
                _context.Locations.Remove(location);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LocationExists(int id)
        {
          return (_context.Locations?.Any(e => e.LocationId == id)).GetValueOrDefault();
        }

        private Location? GetLocationByYogaUserId(int yogauserid)
        {
            Location? findUser = _context.Locations?.FirstOrDefault(e => e.YogaUserId == yogauserid);

            return findUser;
        }

        public async Task<IActionResult> CancelCourse(int? id)
        {
            if (id.HasValue)
            {
                Course removable = _context.Courses.Where(x => x.CourseId == (int)id).FirstOrDefault();

                removable.StatusId = 6;                
                _context.Update(removable);
                await _context.SaveChangesAsync();
            }

            //send emails to users and instructor
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RemoveInstructor(int? id)
        {
            if (id.HasValue)
            {
                LocationInstructorLink lil = _context.LocationInstructorLinks.Where(x => x.Id == id).FirstOrDefault();
                
                if(lil == null)
                    return RedirectToAction("Index");

                lil.ApprovalStatusId= 3;
                lil.RemovalDate = DateTime.Now;
                _context.Update(lil);
                await _context.SaveChangesAsync();
            }

            //send emails to users and instructor
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ApproveInstructor(int? id)
        {
            if (id.HasValue)
            {
                YogaAppUser yogaAppUser = _userManager.GetUserAsync(User).Result;

                LocationInstructorLink lil = _context.LocationInstructorLinks.Where(x => x.Id == id).FirstOrDefault();

                if (lil == null)
                    return RedirectToAction("Index");

                lil.ApprovalStatusId = 2;
                lil.ApprovalDate = DateTime.Now;
                lil.ApprovalBy = yogaAppUser.YogaUserID;

                _context.Update(lil);
                await _context.SaveChangesAsync();
            }

            //send emails to users and instructor
            return RedirectToAction("Index");
        }


        
        public async Task<IActionResult> PayInstructorForCourse(int? id)
        {
            if (id.HasValue)
            {
                YogaAppUser yogaAppUser = _userManager.GetUserAsync(User).Result;

                Course course = _context.Courses.Include(z => z.ClassParticipantLinks).Where(x => x.CourseId == id)
                    .FirstOrDefault();

                if (null == course)
                    return RedirectToAction("Index");

                Transaction t = new Transaction();
                t.InstructorId = course.InstructorId;
                t.LocationId = course.LocationId;
                //pay 50% of fees to instructor
                int nParticipants = course.ClassParticipantLinks.Count;
                t.Amount = (decimal?) (0.5 * nParticipants * (double)course.PricePerSession.Value * course.SessionCount);
                t.TriggerDate = DateTime.Now;
                t.TriggerdBy = yogaAppUser.YogaUserID;

                t.TypeId = 2;//
                t.TstatusId = 1;

                _context.Add(t);

                course.StatusId = 7;//complete instructor paid
                _context.Update(course);

                await _context.SaveChangesAsync();
            }

            //send emails to users and instructor
            return RedirectToAction("Index");
        }


        public class ViewModel
        {
            public Location? thisLocation { get; set; }
            public IEnumerable<Instructor>? availableInstructors { get; set; }
    
            public IEnumerable<Transaction>? transactions { get; set; }


        }

    }
}
