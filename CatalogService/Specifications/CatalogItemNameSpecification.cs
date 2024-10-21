﻿using Ardalis.Specification;
using CatalogService.Entities;

namespace eShopOnWebMicroServices.Specifications;

public class CatalogItemNameSpecification : Specification<CatalogItem>
{
    public CatalogItemNameSpecification(string catalogItemName)
    {
        Query.Where(item => catalogItemName == item.Name);
    }
}
