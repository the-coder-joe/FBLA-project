using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FBLA_project.Controllers
{
    public class JobsController : Controller
    {
        private readonly List<Job> openingsData;
        private const string jobDirectory = @".\JobFolder";

        public JobsController()
        {
            try
            {
                StreamReader jsonStream = new(@".\JobFolder\AvailableJobs.json");
                string jsonString = jsonStream.ReadToEnd();
                this.openingsData = JsonSerializer.Deserialize<List<Job>>(jsonString) ?? [];
            }
            catch
            {
                this.openingsData = [];
            }
        }

        #region privateMethods

        //handles all of the processing after the application has been submitted
        private void ProcessApplication(ApplicationFields? completedApp, IFormFile? ResumeFile, Job job)
        {
            if (ResumeFile is null) { throw new MessageException("No file uploaded"); }
            if (completedApp is null) { throw new MessageException("Please retry"); }

            if (completedApp.Name is null
            || completedApp.Strengths is null
            || completedApp.WhyThisJob is null
            || completedApp.PhoneNumber is null)
            {
                throw new MessageException("A field wasn't filled out");
            }

            List<ProcessedApplication> applications;
            List<string> appIds = [];
            ProcessedApplication newApp;

            //generate lists of existing applications
            try
            {
                using (StreamReader jsonStream = new(Path.Combine(jobDirectory, "Applications.json")))
                {
                    string jsonString = jsonStream.ReadToEnd();
                    applications = JsonSerializer.Deserialize<List<ProcessedApplication>>(jsonString) ?? [];
                }

                //create list of existing application ids
                foreach (ProcessedApplication appl in applications)
                {
                    appIds.Add(appl.ApplicationId);
                }

                //generate unique id

                string newApplicationId = GetUniqueId(appIds, completedApp.Name);

                //create a new application that will be recorded
                newApp = new ProcessedApplication()
                {
                    Fields = completedApp,
                    ApplicationId = newApplicationId,
                    Job = job
                };
            }
            catch
            {
                throw new MessageException("Oh no, it looks like there was a serverside error. ");
            }

            //save the resume file
            try
            {
                using (MemoryStream memoryStream = new())
                {
                    IFormFile resumeFile = ResumeFile;

                    resumeFile.CopyToAsync(memoryStream);

                    string fileName = resumeFile.FileName.Split(".")[0];
                    string fileExtension = resumeFile.FileName.Split(".")[1];
                    string[] exFiles = Directory.GetFiles(@".\JobFolder\Resumes\");
                    List<string> existingFiles = [];
                    foreach (string file in exFiles)
                    {
                        existingFiles.Add(file.Split('\\').Last().Split('.').First());
                    }

                    //generate a unique file name to avoid issues with doubling up on the name
                    string uniqueFileName = GetUniqueId(existingFiles, fileName) + "." + fileExtension;
                    string filePath = Path.Combine(@".\JobFolder\Resumes", uniqueFileName);

                    // Upload the file if less than 2 MB
                    if (memoryStream.Length < 2097152)
                    {
                        try
                        {
                            FileStream stream = new(filePath, FileMode.Create);
                            resumeFile.CopyTo(stream);
                            stream.Close();
                            newApp.ResumeFileName = uniqueFileName;
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
            catch
            {
                throw new MessageException("Oh no, there was an issue saving your resume file serverside. ");
            }

            applications.Add(newApp);

            //write out the new application to the json file
            try
            {
                using StreamWriter File1 = new(Path.Combine(jobDirectory, "Applications.json"));
                string newJson = JsonSerializer.Serialize<List<ProcessedApplication>>(applications, new JsonSerializerOptions() { WriteIndented = true });
                File1.Write(newJson);
            }
            catch
            {
                throw new MessageException("Oh no, there was a serverside error. ");
            }
        }

        //generates a unique id ffor each application
        private static string GetUniqueId(List<string> existingIds, string inclusionVal)
        {
            int suffix = 0;
            string uniqueId = inclusionVal + suffix;

            while (existingIds.Contains(uniqueId + ( ++suffix ).ToString())) { }
            {
                return uniqueId + suffix.ToString();
            }
        }

        #endregion privateMethods

        public IActionResult Index()
        {
            return Redirect("/");
        }

        public IActionResult Application(string? id, ApplicationModel? returnModel)
        {
            if (id == null) { return RedirectToAction("Openings"); }

            Job job = this.openingsData.Find(x => x.Id == id) ?? throw new Exception("Job not found");

            ApplicationModel model;
            string message = "";
            bool complete = false;
            if (Request.Method == "POST")
            {
                //post request - form submitted
                if (returnModel is null)
                    throw new Exception("No return Model");

                try
                {
                    ProcessApplication(returnModel.Application, returnModel.ResumeFile, job);
                    complete = true;
                }
                catch (Exception e)
                {
                    message = e.Message;
                    complete = false;
                }

                //construct the new model for returning
                model = new ApplicationModel()
                {
                    Job = job,
                    Message = message,
                    Completed = complete,
                };
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

        #region staticWebsites

        public IActionResult Information()
        {
            User? user = UserService.GetUserFromHttpContext(HttpContext);
            if (user is null)
            { return View(); }

            return View(new BaseModel { UnprotectedData = user.UnprotectedInfo });
        }

        public IActionResult Openings()
        {
            User? user = UserService.GetUserFromHttpContext(HttpContext);

            OpeningsModel model = new()
            {
                Openings = this.openingsData ?? [],
                UnprotectedData = user?.UnprotectedInfo
            };

            return View(model);
        }

        #endregion staticWebsites
    }
}