using System;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace OnDotNet.Owin.Shield.Frameguard
{
    public class FrameguardMiddleware : OwinMiddleware
    {
        private readonly XFrameOptions _xFrameOptions;
        private readonly Uri _uri;

        public FrameguardMiddleware(
            OwinMiddleware next,
            XFrameOptions xFrameOptions, 
            Uri uri = null) : base(next)
        {
            if (xFrameOptions == XFrameOptions.AllowFrom && uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            _xFrameOptions = xFrameOptions;
            _uri = uri;
        }

        public override Task Invoke(IOwinContext context)
        {
            string header;
            switch (_xFrameOptions)
            {
                case XFrameOptions.Deny:
                    header = XFrameOptionsConstants.Deny;
                    break;

                case XFrameOptions.Sameorigin:
                    header = XFrameOptionsConstants.Sameorigin;
                    break;

                case XFrameOptions.AllowFrom:
                    header = string.Format(XFrameOptionsConstants.AllowFrom, _uri);
                    break;

                default:
                    header = XFrameOptionsConstants.Sameorigin;
                    break;
            }

            var headers = context.Response.Headers;
            if (headers.ContainsKey(XFrameOptionsConstants.Header))
            {
                headers[XFrameOptionsConstants.Header] = header;
            }
            else
            {
                headers.Append(XFrameOptionsConstants.Header, header);
            }

            return Next.Invoke(context);
        }
    }
}
