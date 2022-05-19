using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using YogaApp.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using YogaApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace YogaApp.Controllers
{
    public class HomeController : Controller
    {
     
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<YogaAppUser> _userManager;
        private readonly IConfiguration _config;

        public List<SelectListItem> UserTypes { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "Administrator" },
            new SelectListItem { Value = "2", Text = "Instructor" },
            new SelectListItem { Value = "3", Text = "Participant"  },
            new SelectListItem { Value = "4", Text = "Super"  },
        };

        public HomeController(UserManager<YogaAppUser> manager, ILogger<HomeController> logger, IConfiguration conig)
        {
            _logger = logger;
            _userManager = manager; 
            _config= conig;
        }


        static async Task Exec(YogaAppUser dearUser, string apiKey)
        {



            //var apiKey = Environment.GetEnvironmentVariable("NAME_OF_THE_ENVIRONMENT_VARIABLE_FOR_YOUR_SENDGRID_KEY");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("ANAS.SHAHEEN@stu.mmu.ac.uk", "YogaApp Emailer");
            var subject = "Hello from yoga Index";
            var to = new EmailAddress(dearUser.Email, dearUser.NormalizedEmail);
            
            var plainTextContent = "Dear user and easy to do anywhere, even with C#";
            var htmlContent = "Dear  < strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }

        [Authorize]
        public async Task<IActionResult> SendEmailAsync()
        {



            YogaAppUser user = await _userManager.GetUserAsync(User);
            var email = user.Email;

            Exec(user, _config.GetRequiredSection("SendGridKey").Value).Wait();

            RedirectToAction("Index","Home");

            return View();
        }

        [Authorize]
        public IActionResult Index()
        {
            YogaAppUser yogaAppUser= Task.Run (()=> _userManager.GetUserAsync(User)).Result; ;

            //redirect to different controlelrs based on user type
            switch (yogaAppUser.UserType)
            {
                case 1:
                    return RedirectToAction("Index", "Locations");
                case 2:
                    return RedirectToAction("Index", "Instructors");
                case 3:
                    return RedirectToAction("Index", "Participants");
                case 4:
                    return RedirectToAction("Index", "SuperAdmins");
                default:
                    break;
            }


            return RedirectToPage("Privacy");
        }
        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}