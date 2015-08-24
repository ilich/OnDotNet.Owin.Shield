using System.Collections.Generic;

namespace OnDotNet.Owin.Shield.IpGeoBlock
{
    public class IpGeoBlockOptions
    {
        public IpGeoBlockOptions()
        {
            BlockedIpAddresses = new HashSet<string>();
            BlockedCountries = new HashSet<string>();
            AllowedCountries = new HashSet<string>();
        }

        public string GeoLite2Path { get; set; }

        public HashSet<string> BlockedIpAddresses { get; set; }
        
        public HashSet<string> BlockedCountries { get; set; }

        public HashSet<string> AllowedCountries { get; set; } 
    }
}
