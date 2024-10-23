using CatalogService.API;
using Microsoft.EntityFrameworkCore; // Make sure to include the namespace where your MapCatalogApiV1 is defined

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add your DbContext and other services here
builder.Services.AddDbContext<CatalogContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CatalogConnection"),
    sqlOptions => sqlOptions.EnableRetryOnFailure()));


var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Call the method to map your catalog API
app.MapCatalogApiV1(); // This registers your endpoints

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers(); // This may not be needed if you’re only using minimal APIs

app.Run();
