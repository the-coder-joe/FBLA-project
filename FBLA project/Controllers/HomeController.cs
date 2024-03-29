using FBLA_project.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using System.Diagnostics;
using System.Text.Json;

namespace FBLA_project
{
    public class HomeController : Controller
    {
        private const string jobDirectory = @".\JobFolder";
        protected bool _authenticated = false;
        private UserService _userService = new UserService(@".\UserFolder\Users.json");
        private IDataProtectionProvider _dataProtectionProvider;

        public HomeController(IDataProtectionProvider dataProtectionProvider)
        {
            _dataProtectionProvider = dataProtectionProvider;
        }
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
            User? user = null;
            try
            {
                user = _userService.AuthenticateUser(model.Username, model.Password);
            } catch (Exception ex)
            {
                if (ex.Message == "User does not exist") {
                    model.Message = "This user does not exist, please create an account.";
                }
            }
            if (user is null) {
                model.Message = "Your password is incorrect";
                return View(model);
            }

            if (user.IsAdmin)
            {
                return RedirectToAction("Index", "Home");
            }

            var token = _userService.GenerateSessionToken(user);

            //add the session token validation

            HttpContext.Session.SetString("SessionToken", token);

            return RedirectToAction("AdminView", "Home");
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

        public IActionResult MyGarage()
        {
            return View();
        }

        public IActionResult CreateAccount()
        {
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

        public IActionResult Sources()
        {
            return View();
        }
    }
}