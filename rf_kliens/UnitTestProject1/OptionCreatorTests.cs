using NUnit.Framework;
using Hotcakes.CommerceDTO.v1;
using Hotcakes.CommerceDTO.v1.Catalog;
using Moq;
using Moq.Protected;
using Hotcakes.CommerceDTO.v1.Client;
using YourNamespace.Managers;

namespace UniTestProject1.API
{
    [TestFixture]
    public class OptionCreatorTests
    {
        private Api _dummyApi;
        private Mock<OptionCreator> _mockCreator;

        [SetUp]
        public void SetUp()
        {
            _dummyApi = new Api("https://fake", "KEY");
            _mockCreator = new Mock<OptionCreator>() { CallBase = true };
        }

        [Test]
        public void CreateOption_ReturnsTrue_WhenSuccessful()
        {
            // Arrange
            var mock = new Mock<OptionCreator>("https://fakeurl", "APIKEY") { CallBase = true };

            // Mivel a metódus nem ad vissza konkrét eredményt, csak boolt,
            // itt nincs mit mockolni külön, ha nem használunk partial mockot

            // Act
            var result = mock.Object.CreateOption(
                "Size",
                "Small",
                "Medium",
                "Large",
                "Required",
                "True"
            );

            // Assert
            Assert.That(result, Is.True); // vagy .Is.False ha hibát szimulálsz
        }

    }
}
