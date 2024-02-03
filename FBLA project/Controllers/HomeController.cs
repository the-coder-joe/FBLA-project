using FBLA_project.Models;
using Microsoft.AspNetCore.Mvc;
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
            this._authenticated = false;
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
            return View();
        }       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
                                    {
            if (ModelState.IsValid)
            {
                List<Admin> admins;
                using (StreamReader jsonStream = new(Path.Combine(jobDirectory, "AdminPasswords.json")))
                {
                    string jsonString = jsonStream.ReadToEnd();
                    admins = JsonSerializer.Deserialize<List<Admin>>(jsonString) ?? throw new Exception("Server Error");
                }

                Admin? admin = null;
                foreach (Admin ad in admins)
                {
                    if (ad.username == model.Username)
                    {
                        admin = ad;
                    }
                }

                bool adminExists = true;
                if (admin is null)
                {
                    adminExists = false;
                }
                if (adminExists && admin.password == model.Password)
                {
                    this._authenticated = true;
                    return RedirectToAction("AdminView", "Home");
                }
            }
            ModelState.AddModelError("", "Invalid login attempt.");
            return View(model);

        }

        public IActionResult AdminView()
        {
            this._authenticated = true;
            if (this._authenticated)
            {
                this._authenticated = false;
                List<ProcessedApplication> apps = new List<ProcessedApplication>();

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

        public IActionResult Sources()
        {
            return View();
        }
    }
}
