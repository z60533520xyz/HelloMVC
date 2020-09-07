using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace HelloMVC.Controllers
{
    public class SeanController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.Keys.Contains("name"))
            {
                ViewData["name"] = HttpContext.Session.GetString("name");
            }
            else
            {
                Response.Redirect("/Home/login");
            }
            return View();
        }
        public string ninenine()
        {
            string s = "";
            for(int i = 1; i <= 9; i++)
            {
                for(int j = 1; j <= 9; j++)
                {
                    s += i + " + " + j +" = "+i*j+"\t";
                }
                s += "\n";
            }
            return s;
        }
        
        public IActionResult ninenine2(int nem2 = 9)
        {
            ViewData["nem"] = nem2;
            return View();
        }

        public string lotto(int num = 6) 
        {
            string s = "";
            Random rnd = new Random();
            int[] lotto = new int[num];
            int i = 0;
            while (lotto[num - 1] == 0)
            {
                int temp = rnd.Next(1, 49);
                for (int j = 0; j <= i; j++)
                {
                    if (lotto[j] == temp)
                    {
                        break;
                    }
                    else if (i == j)
                    {
                        lotto[j] = temp;
                        i++;
                        break;
                    }
                }
            }
            for (int k = 0; k < num; k++)
            {
                s += (lotto[k] + "\t");
            }
            return s;
        }

    }
}
