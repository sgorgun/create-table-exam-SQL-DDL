CREATE TABLE Manufacturers (
    ManufacturerID INT PRIMARY KEY,
    ManufacturerName NVARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE Brands (
    BrandID INT PRIMARY KEY,
    BrandName NVARCHAR(100) NOT NULL UNIQUE,
    ManufacturerID INT NOT NULL,
    FOREIGN KEY (ManufacturerID) REFERENCES Manufacturers(ManufacturerID)
);

CREATE TABLE ScreenResolutions (
    ScreenResolutionID INT PRIMARY KEY,
    ResolutionWidth INT NOT NULL,
    ResolutionHeight INT NOT NULL
);

CREATE TABLE MatrixTypes (
    MatrixTypeID INT PRIMARY KEY,
    MatrixTypeName NVARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE RAMOptions (
    RAMID INT PRIMARY KEY,
    RAMSize INT NOT NULL,
    ManufacturerID INT NOT NULL,
    FOREIGN KEY (ManufacturerID) REFERENCES Manufacturers(ManufacturerID)
);

CREATE TABLE ROMOptions (
    ROMID INT PRIMARY KEY,
    ROMSize INT NOT NULL
);

CREATE TABLE ModelNames (
    ModelNameID INT PRIMARY KEY,
    ModelName NVARCHAR(100) NOT NULL UNIQUE,
    BrandID INT NOT NULL,
    FOREIGN KEY (BrandID) REFERENCES Brands(BrandID)
);

CREATE TABLE Screen (
    ScreenID INT PRIMARY KEY,
    ScreenResolutionID INT NOT NULL,
    MatrixTypeID INT NOT NULL,
    FOREIGN KEY (ScreenResolutionID) REFERENCES ScreenResolutions(ScreenResolutionID),
    FOREIGN KEY (MatrixTypeID) REFERENCES MatrixTypes(MatrixTypeID)
);

CREATE TABLE Models (
    ModelID INT PRIMARY KEY,
    ModelNameID INT NOT NULL,
    ScreenID INT,
    RAMID INT,
    ROMID INT,
    FOREIGN KEY (ModelNameID) REFERENCES ModelNames(ModelNameID),
    FOREIGN KEY (ScreenID) REFERENCES Screen(ScreenID),
    FOREIGN KEY (RAMID) REFERENCES RAMOptions(RAMID),
    FOREIGN KEY (ROMID) REFERENCES ROMOptions(ROMID)
);

CREATE TABLE Positions (
    PositionID INT PRIMARY KEY,
    PositionName NVARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE Person (
    PersonID INT PRIMARY KEY,
    LastName NVARCHAR(100) NOT NULL,
    FirstName NVARCHAR(100) NOT NULL,
    PhoneNumber NVARCHAR(15) UNIQUE
);

CREATE TABLE Employees (
    EmployeeID INT PRIMARY KEY,
    PersonID INT NOT NULL,
    PositionID INT NOT NULL,
    FOREIGN KEY (PersonID) REFERENCES Person(PersonID),
    FOREIGN KEY (PositionID) REFERENCES Positions(PositionID)
);

CREATE TABLE OrderStates (
    StateID INT PRIMARY KEY,
    StateName NVARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE Customers (
    CustomerID INT PRIMARY KEY,
    PersonID INT NOT NULL,
    FOREIGN KEY (PersonID) REFERENCES Person(PersonID)
);

CREATE TABLE Smartphones (
    SmartphoneID INT PRIMARY KEY,
    ModelID INT NOT NULL,
    ManufactureYear INT NOT NULL CHECK (ManufactureYear >= 1980),
    ImeiA NVARCHAR(15) UNIQUE NOT NULL,
    ImeiB NVARCHAR(15) UNIQUE,
    OwnerID INT UNIQUE,
    FOREIGN KEY (ModelID) REFERENCES Models(ModelID),
    FOREIGN KEY (OwnerID) REFERENCES Customers(CustomerID)
);

CREATE TABLE Orders (
    OrderID INT PRIMARY KEY,
    SmartphoneID INT NOT NULL,
    DiagnosingEmployeeID INT NOT NULL,
    OrderStateID INT NOT NULL,
    FOREIGN KEY (SmartphoneID) REFERENCES Smartphones(SmartphoneID),
    FOREIGN KEY (DiagnosingEmployeeID) REFERENCES Employees(EmployeeID),
    FOREIGN KEY (OrderStateID) REFERENCES OrderStates(StateID)
);

CREATE TABLE Repairs (
    RepairID INT PRIMARY KEY,
    OrderID INT NOT NULL,
    RepairEmployeeID INT,
    MalfunctionDescription NVARCHAR(255) NOT NULL,
    RepairCost DECIMAL(10, 2) CHECK (RepairCost >= 0),
    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
    FOREIGN KEY (RepairEmployeeID) REFERENCES Employees(EmployeeID)
);