using Microsoft.Owin.Testing;
using NUnit.Framework;
using OnDotNet.Owin.Shield.NoSniff;

namespace OnDotNet.Owin.Shield.Tests.NoSniff
{
    [TestFixture]
    public class NoSniffTests
    {
        [Test]
        public async void VerifyNoSniff()
        {
            using (var server = TestServer.Create(app =>
            {
                app.NoSniff();
            }))
            {
                var response = await server.HttpClient.GetAsync("/");
                var value = string.Join("", response.Headers.GetValues("X-Content-Type-Options"));
                Assert.AreEqual("nosniff", value);
            }
        }
    }
}
