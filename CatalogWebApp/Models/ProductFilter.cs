using System.ComponentModel.DataAnnotations;

namespace DevCon2011.Specifications.Catalog.Models
{
    public class ProductFilter
    {
        [Range(0, 10000)]
        [Display(Name = "Price from")]
        public double? PriceFrom { get; set; }

        [Range(0, 10000)]
        [Display(Name = "Price to")]
        public double? PriceTo { get; set; }
    }
}