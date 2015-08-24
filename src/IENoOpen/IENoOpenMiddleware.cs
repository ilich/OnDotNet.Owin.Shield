// ReSharper disable InconsistentNaming

using System.Threading.Tasks;
using Microsoft.Owin;

namespace OnDotNet.Owin.Shield.IENoOpen
{
    public class IENoOpenMiddleware : OwinMiddleware
    {
        private const string XDownloadOptions = "X-Download-Options";
        private const string XDownloadOptionsValue = "noopen";

        public IENoOpenMiddleware(OwinMiddleware next) : base(next)
        {
        }

        public override Task Invoke(IOwinContext context)
        {
            var headers = context.Response.Headers;
            if (headers.ContainsKey(XDownloadOptions))
            {
                headers[XDownloadOptions] = XDownloadOptionsValue;
            }
            else
            {
                headers.Append(XDownloadOptions, XDownloadOptionsValue);
            }

            return Next.Invoke(context);
        }
    }
}
