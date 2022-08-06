USE master
GO

IF DB_ID('KarnelTravelsDB') IS NOT NULL
 DROP DATABASE KarnelTravelsDB
GO

CREATE DATABASE KarnelTravelsDB
GO

USE KarnelTravelsDB
GO
IF OBJECT_ID('dbo.Country', 'U') IS NOT NULL
  DROP TABLE dbo.Genre
CREATE TABLE Country(
	CountryId INT PRIMARY KEY IDENTITY (1,1),
	CountryName NVARCHAR(255) NOT NULL,
	Continent NVARCHAR(40) NOT NULL,
	RegionCode NVARCHAR(20) NOT NULL	--Ma~ vung`
)

GO


IF OBJECT_ID('dbo.City', 'U') IS NOT NULL
  DROP TABLE dbo.City
CREATE TABLE City(
	CityId INT PRIMARY KEY IDENTITY (1,1),
	CityName NVARCHAR(255) NOT NULL,
	CountryId INT NOT NULL,
	PostalCode NVARCHAR(20) NOT NULL,	--Ma~ buu chinh cho shipping, refund,...
)
GO


IF OBJECT_ID('dbo.Facility', 'U') IS NOT NULL
  DROP TABLE dbo.Facility
CREATE TABLE Facility(					-- Co so vat chat(khach san, nha` hang`,...)
	FacilityId INT PRIMARY KEY IDENTITY (1,1),
	FacilityTypeId INT NOT NULL,
	FacilityName NVARCHAR(255) NOT NULL,
	FacilityLocation NVARCHAR(255) NOT NULL,
	CityId INT NOT NULL,
	FacilityPrice MONEY NOT NULL,
	FacilityQuality FLOAT NOT NULL DEFAULT 1,	--Cham' diem? tu 1-10 (float)
	Quantity INT NOT NULL DEFAULT 0,
	FacilityAvailability BIT NOT NULL DEFAULT 0,	--Do kha? dung. hien tai (boolean)
	DeleteFlag BIT NOT NULL DEFAULT 0	--Truong danh dau xoa' (boolean)
)

ON [PRIMARY]
GO


INSERT INTO Facility(FacilityTypeId, FacilityName, FacilityLocation, CityId, FacilityPrice, FacilityQuality, Quantity, FacilityAvailability) VALUES
 (0, N'None', N'Nowhere', 0, 0, 0, 0, 0)		--Ban ghi rong

GO


IF OBJECT_ID('dbo.FacilityType', 'U') IS NOT NULL
  DROP TABLE dbo.FacilityType
CREATE TABLE FacilityType(
	FacilityTypeId INT PRIMARY KEY IDENTITY (1,1),
	FacilityTypeName NVARCHAR(255) NOT NULL,
	DeleteFlag BIT NOT NULL DEFAULT 0
)

GO


IF OBJECT_ID('dbo.TouristSpots', 'U') IS NOT NULL
  DROP TABLE dbo.TouristSpots
CREATE TABLE TouristSpots(
	TouristSpotsId INT PRIMARY KEY IDENTITY (1,1),
	TouristSpotsName NVARCHAR(255) NOT NULL,
	CityId INT NOT NULL,
	CategoryId INT NOT NULL,
	Rating FLOAT NOT NULL DEFAULT 1,	--Cham diem tu 1-10 (float)
	TouristSpotsAvailability BIT NOT NULL DEFAULT 0,
	DeleteFlag BIT NOT NULL DEFAULT 0
)

ON [PRIMARY]
GO


INSERT INTO TouristSpots(TouristSpotsName, CityId, CategoryId, Rating, TouristSpotsAvailability) VALUES
 (N'None', 0, 0, 0, 0)		--Ban ghi rong

GO


IF OBJECT_ID('dbo.Category', 'U') IS NOT NULL
  DROP TABLE dbo.Category
CREATE TABLE Category(
	CategoryId INT PRIMARY KEY IDENTITY (1,1),
	CategoryName NVARCHAR(255) NOT NULL,
	CategoryNote NVARCHAR(255) NULL,	--Note
	DeleteFlag BIT NOT NULL DEFAULT 0
)

GO


IF OBJECT_ID('dbo.Tour', 'U') IS NOT NULL
  DROP TABLE dbo.Tour
CREATE TABLE Tour(
	TourId INT PRIMARY KEY IDENTITY (1,1),
	TourName NVARCHAR(255) NOT NULL,
	TourAvailability BIT NOT NULL DEFAULT 0,
	TourPrice MONEY NOT NULL,
	TourNote NVARCHAR(255) NULL,
	DeleteFlag BIT NOT NULL DEFAULT 0
)

GO


IF OBJECT_ID('dbo.TourDetails', 'U') IS NOT NULL
  DROP TABLE dbo.TourDetails
CREATE TABLE TourDetails(
	TourDetailsId INT PRIMARY KEY IDENTITY (1,1),
	TourId INT NOT NULL,
	TourDetailsName NVARCHAR(255) NOT NULL,
	Activity NVARCHAR(100) NOT NULL,
	ActivityTime NVARCHAR(30) NOT NULL,
	FacilityId INT NOT NULL DEFAULT 0,
	TouristSpotsId INT NOT NULL DEFAULT 0,
	TourNote NVARCHAR(255) NULL,
	DeleteFlag BIT NOT NULL DEFAULT 0
)

GO

IF OBJECT_ID('dbo.TransactionsRecords', 'U') IS NOT NULL
  DROP TABLE dbo.TransactionsRecords	--Ban ghi cac giao dich
CREATE TABLE TransactionsRecords(
	RecordId INT PRIMARY KEY IDENTITY (1,1),
	TransactionTypeId INT NOT NULL,
	TourId INT NOT NULL,
	CustomerID INT NOT NULL,	--Nguoi mua
	AdminID INT NOT NULL,		--Nguoi edit ban ghi
	DeleteFlag BIT NOT NULL DEFAULT 0
)

GO

IF OBJECT_ID('dbo.TransactionType', 'U') IS NOT NULL
  DROP TABLE dbo.TransactionType
CREATE TABLE TransactionType(
	TransactionTypeId INT PRIMARY KEY IDENTITY (1,1),
	TransactionTypeName NVARCHAR(40) NOT NULL,
	TransactionPrice MONEY NOT NULL DEFAULT 0,
)

GO

IF OBJECT_ID('dbo.Customer', 'U') IS NOT NULL
  DROP TABLE dbo.Customer
CREATE TABLE Customer(
	CustomerId INT PRIMARY KEY IDENTITY (1,1),
	Username NVARCHAR(255) NOT NULL,
	Email NVARCHAR(255) NOT NULL,
	Phone NVARCHAR(20) NOT NULL,
	CityId INT NOT NULL,
	UserPassword NVARCHAR(20) NOT NULL,	
	DeleteFlag BIT NOT NULL DEFAULT 0
)

GO

IF OBJECT_ID('dbo.[Admin]', 'U') IS NOT NULL
  DROP TABLE dbo.[Admin]
CREATE TABLE [Admin](
	AdminId INT PRIMARY KEY IDENTITY (1,1),
	AdminName NVARCHAR(30) NOT NULL,
	AdminPassword NVARCHAR(20) NOT NULL,
	RoleId INT NOT NULL,
	DeleteFlag BIT NOT NULL DEFAULT 0
)

GO

IF OBJECT_ID('dbo.AdminRole', 'U') IS NOT NULL
  DROP TABLE dbo.AdminRole
CREATE TABLE AdminRole(
	RoleId INT PRIMARY KEY IDENTITY (1,1),
	RoleName NVARCHAR(50) NOT NULL,
	RoleDescription NVARCHAR(255) NULL
)

GO