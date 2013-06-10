using System.Web.Configuration;
using System.Web.Mvc;

using DevCon2011.Specifications.Catalog.Models;
using DevCon2011.Specifications.Data;

namespace DevCon2011.Specifications.Catalog.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(FilterCatalog(new ProductFilter()));
        }

        [HttpPost]
        public ActionResult Index(ProductFilter filter)
        {
            return View(FilterCatalog(filter ?? new ProductFilter()));
        }


        private CatalogModel FilterCatalog(ProductFilter filter)
        {
            // Should resolve IRepository from IoC Container
            IRepository repository = new EntityFrameworkRepository(new CatalogDataContext("name=CatalogDataContext"));

            // Create specification
            var specification = new ProductFilterSpecification(filter.PriceFrom, filter.PriceTo);

            // Get products
            var products = repository.Query(specification);


            return new CatalogModel { Filter = filter, Products = products };
        }
    }
}