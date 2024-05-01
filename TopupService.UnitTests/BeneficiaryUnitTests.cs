using Moq;
using Xunit;
using FluentAssertions;
using TopupService.Facade.Interfaces;
using TopupService.Domain;
using TopupService.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Monad;

namespace TopupService.UnitTests
{
    public class BeneficiaryUnitTests
    {
        private readonly Mock<IBeneficiaryFacade> _beneficiaryFacadeMock = new();
        private readonly Mock<ILogger<BeneficiaryController>> _logger = new();
        private readonly Mock<IConfiguration> _configuration = new();

        private const long TestUserId = 1;

        [Fact]
        public void GetActiveBeneficiaries_ShouldReturnSuccessResponse()
        {
            // Setup
            IEnumerable<Beneficiary> ActiveBeneficiaries =
            [
                new Beneficiary() { UserId = 1, PhoneNumber = "+971521503931", NickName = "ErayDu", IsActive = true },
                new Beneficiary() { UserId = 1, PhoneNumber = "+971581203931", NickName = "ErayEtisalat", IsActive = true }
            ];

            _beneficiaryFacadeMock.Setup(x => x.GetUserActiveBeneficiaries(It.IsAny<long>()))
                .Returns(() => ActiveBeneficiaries);

            BeneficiaryController controllerMock = new(_beneficiaryFacadeMock.Object, _logger.Object, _configuration.Object);

            var actionResult = controllerMock.GetActiveBeneficiaries(TestUserId);

            // Assert
            actionResult.Should().NotBeNull("Response is always supplied");

            actionResult.Should().BeAssignableTo<ObjectResult>()
                     .Which.StatusCode.Should().Be(StatusCodes.Status200OK);

            var resultObject = TestUtils.GetObjectResultContent<IEnumerable<Beneficiary>>(actionResult);
            resultObject.Should().NotBeNullOrEmpty();
            resultObject!.Count().Should().Be(ActiveBeneficiaries.Count());
        }
    }
}
