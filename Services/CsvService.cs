using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using Microsoft.Extensions.Logging;

namespace Catalogue.Services
{
    /// <summary>
    /// Responsible for performing all operations on csv files
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICsvService<T>
    {
        /// <summary>
        /// Read csv data into type T from given location
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        List<T> Read(string folder, string filename);

        /// <summary>
        /// Write data of type T to csv file
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="filename"></param>
        /// <param name="records"></param>
        void Write(string folder, string filename, List<T> records);
    }

    /// <summary>
    /// Responsible for performing all operations on csv files
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CsvService<T>: ICsvService<T>
    {
        private readonly IFileService _fileService;
        private readonly ILogger<CsvService<T>> _logger;

        public CsvService(
            IFileService fileService,
            ILogger<CsvService<T>> logger)
        {
            _fileService = fileService;
            _logger = logger;
        }

        /// <summary>
        /// Read csv data into type T from given location
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public List<T> Read(string folder, string filename)
        {
            if (!_fileService.IsDirectoryExists(folder))
            {
                _logger.LogError("folder: {1} does not exists", folder);
                throw new DirectoryNotFoundException();
            }

            var path = _fileService.CombinePaths(new[] { folder, filename });
            if (!_fileService.IsFileExists(path))
            {
                _logger.LogError("file: {1} does not exists", path);
                throw new FileNotFoundException();
            }

            using var textReader = _fileService.ReadText(path);
            var csv = new CsvReader(textReader, CultureInfo.InvariantCulture);
            return csv.GetRecords<T>().ToList();
        }

        /// <summary>
        /// Write data of type T to csv file
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="filename"></param>
        /// <param name="records"></param>
        public void Write(string folder, string filename, List<T> records)
        {
            if (!_fileService.IsDirectoryExists(folder))
            {
                _logger.LogError("output folder: {1} does not exists", folder);
                throw new DirectoryNotFoundException();
            }

            using var writer = new StreamWriter(_fileService.CombinePaths(new[] { folder, filename }));
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords(records);
        }
    }
}
