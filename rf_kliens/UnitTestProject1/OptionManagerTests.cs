using NUnit.Framework;
using Moq;
using Hotcakes.CommerceDTO.v1;
using Hotcakes.CommerceDTO.v1.Catalog;
using Kliens.Interfaces;
using Kliens.Managers;
using proba;
using System.Collections.Generic;

namespace proba.API
{
    [TestFixture]
    public class OptionManagerTests
    {
        private Mock<IApiProxy> _mockProxy;
        private OptionManager _optionManager;

        [SetUp]
        public void SetUp()
        {
            _mockProxy = new Mock<IApiProxy>();
            _optionManager = new OptionManager(_mockProxy.Object);
        }

        [Test]
        public void LoadOptions_ReturnsListOfOptions_WhenApiReturnsContent()
        {
            // Arrange
            var mockOptions = new List<OptionDTO>
            {
                new OptionDTO { Name = "Color", Bvin = "123" },
                new OptionDTO { Name = "Size", Bvin = "456" }
            };

            _mockProxy.Setup(p => p.ProductOptionsFindAll())
                .Returns(new ApiResponse<List<OptionDTO>> { Content = mockOptions });

            // Act
            var result = _optionManager.LoadOptions();

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Name, Is.EqualTo("Color"));
            Assert.That(result[1].Bvin, Is.EqualTo("456"));
        }

        [Test]
        public void AssignOptionToProduct_ReturnsTrue_WhenAllApiCallsSucceed()
        {
            // Arrange
            string optionId = "abc";
            string productId = "xyz";

            _mockProxy.Setup(p => p.ProductOptionsAssignToProduct(optionId, productId, false))
                .Returns(new ApiResponse<bool> { Content = true });

            _mockProxy.Setup(p => p.ProductOptionsFind(optionId))
                .Returns(new ApiResponse<OptionDTO> { Content = new OptionDTO { Bvin = optionId } });

            _mockProxy.Setup(p => p.ProductOptionsUpdate(It.IsAny<OptionDTO>()))
                .Returns(new ApiResponse<OptionDTO> { Content = new OptionDTO() });

            // Act
            var result = _optionManager.AssignOptionToProduct(optionId, productId);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void DeleteOption_ReturnsFalse_WhenApiReturnsFalse()
        {
            // Arrange
            _mockProxy.Setup(p => p.ProductOptionsDelete("111"))
                .Returns(new ApiResponse<bool> { Content = false });

            // Act
            var result = _optionManager.DeleteOption("111");

            // Assert
            Assert.That(result, Is.False);
        }
    }
}
