using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(webblog.Startup))]
namespace webblog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
