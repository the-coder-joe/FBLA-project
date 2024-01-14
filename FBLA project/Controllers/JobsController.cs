using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FBLA_project.Controllers
{
    public class JobsController : Controller
    {

        private List<Job> openingsData;
        public JobsController()
        {
            try
            {
                var jsonStream = new StreamReader("C:\\Users\\jkozi\\source\\repos\\FBLA project\\FBLA project\\AvailableJobs.json");
                String jsonString = jsonStream.ReadToEnd();
                openingsData = JsonSerializer.Deserialize<List<Job>>(jsonString) ?? new List<Job>();
            }
            catch {
            openingsData = new List<Job>(); 
            }
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Application(string? id, ApplicationModel? returnModel)
        {
            if (id == null)
                return RedirectToAction("Openings");

            var job = openingsData.Find(x => x.Id == id);
            if (job == null)
                throw new Exception("Job not found");


            if (Request.Method == "POST")
            {

                Console.Write(returnModel);
                return View();

            }
            else
            {
                //get request - normal
                var model = new ApplicationModel() { Job = job };

                return View(model);
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
                Openings = new List<Job>()
            };
            model.Openings = openingsData;

            
            return View(model);
        }
    }
}
