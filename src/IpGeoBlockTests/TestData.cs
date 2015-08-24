using OnDotNet.Owin.Shield.IpGeoBlock;

namespace OnDotNet.Owin.Shield.Tests.IpGeoBlock
{
    static class TestData
    {
        public static readonly string AllowedIpAddress = "192.168.0.2";

        public static readonly string BlockedIpAddress = "192.168.0.1";

        public static readonly string Fr = "Fr";

        public static readonly string FrIpAddress = "213.128.63.255";

        public static readonly string GrIpAddress = "46.246.128.0";

        public static readonly string Forbidden = "Forbidden";

        public static readonly string Ok = "OK";

        public static IpGeoBlockOptions GetConfiguration()
        {
            var options = new IpGeoBlockOptions();
            options.BlockedIpAddresses.Add(BlockedIpAddress);
            options.GeoLite2Path = "GeoLite2-Country.mmdb";
            return options;
        }

        public static IpGeoBlockOptions AllowOnlyFrance()
        {
            var options = GetConfiguration();
            options.AllowedCountries.AddIsoCode(Fr);
            return options;
        }
    }
}
