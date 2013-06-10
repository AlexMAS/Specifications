using System.Collections.Generic;

namespace DevCon2011.Specifications.Catalog.Models
{
    public class CatalogModel
    {
        public ProductFilter Filter { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}