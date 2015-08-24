// ReSharper disable InconsistentNaming

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Owin.Testing;
using NUnit.Framework;
using OnDotNet.Owin.Shield.XssFilter;

namespace OnDotNet.Owin.Shield.Tests.XssFilter
{
    [TestFixture]
    public class XssFilterTests
    {
        private readonly List<string> _disabledBrowsers = new List<string>
        {
            "Mozilla/5.0 (compatible; MSIE 8.0; Windows NT 5.2; Trident/4.0; Media Center PC 4.0; SLCC1; .NET CLR 3.0.04320)",
            "Mozilla/5.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; SLCC1; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729; .NET CLR 1.1.4322)",
            "Mozilla/5.0 (Windows; U; MSIE 7.0; Windows NT 5.2)",
            "Mozilla/5.0 (compatible; MSIE 7.0; Windows NT 6.0; en-US)",
            "Mozilla/4.0 (compatible; MSIE 6.1; Windows XP)",
            "Mozilla/5.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 1.1.4325)",
            "Mozilla/5.0 (compatible; MSIE 6.0; Windows NT 5.1)"
        };

        private readonly List<string> _enabledBrowsers = new List<string>
        {
            "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; AS; rv:11.0) like Gecko",
            "Mozilla/5.0 (compatible, MSIE 11, Windows NT 6.3; Trident/7.0; rv:11.0) like Gecko",
            "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)",
            "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; Trident/6.0)",
            "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 7.1; Trident/5.0)",
            "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0; SLCC2; Media Center PC 6.0; InfoPath.3; MS-RTC LM 8; Zune 4.7)",
            "Mozilla/5.0 (Windows NT 6.3; rv:36.0) Gecko/20100101 Firefox/36.0",
            "Mozilla/5.0 (Windows NT 6.1; rv:21.0) Gecko/20130401 Firefox/21.0",
            "Mozilla/5.0(Windows; U; Windows NT 7.0; rv:1.9.2) Gecko/20100101 Firefox/3.6",
            "Mozilla/5.0 (X11; FreeBSD i686) Firefox/3.6",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_10_2) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/40.0.2214.115 Safari/537.36",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_10_2) AppleWebKit/600.3.18 (KHTML, like Gecko) Version/8.0.3 Safari/600.3.18",
            "Mozilla/5.0 (iPad; CPU OS 6_0 like Mac OS X) AppleWebKit/536.26 (KHTML, like Gecko) Version/6.0 Mobile/10A5355d Safari/8536.25",
            "Mozilla/5.0 (Linux; U; Android 4.0.3; de-ch; HTC Sensation Build/IML74K) AppleWebKit/534.30 (KHTML, like Gecko) Version/4.0 Mobile Safari/534.30",
            "Mozilla/5.0 (Linux; U; Android 2.3; en-us) AppleWebKit/999+ (KHTML, like Gecko) Safari/999.9"
        };

            [Test]
        public async void ForceXssProtection()
        {
            using (var server = TestServer.Create(app =>
            {
                app.XssFilter(true);
            }))
            {
                var response = await server.HttpClient.GetAsync("/");
                var value = string.Join("", response.Headers.GetValues("X-XSS-Protection"));
                Assert.AreEqual("1; mode=block", value);
            }
        }

        [Test]
        public async void XssProtectionInDisableBrowsers()
        {
            await AssertUserAgents(_disabledBrowsers, "0");
        }

        [Test]
        public async void XssProtectionInEnabledBrowsers()
        {
            await AssertUserAgents(_enabledBrowsers, "1; mode=block");
        }

        private async Task AssertUserAgents(IEnumerable<string> userAgents, string exprected)
        {
            foreach (var browser in userAgents)
            {
                using (var server = TestServer.Create(app => { app.XssFilter(false, c => browser); }))
                {
                    var response = await server.HttpClient.GetAsync("/");
                    var value = string.Join("", response.Headers.GetValues("X-XSS-Protection"));
                    Assert.AreEqual(exprected, value);
                }
            }
        }
    }
}
