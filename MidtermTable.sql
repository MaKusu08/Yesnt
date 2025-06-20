-- ROLES TABLE
CREATE TABLE Roles (
    RoleID INT PRIMARY KEY AUTO_INCREMENT,
    RoleName VARCHAR(50) NOT NULL
);

-- USERS TABLE
CREATE TABLE Users (
    UserID INT PRIMARY KEY AUTO_INCREMENT,
    Username VARCHAR(50) NOT NULL UNIQUE,
    Password VARCHAR(100) NOT NULL,
    RoleID INT,
    FOREIGN KEY (RoleID) REFERENCES Roles(RoleID)
);

-- PRODUCTS TABLE
CREATE TABLE Products (
    ProductID INT PRIMARY KEY AUTO_INCREMENT,
    Name VARCHAR(100),
    Category VARCHAR(100),
    Price DECIMAL(10,2)
);

-- SUPPLIERS TABLE
CREATE TABLE Suppliers (
    SupplierID INT PRIMARY KEY AUTO_INCREMENT,
    Name VARCHAR(100),
    ContactInfo VARCHAR(255)
);

-- STOCK TABLE
CREATE TABLE Stock (
    StockID INT PRIMARY KEY AUTO_INCREMENT,
    ProductID INT,
    SupplierID INT,
    QuantityAdded INT,
    DateAdded DATE,
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID),
    FOREIGN KEY (SupplierID) REFERENCES Suppliers(SupplierID)
);

-- SALES TABLE
CREATE TABLE Sales (
    SaleID INT PRIMARY KEY AUTO_INCREMENT,
    ProductID INT,
    QuantitySold INT,
    SaleDate DATE,
    TotalAmount DECIMAL(10,2),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);

-- SUPPLIERPRODUCTS (Many-to-Many relationship)
CREATE TABLE SupplierProducts (
    SupplierID INT,
    ProductID INT,
    PRIMARY KEY (SupplierID, ProductID),
    FOREIGN KEY (SupplierID) REFERENCES Suppliers(SupplierID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);
