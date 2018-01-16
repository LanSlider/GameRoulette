using System.Web;
using System.Web.Optimization;

namespace GameRoulette
{
    public class BundleConfig
    {
        //Дополнительные сведения об объединении см. по адресу: http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // -------------------- JS ---------------------------
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/default/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/default/jquery.validate*"));

            // Используйте версию Modernizr для разработчиков, чтобы учиться работать. Когда вы будете готовы перейти к работе,
            // используйте средство сборки на сайте http://modernizr.com, чтобы выбрать только нужные тесты.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/default/modernizr-*"));

            // В папке Scripts используется для скриптов, необходимых для работы сайта
            bundles.Add(new ScriptBundle("~/bundles/default").Include(
                      "~/Scripts/jquery.min.js",
                      "~/Scripts/jquery.hexagonprogress.min.js",
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/SmoothScroll.js",
                      "~/Scripts/youplay.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/index").Include(
                      "~/Scripts/jarallax.min.js",
                      "~/Scripts/owl.carousel.min.js",
                      "~/Scripts/carousel.js",
                      "~/Scripts/jquery.countdown.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/profile").Include(
                      "~/Scripts/jarallax.min.js",
                      "~/Scripts/jquery.magnific-popup.min.js"));

            // ------------------- CSS ---------------------------
            bundles.Add(new StyleBundle("~/Content/defaultCss").Include(
                      "~/Content/css/bootstrap.min.css",
                      "~/Content/css/font-awesome.min.css",
                      "~/Content/css/youplay.min.css"));

            bundles.Add(new StyleBundle("~/Content/index").Include(
                      "~/Content/css/owl.carousel.min.css",
                      "~/Content/css/settings.css",
                      "~/Content/css/magnific-popup.css",
                      "~/Content/css/social-likes_flat.css",
                      "~/Content/css/sweet-alert.css"));

            bundles.Add(new StyleBundle("~/Content/profile").Include(
                      "~/Content/css/magnific-popup.css"));
        }
    }
}
