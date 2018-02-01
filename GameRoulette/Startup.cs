using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GameRoulette.Startup))]
namespace GameRoulette
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
