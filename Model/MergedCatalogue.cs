using System;
using CsvHelper.Configuration.Attributes;

namespace Catalogue.Model
{
    public class MergedCatalogue : IEquatable<MergedCatalogue>
    {
        [Name("SKU")]
        public string SKU { get; set; }

        [Name("Description")]
        public string Description { get; set; }

        [Name("Source")]
        public string Source { get; set; }

        public bool Equals(MergedCatalogue other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return SKU == other.SKU && Description == other.Description && Source == other.Source;
        }

        public override bool Equals(object obj)
        {   
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((MergedCatalogue) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(SKU, Description, Source);
        }
    }
}
