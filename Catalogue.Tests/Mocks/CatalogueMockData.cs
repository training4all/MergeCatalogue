using System.Collections;
using System.Collections.Generic;
using Catalogue.Model;

namespace Catalogue.Tests.Mocks
{
    public class CatalogueMockData : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
        {
            new object[] { // Only A products as B.Except(A) = null
                new List<ProductCatalogue>(){new ProductCatalogue()
                {
                    SKU = "1",
                    Description = "desc"
                }},
                new List<ProductCatalogue>(){new ProductCatalogue()
                {
                    SKU = "2"
                }},
                new List<ProductBarcode>(){new ProductBarcode()
                {
                    Barcode = "common",
                    SKU = "1"
                }},
                new List<ProductBarcode>(){new ProductBarcode()
                {
                    Barcode = "common",
                    SKU = "2"
                }},
                new List<MergedCatalogue>() {new MergedCatalogue() {
                        SKU = "1",
                        Description = "desc",
                        Source = "A"
                }}
            },
            new object[] { // Both company A & B products as B.Except(A) != null
                new List<ProductCatalogue>(){new ProductCatalogue()
                {
                    SKU = "1",
                    Description = "CompanyA"
                }},
                new List<ProductCatalogue>(){new ProductCatalogue()
                {
                    SKU = "1",
                    Description = "CompanyB-First-Product-Common-To-CompanyA"
                }, new ProductCatalogue()
                {
                    SKU = "2",
                    Description = "CompanyB"
                }},
                new List<ProductBarcode>(){new ProductBarcode()
                {
                    Barcode = "BarcodeA",
                    SKU = "1"
                }},
                new List<ProductBarcode>(){new ProductBarcode()
                {
                    Barcode = "BarcodeB",
                    SKU = "2"
                }, new ProductBarcode()
                {
                    Barcode = "BarcodeA",
                    SKU = "1"
                }},
                new List<MergedCatalogue>() {new MergedCatalogue() {
                    SKU = "1",
                    Description = "CompanyA",
                    Source = "A"
                },
                    new MergedCatalogue() {
                        SKU = "2",
                        Description = "CompanyB",
                        Source = "B"
                    }
                }},
            new object[] { // Only company A products, as Products(B) = null, hence B.Except(A) = null
                new List<ProductCatalogue>(){new ProductCatalogue()
                {
                    SKU = "1",
                    Description = "product-1"
                }},
                new List<ProductCatalogue>(),
                new List<ProductBarcode>(){new ProductBarcode()
                {
                    Barcode = "barcode-1",
                    SKU = "1"
                }},
                new List<ProductBarcode>(),
                new List<MergedCatalogue>() {new MergedCatalogue() {
                        SKU = "1",
                        Description = "product-1",
                        Source = "A"
                    }
                }},
            new object[] {  // Only company B products, as Products(A) = null, hence B.Except(A) = Products(B)
                new List<ProductCatalogue>(),
                new List<ProductCatalogue>(){new ProductCatalogue()
                {
                    SKU = "1",
                    Description = "First product"
                }, new ProductCatalogue()
                {
                    SKU = "2",
                    Description = "Second product"
                }},
                new List<ProductBarcode>(),
                new List<ProductBarcode>(){new ProductBarcode()
                {
                    Barcode = "Barcode-2",
                    SKU = "2"
                }, new ProductBarcode()
                {
                    Barcode = "Barcode-1",
                    SKU = "1"
                }},
                new List<MergedCatalogue>() {new MergedCatalogue() {
                        SKU = "1",
                        Description = "First product",
                        Source = "B"
                    },
                    new MergedCatalogue() {
                        SKU = "2",
                        Description = "Second product",
                        Source = "B"
                    }
                }}
        };

        public IEnumerator<object[]> GetEnumerator()
        { return _data.GetEnumerator(); }

        IEnumerator IEnumerable.GetEnumerator()
        { return GetEnumerator(); }
    }
}
