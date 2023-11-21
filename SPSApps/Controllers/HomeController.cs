using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using SPSApps.Models;
using SPSApps.Models.Register;
using System.Diagnostics;

namespace SPSApps.Controllers
{
    public class HomeController : Controller
    {
        private readonly DatabaseEntity _context;
        private ISession _session;
        public HomeController(DatabaseEntity context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _session = httpContextAccessor.HttpContext.Session;
        }

        public IActionResult Index()
        {
            var email =  _session.GetString("email");
            var name = _session.GetString("name");
            var allLocation = _context.Buildings.ToList();
            HomeDTO home = new HomeDTO( name, email, allLocation);
            if (email != null)
            {
                return View(home);
            }
            else
            {
               return View(new HomeDTO("name", "email", allLocation));//TODO Remove
                return RedirectToAction("Login", "Users", new { login = true });
            }
        }


        public IActionResult Confirm(int emergency, int request)
        {
            var email = _session.GetString("email");
            var name = _session.GetString("name");
            var allLocation = _context.Buildings.FirstOrDefault(f=>f.Id == emergency || f.Id == request);
            ConfirmDTO home = new ConfirmDTO(name, email, allLocation, emergency > 1);
            if (email != null)
            {
                return View(home);
            }
            else
            {
                return View(new ConfirmDTO("name", "email", allLocation, false));//TODO Remove
                return RedirectToAction("Login", "Users", new { login = true });
            }
        }

        [HttpPost]
        public IActionResult Confirm(int Id)
        {
            var email = _session.GetString("email");
            var name = _session.GetString("name");
            var allLocation = _context.Buildings.FirstOrDefault(f => f.Id == Id);
            ConfirmDTO home = new ConfirmDTO(name, email, allLocation, false);
            if (email != null)
            {
                return View(home);
            }
            else
            {
                return View(new ConfirmDTO("name", "email", allLocation, false));//TODO Remove
                return RedirectToAction("Login", "Users", new { login = true });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            var user = await _context.Users.FirstOrDefaultAsync(f => (f.Email == login.UserName || f.PhoneNumber == login.UserName) && f.Password == login.Password);
            ViewBag.notFound = user == null;
            return View();
        }
    }
}