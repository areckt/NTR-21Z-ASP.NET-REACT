using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using lab1.Models;
using lab1.Helpers;
using lab1.Storage;

namespace lab1.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserStorage _userStorage;

        public HomeController(TMSContext context)
        {
            _userStorage = new UserStorage(context);
        }

        public IActionResult Index()
        {
            // UsersReader ur = new UsersReader();
            var users = _userStorage.GetUsers();
            return View(users);
        }

        public IActionResult AddUserForm()
        {
            return View();
        }

        public IActionResult AddUser(User user)
        {
            // modify data_files/users.json - add Username to the file
            // UsersWriter uw = new UsersWriter();
            // uw.WriteToFile(Username);

            _userStorage.AddUser(user);

            return RedirectToAction("Index");
        }

        public IActionResult Login(string Username)
        {
            HttpContext.Session.Set<string>("currentUsername", Username);
            return RedirectToAction("Index", "Activity");
        }

        public IActionResult ChangeDateForm()
        {
            return View();
        }

        public IActionResult ChangeDate(int Month, int Year)
        {
            HttpContext.Session.Set<string>("currentMonth", Month.ToString());
            HttpContext.Session.Set<string>("currentYear", Year.ToString());

            return RedirectToAction("Index", "Activity");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
