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
    public class ParticipantsController : Controller
    {
        private readonly YogaSystemContext _context;
        private readonly UserManager<YogaAppUser> _userManager;
        public ParticipantsController(YogaSystemContext context, UserManager<YogaAppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Participants
        public async Task<IActionResult> Index()
        {

            YogaAppUser yogaAppUser = _userManager.GetUserAsync(User).Result;

            Participant? thisParticipant = GetParticipantByYogaUserId(yogaAppUser.YogaUserID);

            if (null == thisParticipant)
            {
                //redirect to create page
                return RedirectToAction("Create");
            }
            else
            {
                //redirect to details page with the correct id
                return RedirectToAction("Details", new { id = thisParticipant.ParticipantId });

            }

        }

        // GET: Participants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Participants == null)
            {
                return NotFound();
            }
            //deepload current courses.
            var participant = await _context.Participants
                .Include(x => x.ClassParticipantLinks).ThenInclude(y=> y.Class).ThenInclude(z => z.Status)
                .Include(x => x.Transactions).ThenInclude(y=> y.Type)
                .Include(x => x.Transactions).ThenInclude(y => y.Tstatus)
                .FirstOrDefaultAsync(m => m.ParticipantId == id);
            if (participant == null)
            {
                return NotFound();
            }

            

            //load courses available to this user
            var cq = _context.Courses
                .Include(b => b.Status)
                .Include(b => b.Location)
                .Include(b => b.ClassParticipantLinks)
                .Where(m => m.StatusId == 2 || m.StatusId == 3);

            var currentCourses = cq.ToList();
            //remove already registered classes
            foreach(ClassParticipantLink cl in participant.ClassParticipantLinks)
            {
                currentCourses.Remove(cl.Class);
            }

          
            

            ViewModel vm = new ViewModel();
            vm.thisParticipant = participant;
            vm.availableCourses = currentCourses;

            return View(vm);
        }

        // GET: Participants/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Participants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ParticipantId,Name,AboutMe,LastUpdatedOn,YogaUserId")] Participant participant)
        {
            YogaAppUser yogaAppUser = _userManager.GetUserAsync(User).Result;

            if (ModelState.IsValid)
            {

                participant.YogaUserId = yogaAppUser.YogaUserID;
                participant.LastUpdatedOn = DateTime.Now;

                _context.Add(participant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(participant);
        }

        // GET: Participants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Participants == null)
            {
                return NotFound();
            }

            var participant = await _context.Participants.FindAsync(id);


            if (participant == null)
            {
                return NotFound();
            }
            return View(participant);
        }

        // POST: Participants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ParticipantId,Name,AboutMe,LastUpdatedOn,YogaUserId")] Participant participant)
        {
            if (id != participant.ParticipantId)
            {
                return NotFound();
            }

            YogaAppUser yogaAppUser = _userManager.GetUserAsync(User).Result;

            if (ModelState.IsValid)
            {

                try
                {
                    participant.YogaUserId = yogaAppUser.YogaUserID;
                    participant.LastUpdatedOn = DateTime.Now;
                    _context.Update(participant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParticipantExists(participant.ParticipantId))
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
            return View(participant);
        }

        // GET: Participants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Participants == null)
            {
                return NotFound();
            }

            var participant = await _context.Participants
                .FirstOrDefaultAsync(m => m.ParticipantId == id);
            if (participant == null)
            {
                return NotFound();
            }

            return View(participant);
        }

        // POST: Participants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Participants == null)
            {
                return Problem("Entity set 'YogaSystemContext.Participants'  is null.");
            }
            var participant = await _context.Participants.FindAsync(id);
            if (participant != null)
            {
                _context.Participants.Remove(participant);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParticipantExists(int id)
        {
          return (_context.Participants?.Any(e => e.ParticipantId == id)).GetValueOrDefault();
        }
        private Participant? GetParticipantByYogaUserId(int yogauserid)
        {
            Participant? findUser = _context.Participants?.FirstOrDefault(e => e.YogaUserId == yogauserid);

            return findUser;
        }



        public async Task<IActionResult> WithdrawFromCourse(int? id)
        {
            if (id.HasValue)
            {
                ClassParticipantLink removable = _context.ClassParticipantLinks.Where(x => x.ClassId == id).FirstOrDefault();
                _context.Remove(removable);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RegisterInCourse(int? id)
        {
            if (id.HasValue)
            {

                YogaAppUser yogaAppUser = _userManager.GetUserAsync(User).Result;

                Participant? thisParticipant = GetParticipantByYogaUserId(yogaAppUser.YogaUserID);

                Course newCourse = _context.Courses.Where(x => x.CourseId == id).FirstOrDefault();
                if(newCourse != null)
                {
                    ClassParticipantLink cpl = new ClassParticipantLink();
                    cpl.ClassId = newCourse.CourseId;
                    cpl.ParticipantId = thisParticipant.ParticipantId;
                    cpl.EnrollmentStatusId = 1; //enrolled
                    cpl.CreatedDate = DateTime.Now;
                    cpl.LastUpdatedBy = yogaAppUser.YogaUserID;
                    cpl.LastUpdatedDate = DateTime.Now;

                    _context.Add(cpl);

                    Transaction t = new Transaction();
                    t.TypeId = 1;
                    t.TstatusId = 1;
                    t.LocationId = newCourse.LocationId;
                    t.Amount = newCourse.SessionCount * newCourse.PricePerSession;
                    t.ParticipantId = thisParticipant.ParticipantId;
                    t.TriggerdBy = yogaAppUser.YogaUserID;
                    t.TriggerDate = DateTime.Now;
                    t.UserId = yogaAppUser.YogaUserID;
                    
                    _context.Add(t);


                }
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        public class ViewModel
        {
            public Participant? thisParticipant { get; set; }
            public IEnumerable<Course>? availableCourses { get; set; }




        }

    }




}
