using NUnit.Framework;
using Moq;
using Hotcakes.CommerceDTO.v1;
using Hotcakes.CommerceDTO.v1.Catalog;
using Kliens.Interfaces;
using Kliens.Managers;
using System.Collections.Generic;

namespace UnitTestProject
{
    [TestFixture]
    public class OptionCreatorTests
    {
        private Mock<IApiProxy> _mockProxy;
        private OptionCreator _optionCreator;

        [SetUp]
        public void SetUp()
        {
            _mockProxy = new Mock<IApiProxy>();
            _optionCreator = new OptionCreator(_mockProxy.Object);
        }

        [Test]
        public void CreateOption_ReturnsTrue_WhenCreationSucceeds()
        {
            // Arrange
            var dummyOption = new OptionDTO { Name = "Size" };
            var response = new ApiResponse<OptionDTO> { Content = dummyOption };
            _mockProxy.Setup(p => p.ProductOptionsCreate(It.IsAny<OptionDTO>())).Returns(response);

            // Act
            var result = _optionCreator.CreateOption("Size", "Small", "Medium", "Large", "Required", "True");

            // Assert
            Assert.That(result, Is.True);
            _mockProxy.Verify(p => p.ProductOptionsCreate(It.Is<OptionDTO>(o =>
                o.Name == "Size" &&
                o.Items.Count == 3 &&
                o.Settings.Exists(s => s.Key == "Required" && s.Value == "True")
            )), Times.Once);
        }

        [Test]
        public void CreateOption_ReturnsFalse_WhenApiReturnsNullContent()
        {
            // Arrange
            var response = new ApiResponse<OptionDTO> { Content = null };
            _mockProxy.Setup(p => p.ProductOptionsCreate(It.IsAny<OptionDTO>())).Returns(response);

            // Act
            var result = _optionCreator.CreateOption("Color", "Red", "Green", "Blue", "Required", "True");

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void CreateOption_HandlesException_AndReturnsFalse()
        {
            // Arrange
            _mockProxy.Setup(p => p.ProductOptionsCreate(It.IsAny<OptionDTO>())).Throws(new System.Exception("API error"));

            // Act
            var result = _optionCreator.CreateOption("Material", "Cotton", "", "", "Required", "False");

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void CreateOption_IgnoresEmptyItem2AndItem3()
        {
            // Arrange
            var dummyOption = new OptionDTO { Name = "Material" };
            var response = new ApiResponse<OptionDTO> { Content = dummyOption };
            _mockProxy.Setup(p => p.ProductOptionsCreate(It.IsAny<OptionDTO>())).Returns(response);

            // Act
            var result = _optionCreator.CreateOption("Material", "Cotton", "", null, "Required", "True");

            // Assert
            Assert.That(result, Is.True);
            _mockProxy.Verify(p => p.ProductOptionsCreate(It.Is<OptionDTO>(o =>
                o.Items.Count == 1 &&
                o.Items[0].Name == "Cotton"
            )), Times.Once);
        }
    }
}
