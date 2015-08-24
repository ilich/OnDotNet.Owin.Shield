// ReSharper disable InconsistentNaming

using Microsoft.Owin.Testing;
using NUnit.Framework;
using OnDotNet.Owin.Shield.XssFilter;

namespace OnDotNet.Owin.Shield.Tests.XssFilter
{
    [TestFixture]
    public class XssFilterTests
    {
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
        public async void XssProtectionOnIE()
        {
            using (var server = TestServer.Create(app =>
            {
                app.XssFilter(false, c => "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; Trident/4.0; InfoPath.2; SV1; .NET CLR 2.0.50727; WOW64)");
            }))
            {
                var response = await server.HttpClient.GetAsync("/");
                var value = string.Join("", response.Headers.GetValues("X-XSS-Protection"));
                Assert.AreEqual("1; mode=block", value);
            }
        }

        [Test]
        public async void XssProtectionOnNonIE()
        {
            using (var server = TestServer.Create(app =>
            {
                app.XssFilter();
            }))
            {
                var response = await server.HttpClient.GetAsync("/");
                var value = string.Join("", response.Headers.GetValues("X-XSS-Protection"));
                Assert.AreEqual("0", value);
            }
        }
    }
}
