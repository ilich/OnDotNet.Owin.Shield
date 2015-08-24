using Owin;

namespace OnDotNet.Owin.Shield.NoSniff
{
    public static class NoSniffExtensions
    {
        public static void NoSniff(this IAppBuilder app)
        {
            app.Use<NoSniffMiddleware>();
        }
    }
}
