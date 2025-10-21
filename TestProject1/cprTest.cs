using System.Globalization;
using System.Text.RegularExpressions;
using Test_Backend.Services;

namespace TestProject1
{
    public class cprTest
    {

        [InlineData("12025678", false)]
        [InlineData("120256789", false)]
        [InlineData("1202567890", true)]
        [InlineData("12025678901", false)]
        [InlineData("120256789012", false)]

        [Theory]
        public void BoundaryTest(string cpr,bool expected)
        {
            CprService service = new CprService();

            var result = service.ValidateCprWithDateOfBirth(cpr, new DateTime(1956, 2, 12));

            Assert.Equal(expected, result);
        }

        [InlineData("1985-12-25", "Male",1)]
        [InlineData("1985-12-25", "Female", 0)]


        [Theory]
        public void IsRigthGender(string stringdate, string gender, int expected)
        {
            var date = DateTime.Parse(stringdate, null, System.Globalization.DateTimeStyles.AssumeUniversal);
            CprService service = new CprService();

            var result = service.GenerateCpr(date, gender);

            Assert.Equal(expected, result.Number[9] % 2);
        }



        [InlineData("2512017860", "2001-12-25", true)]
        [InlineData("0212567890", "1985-12-25", false)]
        [InlineData("2512857890", "1985-12-25", true)]
        [Theory]
        public void DateofbirthmatchCpr(string cpr, string date, bool expected)
        {
            var datetime = DateTime.Parse(date, null, System.Globalization.DateTimeStyles.AssumeUniversal);
            CprService service = new CprService();

            var result = service.ValidateCprWithDateOfBirth(cpr, datetime);

            Assert.Equal(expected, result);
        }
    }
}
