-- Create a new database for the catalog service
CREATE DATABASE CatalogDB;

-- Use the new database
USE CatalogDB;

-- Create the CatalogItems table
CREATE TABLE CatalogBrand (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Brand NVARCHAR(100) NOT NULL
);

-- 4. Opret CatalogType Table
CREATE TABLE CatalogType (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Type NVARCHAR(100) NOT NULL
);

CREATE TABLE CatalogItem (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX) NOT NULL,
    Price DECIMAL(18, 2) NOT NULL,
    PictureUri NVARCHAR(255) NULL,
    CatalogTypeId INT NOT NULL REFERENCES CatalogType(Id) ON DELETE CASCADE,
    CatalogBrandId INT NOT NULL REFERENCES CatalogBrand(Id) ON DELETE CASCADE
);