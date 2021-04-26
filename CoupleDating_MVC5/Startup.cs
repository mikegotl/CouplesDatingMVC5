using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CoupleDating_MVC5.Startup))]
namespace CoupleDating_MVC5
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
