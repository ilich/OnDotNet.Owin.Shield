using System.Net;
using Microsoft.Owin.Testing;
using NUnit.Framework;
using OnDotNet.Owin.Shield.IpGeoBlock;
using Owin;

namespace OnDotNet.Owin.Shield.Tests.IpGeoBlock
{
    [TestFixture]
    public class IpGeoBlockTests
    {
        [Test]
        public async void BlockIpAddress()
        {
            using (var server = TestServer.Create(app =>
            {
                var options = TestData.GetConfiguration();
                app.IpGeoBlock(options, c => TestData.BlockedIpAddress);
            }))
            {
                var response = await server.HttpClient.GetAsync("/");
                var content = await response.Content.ReadAsStringAsync();
                Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
                Assert.AreEqual(TestData.Forbidden, content);
            }
        }

        [Test]
        public async void BlockFrance()
        {
            using (var server = TestServer.Create(app =>
            {
                var options = TestData.GetConfiguration();
                options.BlockedCountries.AddIsoCode(TestData.Fr);
                app.IpGeoBlock(options, c => TestData.FrIpAddress);
            }))
            {
                var response = await server.HttpClient.GetAsync("/");
                var content = await response.Content.ReadAsStringAsync();
                Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
                Assert.AreEqual(TestData.Forbidden, content);
            }
        }

        [Test]
        public async void BlockGreeceAndAllowOnlyFrance()
        {
            using (var server = TestServer.Create(app =>
            {
                var options = TestData.AllowOnlyFrance();
                app.IpGeoBlock(options, c => TestData.GrIpAddress);
            }))
            {
                var response = await server.HttpClient.GetAsync("/");
                var content = await response.Content.ReadAsStringAsync();
                Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
                Assert.AreEqual(TestData.Forbidden, content);
            }
        }

        [Test]
        public async void BlockNotFoundIpAndAllowOnlyFrance()
        {
            using (var server = TestServer.Create(app =>
            {
                var options = TestData.AllowOnlyFrance();
                app.IpGeoBlock(options, c => TestData.AllowedIpAddress);
            }))
            {
                var response = await server.HttpClient.GetAsync("/");
                var content = await response.Content.ReadAsStringAsync();
                Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
                Assert.AreEqual(TestData.Forbidden, content);
            }
        }

        [Test]
        public async void AllowOnlyFrance()
        {
            using (var server = TestServer.Create(app =>
            {
                var options = TestData.AllowOnlyFrance();
                app.IpGeoBlock(options, c => TestData.FrIpAddress);
                app.Run(c => c.Response.WriteAsync(TestData.Ok));
            }))
            {
                var response = await server.HttpClient.GetAsync("/");
                var content = await response.Content.ReadAsStringAsync();
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                Assert.AreEqual(TestData.Ok, content);
            }
        }

        [Test]
        public async void AllowedIpAddress()
        {
            using (var server = TestServer.Create(app =>
            {
                var options = TestData.GetConfiguration();
                app.IpGeoBlock(options, c => TestData.AllowedIpAddress);
                app.Run(c => c.Response.WriteAsync(TestData.Ok));
            }))
            {
                var response = await server.HttpClient.GetAsync("/");
                var content = await response.Content.ReadAsStringAsync();
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                Assert.AreEqual(TestData.Ok, content);
            }
        }
    }
}
