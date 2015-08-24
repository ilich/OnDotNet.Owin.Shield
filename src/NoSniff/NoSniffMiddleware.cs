using System.Threading.Tasks;
using Microsoft.Owin;

namespace OnDotNet.Owin.Shield.NoSniff
{
    public class NoSniffMiddleware : OwinMiddleware
    {
        private const string NoSniff = "X-Content-Type-Options";
        private const string NoSniffValue = "nosniff";

        public NoSniffMiddleware(OwinMiddleware next) : base(next)
        {
        }

        public override Task Invoke(IOwinContext context)
        {
            var headers = context.Response.Headers;
            if (headers.ContainsKey(NoSniff))
            {
                headers[NoSniff] = NoSniffValue;
            }
            else
            {
                headers.Append(NoSniff, NoSniffValue);
            }

            return Next.Invoke(context);
        }
    }
}
