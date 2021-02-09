using System;
using Catalogue.Model;
using Catalogue.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Catalogue
{
    public class Program
    {
        private static ServiceProvider _serviceProvider;

        public static void Main(string[] args)
        {   
            SetupGlobalExceptionHandler();
            ConfigureServices();
            var scope = _serviceProvider.CreateScope();
            scope.ServiceProvider.GetRequiredService<ConsoleApplication>().Run();
            DisposeServices();
        }

        /// <summary>
        /// Will setup the DI and configure services needed for app
        /// </summary>
        private static void ConfigureServices()
        {
            var services = new ServiceCollection()
                    .AddLogging(configure => configure.AddConsole())
                    .AddSingleton<IFileService, FileService>()
                    .AddSingleton<ICatalogueService, CatalogueService>()
                    .AddSingleton<IUtilityService, UtilityService>()
                    .AddSingleton<ICsvService<ProductBarcode>, CsvService<ProductBarcode>>()
                    .AddSingleton<ICsvService<ProductCatalogue>, CsvService<ProductCatalogue>>()
                    .AddSingleton<ICsvService<MergedCatalogue>, CsvService<MergedCatalogue>>()
                    .AddSingleton<ConsoleApplication>();
            
            _serviceProvider = services.BuildServiceProvider();
        }

        /// <summary>
        /// Will dispose of all services after used to avoid memory leak
        /// </summary>
        private static void DisposeServices()
        {
            switch (_serviceProvider)
            {
                case null:
                    return;
                case IDisposable disposable:
                    disposable.Dispose();
                    break;
            }
        }

        /// <summary>
        /// This will setup global handler for unhandled exceptions
        /// </summary>
        private static void SetupGlobalExceptionHandler()
        {
            var currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += MyHandler;
        }

        /// <summary>
        /// Global exception handler for unhandled exceptions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            DisposeServices();
            var e = (Exception)args.ExceptionObject;
            Console.WriteLine("MyHandler caught : " + e.Message);
            Console.WriteLine("Runtime terminating: {0}", args.IsTerminating);
        }
    }
}
