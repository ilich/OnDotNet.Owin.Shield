﻿using OnDotNet.Owin.Shield.Frameguard;
using OnDotNet.Owin.Shield.IENoOpen;
using OnDotNet.Owin.Shield.NoSniff;
using OnDotNet.Owin.Shield.XssFilter;
using Owin;

namespace SampleApp
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Frameguard(XFrameOptions.Deny);
            app.XssFilter(true);
            app.NoSniff();
            app.IENoOpen();
            app.UseWelcomePage();
        }
    }
}
