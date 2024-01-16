using Microsoft.AspNetCore.Mvc;
using System.Text.Json;


namespace FBLA_project.Controllers
{
    public class JobsController : Controller
    {

        private readonly List<Job> openingsData;
        public JobsController()
        {
            try
            {
                StreamReader jsonStream = new StreamReader("C:\\Users\\jkozi\\source\\repos\\FBLA project\\FBLA project\\AvailableJobs.json");
                String jsonString = jsonStream.ReadToEnd();
                openingsData = JsonSerializer.Deserialize<List<Job>>(jsonString) ?? new List<Job>();
            }
            catch
            {
                openingsData = new List<Job>();
            }
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ApplicationAsync(string? id, ApplicationModel? returnModel)
        {
            if (id == null)
                return RedirectToAction("Openings");

            Job? job = openingsData.Find(x => x.Id == id);
            if (job == null)
                throw new Exception("Job not found");


            if (Request.Method == "POST")
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    if (returnModel is null)
                        throw new Exception("No return Model");

                    if (returnModel.ResumeFile is null)
                        throw new Exception("No file uploaded");

                    IFormFile resumeFile = returnModel.ResumeFile;
                    resumeFile.CopyToAsync(memoryStream);



                    // Upload the file if less than 2 MB
                    if (memoryStream.Length < 2097152)
                    {
                        string uniqueFileName = resumeFile.FileName;
                        string filePath = Path.Combine(@".\Resumes", uniqueFileName);
                        FileStream stream = new FileStream(filePath, FileMode.Create);
                        resumeFile.CopyTo(stream);
                        stream.Close();
                    }
                    else
                    {
                        ModelState.AddModelError("File", "The file is too large.");
                    }
                }

                ApplicationModel model = new ApplicationModel()
                {
                    Job = job,
                    message = "Thanks"
                };
                return View(model);
            }
            else
            {
                //get request - normal
                ApplicationModel model = new ApplicationModel() { Job = job };

                return View(model);
            }
        }

        public IActionResult Information()
        {
            return View();
        }
        public IActionResult Openings()
        {
            OpeningsModel model = new OpeningsModel
            {
                Openings = new List<Job>()
            };
            model.Openings = openingsData;


            return View(model);
        }
    }
}
