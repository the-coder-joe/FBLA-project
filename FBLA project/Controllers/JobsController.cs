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
                StreamReader jsonStream = new StreamReader(@".\JobFolder\AvailableJobs.json");
                string jsonString = jsonStream.ReadToEnd();
                this.openingsData = JsonSerializer.Deserialize<List<Job>>(jsonString) ?? new List<Job>();
            }
            catch
            {
                this.openingsData = new List<Job>();
            }
        }

        private void processApplication(Application completedApp, IFormFile ResumeFile, Job job)
        {
            completedApp.Job = job;

            //generate lists of existing applications
            StreamReader jsonStream = new StreamReader(@".\JobFolder\Applications.json");
            string jsonString = jsonStream.ReadToEnd();
            List<Application> applications = JsonSerializer.Deserialize<List<Application>>(jsonString) ?? new List<Application>();

            //create list of existing application ids
            List<string> appIds = new List<string>();
            foreach (Application appl in applications)
            {
                appIds.Add(appl.ApplicationId);
            }

            //generate unique id
            string newApplicationId = getUniqueId(appIds, completedApp.Name.Trim().ToLower().Remove(' ')[..5]);


            //save the resume file
            using (MemoryStream memoryStream = new MemoryStream())
            {
                IFormFile resumeFile = ResumeFile;
                resumeFile.CopyToAsync(memoryStream);

                string fileName = resumeFile.FileName.Split(".")[0];
                string fileExtension = resumeFile.FileName.Split(".")[1];

                string uniqueFileName = getUniqueId(new List<string>(Directory.GetFiles(@".\JobFolder\Resumes")), resumeFile.FileName) + "." + fileExtension;
                string filePath = Path.Combine(@".\JobFolder\Resumes", uniqueFileName);

                // Upload the file if less than 2 MB
                if (memoryStream.Length < 2097152)
                {
                    try
                    {
                        FileStream stream = new FileStream(filePath, FileMode.Create);
                        resumeFile.CopyTo(stream);
                        stream.Close();
                        completedApp.ResumeFileName = uniqueFileName;
                    }
                    catch
                    {
                        throw new Exception("File not able to be written");
                    }
                }
                else
                {
                    ModelState.AddModelError("File", "The file is too large.");
                }
            }

        }
        private string getUniqueId(List<string> existingIds, string inclusionVal)
        {
            int suffix = 0;
            string uniqueId = inclusionVal + suffix;

            while (existingIds.Contains(uniqueId + ( ++suffix ).ToString())) { }
            {
                return uniqueId + suffix.ToString();
            }
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ApplicationAsync(string? id, ApplicationModel? returnModel)
        {
            if (id == null) { return RedirectToAction("Openings"); }

            Job job = this.openingsData.Find(x => x.Id == id) ?? throw new Exception("Job not found");

            ApplicationModel model;

            if (Request.Method == "POST")
            {
                if (returnModel is null)
                    throw new Exception("No return Model");


                if (returnModel.ResumeFile is null)
                    throw new Exception("No file uploaded");


                processApplication(returnModel.Application, returnModel.ResumeFile, job);



                //construct the new model for returning
                model = new ApplicationModel()
                {
                    Job = job,
                    Message = "Thanks for completing the application",
                    Completed = true,
                };

                return View(model);
            }
            else
            {
                //get request - normal
                model = new ApplicationModel()
                {
                    Job = job,
                    Completed = false
                };
            }

            return View(model);
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
            model.Openings = this.openingsData;


            return View(model);
        }
    }
}
