// ReSharper disable InconsistentNaming

using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace OnDotNet.Owin.Shield.XssFilter
{
    using UserAgentFunc = Func<IOwinContext, string>;

    public class XssFilterMiddleware : OwinMiddleware
    {
        private const string XssProtection = "X-XSS-Protection";
        private const string Blocked = "1; mode=block";
        private const string Unblocked = "0";

        private readonly bool _setOnOldIE;
        private readonly UserAgentFunc _getUserAgent = c => c.Request.Headers.Get("User-Agent");

        public XssFilterMiddleware(
            OwinMiddleware next,
            bool setOnOldIE = false,
            UserAgentFunc getUserAgent = null) : base(next)
        {
            _setOnOldIE = setOnOldIE;
            if (getUserAgent != null)
            {
                _getUserAgent = getUserAgent;
            }
        }

        public override Task Invoke(IOwinContext context)
        {
            string header;

            if (_setOnOldIE)
            {
                header = Blocked;
            }
            else
            {
                header = ParseUserAgent(context);
            }

            var headers = context.Response.Headers;
            if (headers.ContainsKey(XssProtection))
            {
                headers[XssProtection] = header;
            }
            else
            {
                headers.Append(XssProtection, header);
            }

            return Next.Invoke(context);
        }

        private string ParseUserAgent(IOwinContext context)
        {
            string header;
            var userAgent = _getUserAgent(context) ?? string.Empty;
            var matches = Regex.Match(userAgent, @"msie\s*(\d+)", RegexOptions.IgnoreCase);
            if (!matches.Success)
            {
                header = Blocked;
            }
            else
            {
                int ieVersion;
                if (!int.TryParse(matches.Groups[1].Value, out ieVersion))
                {
                    header = Blocked;
                }
                else
                {
                    header = ieVersion >= 9 ? Blocked : Unblocked;
                }
            }
            return header;
        }
    }
}
