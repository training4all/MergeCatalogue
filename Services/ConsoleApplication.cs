using System;
using Microsoft.Extensions.Logging;

namespace Catalogue.Services
{
    /// <summary>
    /// Startup service
    /// </summary>
    public class ConsoleApplication
    {
        private readonly ICatalogueService _catalogueService;
        private readonly ILogger<ConsoleApplication> _logger;

        public ConsoleApplication(
            ILogger<ConsoleApplication> logger,
            ICatalogueService catalogueService
            )
        {
            _logger = logger;
            _catalogueService = catalogueService;
        }

        public void Run()
        {   
            _logger.LogInformation(@"
               Some of the Assumptions based on ReadMe for this project to work
               1) Input Folder - should contain below csv file
                    - catalogA.csv
                    - catalogB.csv
                    - barcodesA.csv
                    - barcodesB.csv
                    - suppliersA.csv
                    - suppliersB.csv                                
              2) Output Folder - where output file called 'result_output.csv' will be saved
             ");

            _logger.LogInformation("Enter Input folder path");
            var inputFolder = Console.ReadLine();
            _catalogueService.Merge(inputFolder);
        }
    }
}
