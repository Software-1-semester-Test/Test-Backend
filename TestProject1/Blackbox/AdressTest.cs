using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Test_Backend.Services;

namespace Test.Blackbox
{
    public class AdressTest
    {
        private readonly AddressService _svc;

        public AdressTest()
        {
            _svc = new AddressService();
        }

        [Fact]
        public void StreetNumberIsBetween1And999()
        {
            var numberRe = new Regex(@"^([1-9]\d{0,2})([A-Z])?$");

            
            for (int i = 0; i < 5000; i++)
            {
                var number = _svc.RandomNumber();
                Assert.Matches(numberRe, number);
            }
        }
        [Fact]
        public void FloorIsValid()
        {
            var floorRe = new Regex(@"^(st|[1-9]\d?)$");

         
            for (int i = 0; i < 5000; i++)
            {
                var floor = _svc.RandomFloor();
                Assert.Matches(floorRe, floor);
            }
        }
        [Fact]
        public void DoorIsValid()
        {
            var doorRe = new Regex(@"^(th|mf|tv|([1-9]|[1-4]\d|50)|([a-z]-?\d{1,3}))$");

            
            for (int i = 0; i < 5000; i++)
            {
                var door = _svc.RandomDoor();
                Assert.Matches(doorRe, door);
            }
        }
       

    } 
}
