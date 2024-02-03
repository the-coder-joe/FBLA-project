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
                StreamReader jsonStream = new StreamReader(@".\JobFolder\AvailableJobs.json");
                string jsonString = jsonStream.ReadToEnd();
                this.openingsData = JsonSerializer.Deserialize<List<Job>>(jsonString) ?? new List<Job>();
            }
            catch
            {
                this.openingsData = new List<Job>();
            }
        }

        #region privateMethods
        //handles all of the processing after the application has been submitted
        private void processApplication(ApplicationFields? completedApp, IFormFile? ResumeFile, Job job)
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
            List<string> appIds = new List<string>();
            ProcessedApplication newApp;

            //generate lists of existing applications
            try
            {
                using (StreamReader jsonStream = new(Path.Combine(jobDirectory, "Applications.json")))
                {
                    string jsonString = jsonStream.ReadToEnd();
                    applications = JsonSerializer.Deserialize<List<ProcessedApplication>>(jsonString) ?? new List<ProcessedApplication>();

                }

                //create list of existing application ids
                foreach (ProcessedApplication appl in applications)
                {
                    appIds.Add(appl.ApplicationId);
                }

                //generate unique id

                string newApplicationId = getUniqueId(appIds, completedApp.Name);

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
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    IFormFile resumeFile = ResumeFile;

                    resumeFile.CopyToAsync(memoryStream);

                    string fileName = resumeFile.FileName.Split(".")[0];
                    string fileExtension = resumeFile.FileName.Split(".")[1];
                    string[] exFiles = Directory.GetFiles(@".\JobFolder\Resumes\");
                    List<string> existingFiles = new List<string>();
                    foreach (string file in exFiles)
                    {
                        existingFiles.Add(file.Split('\\').Last().Split('.').First());
                    }

                    //generate a unique file name to avoid issues with doubling up on the name
                    string uniqueFileName = getUniqueId(existingFiles, fileName) + "." + fileExtension;
                    string filePath = Path.Combine(@".\JobFolder\Resumes", uniqueFileName);

                    // Upload the file if less than 2 MB
                    if (memoryStream.Length < 2097152)
                    {
                        try
                        {
                            FileStream stream = new FileStream(filePath, FileMode.Create);
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
                using (StreamWriter File1 = new StreamWriter(Path.Combine(jobDirectory, "Applications.json")))
                {
                    string newJson = JsonSerializer.Serialize<List<ProcessedApplication>>(applications, new JsonSerializerOptions() { WriteIndented = true });
                    File1.Write(newJson);
                }
            }
            catch
            {
                throw new MessageException("Oh no, there was a serverside error. ");
            }
        }

        //generates a unique id ffor each application 
        private string getUniqueId(List<string> existingIds, string inclusionVal)
        {
            int suffix = 0;
            string uniqueId = inclusionVal + suffix;

            while (existingIds.Contains(uniqueId + ( ++suffix ).ToString())) { }
            {
                return uniqueId + suffix.ToString();
            }
        }
        #endregion

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
                    processApplication(returnModel.Application, returnModel.ResumeFile, job);
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
#endregion
    }
}
