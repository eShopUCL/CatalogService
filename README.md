# CatalogService.API Endpoints 📦

## Working Endpoints 🚀
These endpoints are active and ready for use:

- **GET** `/api/catalog/items` - Get all catalog items.
- **GET** `/api/catalog/items/by` - Get items by their IDs.
- **GET** `/api/catalog/items/{id:int}` - Get a specific item by its ID.
- **GET** `/api/catalog/items/by/{name:minlength(1)}` - Get items by name.
- **GET** `/api/catalog/catalogtypes` - Get all catalog types.
- **GET** `/api/catalog/catalogbrands` - Get all catalog brands.
- **GET** `/api/catalog/items/type/{typeId}/brand/{brandId?}` - Get items by type and brand ID.
- **DELETE** `/api/catalog/items/{id:int}` - Delete an item by its ID.
- **GET** `/api/catalog/items/type/all/brand/{brandId:int?}` - Get all items by brand ID.
- **POST** `/api/catalog/items` - Create a new item
- **PUT** `/api/catalog/items` - Update an existing item
---

## Todo 🛠️
- RabbitMQ/Event Bus stuff
- DB

