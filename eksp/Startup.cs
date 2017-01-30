using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(eksp.Startup))]
namespace eksp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();

            ConfigureAuth(app);
            
        }
    }
}
