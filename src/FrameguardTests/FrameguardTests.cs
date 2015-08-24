using System;
using System.Reflection;
using Microsoft.Owin.Testing;
using NUnit.Framework;
using OnDotNet.Owin.Shield.Frameguard;

namespace OnDotNet.Owin.Shield.Tests.Frameguard
{
    [TestFixture]
    public class FrameguardTests
    {
        [Test]
        public async void VerifyDeny()
        {
            using (var server = TestServer.Create(app =>
            {
                app.Frameguard(XFrameOptions.Deny);
            }))
            {
                var response = await server.HttpClient.GetAsync("/");
                var value = string.Join("", response.Headers.GetValues("X-Frame-Options"));
                Assert.AreEqual("DENY", value);
            }
        }

        [Test]
        public async void VerifySameorigin()
        {
            using (var server = TestServer.Create(app =>
            {
                app.Frameguard(XFrameOptions.Sameorigin);
            }))
            {
                var response = await server.HttpClient.GetAsync("/");
                var value = string.Join("", response.Headers.GetValues("X-Frame-Options"));
                Assert.AreEqual("SAMEORIGIN", value);
            }
        }

        [Test]
        public async void VerifyAllowFrom()
        {
            using (var server = TestServer.Create(app =>
            {
                app.Frameguard(XFrameOptions.AllowFrom, new Uri("http://localhost/"));
            }))
            {
                var response = await server.HttpClient.GetAsync("/");
                var value = string.Join("", response.Headers.GetValues("X-Frame-Options"));
                Assert.AreEqual("ALLOW-FROM http://localhost/", value);
            }
        }

        [Test]
        [ExpectedException(typeof(TargetInvocationException))]
        public async void VerifyValidationForAllowFrom()
        {
            using (var server = TestServer.Create(app =>
            {
                app.Frameguard(XFrameOptions.AllowFrom);
            }))
            {
                await server.HttpClient.GetAsync("/");
            }
        }
    }
}
