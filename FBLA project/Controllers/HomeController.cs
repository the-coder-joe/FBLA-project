using FBLA_project.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using System.Diagnostics;
using System.Security.Permissions;
using System.Text.Json;

namespace FBLA_project
{
    public class HomeController : Controller
    {
        private const string jobDirectory = @".\JobFolder";
        protected bool _authenticated = false;

        public HomeController()
        {
        }
        public IActionResult Index()
        {
            User? user = UserService.GetUserFromHttpContext(HttpContext);

            if (user is null) {
                return View();
            }

            return View(new BaseModel { User = user});
        }

        public IActionResult Products()
        {
            return View();
        }

        #region AdminLogin

        //distributes actual login page
        public ActionResult Login()
        {
            return View();
        }

        public IActionResult Account()
        { 
            

            }

        //handles form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User? user = null;
                try
                {
                    user = UserService.AuthenticateUser(model.Username, model.Password);
                }
                catch (Exception ex)
                {
                    if (ex.Message == "User does not exist")
                    {
                        model.Message = "This user does not exist, please create an account.";
                    }
                }
                if (user is null)
                {
                    model.Message = "Your password is incorrect";
                    return View(model);
                }

                if (user.IsAdmin)
                {
                    return RedirectToAction("Index", "Home");
                }

                var token = UserService.GenerateSessionToken(user);

                //add the session token validation

                HttpContext.Session.SetString("SessionToken", token);

                return RedirectToAction("AdminView", "Home");
            }
            return View();
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

        [HttpPost]
        public IActionResult CreateAccount(AccountCreationModel model)
        {
            if (ModelState.IsValid) {
                UserBase userConstruct = new UserBase 
                { 
                    Name = model.Name,
                    Username = model.Username,
                    Password = model.Password
                };

                UserService.CreateNewUser(userConstruct);
                model.Message = "Account has successfully been created";

                return View(model);
            }
            

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