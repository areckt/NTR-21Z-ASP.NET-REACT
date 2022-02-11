using System;
using System.IO;
using System.Dynamic;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using lab1.Helpers;
using lab1.Models;
using lab1.Utils;

namespace lab1.Controllers
{
    public class ActivityController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public ActivityController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.Get<string>("currentYear") == default)
            {
                DateTime date = DateTime.Now;
                string currentMonth = date.ToString("MM");
                string currentYear = date.ToString("yyyy");

                HttpContext.Session.Set<string>("currentMonth", currentMonth);
                HttpContext.Session.Set<string>("currentYear", currentYear);
            }

            // create dynamic object for View
            dynamic projectsAndUserDataModel = new ExpandoObject();

            // add all projects
            ProjectsReader pr = new ProjectsReader();
            projectsAndUserDataModel.Projects = pr.Projects;

            UserActivityReader uar = new UserActivityReader();

            // check if there is user activity for current user, month and year
            if (uar.UserActivities.Any(activity =>
                activity.Month == Convert.ToInt32(HttpContext.Session.Get<string>("currentMonth")) &&
                activity.Year == Convert.ToInt32(HttpContext.Session.Get<string>("currentYear")) &&
                activity.Name == HttpContext.Session.Get<string>("currentUsername")
                ))
            {
                // if yes - add it to the dynamic object for View
                projectsAndUserDataModel.UserActivity = uar.UserActivities.Find(activity =>
                    activity.Month == Convert.ToInt32(HttpContext.Session.Get<string>("currentMonth")) &&
                    activity.Year == Convert.ToInt32(HttpContext.Session.Get<string>("currentYear")) &&
                    activity.Name == HttpContext.Session.Get<string>("currentUsername")
                );
            }
            else
            {
                // if not = create new user activity file
                UserActivityWriter uaw = new UserActivityWriter(
                    HttpContext.Session.Get<string>("currentUsername"),
                    HttpContext.Session.Get<string>("currentYear"),
                    HttpContext.Session.Get<string>("currentMonth")
                );
                projectsAndUserDataModel.UserActivity = uaw.CreateNewUserActivityFile();
            }

            // add other data
            projectsAndUserDataModel.CurrentUser = new User(HttpContext.Session.Get<string>("currentUsername"));
            projectsAndUserDataModel.CurrentMonth = Convert.ToInt32(HttpContext.Session.Get<string>("currentMonth"));
            projectsAndUserDataModel.CurrentYear = Convert.ToInt32(HttpContext.Session.Get<string>("currentYear"));

            // jeśli takiego pliku nie ma, stwórz go z defaultowymi wartościami
            return View(projectsAndUserDataModel);
        }

        public IActionResult AddProjectForm()
        {
            return View();
        }

        public IActionResult AddProject(string Name, int Budget, string Subact1, string Subact2)
        {
            // create list of subactivities
            List<string> subactivitiesList = new List<string>();
            if (Subact1 != null) { subactivitiesList.Add(Subact1); }
            if (Subact2 != null) { subactivitiesList.Add(Subact2); }

            // create new project
            Project newProject = new Project(
                DateTime.Now.ToString(),
                HttpContext.Session.Get<string>("currentUsername"),
                Name,
                Budget,
                true,
                subactivitiesList.ToArray()
            );

            // add new project to the file
            ProjectsWriter pw = new ProjectsWriter();
            pw.WriteToFile(newProject);

            return RedirectToAction("Index", "Activity");
        }

        public IActionResult EditProjectForm(string Code, string Name, int Budget)
        {
            // create dynamic object for View
            dynamic projectOldData = new ExpandoObject();
            projectOldData.Name = Name;
            projectOldData.Budget = Budget;

            HttpContext.Session.Set<string>("tempProjectCode", Code);
            return View(projectOldData);
        }

        public IActionResult EditProject(string Name, int Budget)
        {
            // find project with tempProjectCode
            ProjectsReader pr = new ProjectsReader();
            var editedProject = pr.Projects.Find(project => project.Code == HttpContext.Session.Get<string>("tempProjectCode"));

            // change its name and budget properties
            editedProject.Name = Name;
            editedProject.Budget = Budget;

            // save it to file
            ProjectsWriter pw = new ProjectsWriter();
            pw.WriteToFile(editedProject);

            return RedirectToAction("Index", "Activity");
        }

        public IActionResult CloseProject(string Code)
        {
            // find project with Code
            ProjectsReader pr = new ProjectsReader();
            var editedProject = pr.Projects.Find(project => project.Code == Code);

            // deactivate it
            editedProject.Active = false;

            // save it to file
            ProjectsWriter pw = new ProjectsWriter();
            pw.WriteToFile(editedProject);

            // find all usernames
            UsersReader ur = new UsersReader();
            DirectoryInfo di = new DirectoryInfo("data_files/activity_files");
            FileInfo[] files = di.GetFiles("*.json");

            List<string> allUsers = new List<string>();
            foreach (User user in ur.Users)
            {
                allUsers.Add(user.Name);
            }


            List<string> filenamesList = new List<string>();
            foreach (FileInfo file in files)
            {
                filenamesList.Add(file.Name);
            }

            string filenameEnding =
                "_" + HttpContext.Session.Get<string>("currentYear") +
                "_" + HttpContext.Session.Get<string>("currentMonth") +
                ".json";

            List<string> usersToCheckAcceptedTime = new List<string>();
            foreach (string username in allUsers)
            {
                foreach (string filename in filenamesList)
                {
                    if (username + filenameEnding == filename)
                    {
                        usersToCheckAcceptedTime.Add(username);
                        break;
                    }
                }
            }

            if (usersToCheckAcceptedTime.Count > 0)
            {
                foreach (string user in usersToCheckAcceptedTime)
                {
                    UserActivityWriter uaw = new UserActivityWriter(
                        user,
                        HttpContext.Session.Get<string>("currentYear"),
                        HttpContext.Session.Get<string>("currentMonth")
                    );
                    uaw.SetAcceptedToZeroIfUndefined(Code);
                }
            }

            return RedirectToAction("Index", "Activity");
        }

        public IActionResult AddActivityForm()
        {
            // create dynamic object for View
            dynamic projectsModel = new ExpandoObject();

            // add all active projects
            ProjectsReader pr = new ProjectsReader();
            projectsModel.Projects = pr.Projects.Where(project =>
                project.Active == true
            );

            return View(projectsModel);
        }

        public IActionResult AddActivity(int Day, string Code, string Subcode, int Time, string Description)
        {
            // generate activity date
            string day = (Day.ToString().Length == 1 ? "0" + Day.ToString() : Day.ToString());
            string newActivityDate =
                HttpContext.Session.Get<string>("currentYear") + "-" +
                HttpContext.Session.Get<string>("currentMonth") + "-" +
                day;

            // create new ActivityEntry
            ActivityEntry newActivity = new ActivityEntry(
                newActivityDate,
                Code,
                Subcode,
                Time,
                Description);

            // update user activity file
            UserActivityWriter uaw = new UserActivityWriter(
                HttpContext.Session.Get<string>("currentUsername"),
                HttpContext.Session.Get<string>("currentYear"),
                HttpContext.Session.Get<string>("currentMonth")
            );
            uaw.AddNewActivityEntry(newActivity);

            return RedirectToAction("Index", "Activity");
        }

        // "@Url.Action("DeleteActivity", "Activity", new
        // { Date = entry.Date, Code = entry.Code, Time = entry.Time })";' />

        public IActionResult DeleteActivity(string Date, string Code, int Time)
        {
            UserActivityReader uar = new UserActivityReader();
            UserActivity userActivityToModify = uar.UserActivities.Find(userActivity =>
                userActivity.Name == HttpContext.Session.Get<string>("currentUsername") &&
                userActivity.Month.ToString() == HttpContext.Session.Get<string>("currentMonth") &&
                userActivity.Year.ToString() == HttpContext.Session.Get<string>("currentYear")
            );

            UserActivityWriter uaw = new UserActivityWriter(
                HttpContext.Session.Get<string>("currentUsername"),
                HttpContext.Session.Get<string>("currentYear"),
                HttpContext.Session.Get<string>("currentMonth")
            );
            // delete activity entry
            uaw.DeleteActivityEntry(Date, Code, Time, userActivityToModify);


            return RedirectToAction("Index", "Activity");
        }


        public IActionResult FreezeMonth()
        {
            UserActivityWriter uaw = new UserActivityWriter(
                HttpContext.Session.Get<string>("currentUsername"),
                HttpContext.Session.Get<string>("currentYear"),
                HttpContext.Session.Get<string>("currentMonth")
            );

            uaw.FreezeUserActivity();

            return RedirectToAction("Index", "Activity");
        }

        public IActionResult ProjectDetails(string Code)
        {
            // create dynamic object for View
            dynamic projectAndUserDataModel = new ExpandoObject();

            // get project with Code
            ProjectsReader pr = new ProjectsReader();
            projectAndUserDataModel.Project = pr.Projects.Find(project => project.Code == Code);

            // get all user activities for this month and year
            UserActivityReader uar = new UserActivityReader();
            List<UserActivity> userActivitiesForProject = new List<UserActivity>();
            int TotalTime = 0;
            int AcceptedTime = 0;
            var TimeDeclaredByUser = new Dictionary<string, int>();
            foreach (var userActivity in uar.UserActivities)
            {
                if (userActivity.Month.ToString() == HttpContext.Session.Get<string>("currentMonth") &&
                    userActivity.Year.ToString() == HttpContext.Session.Get<string>("currentYear"))
                {
                    int declaredTime = 0;
                    // we want entries only for this project
                    List<ActivityEntry> entriesForThisProject = new List<ActivityEntry>();
                    foreach (var activityEntry in userActivity.Entries)
                    {
                        if (activityEntry.Code == Code)
                        {
                            TotalTime += activityEntry.Time;
                            declaredTime += activityEntry.Time;
                            entriesForThisProject.Add(activityEntry);
                        }
                    }
                    userActivity.Entries = entriesForThisProject;
                    TimeDeclaredByUser[userActivity.Name] = declaredTime;

                    // also check if there is 'accepted' defined for this project
                    List<AcceptedActivity> acceptedActivitiesForThisProject = new List<AcceptedActivity>();
                    foreach (var acceptedActivity in userActivity.AcceptedActivities)
                    {
                        if (acceptedActivity.Code == Code)
                        {
                            acceptedActivitiesForThisProject.Add(acceptedActivity);
                            AcceptedTime += acceptedActivity.Time;
                        }
                    }

                    userActivitiesForProject.Add(userActivity);
                }
            }
            projectAndUserDataModel.Activities = userActivitiesForProject;

            projectAndUserDataModel.CurrentUser = new User(HttpContext.Session.Get<string>("currentUsername"));
            projectAndUserDataModel.TotalTime = TotalTime;
            projectAndUserDataModel.AcceptedTime = AcceptedTime;
            projectAndUserDataModel.DeclaredTimeByUser = TimeDeclaredByUser;

            return View(projectAndUserDataModel);
        }
        public IActionResult ProjectDetailsAllTime(string Code)
        {
            // create dynamic object for View
            dynamic projectAndUserDataModel = new ExpandoObject();

            // get project with Code
            ProjectsReader pr = new ProjectsReader();
            projectAndUserDataModel.Project = pr.Projects.Find(project => project.Code == Code);

            // get all user activities
            UserActivityReader uar = new UserActivityReader();
            List<UserActivity> userActivitiesForProject = new List<UserActivity>();
            int TotalTime = 0;
            int AcceptedTime = 0;
            foreach (var userActivity in uar.UserActivities)
            {
                // we want entries only for this project
                List<ActivityEntry> entriesForThisProject = new List<ActivityEntry>();
                foreach (var activityEntry in userActivity.Entries)
                {
                    if (activityEntry.Code == Code)
                    {
                        TotalTime += activityEntry.Time;
                        entriesForThisProject.Add(activityEntry);
                    }
                }
                userActivity.Entries = entriesForThisProject;

                // also check if there is 'accepted' defined for this project
                List<AcceptedActivity> acceptedActivitiesForThisProject = new List<AcceptedActivity>();
                foreach (var acceptedActivity in userActivity.AcceptedActivities)
                {
                    if (acceptedActivity.Code == Code)
                    {
                        acceptedActivitiesForThisProject.Add(acceptedActivity);
                        AcceptedTime += acceptedActivity.Time;
                    }
                }

                userActivitiesForProject.Add(userActivity);
            }
            projectAndUserDataModel.Activities = userActivitiesForProject;

            projectAndUserDataModel.CurrentUser = new User(HttpContext.Session.Get<string>("currentUsername"));
            projectAndUserDataModel.TotalTime = TotalTime;
            projectAndUserDataModel.AcceptedTime = AcceptedTime;

            return View(projectAndUserDataModel);
        }

        public IActionResult SetAcceptedTimeForm(string Name, string Code)
        {
            HttpContext.Session.Set<string>("userToUpdateAccepted", Name);
            HttpContext.Session.Set<string>("ProjectCodeToUpdateAccepted", Code);
            return View();
        }

        public IActionResult SetAcceptedTime(int AcceptedTime)
        {
            UserActivityWriter uaw = new UserActivityWriter(
                HttpContext.Session.Get<string>("userToUpdateAccepted"),
                HttpContext.Session.Get<string>("currentYear"),
                HttpContext.Session.Get<string>("currentMonth")
            );
            uaw.SetAccepted(
                HttpContext.Session.Get<string>("ProjectCodeToUpdateAccepted"),
                AcceptedTime
            );

            return RedirectToAction("Index", "Activity");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
