using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Test_Backend.Models;
using Test_Backend.Services;

namespace Test.Blackbox
{
    
    public class PhonenumberTest
    {
        private static readonly string[] AllowedTokens = new[]
    {
        "2", "30", "31", "40", "41", "42", "50", "51", "52", "53",
        "60", "61", "71", "81", "91", "92", "93", "342",
        "344-349", "356-357", "359", "362", "365-366", "389", "398",
        "431", "441", "462", "466", "468", "472", "474", "476", "478",
        "485-486", "488-489", "493-496", "498-499", "542-543", "545",
        "551-552", "556", "571-574", "577", "579", "584", "586-587",
        "589", "597-598", "627", "629", "641", "649", "658",
        "662-665", "667", "692-694", "697", "771-772", "782-783",
        "785-786", "788-789", "826-827", "829"
    };

        private static readonly HashSet<string> AllowedPrefixes = ExpandTokens(AllowedTokens);

        [Fact]
        public void RandomPhone_Always_8Digits_And_ValidPrefix()
        {
            var svc = new PhoneNumberService(); 
            var digitRe = new Regex(@"^\d{8}$");

            
            const int N = 10000;

            for (int i = 0; i < N; i++)
            {
                PhoneNumber p = svc.GetRandomPhoneNumber();

                Assert.NotNull(p);
                Assert.False(string.IsNullOrWhiteSpace(p.Number), "Number is null/empty.");

                // Rule 1: exactly 8 digits
                Assert.Matches(digitRe, p.Number);

                // Rule 2: must start with one of the allowed prefixes
                Assert.True(
                    AllowedPrefixes.Any(pref => p.Number.StartsWith(pref, StringComparison.Ordinal)),
                    $"Generated number '{p.Number}' does not start with an allowed prefix."
                );
            }
        }

        [Fact]
        public void RandomPhone_Uses_Multiple_Prefixes_Eventually()
        {
            var svc = new PhoneNumberService();
            var seen = new HashSet<string>();

            const int N = 5000;
            for (int i = 0; i < N; i++)
            {
                var n = svc.GetRandomPhoneNumber().Number;
                var pref = LongestMatchingAllowedPrefix(n);
                if (pref != null) seen.Add(pref);
                if (seen.Count >= 5) break; 
            }

            Assert.True(seen.Count >= 5,
                $"Expected to observe multiple different prefixes, but only saw: {string.Join(", ", seen)}");
        }

       

        private static HashSet<string> ExpandTokens(IEnumerable<string> tokens)
        {
            var set = new HashSet<string>(StringComparer.Ordinal);

            foreach (var t in tokens)
            {
                if (t.Contains('-'))
                {
                    var parts = t.Split('-', 2, StringSplitOptions.TrimEntries);
                    int start = int.Parse(parts[0]);
                    int end = int.Parse(parts[1]);
                    for (int x = start; x <= end; x++)
                        set.Add(x.ToString());
                }
                else
                {
                    set.Add(t);
                }
            }

            return set;
        }

        private static string? LongestMatchingAllowedPrefix(string number)
        {
            
            return AllowedPrefixes
                .Where(p => number.StartsWith(p, StringComparison.Ordinal))
                .OrderByDescending(p => p.Length)
                .FirstOrDefault();
        }
    }
}

