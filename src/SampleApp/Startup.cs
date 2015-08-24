using OnDotNet.Owin.Shield.Frameguard;
using Owin;

namespace SampleApp
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Frameguard(XFrameOptions.Deny);
            app.UseWelcomePage();
        }
    }
}
