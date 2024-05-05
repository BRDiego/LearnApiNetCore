using ApiNetCore.Application.Services.Interfaces;
using ApiNetCore.Business.AlertsManagement;
using ApiNetCore.Business.Interfaces;
using ApiNetCore.Business.Models;
using ApiNetCore.Data.EFContext.Repository.Interfaces;
using APINetCore.Api.Controllers.V2;
using AutoFixture;
using Moq;

namespace Tests
{
    public class BandsTests
    {
        [Fact]
        public async Task ListRequest_NoBands_EmptyResult()
        {
            //GIVEN WHEN THEN

            // Arrange
            var alerts = new Mock<IAlertManager>();
            var bandService = new Mock<IBandService>();
            var user = new Mock<IUser>();

            var bandController = new BandsController(bandService.Object, alerts.Object, user.Object);

            // Act
            var result = await bandController.List();

            var list = result.Value;

            // Assert
            Assert.Null(list);
        }
    }
}