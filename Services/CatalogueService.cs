using System.Collections.Generic;
using System.Linq;
using Catalogue.Model;
using Catalogue.Comparer;
using Microsoft.Extensions.Logging;

namespace Catalogue.Services
{
    /// <summary>
    /// Service to sort the products from company A and B
    /// </summary>
    public interface ICatalogueService
    {
        /// <summary>
        /// 1. Will copy all products from company A to final MergedCatalogue list
        /// 2. Parse Company A and B barcodes and will add only products which are unique to company B to final MergedCatalogues list
        /// 3. Write MergedCatalogues list to output csv file
        /// </summary>
        /// <param name="inputFolder"></param>
        void Merge(string inputFolder);
    }

    /// <summary>
    /// Service to sort the products from company A and B
    /// </summary>
    public class CatalogueService : ICatalogueService
    {
        private readonly ILogger<CatalogueService> _logger;
        private readonly ICsvService<ProductCatalogue> _productCatalogueCsvService;
        private readonly ICsvService<ProductBarcode> _productBarcodeCsvService;
        private readonly ICsvService<MergedCatalogue> _mergedCatalogueCsvService;
        private readonly IUtilityService _utilityService;
        private readonly IFileService _fileService;

        public CatalogueService(
            ICsvService<ProductCatalogue> productCatalogueCsvService,
            ICsvService<ProductBarcode> productBarcodeCsvService,
            ICsvService<MergedCatalogue> mergedCatalogueCsvService,
            IUtilityService utilityService,
            IFileService fileService,
            ILogger<CatalogueService> logger)
        {
            _logger = logger;
            _fileService = fileService;
            _productCatalogueCsvService = productCatalogueCsvService;
            _productBarcodeCsvService = productBarcodeCsvService;
            _mergedCatalogueCsvService = mergedCatalogueCsvService;
            _utilityService = utilityService;
        }

        /// <summary>
        /// 1. Will copy all products from company A to final MergedCatalogue list
        /// 2. Parse Company A and B barcodes and will add only products which are unique to company B to final MergedCatalogues list
        /// 3. Write MergedCatalogues list to output csv file
        /// </summary>
        /// <param name="inputFolder"></param>
        public void Merge(string inputFolder)
        {
            if (!_fileService.IsDirectoryExists(inputFolder))
            {
                _logger.LogError("Input folder: {1} does not exists", inputFolder);
                return;
            }

            var mergedCatalogues = new List<MergedCatalogue>();

            CopySupplierACataloguesToMergedCatalogues(inputFolder, mergedCatalogues);

            AddSupplierBUniqueProductsToMergedCatalogues(inputFolder, mergedCatalogues);

            WriteToOutputFile(mergedCatalogues);
        }

        /// <summary>
        /// This will copy all products from supplier A to final merge catalogue list
        /// </summary>
        /// <param name="inputFolder"></param>
        /// <param name="mergedCatalogues"></param>
        private void CopySupplierACataloguesToMergedCatalogues(string inputFolder, List<MergedCatalogue> mergedCatalogues)
        {   
            var aCatalogues = _productCatalogueCsvService.Read(inputFolder, Constants.CatalogueAFileName);
            mergedCatalogues.AddRange(_utilityService.ConvertProductToMergedCatalogue(aCatalogues, "A"));
            _logger.LogInformation("Company A products copied to final merged catalogue lists");
        }

        /// <summary>
        /// 1. Parses barcodes for both supplier A and B, will return only  barcodes which are in B not in A
        /// 2. Merge unique supplier B products to mergedCatalogue
        /// 3. order final list of catalogues i.e mergedCatalogues by description
        /// </summary>
        /// <param name="inputFolder"></param>
        /// <returns></returns>
        private void AddSupplierBUniqueProductsToMergedCatalogues(string inputFolder, List<MergedCatalogue> mergedCatalogues)
        {
            var bCatalogues = _productCatalogueCsvService.Read(inputFolder, Constants.CatalogueBFileName);
            var aBarcodes = _productBarcodeCsvService.Read(inputFolder, Constants.BarcodeAFileName);
            var bBarcodes = _productBarcodeCsvService.Read(inputFolder, Constants.BarcodeBFileName); 
            var uniqueBBarcodes = bBarcodes.Except(aBarcodes, new ProductBarcodeComparer()).ToList();

            if (uniqueBBarcodes.Count <= 0) return;
            var uniqueToBProducts = uniqueBBarcodes.GroupBy(
                p => p.SKU).Select(p => new { SKU = p.Key });
            mergedCatalogues.AddRange(uniqueToBProducts
                .Select(product => new MergedCatalogue()
                {
                    SKU = product.SKU,
                    Description = bCatalogues.Where(c => c.SKU == product.SKU).Select(c => c.Description).FirstOrDefault(),
                    Source = "B"
                }));
            _logger.LogInformation("Company B unique products added to final merged catalogue lists");
        }

        /// <summary>
        /// Will create new csv file in the output folder, name of csv file is configurable in Constants
        /// </summary>
        /// <param name="mergedCatalogues"></param>
        private void WriteToOutputFile(List<MergedCatalogue> mergedCatalogues)
        {
            var outputFolder = _utilityService.GetOutputFolder();
            _mergedCatalogueCsvService.Write(outputFolder, Constants.OutputFileName, mergedCatalogues);
            _logger.LogInformation("Output file created at location: {0}", outputFolder);
        }
    }
}
