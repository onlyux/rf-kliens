using NUnit.Framework;
using Moq;
using proba;
using Hotcakes.CommerceDTO.v1;
using Hotcakes.CommerceDTO.v1.Catalog;
using System.Collections.Generic;
using static proba.Form1;

namespace probaTeszt
{
    public class TermekSzolgaltatasTeszt
    {
        [Test]
        public void LetrehozValasztek_HelyesAdatokkal_Sikeres()
        {
            // Arrange
            var valasz = new ApiResponse<OptionDTO>
            {
                Content = new OptionDTO { Name = "Szín" }
            };

            var kliensMock = new Mock<ITermekKliens>();
            kliensMock
                .Setup(k => k.LetrehozValasztek(It.IsAny<OptionDTO>()))
                .Returns(valasz);

            var szolgaltatas = new TermekSzolgaltatas(kliensMock.Object);
            var opciok = new List<string> { "Piros", "Kék", "Zöld" };

            // Act
            var sikeres = szolgaltatas.LetrehozValasztek("Szín", opciok, "Max", "3");

            // Assert
            Assert.IsTrue(sikeres);
        }

        [Test]
        public void LetrehozValasztek_HibasValasz_NemSikeres()
        {
            // Arrange
            var valasz = new ApiResponse<OptionDTO>
            {
                Content = null // API hibás válasz
            };

            var kliensMock = new Mock<ITermekKliens>();
            kliensMock
                .Setup(k => k.LetrehozValasztek(It.IsAny<OptionDTO>()))
                .Returns(valasz);

            var szolgaltatas = new TermekSzolgaltatas(kliensMock.Object);
            var opciok = new List<string> { "Piros", "Kék" };

            // Act
            var sikeres = szolgaltatas.LetrehozValasztek("Szín", opciok, "Max", "3");

            // Assert
            Assert.IsFalse(sikeres);
        }
    }
}
