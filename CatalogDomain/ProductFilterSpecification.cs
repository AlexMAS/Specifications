using System;
using System.Linq.Expressions;

#region
using DevCon2011.Specifications.Expressions;
#endregion

namespace DevCon2011.Specifications.Catalog
{
    public class ProductFilterSpecification : Specification<Product>
    {
        public ProductFilterSpecification(double? priceFrom, double? priceTo)
        {
            SetUpPredicate(priceFrom, priceTo);
        }

        private void SetUpPredicate(double? priceFrom, double? priceTo)
        {
            if (priceFrom != null && priceTo != null)
            {
                _predicate = p => p.Price >= priceFrom.Value && p.Price <= priceTo.Value;
            }
            else if (priceFrom != null)
            {
                _predicate = p => p.Price >= priceFrom.Value;
            }
            else if (priceTo != null)
            {
                _predicate = p => p.Price <= priceTo.Value;
            }
            else
            {
                _predicate = p => true;
            }

            #region
            // _predicate = _predicate.And(p => p.IsPublished);
            #endregion
        }

        private Expression<Func<Product, bool>> _predicate;

        protected override Expression<Func<Product, bool>> Predicate { get { return _predicate; } }
    }
}