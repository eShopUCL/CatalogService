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



# CatalogService

## Overview

**CatalogService** is a microservice responsible for managing and serving the catalog-items within the **eShopOnWeb** ecosystem. This service communicates with the **eShopOnWeb** monolith via **messaging**, enabling asynchronous, decoupled interactions. It handles catalog-related CRUD operations, contributing to a more modular and scalable architecture.

---

## Features

- **Asynchronous Communication**: Interacts with the monolithic **eShopOnWeb** application through our messaging system.
- **Product Management**: Supports CRUD operations for catalog data.
- **Event-Driven Architecture**: Subscribes to domain events to synchronize data and handle business logic.
- **Scalability**: Can be independently scaled to manage high-volume catalog workloads.
- **Extensibility**: Built for easy enhancement to support additional features or integrations.

---

## Architecture

CatalogService is a standalone microservice that complements the monolithic **eShopOnWeb** application by leveraging **messaging** for seamless integration. It adheres to microservice design principles, ensuring modularity and scalability.

### Technology Stack

- **.NET Core/ASP.NET Core**: Framework for service implementation.
- **Messaging Platform**: Uses RabbitMQ for asynchronous communication.
- **Database**: SQL Server for persistence.

### Communication

- **Messaging Protocol**: Subscribes to messages(events) for catalog-item synchronization.
- **Event-Driven Design**: Reduces coupling and enhances reliability.

### Database

- Stores everything related to Catalog within an MSSQL-db


---

## Getting Started

### Prerequisites

1. [.NET SDK](https://dotnet.microsoft.com/) (8.0)
2. An instance of RabbitMQ running
3. SQL Server
4. Docker

### Setup

1. Clone the repository:
   ```bash
   git clone https://github.com/eShopUCL/CatalogService.git
   cd CatalogService

2. Configure the database connection:  
   Update `appsettings.json` with the appropriate database connection string.

4. Apply database migrations** (if applicable):  
   ```bash
   dotnet ef database update
   
5. Run the service using Docker Compose
   ```bash
   docker-compose up
