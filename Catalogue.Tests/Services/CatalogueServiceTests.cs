using System.Collections.Generic;
using System.Linq;
using Catalogue.Model;
using Catalogue.Services;
using Catalogue.Tests.Mocks;
using Xunit;
using Moq;
using Moq.AutoMock;

namespace Catalogue.Tests
{
    public class CatalogueServiceTests
    {   
        private readonly AutoMocker _mocker;
        private readonly ICatalogueService _service;
        
        public CatalogueServiceTests()
        {
            _mocker = new AutoMocker();
            _service = _mocker.CreateInstance<CatalogueService>();
        }

        [Fact]
        public void Merge_ShouldNot_WriteToOutputFolder_IfInputFolderDoesNotExists()
        {
            // Act
            _service.Merge(string.Empty);

            // Assert
            _mocker.GetMock<IUtilityService>()
                .Verify(u => u.GetOutputFolder(), Times.Never);
        }

        [Theory, ClassData(typeof(CatalogueMockData))]
        public void Merge_Should_WriteToOutputFolder(
            List<ProductCatalogue> aCatalogues, 
            List<ProductCatalogue> bCatalogues,
            List<ProductBarcode> aBarcodes,
            List<ProductBarcode> bBarcodes,
            List<MergedCatalogue> expectedMergedCatalogues
        )
        {
            // Arrange
            _mocker.GetMock<ICsvService<ProductCatalogue>>()
                .Setup(s => s.Read(It.IsAny<string>(), Constants.CatalogueAFileName))
                .Returns(aCatalogues);

            _mocker.GetMock<ICsvService<ProductCatalogue>>()
                .Setup(s => s.Read(It.IsAny<string>(), Constants.CatalogueBFileName))
                .Returns(bCatalogues);

            _mocker.GetMock<ICsvService<ProductBarcode>>()
                .Setup(s => s.Read(It.IsAny<string>(), Constants.BarcodeAFileName))
                .Returns(aBarcodes);

            _mocker.GetMock<ICsvService<ProductBarcode>>()
                .Setup(s => s.Read(It.IsAny<string>(), Constants.BarcodeBFileName))
                .Returns(bBarcodes);

            _mocker.GetMock<IFileService>()
                .Setup(s => s.IsDirectoryExists(It.IsAny<string>()))
                .Returns(true);

            var convertedACatalogues = aCatalogues.Select(c => new MergedCatalogue()
                {SKU = c.SKU, Description = c.Description, Source = "A"}).ToList();
            _mocker.GetMock<IUtilityService>()
                .Setup(s => s.ConvertProductToMergedCatalogue(It.IsAny<List<ProductCatalogue>>(), It.IsAny<string>()))
                .Returns(convertedACatalogues);
            _mocker.GetMock<IUtilityService>()
                .Setup(s => s.GetOutputFolder()).Returns("outputFolder");

            // Act
            _service.Merge(string.Empty);

            // Assert
            _mocker.GetMock<IUtilityService>()
                .Verify(u => u.GetOutputFolder(), Times.Once);

            _mocker.GetMock<ICsvService<MergedCatalogue>>()
                .Verify(u => u.Write(It.IsAny<string>(), It.IsAny<string>(),
                    It.Is<List<MergedCatalogue>>(args => CompareEquality(args, expectedMergedCatalogues))), Times.Once);
        }

        private static bool CompareEquality(IReadOnlyCollection<MergedCatalogue> A, IReadOnlyCollection<MergedCatalogue> B)
        {
            var result = (A.Count() == B.Count() && 
                         (!A.Except(B).Any() || !B.Except(A).Any())
                   );
            return result;
        }
    }
}
