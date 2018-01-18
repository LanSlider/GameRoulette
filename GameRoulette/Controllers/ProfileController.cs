using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameRoulette.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        public ActionResult UserActivity()
        {
            return View();
        }

        public ActionResult UserRecharge()
        {
            return View();
        }
    }
}