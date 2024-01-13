using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using SPSApps.Models;
using SPSApps.Models.Parking;
using SPSApps.Models.Register;
using System.Diagnostics;
using System.Xml.Schema;

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
            var email = _session.GetString("email");

            var allLocation = _context.Buildings.Where(f => f.Status == 1).ToList();//For map marking
            var parkings = _context.RequestParkings.Where(f => f.RequestUserEmail == email && f.IsPaid == false).Take(3).ToList(); //show max 3 

            if (email == null)
            {
                return RedirectToAction("Login", "Users", new { login = true });
            }
            return View(new HomeDTO(email, allLocation, parkings));
        }


        // GET: Buildings/Create
        public IActionResult AddBuilding()
        {
            var email = _session.GetString("email");
            if (email == null)
            {
                return RedirectToAction("Login", "Users", new { login = true });
            }
            var building = _context.Buildings.FirstOrDefault(f => f.email == email && f.Status == 1);
            if (building != null)
            {
                return RedirectToAction("OwnerDashboard", "Home", new { login = true });
            }
            return View(new BuildingDTO("", "", 0, 0, 0, 0, 0));
        }

        // POST: Buildings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBuilding([Bind("Latitude,Longitude,TotalAvailableParking,FairPerParking, Info, EmergencyFairPerParking")] Building building)
        {
            building.email = _session.GetString("email");
            building.Status = 1;
            _context.Add(building);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), "Home");
        }


        public async Task<IActionResult> OwnerDashboard()
        {
            var email = _session.GetString("email");

            var databaseEntity = _context.RequestParkings.Include(r => r.Building).Where(f => f.Building.email == email && f.IsActive == 0 && f.Status == 1).Take(5);

            List<RequestParking> reqHistory = _context.RequestParkings.Include(r => r.Building).Where(f => f.Building.email == email && f.IsActive == 1 && f.Status == 1).ToList();

            List<RequestParking> data = await databaseEntity.ToListAsync();

            return View(new RequestParkingDTO(data, reqHistory));
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

            return RedirectToAction("OwnerDashboard");
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
            return RedirectToAction("OwnerDashboard");
        }


        public IActionResult Confirm(int emergency, int request)
        {
            var email = _session.GetString("email");
            var requestLocation = _context.Buildings.FirstOrDefault(f => (f.Id == emergency || f.Id == request) && f.Status == 1);

            var requestedCar = _context.RequestParkings.Where(f => f.IsPaid == false && f.IsActive == 1 && f.Status == 1 && f.RequestUserEmail == requestLocation.email).ToList();

            if (requestLocation == null || email == null)
            {
                return RedirectToAction("Index", "Home", new { login = true });
            }

            var isAvailable = requestLocation.TotalAvailableParking > requestedCar.Count();
            var totalWaitingHour = requestedCar.Sum(f => f.Hour);
            var minWaitingTime = requestedCar.Any() ? requestedCar.Select(f => { f.AccessTime = f.AccessTime.AddHours(totalWaitingHour); return f; }).Min(j => j.AccessTime) : DateTime.Now;
            var TotalMinutes = (minWaitingTime - DateTime.Now).TotalMinutes;

            return View(new ConfirmDTO(email, requestLocation, emergency >= 1 ? 1 : 0, isAvailable ? 1 : 0, TotalMinutes));
        }

        [HttpPost]
        public IActionResult Confirm([Bind("Id, IsEmergency, Hour, WaitingTime, IsAvailable")] ParkingRequest parkingRequest)
        {
            var email = _session.GetString("email");
            var allLocation = _context.Buildings.FirstOrDefault(f => f.Id == parkingRequest.Id);

            _context.RequestParkings.Add(new RequestParking()
            {
                AccessTime = parkingRequest.IsAvailable == 1 ? DateTime.Now.AddHours(parkingRequest.WaitingTime) : DateTime.Now,
                IsActive = parkingRequest.IsEmergency,
                BuildingId = allLocation.Id,
                Fair = allLocation.FairPerParking,
                Hour = parkingRequest.Hour > 0 ? parkingRequest.Hour : 1,
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