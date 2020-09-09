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

        public IActionResult Conversation(string sationname="admin")
        {
            if (HttpContext.Session.GetString("username") != null ){
                string username = HttpContext.Session.GetString("username");
                DBmanager dBmanager = new DBmanager();
                List<Conversation> conversations = dBmanager.GetConversations(username, sationname);
                ViewBag.conversations = conversations;
                ViewData["username"] = username;
                List<Target> targets = dBmanager.GetTargets(ViewData["username"].ToString());
                foreach (Target target in targets)
                {
                    ViewBag.target = target.target;
                }
                ViewData["sationname"] = sationname;
                HttpContext.Session.SetString("sationname", sationname);

                return View();
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
            
        }
        public IActionResult SendMag(string send = "",string sationname = "")
        {
            if(send != "") {
                DBmanager dBmanager = new DBmanager();
                dBmanager.SendMag(HttpContext.Session.GetString("username"), HttpContext.Session.GetString("sationname"), send);
            }
            return RedirectToAction("Conversation", "Home",new { sationname = sationname });


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
                    return RedirectToAction("Conversation", "Home");
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
        public IActionResult Like(string likename = "admin")
        {
            DBmanager dBmanager = new DBmanager();
            List<Userdata> userdatas = dBmanager.GetUserdatas(likename);
            ViewBag.username = HttpContext.Session.GetString("username");
            foreach(Userdata userdata in userdatas)
            {
                ViewBag.likename = userdata.username;
                ViewBag.name = userdata.name;
                ViewBag.sex = userdata.sex;
                ViewBag.address = userdata.address;
                ViewBag.image = userdata.image;
                ViewBag.logindate = userdata.logindate;
            }

            return View();
        }

        public IActionResult signup()
        {
            return View();
        }
        public IActionResult test(string username = "",string likename = "",bool like = false)
        {
            string newlike;
            string oldlike = "";
            if(username=="" | likename == "")
            {
                return View();
            }
            else
            {
                DBmanager dBmanager = new DBmanager();
                if (like)
                {
                    bool likeck = true; 
                    List<Like> likes = dBmanager.GetLike(username);
                    foreach(Like l in likes)
                    {
                        oldlike = l.tolikes;
                        foreach(string s in l.tolikes.Split(","))
                        {
                            if (likename == s) likeck = false;
                        }
                    }
                    if (likeck)
                    {
                        newlike = oldlike + "," + likename;
                        dBmanager.SetTolike(username, newlike);

                    }
                }

            }
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
