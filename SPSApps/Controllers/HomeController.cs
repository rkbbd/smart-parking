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
            if (email != null)
            {
                return View();
            }
            else
            {
                //return View();//TODO Remove
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