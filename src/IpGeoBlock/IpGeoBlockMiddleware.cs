using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using MaxMind.GeoIP2;
using MaxMind.GeoIP2.Exceptions;
using MaxMind.GeoIP2.Responses;
using Microsoft.Owin;

namespace OnDotNet.Owin.Shield.IpGeoBlock
{
    using AppFunc = Func<IDictionary<string, object>, Task>;
    using RemoteIdAddressFunc = Func<IOwinContext, string>;

    public class IpGeoBlockMiddleware : IDisposable
    {
        private readonly AppFunc _next;
        private readonly IpGeoBlockOptions _options;
        private readonly RemoteIdAddressFunc _getRemoteIpAddress = c => c.Request.RemoteIpAddress;
        private readonly DatabaseReader _ipDbReader;

        public IpGeoBlockMiddleware(
            AppFunc next, 
            IpGeoBlockOptions options,
            RemoteIdAddressFunc getRemoteIpAddress = null)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            // Check if MaxMind GeoLite2 database is available
            if (string.IsNullOrEmpty(options.GeoLite2Path))
            {
                throw new ArgumentNullException(nameof(options.GeoLite2Path));
            }

            if (!File.Exists(options.GeoLite2Path))
            {
                throw new FileNotFoundException($"MaxMind GeoLit2 Country database is not found in {options.GeoLite2Path}. Please download the database from https://dev.maxmind.com/geoip/geoip2/geolite2/.");
            }

            // Check that user added only denied countries or only allowed countries
            if (options.AllowedCountries.Count > 0 && options.BlockedCountries.Count > 0)
            {
                throw new InvalidOperationException("You have to choose only allowed contries or only blocked countries.");
            }

            _next = next;
            _options = options;
            _ipDbReader = new DatabaseReader(_options.GeoLite2Path);

            if (getRemoteIpAddress != null)
            {
                _getRemoteIpAddress = getRemoteIpAddress;
            }
        }

        public Task Invoke(IDictionary<string, object> environment)
        {
            var owinContext = new OwinContext(environment);
            var ipAddress = _getRemoteIpAddress(owinContext);
            if (IsBlocked(ipAddress))
            {
                owinContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return owinContext.Response.WriteAsync("Forbidden");
            }

            return _next(environment);
        }

        private bool IsBlocked(string ipAddress)
        {
            // 1. Check that IP address is blocked
            var isBlocked = _options.BlockedIpAddresses.Contains(ipAddress);
            if (isBlocked)
            {
                return true;
            }

            CountryResponse country;
            try
            {
                country = _ipDbReader.Country(ipAddress);
            }
            catch (AddressNotFoundException)
            {
                country = null;
            }
            
            if (_options.BlockedCountries.Count > 0)
            {
                // 2. If user added country to Blocked Countries collection then only those countries 
                // are blocked 

                isBlocked = country != null && _options.BlockedCountries.Contains(country.Country.IsoCode);
            }
            else if (_options.AllowedCountries.Count > 0)
            {
                // 3. If user added country to Allowed Countries collecction then all countries except allowed
                // are blocked

                isBlocked = country == null || !_options.AllowedCountries.Contains(country.Country.IsoCode);
            }

            return isBlocked;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _ipDbReader?.Dispose();
            }
        }
    }
}
    