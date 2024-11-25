SET IDENTITY_INSERT CatalogBrands ON;

INSERT INTO CatalogBrands (Id, Brand) VALUES
(1, 'Azure'),
(2, '.NET'),
(3, 'Visual Studio'),
(4, 'SQL Server'),
(5, 'Other');

SET IDENTITY_INSERT CatalogBrands OFF;

SET IDENTITY_INSERT CatalogTypes ON;

INSERT INTO CatalogTypes (Id, Type) VALUES
(1, 'Mug'),
(2, 'T-Shirt'),
(3, 'Sheet'),
(4, 'USB Memory Stick'),
(5, 'Test');

SET IDENTITY_INSERT CatalogTypes OFF;

SET IDENTITY_INSERT CatalogItems ON;

INSERT INTO CatalogItems (Id, CatalogBrandId, CatalogTypeId, Description, Name, PictureUri, Price) VALUES
(2, 1, 1, 'Testing Edit', '.NET Black & White Mug', 'http://4.207.200.245/eshopwebmvc/images/products/2.png?0?0?0?0', 99.00),
(3, 5, 2, 'Prism White T-Shirt', 'Prism White T-Shirt', 'http://4.207.200.245/eshopwebmvc/images/products/3.png', 12.00),
(4, 2, 2, '.NET Foundation Sweatshirt', '.NET Foundation Sweatshirt', 'http://4.207.200.245/eshopwebmvc/images/products/4.png', 12.00),
(5, 5, 3, 'Roslyn Red Sheet', 'Roslyn Red Sheet', 'http://4.207.200.245/eshopwebmvc/images/products/5.png', 8.50),
(6, 2, 2, '.NET Blue Sweatshirt', '.NET Blue Sweatshirt', 'http://4.207.200.245/eshopwebmvc/images/products/6.png', 12.00),
(7, 5, 2, 'Roslyn Red T-Shirt', 'Roslyn Red T-Shirt', 'http://4.207.200.245/eshopwebmvc/images/products/7.png', 12.00),
(8, 5, 2, 'Kudu Purple Sweatshirt', 'Kudu Purple Sweatshirt', 'http://4.207.200.245/eshopwebmvc/images/products/8.png', 8.50),
(9, 5, 1, 'Cup<T> White Mug', 'Cup<T> White Mug', 'http://4.207.200.245/eshopwebmvc/images/products/9.png', 12.00),
(10, 2, 3, '.NET Foundation Sheet', '.NET Foundation Sheet', 'http://4.207.200.245/eshopwebmvc/images/products/10.png', 12.00),
(11, 2, 3, 'Cup<T> Sheet', 'Cup<T> Sheet', 'http://4.207.200.245/eshopwebmvc/images/products/11.png', 8.50),
(511, 2, 3, 'TestDescriptiones', 'TestingCreate', '', 500.00);

SET IDENTITY_INSERT CatalogItems OFF;