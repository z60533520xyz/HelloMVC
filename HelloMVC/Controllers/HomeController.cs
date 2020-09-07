using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HelloMVC.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.Design.Serialization;


namespace HelloMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            DBmanager dBmanager = new DBmanager();
            List<Userdata> userdatas = dBmanager.GetUserdatas();
            ViewBag.userdatas = userdatas;
            return View();
        }

        public IActionResult Privacy2()
        {
            if (HttpContext.Session.GetString("username") != null ){ 
                DBmanager dBmanager = new DBmanager();
                List<Conversation> conversations = dBmanager.GetConversations();
                ViewBag.conversations = conversations;
                ViewData["username"] = HttpContext.Session.GetString("username");
                ViewData["sationname"] = "mary";
                HttpContext.Session.SetString("sationname", "mary");
                return View();
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
            
        }
        public IActionResult SendMag(string send = "")
        {
            if(send != "") {
                DBmanager dBmanager = new DBmanager();
                dBmanager.SendMag(HttpContext.Session.GetString("username"), HttpContext.Session.GetString("sationname"), send);
            }
            return RedirectToAction("Privacy2", "Home");


        }
        public IActionResult login(string name = "",string password = "")
        {
            
            string id = "", pd= "";
            if (name=="" & password =="")
            {
                ViewData["masg"] = "";
                return View();
            }
            else if(name != "" & password != "")
            {
                DBmanager dBmanager = new DBmanager();
                List<Login> login = dBmanager.GetLogins(name);
                foreach (Login logins in login)
                {
                    pd = logins.password;
                    id = logins.username;
                }
                //ViewData["masg"] = id+pd;

                if ((name == id) & (pd == password))
                {
                    ViewData["masg"] = "";
                    HttpContext.Session.SetString("username", name);
                    return RedirectToAction("Privacy2", "Home");
                }
                else
                {
                    ViewData["masg"] = "Enter Error";
                }
            }
            return View();
        }
        public IActionResult login2()
        {
            return View();
        }

        public IActionResult product()
        {
            if (HttpContext.Session.Keys.Contains("name"))
            {
                ViewData["name"] = HttpContext.Session.GetString("name");
            }
                
            var user = new UserModel();
            user.Age += 30;
            return View(model:user);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
