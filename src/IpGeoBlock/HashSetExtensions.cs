using System.Collections.Generic;

namespace OnDotNet.Owin.Shield.IpGeoBlock
{
    public static class HashSetExtensions
    {
        public static void AddIsoCode(this HashSet<string> set, string value)
        {
            value = value.ToUpper();
            set.Add(value);
        }
    }
}
