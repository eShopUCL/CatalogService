using Ardalis.Specification;
using CatalogService.Entities;

namespace eShopOnWebMicroServices.Specifications;

public class CatalogItemsSpecification : Specification<CatalogItem>
{
    public CatalogItemsSpecification(params int[] ids)
    {
        Query.Where(c => ids.Contains(c.Id));
    }
}
