IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Category] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Category] PRIMARY KEY ([Id])
);

CREATE TABLE [Order] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(max) NOT NULL,
    [TotalAmount] decimal(18,2) NOT NULL,
    [Status] nvarchar(max) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Order] PRIMARY KEY ([Id])
);

CREATE TABLE [Product] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [Price] decimal(18,2) NOT NULL,
    [StockQuantity] int NOT NULL,
    [CategoryId] int NOT NULL,
    CONSTRAINT [PK_Product] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Product_Category_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Category] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [OrderItem] (
    [Id] int NOT NULL IDENTITY,
    [OrderId] int NOT NULL,
    [ProductId] int NOT NULL,
    [Quantity] int NOT NULL,
    [UnitPrice] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_OrderItem] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_OrderItem_Order_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Order] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_OrderItem_Product_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Product] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_OrderItem_OrderId] ON [OrderItem] ([OrderId]);

CREATE INDEX [IX_OrderItem_ProductId] ON [OrderItem] ([ProductId]);

CREATE INDEX [IX_Product_CategoryId] ON [Product] ([CategoryId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251207070453_InitialCreate', N'9.0.0');

ALTER TABLE [Product] ADD [Rating] nvarchar(max) NOT NULL DEFAULT N'';

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251209220519_Rating_column_add_in_product_model', N'9.0.0');

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Product]') AND [c].[name] = N'Rating');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Product] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Product] ALTER COLUMN [Rating] nvarchar(30) NOT NULL;

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Product]') AND [c].[name] = N'Name');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Product] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Product] ALTER COLUMN [Name] nvarchar(60) NOT NULL;

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Product]') AND [c].[name] = N'Description');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Product] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [Product] ALTER COLUMN [Description] nvarchar(60) NOT NULL;

UPDATE [Order] SET [Status] =  0 WHERE [Status] = 'pending'

UPDATE [Order] SET [Status] =  1 WHERE [Status] = 'cancelled'

UPDATE [Order] SET [Status] =  2 WHERE [Status] = 'completed'

UPDATE [Order] SET [Status] =  3 WHERE [Status] = 'delivered'

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Order]') AND [c].[name] = N'Status');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Order] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [Order] ALTER COLUMN [Status] int NOT NULL;

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Category]') AND [c].[name] = N'Name');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [Category] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [Category] ALTER COLUMN [Name] nvarchar(60) NOT NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251210002218_addAnnotationsForDataValidations', N'9.0.0');

COMMIT;
GO

