-- 1. Create the database
CREATE DATABASE IF NOT EXISTS InventoryDB;
GO

-- 2. Use the database
USE InventoryDB;
GO

-- 3. Create Products table
CREATE TABLE Products (
    ProductID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Quantity INT NOT NULL DEFAULT 0,
    Size DECIMAL(10,2) NOT NULL DEFAULT 0,
    Unit NVARCHAR(10) NOT NULL,
    Price DECIMAL(10,2) NOT NULL DEFAULT 0
);
GO

-- 4. Optional: Insert sample data
INSERT INTO Products (Name, Quantity, Size, Unit, Price) VALUES
('Laptop', 10, 1.0, 'pcs', 1200.00),
('Notebook', 200, 100.0, 'pcs', 2.50),
('Water Bottle', 50, 500.0, 'ml', 5.00);
GO
