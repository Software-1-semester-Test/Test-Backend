using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Test_Backend.Services;

namespace Test
{
    public class AdressTest
    {
        private readonly AddressService _svc;
        private readonly string _connectionString;
        public AdressTest()
        {
            // Requires a real MySQL with postal_code table or a test connection string.
            // If you don't have DB in CI, skip postal code assertions (see comments below).
            var cfg = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ConnectionStrings:DefaultConnection"] = "server=localhost;uid=test;pwd=test;database=addresses_test"
                })
                .Build();

            _svc = new AddressService(cfg);

        }
        [Fact]
        public void ManySamples_Respect_Format_Rules()
        {
            var streetRe = new Regex(@"^[A-Za-z]{5,11}vej$");
            var numberRe = new Regex(@"^([1-9]\d{0,2})([A-Z])?$");
            var floorRe = new Regex(@"^(st|[1-9]\d?)$");
            var doorRe = new Regex(@"^(th|mf|tv|([1-9]|[1-4]\d|50)|([a-z]-?\d{1,3}))$");

            // Try enough samples to probabilistically cover branches.
            for (int i = 0; i < 5000; i++)
            {
                var a = _svc.GetRandomAddress();

                Assert.Matches(streetRe, a.Street);
                Assert.Matches(numberRe, a.Number);
                Assert.Matches(floorRe, a.Floor);
                Assert.True(a.Floor != "100", "Floor should never be 100.");

                Assert.Matches(doorRe, a.Door);

                // If DB is present, basic sanity checks:
                Assert.False(string.IsNullOrWhiteSpace(a.PostalCode));
                Assert.False(string.IsNullOrWhiteSpace(a.Town));
                // Optionally: Danish postal codes are typically 4 digits:
                Assert.Matches(@"^\d{4}$", a.PostalCode);
            }
        }
    }
}
