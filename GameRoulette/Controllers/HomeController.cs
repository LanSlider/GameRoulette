using GameRoulette.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameRoulette.Controllers
{
    public class HomeController : Controller
    {
        public void GetInfoMoney()
        {
            User user = null;
            using (UserContext db = new UserContext())
            {
                user = db.Users.FirstOrDefault(u => u.Email == User.Identity.Name);
                if (user != null)                
                    ViewData["Money"] = user.Money;
                else
                {
                    user = db.Users.FirstOrDefault(u => u.Name == User.Identity.Name);
                    if (user != null)
                        ViewData["Money"] = user.Money;
                }
            }
        }

        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                GetInfoMoney();
            }
            return View();
        }

        public ActionResult About()
        {
            if (Request.IsAuthenticated)
            {
                GetInfoMoney();
            }
            return View();
        }

        public ActionResult Contact()
        {
            if (Request.IsAuthenticated)
            {
                GetInfoMoney();
            }
            return View();
        }
    }
}