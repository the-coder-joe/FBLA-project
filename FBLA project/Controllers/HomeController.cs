using FBLA_project.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace FBLA_project
{
    public class HomeController() : Controller
    {
        private const string jobDirectory = @".\JobFolder";
        protected bool _authenticated = false;

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Products()
        {
            return View();
        }

        #region AdminLogin

        //distributes actual login page
        public ActionResult Login()
        {
            return View();
        }

        //handles form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                //generate list of known admins
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


                //check if the username and password match
                if ((admin is not null)  && admin.password == model.Password)
                { 
                    this._authenticated = true;
                    //if they match, take you to the admin view page
                    return RedirectToAction("AdminView", "Home");
                }
            }
            ModelState.AddModelError("", "Invalid login attempt.");
            return View(model);
        }

        public IActionResult AdminView()
        {
            //double check if the authentication has taken place on the prior page
            if (this._authenticated)
            {
                //make sure that the authentication is immediatly killed
                this._authenticated = false;
                List<ProcessedApplication> apps = [];

                //read the applications from file
                using (StreamReader jsonStream = new(Path.Combine(jobDirectory, "Applications.json")))
                {
                    string jsonString = jsonStream.ReadToEnd();
                    apps = JsonSerializer.Deserialize<List<ProcessedApplication>>(jsonString) ?? [];
                }

                //create the model for the admin view and return it
                AdminViewModel model = new() { Applications = apps };
                return View(model);
            }
            return RedirectToAction("Index", "Home");
        }

        #endregion AdminLogin

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