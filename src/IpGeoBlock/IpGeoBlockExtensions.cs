using System;
using Microsoft.Owin;
using Owin;

namespace OnDotNet.Owin.Shield.IpGeoBlock
{
    using RemoteIdAddressFunc = Func<IOwinContext, string>;

    public static class IpGeoBlockExtensions
    {
        public static void IpGeoBlock(this IAppBuilder app, IpGeoBlockOptions options)
        {
            app.IpGeoBlock(options, null);
        }

        internal static void IpGeoBlock(
            this IAppBuilder app, 
            IpGeoBlockOptions options, 
            RemoteIdAddressFunc getRemoteIpAddress)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            app.Use<IpGeoBlockMiddleware>(options, getRemoteIpAddress);
        }
    }
}
