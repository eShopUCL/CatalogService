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

app.UseSwagger();
app.UseSwaggerUI();
app.MapCatalogApiV1();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
