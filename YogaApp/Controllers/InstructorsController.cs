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

namespace YogaApp.Views
{
    public class InstructorsController : Controller
    {
        private readonly YogaSystemContext _context;
        private readonly UserManager<YogaAppUser> _userManager;

        public InstructorsController(YogaSystemContext context, UserManager<YogaAppUser> userManager  )
        {
            _context = context;
            _userManager= userManager;
        }

        // GET: Instructors
        public async Task<IActionResult> Index()
        {
            YogaAppUser yogaAppUser = _userManager.GetUserAsync(User).Result;

            Instructor? thisInstructor = GetInstructorByYogaUserId(yogaAppUser.YogaUserID);
            
            if(null == thisInstructor )
            {
                //redirect to create page
                return RedirectToAction("Create");
            }else
            {
              
               
                return RedirectToAction("Details", new { id = thisInstructor.InstructorId });
              //redirect to details page with the correct id

            }

        }

        // GET: Instructors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Instructors == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .Include(x => x.LocationInstructorLinks).ThenInclude(lo => lo.Location)
                .Include(x => x.LocationInstructorLinks).ThenInclude(lo => lo.ApprovalStatus)
                .Include(x => x.Reviews)
                .Include(x => x.Transactions).ThenInclude(ta => ta.Tstatus)
                .Include(x => x.Transactions).ThenInclude(ta => ta.Type)
                .FirstOrDefaultAsync(m => m.InstructorId == id);
            
            
            
            if (instructor == null)
            {
                return NotFound();
            }

            //load courses related to this user
            var thisCourses = _context.Courses
                .Include(b => b.Status)
                .Include(b => b.Location)
                .Include(b => b.ClassParticipantLinks)
                .Where(m => m.InstructorId == id );

            var los = _context.Locations.AsQueryable();
            var locations = los.ToList();

            //remove locations that already have a link or have 
            foreach(var l in instructor.LocationInstructorLinks)
            {
                if(l.ApprovalStatusId == 1 || l.ApprovalStatusId == 2)
                    locations.Remove(l.Location);
            }

            ViewModel viewModel = new ViewModel();
            viewModel.thisInstructor = instructor;
            viewModel.thisCourses = thisCourses;
            viewModel.Locations = locations; 

            return View(viewModel);
        }

        // GET: Instructors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Instructors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InstructorId,Name,Heading,Body,Photo,ContactInfo,LastUpdatedDate")] Instructor instructor)
        {
            YogaAppUser yogaAppUser = _userManager.GetUserAsync(User).Result;

            if (ModelState.IsValid)
            {

                instructor.YogaUserId = yogaAppUser.YogaUserID;
                instructor.LastUpdatedDate = DateTime.Now;

                _context.Add(instructor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(instructor);
        }

        // GET: Instructors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Instructors == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors.FindAsync(id);
            if (instructor == null)
            {
                return NotFound();
            }
            return View(instructor);
        }

        // POST: Instructors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InstructorId,Name,Heading,Body,Photo,ContactInfo,LastUpdatedDate")] Instructor instructor)
        {
            YogaAppUser yogaAppUser = _userManager.GetUserAsync(User).Result;

            if (id != instructor.InstructorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    instructor.YogaUserId = yogaAppUser.YogaUserID;
                    instructor.LastUpdatedDate = DateTime.Now;
                    
                    _context.Update(instructor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstructorExists(instructor.InstructorId))
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
            return View(instructor);
        }

        [HttpGet("{locationid:int}/{instructorid:int}")]
        public async Task<IActionResult> RemoveFromLocation(int? locationid, int? instructorid)
        {
            if (locationid.HasValue && instructorid.HasValue)
            {
                LocationInstructorLink? lil = _context.LocationInstructorLinks.Where(x => x.InstructorId == instructorid.Value && x.LocationId == locationid.Value).FirstOrDefault();
                if (lil != null)
                {
                    lil.RemovalDate = DateTime.Now;
                    lil.ApprovalStatusId = 3;
                    _context.Update(lil);
                    await _context.SaveChangesAsync();
                }
            
            }


            return RedirectToAction("Index");
        }




        public async Task<IActionResult> InstructorJoinLocation(int? id)
        {
            YogaAppUser yogaAppUser = _userManager.GetUserAsync(User).Result;

            Instructor? thisInstructor = GetInstructorByYogaUserId(yogaAppUser.YogaUserID);



            if (id.HasValue && thisInstructor!=null)
            {
                LocationInstructorLink lil = new LocationInstructorLink();
                lil.LocationId = id;
                lil.InstructorId = thisInstructor.InstructorId;
                lil.ApprovalStatusId = 1;

                _context.Add(lil);
                await _context.SaveChangesAsync();

            }


            return RedirectToAction("Index");
        }


        public async Task<IActionResult> WithdrawFromCourse(int? id)
        {
            if (id.HasValue)
            {
                Course removable = _context.Courses.Where(x => x.CourseId == (int)id).FirstOrDefault();

                removable.InstructorId = null;
                _context.Update(removable);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        private bool InstructorExists(int id)
        {
          return (_context.Instructors?.Any(e => e.InstructorId == id)).GetValueOrDefault();
        }
        private Instructor? GetInstructorByYogaUserId(int yogauserid)
        {
            Instructor? findUser = _context.Instructors?.FirstOrDefault(e => e.YogaUserId == yogauserid);

            return findUser;
        }


        public class ViewModel
        {
            public Instructor thisInstructor { get; set; }
            public IEnumerable<Course> thisCourses { get; set; }

            public IEnumerable<Location> Locations { get; set; }


        }


    }
}
