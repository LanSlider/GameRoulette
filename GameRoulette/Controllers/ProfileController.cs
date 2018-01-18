using GameRoulette.Models;
using System.Linq;
using System.Web.Mvc;

namespace GameRoulette.Controllers
{
    public class ProfileController : Controller
    {
        public void GetInfoUser()
        {
            User user = null;
            using (UserContext db = new UserContext())
            {
                user = db.Users.FirstOrDefault(u => u.Email == User.Identity.Name);
                if (user != null)
                {
                    ViewData["Name"] = user.Name;
                    ViewData["ID"] = user.ID;
                    ViewBag.Role = user.Role;
                    ViewData["Image"] = user.Image;
                    ViewData["Money"] = user.Money;
                }
                else
                {
                    user = db.Users.FirstOrDefault(u => u.Name == User.Identity.Name);
                    if (user != null)
                    {
                        ViewData["Name"] = user.Name.Split(' ')[0];
                        ViewData["ID"] = user.ID;
                        ViewBag.Role = user.Role;
                        ViewData["Image"] = user.Image;
                        ViewData["Money"] = user.Money;
                    }
                }
            }
        }

        // GET: Profile
        public ActionResult UserActivity()
        {
            GetInfoUser();
            if (ViewBag.Role == 0 || ViewBag.Role == 1)
                return View();
            return Redirect("/");
        }

        public ActionResult UserRecharge()
        {
            GetInfoUser();
            if (ViewBag.Role == 0 || ViewBag.Role == 1)
                return View();
            return Redirect("/");
        }

        public ActionResult UserControl()
        {
            GetInfoUser();
            if (ViewBag.Role == 1)
                return View();
            return Redirect("/");
        }

        public ActionResult GameControl()
        {
            GetInfoUser();
            if (ViewBag.Role == 1)
                return View();
            return Redirect("/");
        }

        public ActionResult SettingControl()
        {
            GetInfoUser();
            if (ViewBag.Role == 1)
                return View();
            return Redirect("/");
        }
    }
}