using CatalogService.API;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CatalogContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("CatalogConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure()
    ));

var app = builder.Build();

if (app.Environment.IsProduction())
{
    app.UsePathBase("/catalogservice");
}

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    var swaggerEndpoint = app.Environment.IsProduction() 
        ? "/catalogservice/swagger/v1/swagger.json" 
        : "/swagger/v1/swagger.json";
    
    c.SwaggerEndpoint(swaggerEndpoint, "Catalog Service API V1");
    c.RoutePrefix = "swagger"; // This keeps Swagger UI accessible at /swagger or /catalogservice/swagger
});

app.MapCatalogApiV1();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
