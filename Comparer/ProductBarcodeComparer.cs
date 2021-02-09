using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Catalogue.Model;

namespace Catalogue.Comparer
{
    /// <summary>
    /// It will compare the barcodes in ProductBarcode model to check if the two objects have same barcodes or not
    /// </summary>
    public class ProductBarcodeComparer : IEqualityComparer<ProductBarcode>
    {
        public bool Equals([AllowNull] ProductBarcode a, [AllowNull] ProductBarcode b)
        {
            return a!=null && b!=null && string.Equals(a.Barcode, b.Barcode, StringComparison.InvariantCultureIgnoreCase);
        }

        public int GetHashCode([DisallowNull] ProductBarcode obj)
        {
            return obj.Barcode.GetHashCode();
        }
    }
}
