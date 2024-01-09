using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FBLA_project.Controllers
{
    public class JobsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Application()
        {
            if (Request.Method == "POST")
            {
                object applicationForm = Request.Form;

                return View();
            }
            else
            {

                return View();
            }
        }

        public IActionResult Information()
        {
            return View();
        }
        public IActionResult Openings()
        {
            var model = new OpeningsModel
            {
                Openings = new List<Opening>()
            };



            var jsonStream = new StreamReader("C:\\Users\\jkozi\\source\\repos\\FBLA project\\FBLA project\\AvailableJobs.json");
            String jsonString = jsonStream.ReadToEnd();
            var openingData = JsonSerializer.Deserialize<List<Opening>>(jsonString);


            model.Openings = openingData;




            return View();
        }
    }
}
