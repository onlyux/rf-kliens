using NUnit.Framework;
using Moq;
using proba.API;
using proba;
using Hotcakes.CommerceDTO.v1;
using Hotcakes.CommerceDTO.v1.Catalog;
using System.Collections.Generic;
using System.Linq;
using Kliens.Interfaces;
using Kliens.Managers;

namespace proba.API
{
    [TestFixture]
    public class ProductManagerTests
    {
        private Mock<IApiProxy> _apiProxyMock;
        private IProductManager _productManager;

        [SetUp]
        public void Setup()
        {
            _apiProxyMock = new Mock<IApiProxy>();
            _productManager = new ProductManager(_apiProxyMock.Object);
        }

        [Test]
        public void LoadProducts_ReturnsMappedProducts_WhenApiReturnsContent()
        {
            // Arrange
            var apiResponse = new ApiResponse<List<ProductDTO>>
            {
                Content = new List<ProductDTO>
                {
                    new ProductDTO { Bvin = "1", Sku = "SKU1", ProductName = "Product 1", SitePrice = 100 },
                    new ProductDTO { Bvin = "2", Sku = "SKU2", ProductName = "Product 2", SitePrice = 200 }
                }
            };

            _apiProxyMock.Setup(proxy => proxy.ProductsFindAll()).Returns(apiResponse);

            // Act
            var result = _productManager.LoadProducts();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Bvin, Is.EqualTo("1"));
            Assert.That(result[1].ProductName, Is.EqualTo("Product 2"));
        }

        [Test]
        public void LoadProducts_ShowsWarning_WhenContentIsNull()
        {
            // Arrange
            var apiResponse = new ApiResponse<List<ProductDTO>> { Content = null };
            _apiProxyMock.Setup(proxy => proxy.ProductsFindAll()).Returns(apiResponse);

            // Act
            var result = _productManager.LoadProducts();

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void LoadProducts_ThrowsException_WhenApiFails()
        {
            // Arrange
            _apiProxyMock.Setup(proxy => proxy.ProductsFindAll()).Throws(new System.Exception("API error"));

            // Act & Assert
            Assert.Throws<System.Exception>(() => _productManager.LoadProducts());
        }
    }
}
