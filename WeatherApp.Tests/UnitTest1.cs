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
        private Mock<UserManager<ApplicationUser>> GetMockUserManager()
        {
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            return new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);
        }

        [Fact]
        public void Test1()
        {
            var manMock = new Mock<IWeatherManager>();
            var userMock = new Mock<IdentityUser>();
            var usersMock = GetMockUserManager();
            var locMock = new Mock<IStringLocalizer<WeatherController>>();

            WeatherController item = new WeatherController(manMock.Object, usersMock.Object, locMock.Object);

            Assert.IsType(typeof(WeatherController), item);
        }
    }
}
