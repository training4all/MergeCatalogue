using CsvHelper.Configuration.Attributes;

namespace Catalogue.Model
{
    public class ProductBarcode
    {
        [Name("SupplierID")]
        public string SupplierID { get; set; }

        [Name("SKU")]
        public string SKU { get; set; }

        [Name("Barcode")]
        public string Barcode { get; set; }
    }
}
