using CatalogService.Entities;
using CatalogService.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.API;

public static class CatalogApi
{
    public static IEndpointRouteBuilder MapCatalogApiV1(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/catalog");

        api.MapGet("/items", GetAllItems);
        api.MapGet("/items/paged", GetAllItemsPaged);
        api.MapGet("/items/by", GetItemsByIds);
        api.MapGet("/items/{id:int}", GetItemById);
        api.MapGet("/items/by/{name:minlength(1)}", GetItemsByName);

        api.MapGet("/items/type/{typeId}/brand/{brandId?}", GetItemsByBrandAndTypeId);
        api.MapGet("/items/type/all/brand/{brandId:int?}", GetItemsByBrandId);
        api.MapGet("/catalogtypes", async (CatalogContext context) => await context.CatalogTypes.OrderBy(x => x.Type).ToListAsync());
        api.MapGet("/catalogbrands", async (CatalogContext context) => await context.CatalogBrands.OrderBy(x => x.Brand).ToListAsync());

        api.MapPut("/items/{id:int}", UpdateItem);
        api.MapPost("/items", CreateItem);
        api.MapDelete("/items/{id:int}", DeleteItemById);

        return app;
    }

    // Hent alle items i paged format - Tager en paginationRequest med mængden af
    // resultater per side samt sidenummer
    public static async Task<Results<Ok<PaginatedItems<CatalogItem>>, BadRequest<string>>> GetAllItemsPaged(
        [AsParameters] PaginationRequest paginationRequest,
        [AsParameters] CatalogServices services)
    {
        var pageSize = paginationRequest.PageSize;
        var pageIndex = paginationRequest.PageIndex;

        var totalItems = await services.Context.CatalogItems
            .LongCountAsync();

        var itemsOnPage = await services.Context.CatalogItems
            .OrderBy(c => c.Name)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();

        return TypedResults.Ok(new PaginatedItems<CatalogItem>(pageIndex, pageSize, totalItems, itemsOnPage));
    }

    // Hent ALLE Catalog Items
    public static async Task<Results<Ok<List<CatalogItem>>, BadRequest<string>>> GetAllItems(
    [AsParameters] CatalogServices services)
    {
        var items = await services.Context.CatalogItems
            .OrderBy(c => c.Name)
            .ToListAsync();

        return TypedResults.Ok(items);
    }
    
    // Hent items ud fra flere ID'er, f.eks. ids [1, 2, 3]
    public static async Task<Ok<List<CatalogItem>>> GetItemsByIds(
        [AsParameters] CatalogServices services,
        int[] ids)
    {
        // For hvert item fundet, tilføj til en liste og returner denne
        var items = await services.Context.CatalogItems.Where(item => ids.Contains(item.Id)).ToListAsync();
        return TypedResults.Ok(items);
    }

    public static async Task<Results<Ok<CatalogItemResponse>, NotFound, BadRequest<string>>> GetItemById(
    [AsParameters] CatalogServices services,
    int id)
    {
        // Hvis ID er mindre end eller 0, returner en 400 Bad Request
        if (id <= 0)
        {
            return TypedResults.BadRequest("Id is not valid.");
        }

        // Hent Catalog Item samt dens CatalogBrand og CatalogType
        var item = await services.Context.CatalogItems
            .Include(ci => ci.CatalogBrand)
            .Include(ci => ci.CatalogType)
            .SingleOrDefaultAsync(ci => ci.Id == id);

        // Hvis intet item er fundet, returner 404
        if (item == null)
        {
            return TypedResults.NotFound();
        }

        // Map Catalog Item til CatalogItemResponse DTO, da vi ikke vil have vores CatalogType og
        // CatalogBrand til at være hele objekter, men i stedet bare en string for navnet for brand / type
        var response = new CatalogItemResponse
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            Price = item.Price,
            PictureUri = item.PictureUri,
            CatalogTypeId = item.CatalogTypeId,
            CatalogType = item.CatalogType?.Type ?? "Unknown",
            CatalogBrandId = item.CatalogBrandId,
            CatalogBrand = item.CatalogBrand?.Brand ?? "Unknown"
        };

        return TypedResults.Ok(response);
    }


    public static async Task<Ok<PaginatedItems<CatalogItem>>> GetItemsByName(
        [AsParameters] PaginationRequest paginationRequest,
        [AsParameters] CatalogServices services,
        string name)
    {
        var pageSize = paginationRequest.PageSize;
        var pageIndex = paginationRequest.PageIndex;

        var totalItems = await services.Context.CatalogItems
            .Where(c => c.Name.StartsWith(name))
            .LongCountAsync();

        var itemsOnPage = await services.Context.CatalogItems
            .Where(c => c.Name.StartsWith(name))
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();

        return TypedResults.Ok(new PaginatedItems<CatalogItem>(pageIndex, pageSize, totalItems, itemsOnPage));
    }

    public static async Task<Ok<PaginatedItems<CatalogItem>>> GetItemsByBrandAndTypeId(
        [AsParameters] PaginationRequest paginationRequest,
        [AsParameters] CatalogServices services,
        int typeId,
        int? brandId)
    {
        var pageSize = paginationRequest.PageSize;
        var pageIndex = paginationRequest.PageIndex;

        var root = (IQueryable<CatalogItem>)services.Context.CatalogItems;
        root = root.Where(c => c.CatalogTypeId == typeId);
        if (brandId is not null)
        {
            root = root.Where(c => c.CatalogBrandId == brandId);
        }

        var totalItems = await root
            .LongCountAsync();

        var itemsOnPage = await root
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();

        return TypedResults.Ok(new PaginatedItems<CatalogItem>(pageIndex, pageSize, totalItems, itemsOnPage));
    }

    public static async Task<Ok<PaginatedItems<CatalogItem>>> GetItemsByBrandId(
        [AsParameters] PaginationRequest paginationRequest,
        [AsParameters] CatalogServices services,
        int? brandId)
    {
        var pageSize = paginationRequest.PageSize;
        var pageIndex = paginationRequest.PageIndex;

        var root = (IQueryable<CatalogItem>)services.Context.CatalogItems;

        if (brandId is not null)
        {
            root = root.Where(ci => ci.CatalogBrandId == brandId);
        }

        var totalItems = await root
            .LongCountAsync();

        var itemsOnPage = await root
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();

        return TypedResults.Ok(new PaginatedItems<CatalogItem>(pageIndex, pageSize, totalItems, itemsOnPage));
    }

    public static async Task<Results<NotFound, PhysicalFileHttpResult>> GetItemPictureById(
    [AsParameters] CatalogServices services,
    IWebHostEnvironment environment,
    int catalogItemId)
    {
        var item = await services.Context.CatalogItems.FindAsync(catalogItemId);

        if (item is null || string.IsNullOrEmpty(item.PictureUri))
        {
            return TypedResults.NotFound();
        }

        var path = GetFullPath(environment.ContentRootPath, item.PictureUri);

        if (!File.Exists(path))
        {
            return TypedResults.NotFound();
        }

        string imageFileExtension = Path.GetExtension(item.PictureUri);
        string mimetype = GetImageMimeTypeFromImageFileExtension(imageFileExtension);
        DateTime lastModified = File.GetLastWriteTimeUtc(path);

        return TypedResults.PhysicalFile(path, mimetype, lastModified: lastModified);
    }


    public static async Task<Results<Created<CatalogItem>, BadRequest<string>>> CreateItem(
        [AsParameters] CatalogServices services,
        [FromBody] CatalogItem catalogItem)
    {
        // Brug services.context til at tilføje catalog Item til DB
        services.Context.CatalogItems.Add(catalogItem);
        await services.Context.SaveChangesAsync();

        // Returnér det oprettede item
        return TypedResults.Created($"/api/catalog/items/{catalogItem.Id}", catalogItem);
    }


    public static async Task<Results<NoContent, NotFound, BadRequest<string>>> UpdateItem(
    [AsParameters] CatalogServices services,
    int id,
    [FromBody] CatalogItem updatedItem)
    {
        // Log the request
        Console.WriteLine($"Received request to update item with ID: {id}");

        var catalogItem = await services.Context.CatalogItems
            .SingleOrDefaultAsync(item => item.Id == id);

        if (catalogItem is null)
        {
            Console.WriteLine($"Item with ID: {id} not found.");
            return TypedResults.NotFound();
        }

        // Proceed with updating the item
        catalogItem.UpdateDetails(new CatalogItem.CatalogItemDetails(
            updatedItem.Name,
            updatedItem.Description,
            updatedItem.Price
        ));

        catalogItem.UpdateBrand(updatedItem.CatalogBrandId);
        catalogItem.UpdateType(updatedItem.CatalogTypeId);
        catalogItem.UpdatePictureUri(updatedItem.PictureUri);

        await services.Context.SaveChangesAsync();
        Console.WriteLine($"Item with ID: {id} successfully updated.");

        return TypedResults.NoContent();
    }



    public static async Task<Results<NoContent, NotFound>> DeleteItemById(
        [AsParameters] CatalogServices services,
        int id)
    {
        var item = services.Context.CatalogItems.SingleOrDefault(x => x.Id == id);

        if (item is null)
        {
            return TypedResults.NotFound();
        }

        services.Context.CatalogItems.Remove(item);
        await services.Context.SaveChangesAsync();
        return TypedResults.NoContent();
    }

    private static string GetImageMimeTypeFromImageFileExtension(string extension) => extension switch
    {
        ".png" => "image/png",
        ".gif" => "image/gif",
        ".jpg" or ".jpeg" => "image/jpeg",
        ".bmp" => "image/bmp",
        ".tiff" => "image/tiff",
        ".wmf" => "image/wmf",
        ".jp2" => "image/jp2",
        ".svg" => "image/svg+xml",
        ".webp" => "image/webp",
        _ => "application/octet-stream",
    };

    public static string GetFullPath(string contentRootPath, string pictureFileName) =>
        Path.Combine(contentRootPath, "Pics", pictureFileName);

}
