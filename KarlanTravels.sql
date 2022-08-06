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
	CountryId NVARCHAR(40) PRIMARY KEY,
	CountryName NVARCHAR(255) NOT NULL,
	Continent NVARCHAR(40) NOT NULL,
	RegionCode NVARCHAR(20) NOT NULL	--Ma~ vung`
)

GO


IF OBJECT_ID('dbo.City', 'U') IS NOT NULL
  DROP TABLE dbo.City
CREATE TABLE City(
	CityId NVARCHAR(40) PRIMARY KEY,
	CityName NVARCHAR(255) NOT NULL,
	CountryId INT NOT NULL,
	PostalCode NVARCHAR(20) NOT NULL,	--Ma~ buu chinh cho shipping, refund,...
)
GO


IF OBJECT_ID('dbo.Facility', 'U') IS NOT NULL
  DROP TABLE dbo.Facility
CREATE TABLE Facility(					-- Co so vat chat(khach san, nha` hang`,...)
	FacilityId NVARCHAR(40) PRIMARY KEY,
	FacilityTypeId NVARCHAR(10) NOT NULL,
	FacilityName NVARCHAR(255) NOT NULL,
	FacilityLocation NVARCHAR(255) NOT NULL,
	CityId NVARCHAR(40) NOT NULL,
	FacilityPrice MONEY NOT NULL,
	FacilityQuality FLOAT NOT NULL DEFAULT 1,	--Cham' diem? tu 1-10 (float)
	Quantity INT NOT NULL DEFAULT 0,
	FacilityAvailability BIT NOT NULL DEFAULT 0,	--Do kha? dung. hien tai (boolean)
	DeleteFlag BIT NOT NULL DEFAULT 0	--Truong danh dau xoa' (boolean)
)

ON [PRIMARY]
GO


INSERT INTO Facility(FacilityId, FacilityTypeId, FacilityName, FacilityLocation, CityId, FacilityPrice, FacilityQuality, Quantity, FacilityAvailability) VALUES
 (N'NONE_F' ,N'NONE_FT', N'None', N'None', N'None', 0, 0, 0, 0)		--Ban ghi rong

GO


IF OBJECT_ID('dbo.FacilityType', 'U') IS NOT NULL
  DROP TABLE dbo.FacilityType
CREATE TABLE FacilityType(
	FacilityTypeId NVARCHAR(40) PRIMARY KEY,
	FacilityTypeName NVARCHAR(255) NOT NULL,
	DeleteFlag BIT NOT NULL DEFAULT 0
)

ON [PRIMARY]
GO

INSERT INTO FacilityType(FacilityTypeId, FacilityTypeName) VALUES
(N'NONE_FT', N'Uncategorized'),
(N'HTEL', N'Hotel'),
(N'RSRT', N'Resort'),
(N'LODG', N'Lodging house'),
(N'RSTR', N'Restaurant'),
(N'SHP_MCHD', N'Merchandise shop'),
(N'BNK', N'Bank'),
(N'STA_VHC', N'Vehicle station'),
(N'STA_TRN', N'Train station'),
(N'STA_DCK', N'Ships docking station'),
(N'AIRPRT', N'Airport')


IF OBJECT_ID('dbo.TouristSpot', 'U') IS NOT NULL
  DROP TABLE dbo.TouristSpot
CREATE TABLE TouristSpot(
	TouristSpotId NVARCHAR(40) PRIMARY KEY,
	TouristSpotName NVARCHAR(255) NOT NULL,
	CityId NVARCHAR(40) NOT NULL,
	CategoryId INT NOT NULL,
	Rating FLOAT NOT NULL DEFAULT 1,	--Cham diem tu 1-10 (float)
	TouristSpotAvailability BIT NOT NULL DEFAULT 0,
	DeleteFlag BIT NOT NULL DEFAULT 0
)

ON [PRIMARY]
GO


INSERT INTO TouristSpot(TouristSpotName, CityId, CategoryId, Rating, TouristSpotAvailability) VALUES
 (N'None', 0, 0, 0, 0)	--Ban ghi rong


GO


IF OBJECT_ID('dbo.Category', 'U') IS NOT NULL
  DROP TABLE dbo.Category
CREATE TABLE Category(
	CategoryId NVARCHAR(40) PRIMARY kEY,
	CategoryName NVARCHAR(255) NOT NULL,
	CategoryNote NVARCHAR(255) NULL,	--Note
	DeleteFlag BIT NOT NULL DEFAULT 0
)

GO


IF OBJECT_ID('dbo.Tour', 'U') IS NOT NULL
  DROP TABLE dbo.Tour
CREATE TABLE Tour(
	TourId NVARCHAR(40) PRIMARY KEY,
	TourName NVARCHAR(255) NOT NULL,
	TourAvailability BIT NOT NULL DEFAULT 0,
	TourStart DATETIME NOT NULL,
	TourEnd DATETIME NOT NULL,
	TourPrice MONEY NOT NULL,
	TourNote NVARCHAR(255) NULL,
	DeleteFlag BIT NOT NULL DEFAULT 0
)

GO


IF OBJECT_ID('dbo.TourDetails', 'U') IS NOT NULL
  DROP TABLE dbo.TourDetails
CREATE TABLE TourDetails(
	TourDetailsId NVARCHAR(40) PRIMARY KEY,
	TourId NVARCHAR(40) NOT NULL,
	TourDetailsName NVARCHAR(255) NOT NULL,
	Activity NVARCHAR(100) NOT NULL,
	ActivityTime NVARCHAR(30) NOT NULL,
	FacilityId NVARCHAR(40) NOT NULL DEFAULT 0,
	TouristSpotsId NVARCHAR(40) NOT NULL DEFAULT 0,
	TourNote NVARCHAR(255) NULL,
	DeleteFlag BIT NOT NULL DEFAULT 0
)

GO

IF OBJECT_ID('dbo.TransactionsRecords', 'U') IS NOT NULL
  DROP TABLE dbo.TransactionsRecords	--Ban ghi cac giao dich
CREATE TABLE TransactionsRecords(
	RecordId NVARCHAR(40) PRIMARY KEY,
	TransactionTypeId INT NOT NULL,
	TourId NVARCHAR(40) NOT NULL,
	CustomerID INT NOT NULL,	--Nguoi mua
	AdminID INT NOT NULL,		--Nguoi edit ban ghi
	DeleteFlag BIT NOT NULL DEFAULT 0
)

GO

IF OBJECT_ID('dbo.TransactionType', 'U') IS NOT NULL
  DROP TABLE dbo.TransactionType
CREATE TABLE TransactionType(
	TransactionTypeId NVARCHAR(40) PRIMARY KEY,
	TransactionTypeName NVARCHAR(40) NOT NULL,
	TransactionDescription NVARCHAR(255) NULL,
	TransactionPriceRate FLOAT NOT NULL DEFAULT 0,	--La` 1 he. so' nhan^ vs gia' cua tour
)

ON [PRIMARY]
GO


INSERT INTO TransactionType(TransactionTypeId, TransactionTypeName, TransactionDescription, TransactionPriceRate) VALUES
(N'DEPOSIT', N'Leave a deposit', N'Leave an amount of money as a deposit', 0),	--Dat. coc.
(N'PURCHSE', N'Purchase tour', N'Normal tour price', 1),
(N'CANCL_EARL', N'Cancel tour early', N'No fee charges, full refund', 0),
(N'CANCL_LATE', N'Cancel tour late', N'(Few days before tour starts) Half refund', 0.5)
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

ON [PRIMARY]
GO


INSERT INTO [Admin](AdminName, AdminPassword ,RoleId) VALUES
(N'NTA', N'40bd001563085fc35165329ea1ff5c5ecbdbbeef', N'SN_MG'),	--Pass 123
(N'VHL', N'51eac6b471a284d3341d8c0c63d0f1a286262a18', N'SALE_MG'),	--Pass 456
(N'VCM', N'fc1200c7a7aa52109d762a9f005b149abef01479', N'TOUR_MG')	--Pass 789

GO


IF OBJECT_ID('dbo.AdminRole', 'U') IS NOT NULL
  DROP TABLE dbo.AdminRole
CREATE TABLE AdminRole(
	RoleId NVARCHAR(40) PRIMARY KEY,
	RoleName NVARCHAR(50) NOT NULL,
	RoleDescription NVARCHAR(255) NULL
)

ON [PRIMARY]
GO

INSERT INTO AdminRole(RoleId, RoleName, RoleDescription) VALUES
(N'SALE_MG',N'Saler', N'Manage sales, trasactions and user accounts, cannot access tour database'),
(N'TOUR_MG', N'Tour Manager', N'Manage the tour database, cannot access sales and transactions records'),
(N'SN_MG', N'Senior Maneger', N'Have access to all divisions, including admin accounts')

GO
