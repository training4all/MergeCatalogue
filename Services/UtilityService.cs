using System;
using System.Collections.Generic;
using System.Linq;
using Catalogue.Model;

namespace Catalogue.Services
{
    /// <summary>
    /// Service to support miscellaneous tasks
    /// </summary>
    public interface IUtilityService
    {
        /// <summary>
        /// Will check if the output folder specified in constants exists then will use that
        /// else will create new output folder named as output inside current directory
        /// </summary>
        /// <returns></returns>
        string GetOutputFolder();

        /// <summary>
        /// Will map the object ProductCatalogue to MergedCatalogue
        /// </summary>
        /// <param name="catalogues"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        List<MergedCatalogue> ConvertProductToMergedCatalogue(List<ProductCatalogue> catalogues, string source);
    }

    /// <summary>
    /// Service to support miscellaneous tasks
    /// </summary>
    public class UtilityService : IUtilityService
    {
        private readonly IFileService _fileService;

        public UtilityService(
            IFileService fileService)
        {
            _fileService = fileService;
        }

        /// <summary>
        /// Will check if the output folder specified in constants exists then will use that
        /// else will create new output folder named as output inside current directory
        /// </summary>
        /// <returns></returns>
        public string GetOutputFolder()
        {
            if (!string.IsNullOrEmpty(Constants.OutputFolder) && _fileService.IsDirectoryExists(Constants.OutputFolder)) return Constants.OutputFolder;

            var outputDir = _fileService.CombinePaths(new []
            {
                Environment.CurrentDirectory, "Output"
            });
            if (!_fileService.IsDirectoryExists(outputDir))
            {
                _fileService.CreateDirectory(outputDir);
            }
            return outputDir;
        }

        /// <summary>
        /// Will map the object ProductCatalogue to MergedCatalogue
        /// </summary>
        /// <param name="catalogues"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public List<MergedCatalogue> ConvertProductToMergedCatalogue(List<ProductCatalogue> catalogues, string source)
        {
            return catalogues.Select(c => new MergedCatalogue() {SKU = c.SKU, Description = c.Description, Source = source}).ToList();
        }
    }
}
