using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using SPSApps.Models;
using SPSApps.Models.Parking;
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

        public async Task<IActionResult> RequestParking()
        {
            var email = _session.GetString("email");
            var name = _session.GetString("name");
            //var allLocation = _context.Buildings.ToList();
            //HomeDTO home = new HomeDTO(name, email, allLocation);
            //if (email != null)
            //{
            //    return View(home);
            //}
            //else
            //{
            //    return View(new HomeDTO("name", "email", allLocation));//TODO Remove
            //    return RedirectToAction("Login", "Users", new { login = true });
            //}
            var databaseEntity = _context.RequestParkings.Include(r => r.Building).Where(f=>f.Building.email == email && f.IsActive == 0);
            List<RequestParking> data = await databaseEntity.ToListAsync();
            RequestParkingDTO parking = new RequestParkingDTO( data,  email,  name );
            return View(parking);
        }


        public IActionResult Confirm(int emergency, int request)
        {
            var email = _session.GetString("email");
            var name = _session.GetString("name");
            //string url = HttpContext.Request.QueryString.Value;
            //String data = url.Split('?')[1].Split('=')[1];
            var allLocation = _context.Buildings.FirstOrDefault(f=>(f.Id == emergency || f.Id == request));
            ConfirmDTO home = new ConfirmDTO(name, email, allLocation, emergency >= 1 ? 1 : 0 );
            if (email != null)
            {
                return View(home);
            }
            else
            {
                return View(home);//TODO Remove
                return RedirectToAction("Login", "Users", new { login = true });
            }
        }

        [HttpPost]
        public IActionResult Confirm([Bind("Id, IsEmergency")] ParkingRequest parkingRequest )
        {
           
            var email = _session.GetString("email");
            var name = _session.GetString("name");
            var allLocation = _context.Buildings.FirstOrDefault(f => f.Id == parkingRequest.Id);

            _context.RequestParkings.Add(new RequestParking()
            {
                AccessTime = DateTime.Now,
                IsActive = parkingRequest.IsEmergency,
                BuildingId = allLocation.Id,
                Fair = allLocation.FairPerParking,
                RequestUserEmail = email,
                Status = 1
            });;

            _context.SaveChanges();

            return RedirectToAction("Index", "Home", new { login = true });
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