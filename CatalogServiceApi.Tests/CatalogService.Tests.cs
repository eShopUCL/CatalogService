using CatalogService.API;
using CatalogService.Entities;
using CatalogService.Model;
using Moq;
using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;

public class CatalogApiTests
{
    private readonly Mock<CatalogServices> _mockCatalogServices;
    private readonly Mock<DbSet<CatalogItem>> _mockCatalogItemsDbSet;

    public CatalogApiTests()
    {
        _mockCatalogServices = new Mock<CatalogServices>();
        _mockCatalogItemsDbSet = new Mock<DbSet<CatalogItem>>();
        var mockContext = new Mock<CatalogContext>();

        var catalogItems = new List<CatalogItem>
        {
            CreateCatalogItem(1, 1, "Description for Item1", "Item1", 10.0m, "https://example.com/image1.jpg", 1),
            CreateCatalogItem(1, 2, "Description for Item2", "Item2", 20.0m, "https://example.com/image2.jpg", 2)
        }.AsQueryable();

        _mockCatalogItemsDbSet.As<IQueryable<CatalogItem>>().Setup(m => m.Provider).Returns(catalogItems.Provider);
        _mockCatalogItemsDbSet.As<IQueryable<CatalogItem>>().Setup(m => m.Expression).Returns(catalogItems.Expression);
        _mockCatalogItemsDbSet.As<IQueryable<CatalogItem>>().Setup(m => m.ElementType).Returns(catalogItems.ElementType);
        _mockCatalogItemsDbSet.As<IQueryable<CatalogItem>>().Setup(m => m.GetEnumerator()).Returns(catalogItems.GetEnumerator());

        mockContext.Setup(c => c.CatalogItems).Returns(_mockCatalogItemsDbSet.Object);
        _mockCatalogServices.Setup(s => s.Context).Returns(mockContext.Object);
    }

    private CatalogItem CreateCatalogItem(int catalogTypeId, int catalogBrandId, string description, string name, decimal price, string pictureUri, int id)
    {
        var item = new CatalogItem(catalogTypeId, catalogBrandId, description, name, price, pictureUri);

        var idProperty = typeof(BaseEntity).GetProperty("Id", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        idProperty?.SetValue(item, id);

        return item;
    }

    [Fact]
    public async Task GetAllItems_ShouldReturnPaginatedItems()
    {
        // Arrange
        var paginationRequest = new PaginationRequest { PageSize = 10, PageIndex = 0 };

        // Act
        var result = await CatalogApi.GetAllItems(paginationRequest, _mockCatalogServices.Object);

        // Assert
        Assert.IsType<Ok<PaginatedItems<CatalogItem>>>(result.Result);
        var okResult = result.Result as Ok<PaginatedItems<CatalogItem>>;
        Assert.Equal(2, okResult.Value.Count);
    }
}
