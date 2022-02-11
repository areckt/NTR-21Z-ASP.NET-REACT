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
using Microsoft.EntityFrameworkCore;
using lab1.Helpers;
using lab1.Models;
using lab1.Storage;

namespace lab1.Controllers
{
    public class ActivityController : Controller
    {
        private readonly UserStorage _userStorage;
        private readonly ActivityEntryStorage _activityEntryStorage;
        private readonly ProjectStorage _projectStorage;
        private readonly MonthInfoStorage _monthStorage;

        public ActivityController(TMSContext context)
        {
            _userStorage = new UserStorage(context);
            _activityEntryStorage = new ActivityEntryStorage(context);
            _projectStorage = new ProjectStorage(context);
            _monthStorage = new MonthInfoStorage(context);
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
            var projects = _projectStorage.GetAllProjects();
            projectsAndUserDataModel.Projects = projects;

            // add user's activity entries for this month
            var userActivities = _activityEntryStorage.GetActivityEntriesByUserAndMonth(
                HttpContext.Session.Get<string>("currentUsername"),
                Convert.ToInt32(HttpContext.Session.Get<string>("currentMonth")),
                Convert.ToInt32(HttpContext.Session.Get<string>("currentYear"))
            );

            projectsAndUserDataModel.UserActivity = userActivities;

            // add user's month info
            var isMonthFrozen = _monthStorage.CheckIfMonthFrozen(
                HttpContext.Session.Get<string>("currentUsername"),
                Convert.ToInt32(HttpContext.Session.Get<string>("currentMonth")),
                Convert.ToInt32(HttpContext.Session.Get<string>("currentYear"))
            );

            projectsAndUserDataModel.isMonthFrozen = isMonthFrozen;

            // add other data
            projectsAndUserDataModel.CurrentUser = HttpContext.Session.Get<string>("currentUsername");
            projectsAndUserDataModel.CurrentMonth = Convert.ToInt32(HttpContext.Session.Get<string>("currentMonth"));
            projectsAndUserDataModel.CurrentYear = Convert.ToInt32(HttpContext.Session.Get<string>("currentYear"));

            return View(projectsAndUserDataModel);
        }

        public IActionResult AddProjectForm()
        {
            return View();
        }

        public IActionResult AddProject(string Name, int Budget)
        {
            var newProject = new Project{
                Code = Name,
                Manager = HttpContext.Session.Get<string>("currentUsername"),
                Name = Name,
                Budget = Budget,
                Active = true
            };
            _projectStorage.AddProject(newProject);
            return RedirectToAction("Index", "Activity");
        }

        public IActionResult EditProjectForm(string Code)
        {
            // dynamic projectOldData = new ExpandoObject();
            // projectOldData.Name = Name;
            // projectOldData.Budget = Budget;

            // find project in database
            var project = _projectStorage.GetProjectByCode(Code);

            HttpContext.Session.Set<string>("tempProjectCode", Code);
            return View(project);
        }

        public IActionResult EditProject(Project updatedProject)
        {
            // string editedProjectCode = HttpContext.Session.Get<string>("tempProjectCode");
            // _projectStorage.UpdateProject(editedProjectCode, Name, Budget); 
            try
            {
                _projectStorage.UpdateProject(updatedProject);     
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return View("ConcurrencyError");
            }
            
            return RedirectToAction("Index", "Activity");
        }

        public IActionResult CloseProject(string Code)
        {
            _projectStorage.CloseProject(Code);
            return RedirectToAction("Index", "Activity");
        }

        public IActionResult AddActivityForm()
        {
            // create dynamic object for View
            dynamic projectsModel = new ExpandoObject();

            // add all active projects
            var activeProjects = _projectStorage.GetActiveProjects();
            projectsModel.Projects = activeProjects;

            return View(projectsModel);
        }

        public IActionResult AddActivity(int Day, string Code, string Subcode, int Time, string Description)
        {
            var newActivityEntry = new ActivityEntry{
                Username = HttpContext.Session.Get<string>("currentUsername"),
                Month = Convert.ToInt32(HttpContext.Session.Get<string>("currentMonth")),
                Year = Convert.ToInt32(HttpContext.Session.Get<string>("currentYear")),
                Day = Day,
                Code = Code,
                Subcode = Subcode,
                Time = Time,
                Description = Description
            };

            _activityEntryStorage.AddActivityEntry(newActivityEntry);
            bool doesMonthInfoExist = _monthStorage.CheckIfMonthInfoExists(
                HttpContext.Session.Get<string>("currentUsername"),
                newActivityEntry.Code,
                Convert.ToInt32(HttpContext.Session.Get<string>("currentMonth")),
                Convert.ToInt32(HttpContext.Session.Get<string>("currentYear"))
            );
            if (doesMonthInfoExist == false)
            {
                var newMonthInfo = new MonthInfo{
                    Month = Convert.ToInt32(HttpContext.Session.Get<string>("currentMonth")),
                    Year = Convert.ToInt32(HttpContext.Session.Get<string>("currentYear")),
                    ProjectCode = newActivityEntry.Code,
                    Username = HttpContext.Session.Get<string>("currentUsername"),
                    Frozen = false,
                    AcceptedTime = 0
                };
                _monthStorage.AddMonthInfo(newMonthInfo);
            }
            return RedirectToAction("Index", "Activity");
        }

        public IActionResult DeleteActivity(int Id)
        {
            _activityEntryStorage.RemoveActivityEntry(Id);
            return RedirectToAction("Index", "Activity");
        }


        public IActionResult FreezeMonth()
        {
            _monthStorage.FreezeMonth(
                HttpContext.Session.Get<string>("currentUsername"),
                Convert.ToInt32(HttpContext.Session.Get<string>("currentMonth")),
                Convert.ToInt32(HttpContext.Session.Get<string>("currentYear"))
            );

            return RedirectToAction("Index", "Activity");
        }

        public IActionResult ProjectDetails(string Code)
        {
            // create dynamic object for View
            dynamic projectAndUserDataModel = new ExpandoObject();

            // get project with Code
            projectAndUserDataModel.Project = _projectStorage.GetProjectByCode(Code);

            // get all activity entries for this project, this month and year
            var activityEntries = _activityEntryStorage.GetActivityEntriesByProjectAndMonth(
                Code,
                Convert.ToInt32(HttpContext.Session.Get<string>("currentMonth")),
                Convert.ToInt32(HttpContext.Session.Get<string>("currentYear"))
            );

            // get all activity entries for this project, all time
            var activityEntriesAllTime = _activityEntryStorage.GetActivityEntriesByProjectAllTime(Code);

            int TotalTime = 0;
            int TotalAcceptedTime = 0;
            int declaredTime = 0;
            var TimeDeclaredByUser = new Dictionary<string, int>();
            var ActivityEntriesByUser = new Dictionary<string, List<ActivityEntry>>();
            var IsMonthFrozenByUser = new Dictionary<string, bool>();
            var AcceptedTimeByUser = new Dictionary<string, int>();
            var users = _userStorage.GetUsers();
            
            foreach(var user in users)
            {
                TimeDeclaredByUser[user.Name] = 0;
                
                int acceptedTimeTemp = _monthStorage.GetAcceptedTime(
                    user.Name,
                    Code,
                    Convert.ToInt32(HttpContext.Session.Get<string>("currentMonth")),
                    Convert.ToInt32(HttpContext.Session.Get<string>("currentYear"))
                );
                TotalAcceptedTime += acceptedTimeTemp;
                AcceptedTimeByUser[user.Name] = acceptedTimeTemp;
                
                ActivityEntriesByUser[user.Name] = new List<ActivityEntry>();

                IsMonthFrozenByUser[user.Name] = _monthStorage.CheckIfMonthFrozen(
                    user.Name,
                    Convert.ToInt32(HttpContext.Session.Get<string>("currentMonth")),
                    Convert.ToInt32(HttpContext.Session.Get<string>("currentYear"))
                );
            }

            foreach (var activityEntry in activityEntries)
            {
                declaredTime += activityEntry.Time;
                TimeDeclaredByUser[activityEntry.Username] += activityEntry.Time;
                ActivityEntriesByUser[activityEntry.Username].Add(activityEntry);
            }

            foreach (var activityEntryAllTime in activityEntriesAllTime)
            {
                TotalTime += activityEntryAllTime.Time;
            }

            projectAndUserDataModel.Activities = ActivityEntriesByUser;
            projectAndUserDataModel.CurrentUser = HttpContext.Session.Get<string>("currentUsername");
            projectAndUserDataModel.TotalTime = TotalTime;
            projectAndUserDataModel.TotalAcceptedTime = TotalAcceptedTime;
            projectAndUserDataModel.DeclaredTimeByUser = TimeDeclaredByUser;
            projectAndUserDataModel.AllUsers = users;
            projectAndUserDataModel.IsMonthFrozenByUser = IsMonthFrozenByUser;
            projectAndUserDataModel.AcceptedTimeByUser = AcceptedTimeByUser;

            return View(projectAndUserDataModel);
        }
        public IActionResult ProjectDetailsAllTime(string Code)
        {
            // create dynamic object for View
            dynamic projectAndUserDataModel = new ExpandoObject();

            // get project with Code
            projectAndUserDataModel.Project = _projectStorage.GetProjectByCode(Code);

            // get all activity entries for this project, all time
            var activityEntries = _activityEntryStorage.GetActivityEntriesByProjectAllTime(Code);

            int TotalTime = 0;
            int AcceptedTime = 0;
            var TimeDeclaredByUser = new Dictionary<string, int>();
            var ActivityEntriesByUser = new Dictionary<string, List<ActivityEntry>>();
            var users = _userStorage.GetUsers();
            
            foreach(var user in users)
            {
                TimeDeclaredByUser[user.Name] = 0;
                AcceptedTime += _monthStorage.GetAcceptedTime(
                    user.Name,
                    Code,
                    Convert.ToInt32(HttpContext.Session.Get<string>("currentMonth")),
                    Convert.ToInt32(HttpContext.Session.Get<string>("currentYear"))
                );
                ActivityEntriesByUser[user.Name] = new List<ActivityEntry>();
            }

            foreach (var activityEntry in activityEntries)
            {
                TotalTime += activityEntry.Time;
                TimeDeclaredByUser[activityEntry.Username] += activityEntry.Time;
                ActivityEntriesByUser[activityEntry.Username].Add(activityEntry);
            }

            projectAndUserDataModel.Activities = ActivityEntriesByUser;
            projectAndUserDataModel.CurrentUser = HttpContext.Session.Get<string>("currentUsername");
            projectAndUserDataModel.TotalTime = TotalTime;
            projectAndUserDataModel.AcceptedTime = AcceptedTime;
            projectAndUserDataModel.DeclaredTimeByUser = TimeDeclaredByUser;
            projectAndUserDataModel.AllUsers = users;

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
            _monthStorage.SetAcceptedTime(
                HttpContext.Session.Get<string>("userToUpdateAccepted"),
                HttpContext.Session.Get<string>("ProjectCodeToUpdateAccepted"),
                Convert.ToInt32(HttpContext.Session.Get<string>("currentMonth")),
                Convert.ToInt32(HttpContext.Session.Get<string>("currentYear")),
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
