using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NetFrameworkApplication.Startup))]
namespace NetFrameworkApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
