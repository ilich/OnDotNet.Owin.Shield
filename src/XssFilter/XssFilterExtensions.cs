// ReSharper disable InconsistentNaming

using System;
using Microsoft.Owin;
using Owin;

namespace OnDotNet.Owin.Shield.XssFilter
{
    using UserAgentFunc = Func<IOwinContext, string>;

    public static class XssFilterExtensions
    {
        public static void XssFilter(this IAppBuilder app, bool setOnOldIE = false)
        {
            app.XssFilter(setOnOldIE, null);
        }

        internal static void XssFilter(
            this IAppBuilder app, 
            bool setOnOldIE,
            UserAgentFunc getUserAgent)
        {
            app.Use<XssFilter>(setOnOldIE, getUserAgent);
        }
    }
}
