using NUnit.Framework;
using Moq;
using proba; // A fő projekt névtere
using HotcakesCommerce;

namespace HotcakesCommerce.Tests
{
    [TestFixture]
    public class Form1Tests
    {
        // A. Teszt a termékbetöltésre
        [Test]
        public void TermekBetolt_SikeresApiValasz_MegjelenitiTermekeket()
        {
            // Arrange
            var mockApi = new Mock<Api>(url, key);
            mockApi.Setup(api => api.ProductsFindAll())
                   .Returns(new ApiResponse<List<ProductDTO>>
                   {
                       Content = new List<ProductDTO>
                       {
                   new ProductDTO { Bvin = "1", ProductName = "Termék 1" }
                       }
                   });

            var form = new Form1(mockApi.Object); // Dependency Injection szükséges a Form1-be!

            // Act
            form.TermekBetolt();

            // Assert
            Assert.AreEqual(1, form.listBox1.Items.Count);
            Assert.AreEqual("Termék 1", form.listBox1.Items[0].ToString());
        }

        // B. Teszt az opcióbetöltésre
        [Test]
        public void OpcioBetolt_SikeresApiValasz_MegjelenitiOpciokat()
        {
            // Arrange
            var mockApi = new Mock<Api>(url, key);
            mockApi.Setup(api => api.ProductOptionsFindAll())
                   .Returns(new ApiResponse<List<OptionDTO>>
                   {
                       Content = new List<OptionDTO>
                       {
                   new OptionDTO { Name = "Szín" }
                       }
                   });

            var form = new Form1(mockApi.Object);

            // Act
            form.OpcioBetolt();

            // Assert
            Assert.AreEqual(1, form.listBox2.Items.Count);
            Assert.AreEqual("Szín", form.listBox2.Items[0].ToString());
        }

        [Test]
        public void Listazas_KivalasztottTermek_NemNullApiValasz_DataGridViewFrissul()
        {
            // Arrange
            var mockApi = new Mock<Api>(url, key);
            mockApi.Setup(api => api.ProductOptionsFindAllByProductId(It.IsAny<string>()))
                   .Returns(new ApiResponse<List<OptionDTO>>
                   {
                       Content = new List<OptionDTO>
                       {
                   new OptionDTO { Name = "Méret", Bvin = "2" }
                       }
                   });

            var form = new Form1(mockApi.Object);
            form.listBox1.SelectedItem = new Termekek { Bvin = "1" }; // Tesztadatok

            // Act
            form.Listazas();

            // Assert
            Assert.AreEqual(1, form.dataGridView1.Rows.Count);
            Assert.AreEqual("Méret", form.dataGridView1.Rows[0].Cells["Name"].Value);
        }

        [Test]
        public void Create_ValidInput_ApiHivasSikeres()
        {
            // Arrange
            var mockApi = new Mock<Api>(url, key);
            mockApi.Setup(api => api.ProductOptionsCreate(It.IsAny<OptionDTO>()))
                   .Returns(new ApiResponse<OptionDTO> { Content = new OptionDTO() });

            var form = new Form1(mockApi.Object);
            form.textBox1.Text = "Szín"; // Kötelező mezők kitöltése
            form.textBox2.Text = "Piros";
            form.textBox5.Text = "Ár";
            form.textBox6.Text = "1000";

            // Act
            form.Create();

            // Assert
            mockApi.Verify(api => api.ProductOptionsCreate(It.IsAny<OptionDTO>()), Times.Once);
        }

        [Test]
        public void TextBox6_TextChanged_NonNumericInput_RejectsInput()
        {
            // Arrange
            var form = new Form1();
            form.textBox6.Text = "abc"; // Érvénytelen bemenet

            // Act
            form.textBox6_TextChanged(null, EventArgs.Empty);

            // Assert
            Assert.AreEqual("", form.textBox6.Text); // Visszaáll üresre vagy az előző érvényes értékre
        }
    }
}