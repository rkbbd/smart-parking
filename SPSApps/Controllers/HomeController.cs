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
            var allLocation = _context.Buildings.Where(f=>f.Status == 1).ToList();
            var parkings = _context.RequestParkings.Where(f=>f.RequestUserEmail == email && f.IsPaid == false).Take(3).ToList();
            HomeDTO home = new HomeDTO( name, email, allLocation, parkings);
            if (email != null)
            {
                return View(home);
            }
            else
            {
                return RedirectToAction("Login", "Users", new { login = true });
            }
        }

        public async Task<IActionResult> RequestParking()
        {
            var email = _session.GetString("email");
            var name = _session.GetString("name");

            var databaseEntity = _context.RequestParkings.Include(r => r.Building).Where(f=>f.Building.email == email && f.IsActive == 0 && f.Status == 1).Take(5);
            List<RequestParking> reqHistory = _context.RequestParkings.Include(r => r.Building).Where(f => f.Building.email == email && f.IsActive == 1 && f.Status == 1).ToList();
            List<RequestParking> data = await databaseEntity.ToListAsync();
            RequestParkingDTO parking = new RequestParkingDTO( data, reqHistory, name, email);
            return View(parking);
        }

        public async Task<IActionResult> EditRequestParking(int? id)
        {
            if (id == null || _context.Buildings == null)
            {
                return NotFound();
            }

            var building = await _context.RequestParkings.FindAsync(id);
            building.IsActive = 1;
            _context.Update(building);
            _context.SaveChanges();

            return RedirectToAction("RequestParking");
        }

        public async Task<IActionResult> CompleteParking(int? id)
        {
            if (id == null || _context.Buildings == null)
            {
                return NotFound();
            }

            var building = await _context.RequestParkings.FindAsync(id);
            building.IsPaid = true;
            _context.Update(building);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        
        public async Task<IActionResult> CancelRequestParking(int? id)
        {
            if (id == null || _context.Buildings == null)
            {
                return NotFound();
            }

            var building = await _context.RequestParkings.FindAsync(id);
            building.Status = 2;
            _context.Update(building);
            _context.SaveChanges();
            if (building == null)
            {
                return NotFound();
            }
            return RedirectToAction("RequestParking");
        }


        public IActionResult Confirm(int emergency, int request)
        {
            var email = _session.GetString("email");
            var name = _session.GetString("name");
            var allLocation = _context.Buildings.FirstOrDefault(f=>(f.Id == emergency || f.Id == request) && f.Status == 1);
            ConfirmDTO home = new ConfirmDTO(name, email, allLocation, emergency >= 1 ? 1 : 0 );
            if (allLocation != null && email != null)
            {
                return View(home);
            }
            else
            {
                return RedirectToAction("Index", "Home", new { login = true });
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
            });

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