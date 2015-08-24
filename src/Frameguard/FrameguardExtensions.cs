using System;
using Owin;

namespace OnDotNet.Owin.Shield.Frameguard
{
    public static class FrameguardExtensions
    {
        public static void Frameguard(this IAppBuilder app, XFrameOptions xFrameOptions, Uri uri = null)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            app.Use<FrameguardMiddleware>(xFrameOptions, uri);
        }
    }
}
