using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Moq;
using System;
using WeatherApp.Data.Entities;
using WeatherApp.Logic.Interfaces;
using WeatherApp.Web.Controllers;
using Xunit;

namespace WeatherApp.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var manMock = new Mock<IWeatherManager>();
            var userMock = new Mock<IdentityUser>();
            var usersMock = new Mock<UserManager<ApplicationUser>>();
            var locMock = new Mock<IStringLocalizer<WeatherController>>();

            WeatherController item = new WeatherController(manMock.Object, usersMock.Object, locMock.Object);

            Assert.IsType(typeof(int), item);
        }
    }
}
