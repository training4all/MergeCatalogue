using CsvHelper.Configuration.Attributes;

namespace Catalogue.Model
{
    public class ProductCatalogue
    {
        [Name("SKU")]
        public string SKU { get; set; }

        [Name("Description")]
        public string Description { get; set; }
    }
}
