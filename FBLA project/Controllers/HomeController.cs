using FBLA_project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Diagnostics;
using System.Text.Json;

namespace FBLA_project
{ 
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private const string jobDirectory = @".\JobFolder";
        protected private bool _authenticated;

        public HomeController(ILogger<HomeController> logger)
        {
            this._logger = logger;
            _authenticated = false;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Products()
        {
            return View();
        }

        public ActionResult Login()
        {
            return RedirectToAction("AdminView", "Home");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                if (true)
                {
                    _authenticated = true;
                    return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError("", "Invalid login attempt.");
            return View(model);

        }

        public IActionResult AdminView()
        {
            _authenticated = true;
            if (_authenticated)
            {
                _authenticated = false;
                List<ProcessedApplication> apps =  new List<ProcessedApplication>();

                using (StreamReader jsonStream = new(Path.Combine(jobDirectory, "Applications.json")))
                {
                    string jsonString = jsonStream.ReadToEnd();
                    apps = JsonSerializer.Deserialize<List<ProcessedApplication>>(jsonString) ?? new List<ProcessedApplication>();

                }

                AdminViewModel model = new() { Applications = apps };
                return View(model);
            }
            return RedirectToAction("Index", "Home");
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
