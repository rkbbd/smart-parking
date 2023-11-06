using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPSApps.Models;
using SPSApps.Models.Register;
using System.Diagnostics;

namespace SPSApps.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        //public IActionResult Index()
        //{
        //    return View();
        //}

        private readonly DatabaseEntity _context;

        public HomeController(DatabaseEntity context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginDTO login)
        {
            var user = await _context.Users.FirstOrDefaultAsync(f => (f.Email == login.UserName || f.PhoneNumber == login.UserName) && f.Password == login.Password);
            ViewBag.notFound = user == null;
            return View();
        }

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