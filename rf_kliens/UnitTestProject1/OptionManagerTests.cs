using Moq;
using Moq.Protected;
using NUnit.Framework;
using Hotcakes.CommerceDTO.v1;
using Hotcakes.CommerceDTO.v1.Catalog;
using Hotcakes.CommerceDTO.v1.Client;
using System.Collections.Generic;
using YourNamespace.Managers;

namespace UnitTestProject1
{
    [TestFixture]
    public class OptionManagerTests
    {
        private string _url = "https://fake";
        private string _key = "KEY";
        private Mock<Api> _mockApi;
        private OptionManager _manager;

        [SetUp]
        public void Setup()
        {
            _mockApi = new Mock<Api>(_url, _key);
            _manager = new OptionManager(_url, _key);
        }

        [Test]
        public void AssignOptionToProduct_ReturnsTrue_OnSuccess()
        {
            var response = new ApiResponse<bool> { Content = true };
            _mockApi.Setup(api => api.ProductOptionsAssignToProduct("opt123", "prod123", false))
                    .Returns(response);

            var result = _manager.AssignOptionToProduct("opt123", "prod123");
            Assert.That(result, Is.True);
        }

        [Test]
        public void AssignOptionToProduct_ReturnsFalse_OnFailure()
        {
            var response = new ApiResponse<bool> { Content = false };
            _mockApi.Setup(api => api.ProductOptionsAssignToProduct("opt123", "prod123", false))
                    .Returns(response);

            var result = _manager.AssignOptionToProduct("opt123", "prod123");
            Assert.That(result, Is.False);
        }

        [Test]
        public void UnassignOptionFromProduct_ReturnsTrue_OnSuccess()
        {
            var response = new ApiResponse<bool> { Content = true };
            _mockApi.Setup(api => api.ProductOptionsUnassignFromProduct("opt123", "prod123"))
                    .Returns(response);

            var result = _manager.UnassignOptionFromProduct("opt123", "prod123");
            Assert.That(result, Is.True);
        }

        [Test]
        public void DeleteOption_ReturnsTrue_OnSuccess()
        {
            var response = new ApiResponse<bool> { Content = true };
            _mockApi.Setup(api => api.ProductOptionsDelete("opt123")).Returns(response);

            var result = _manager.DeleteOption("opt123");
            Assert.That(result, Is.True);
        }
    }
}
