﻿// ReSharper disable InconsistentNaming

using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace OnDotNet.Owin.Shield.XssFilter
{
    using UserAgentFunc = Func<IOwinContext, string>;

    public class XssFilter : OwinMiddleware
    {
        private const string XssProtection = "X-XSS-Protection";

        private readonly bool _setOnOldIE;
        private readonly UserAgentFunc _getUserAgent = c => c.Request.Headers.Get("User-Agent");

        public XssFilter(
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
                header = "1; mode=block";
            }
            else
            {
                var userAgent = _getUserAgent(context) ?? string.Empty;
                header = Regex.IsMatch(userAgent, @"msie\s*(\d+)", RegexOptions.IgnoreCase) ? "1; mode=block" : "0";
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
    }
}
