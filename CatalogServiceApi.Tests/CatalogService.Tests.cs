using CatalogService.API;
using CatalogService.Entities;
using CatalogService.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

public class CatalogApiTests
{
    private readonly CatalogContext _context;
    private readonly CatalogServices _catalogServices;

    public CatalogApiTests()
    {
        // Setup in-memory database for testing
        var options = new DbContextOptionsBuilder<CatalogContext>()
            .UseInMemoryDatabase(databaseName: "TestCatalogDb")
            .Options;

        _context = new CatalogContext(options);

        // Seed data into the in-memory database
        SeedDatabase();

        // Initialize CatalogServices with the in-memory context
        _catalogServices = new CatalogServices(_context);
    }

    private void SeedDatabase()
    {
        _context.CatalogItems.AddRange(
            new CatalogItem(1, 1, "Description for Item1", "Item1", 10.0m, "https://example.com/image1.jpg"),
            new CatalogItem(1, 2, "Description for Item2", "Item2", 20.0m, "https://example.com/image2.jpg")
        );

        _context.SaveChanges();
    }

    [Fact]
    public async Task GetAllItems_ShouldReturnPaginatedItems()
    {
        // Arrange
        var paginationRequest = new PaginationRequest { PageSize = 10, PageIndex = 0 };

        // Act
        var result = await CatalogApi.GetAllItemsPaginated(paginationRequest, _catalogServices);

        // Assert
        Assert.IsType<Ok<PaginatedItems<CatalogItem>>>(result.Result);
        var okResult = result.Result as Ok<PaginatedItems<CatalogItem>>;
        Assert.Equal(2, okResult.Value.Count);
    }
}
