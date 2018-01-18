using System.Linq;
using System.Web.Security;
using System.Web.Mvc;
using GameRoulette.Models;
using System.Net.Mail;
using System;
using System.Threading.Tasks;
using DotNetOpenAuth.OpenId.RelyingParty;

namespace FormsAuthApp.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                // поиск пользователя в бд
                User user = null;
                using (UserContext db = new UserContext())
                {
                    user = db.Users.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

                }
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Email, true);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Пользователя с таким логином и паролем нет");
                }
            }

            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = null;
                using (UserContext db = new UserContext())
                {
                    user = db.Users.FirstOrDefault(u => u.Email == model.Email);
                }
                if (user == null)
                {
                    // создаем нового пользователя
                    using (UserContext db = new UserContext())
                    {
                        var email = new MailAddress(model.Email);
                        db.Users.Add(new User { Name = email.User, Email = model.Email, Password = model.Password });
                        db.SaveChanges();

                        user = db.Users.Where(u => u.Email == model.Email && u.Password == model.Password).FirstOrDefault();
                    }
                    // если пользователь удачно добавлен в бд
                    if (user != null)
                    {
                        FormsAuthentication.SetAuthCookie(model.Email, true);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь с таким логином уже существует");
                }
            }

            return View(model);
        }

        public ActionResult RegisterID()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterID(RegisterModelID model)
        {
            if (ModelState.IsValid)
            {
                AppID user = null;
                using (AppIDContext db = new AppIDContext())
                {
                    user = db.AppID.FirstOrDefault(u => u.appID == model.AppId);
                }
                if (user == null)
                {
                    // создаем нового пользователя
                    using (AppIDContext db = new AppIDContext())
                    {
                        
                        db.AppID.Add(new AppID { appID = model.AppId, PrivateKey = model.PrivateKey, SteamKey = model.SteamKey });
                        db.SaveChanges();

                        user = db.AppID.Where(u => u.appID == model.AppId).FirstOrDefault();
                    }
                    // если пользователь удачно добавлен в бд
                    if (user != null)
                    {
                        
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь с таким логином уже существует");
                }
            }

            return View(model);
        }

        public ActionResult VKLogin()
        {          
                AppID appID = null;
                using (AppIDContext db = new AppIDContext())
                {
                    appID = db.AppID.FirstOrDefault();
                }
                if (appID != null)
                {
                    string domain = System.Web.HttpContext.Current.Request.Url.Authority;
                    string url = "https://oauth.vk.com/authorize?client_id=" + appID.appID + "&redirect_uri=http://localhost:54106/Account/VK&display=popup&response_type=code";
                    return Redirect(url);
                }
                else
                {
                    using (ErrorsContext db = new ErrorsContext())
                    {
                        db.Errors.Add(new Error { ErrorText = "VK: Пустой VKAppID!" });
                    }
                    return Redirect("/");
                }
        }

        public async Task<ActionResult> VK()
        {
            //http://minecraft-sodeon.ru/#access_token=6a744edc0b862ee6d2d52a0a95a3097fc7b8726ce199260ab62e5e0e4b41150658edc9ac73c9f90bab83f&expires_in=86400&user_id=114224258
            //https://api.vk.com/method/users.get.xml?user_ids=210700286&fields=bdate&v=5.69
            //https://oauth.vk.com/access_token?client_id=1&client_secret=H2Pk8htyFD8024mZaPHm&redirect_uri=http://mysite.ru&code=7a6fa4dff77a228eeda56603b8f53806c883f011c40b72630bb50df056f6479e52a
            string jsonToken = null;

            try
            {
                string path = Request.Url.AbsoluteUri;
                string code = path.Split('=')[1];

                AppID appID = null;
                using (AppIDContext db = new AppIDContext())
                {
                    appID = db.AppID.FirstOrDefault();
                }

                string domain = System.Web.HttpContext.Current.Request.Url.Authority;
                string url = "https://oauth.vk.com/access_token?client_id=" + appID.appID + "&client_secret=" + appID.PrivateKey + "&redirect_uri=http://localhost:54106/Account/VK&code=" + code;
                jsonToken = await GetAccessToken(url);
            }
            catch
            {
                using (ErrorsContext db = new ErrorsContext())
                {
                    db.Errors.Add(new Error { ErrorText = "VK: Неверный VKAppID!" });
                }
            }

            //"{\"access_token\": \"20972fdda2157fe9dcbf61754cd999bf1c03930ef45ebf8d62925a756d05340996805433e53d17010889d\", \"expires_in\": 86400, \"user_id\": 114224258}"
            //"{\"response\": [{\"first_name\": \"Антон\", \"id\": 114224258, \"last_name\": \"Репетухо\", \"photo_200_orig\": \"https://pp.userapi.com/c836636/v836636617/49c88/lyxqzBnvsuI.jpg\"}]}"
            if (jsonToken != null)
            {
                jsonToken = jsonToken.Split(' ')[5];
                string userID = jsonToken.Split('}')[0];
                if (userID != jsonToken)
                {
                    string url = "https://api.vk.com/method/users.get.json?user_ids=" + userID + "&fields=photo_200_orig&v=5.69";
                    string jsonUser = await GetAccessToken(url);

                    string firstName = jsonUser.Split('\"')[5];
                    string lastName = jsonUser.Split('\"')[11];
                    string idUser = jsonUser.Split(',')[1];
                    idUser = idUser.Split(' ')[2];
                    string photo = jsonUser.Split('\"')[15];

                    User user = null;
                    using (UserContext db = new UserContext())
                    {
                        user = db.Users.FirstOrDefault(u => u.UserID == idUser);
                    }
                    if (user == null)
                    {
                        // создаем нового пользователя
                        using (UserContext db = new UserContext())
                        {
                            db.Users.Add(new User { Name = firstName + " " + lastName, UserID = idUser, Image = photo });
                            db.SaveChanges();

                            user = db.Users.Where(u => u.UserID == idUser).FirstOrDefault();
                        }
                        // если пользователь удачно добавлен в бд
                        if (user != null)
                        {
                            FormsAuthentication.SetAuthCookie(firstName + " " + lastName, true);
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        FormsAuthentication.SetAuthCookie(firstName + " " + lastName, true);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    using (ErrorsContext db = new ErrorsContext())
                    {
                        db.Errors.Add(new Error { ErrorText = "VK: Ошибка получения Токена (ID)!" });
                    }
                }
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<ActionResult> SteamLogin()
        {
            
            AppID appID = null;
            using (AppIDContext db = new AppIDContext())
            {
                appID = db.AppID.Where(u => u.SteamKey != null).FirstOrDefault();
            }
            if (appID != null)
            {
                var openid = new OpenIdRelyingParty();
                var response = openid.GetResponse();

                if (response == null)
                {
                    using (OpenIdRelyingParty openidd = new OpenIdRelyingParty())
                    {
                        IAuthenticationRequest request = openidd.CreateRequest("http://steamcommunity.com/openid");
                        request.RedirectToProvider();
                    }
                }
                else
                {
                    switch (response.Status)
                    {
                        case AuthenticationStatus.Authenticated:
                            var userID = response.ClaimedIdentifier.ToString();

                            userID = userID.Split('/')[5];
                            //"{\"response\": {\"players\": [{\"avatar\": \"https://steamcdn-a.akamaihd.net/steamcommunity/public/images/avatars/eb/ebec8ea57ecad775b18e748e24007ad9943af624.jpg\", \"avatarfull\": \"https://steamcdn-a.akamaihd.net/steamcommunity/public/images/avatars/eb/ebec8ea57ecad775b18e748e24007ad9943af624_full.jpg\", \"avatarmedium\": \"https://steamcdn-a.akamaihd.net/steamcommunity/public/images/avatars/eb/ebec8ea57ecad775b18e748e24007ad9943af624_medium.jpg\", \"commentpermission\": 1, \"communityvisibilitystate\": 3, \"lastlogoff\": 1516243226, \"personaname\": \"DevilWars\", \"personastate\": 0, \"personastateflags\": 0, \"primaryclanid\": \"103582791429521408\", \"profilestate\": 1, \"profileurl\": \"http://steamcommunity.com/profiles/76561198070473053/\", \"steamid\": \"76561198070473053\", \"timecreated\": 1346268753}]}}"
                            string Url = "http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key=" + appID.SteamKey + "&steamids=" + userID;
                            string User = await GetAccessToken(Url);
                            string userName = User.Split('\"')[25];
                            string photo = User.Split('\"')[11];

                            User user = null;
                            using (UserContext db = new UserContext())
                            {
                                user = db.Users.FirstOrDefault(u => u.UserID == userID);
                            }
                            if (user == null)
                            {
                                // создаем нового пользователя
                                using (UserContext db = new UserContext())
                                {
                                    db.Users.Add(new User { Name = userName, UserID = userID, Image = photo });
                                    db.SaveChanges();

                                    user = db.Users.Where(u => u.UserID == userID).FirstOrDefault();
                                }
                                // если пользователь удачно добавлен в бд
                                if (user != null)
                                {
                                    FormsAuthentication.SetAuthCookie(userName, true);
                                    return RedirectToAction("Index", "Home");
                                }
                            }
                            else
                            {
                                FormsAuthentication.SetAuthCookie(userName, true);
                                return RedirectToAction("Index", "Home");
                            }

                            break;

                        case AuthenticationStatus.Canceled:
                        case AuthenticationStatus.Failed:
                            return Redirect("/");                           
                    }
                }
                return Redirect("/");
            }
            else
            {
                using (ErrorsContext db = new ErrorsContext())
                {
                    db.Errors.Add(new Error { ErrorText = "Steam: Пустой SteamKey!" });
                }
                return Redirect("/");
            }
        }


        public async Task<String> GetAccessToken(string url)
        {
            try
            {                
                var client = new System.Net.Http.HttpClient();
                var response = await client.GetAsync(new Uri(url));
                var result = await response.Content.ReadAsStringAsync();

                System.Json.JsonValue json = System.Json.JsonValue.Parse(result);

                return json.ToString();
            }
            catch
            {
                using (ErrorsContext db = new ErrorsContext())
                {
                    db.Errors.Add(new Error { ErrorText = "Ошибка await GetAccessToken(Не получен code)!" });
                }
                return "";
            }
        }

        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}