using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(AppServiceMDC.MobileApp.Startup))]

namespace AppServiceMDC.MobileApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}