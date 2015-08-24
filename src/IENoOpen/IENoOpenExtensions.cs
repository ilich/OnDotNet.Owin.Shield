// ReSharper disable InconsistentNaming

using Owin;

namespace OnDotNet.Owin.Shield.IENoOpen
{
    public static class IENoOpenExtensions
    {
        public static void IENoOpen(this IAppBuilder app)
        {
            app.Use<IENoOpenMiddleware>();
        }
    }
}
