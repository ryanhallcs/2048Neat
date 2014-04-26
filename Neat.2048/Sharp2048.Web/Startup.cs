using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Sharp2048.Web.Startup))]
namespace Sharp2048.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
