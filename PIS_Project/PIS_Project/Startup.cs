using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PIS_Project.Startup))]
namespace PIS_Project
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
