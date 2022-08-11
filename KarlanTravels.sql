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
	CountryCode NVARCHAR(20) NOT NULL
)

GO

IF OBJECT_ID('dbo.City', 'U') IS NOT NULL
  DROP TABLE dbo.City
CREATE TABLE City(
	CityId NVARCHAR(40) PRIMARY KEY,
	CityName NVARCHAR(255) NOT NULL,
	CountryId NVARCHAR(40) FOREIGN KEY REFERENCES Country(CountryId) NOT NULL,
	PostalCode NVARCHAR(20) NOT NULL,
	CityNote NVARCHAR(255) NULL
)

GO

IF OBJECT_ID('dbo.FacilityType', 'U') IS NOT NULL
  DROP TABLE dbo.FacilityType
CREATE TABLE FacilityType(
	FacilityTypeId NVARCHAR(40) PRIMARY KEY,
	FacilityTypeName NVARCHAR(255) NOT NULL,
	FacilityTypeNote NVARCHAR(255) NULL,
	DeleteFlag BIT NOT NULL DEFAULT 0
)

GO

IF OBJECT_ID('dbo.Category', 'U') IS NOT NULL
  DROP TABLE dbo.Category
CREATE TABLE Category(
	CategoryId NVARCHAR(40) PRIMARY KEY,
	CategoryName NVARCHAR(255) NOT NULL,
	CategoryNote NVARCHAR(255) NULL,
	DeleteFlag BIT NOT NULL DEFAULT 0
)

GO

IF OBJECT_ID('dbo.TransactionType', 'U') IS NOT NULL
  DROP TABLE dbo.TransactionType
CREATE TABLE TransactionType(
	TransactionTypeId NVARCHAR(40) PRIMARY KEY,
	TransactionTypeName NVARCHAR(40) NOT NULL,
	TransactionTypeNote NVARCHAR(255) NULL,
	TransactionPriceRate FLOAT NOT NULL DEFAULT 0
)

GO

IF OBJECT_ID('dbo.AdminRole', 'U') IS NOT NULL
  DROP TABLE dbo.AdminRole
CREATE TABLE AdminRole(
	RoleId NVARCHAR(40) PRIMARY KEY,
	RoleName NVARCHAR(50) NOT NULL,
	RoleNote NVARCHAR(255) NULL
)

GO

IF OBJECT_ID('dbo.Customer', 'U') IS NOT NULL
  DROP TABLE dbo.Customer
CREATE TABLE Customer(
	CustomerId INT PRIMARY KEY IDENTITY (1,1),
	Username NVARCHAR(255) UNIQUE NOT NULL,
	Email NVARCHAR(255) UNIQUE NOT NULL,
	Phone NVARCHAR(20) UNIQUE NOT NULL,
	CityId NVARCHAR(40) FOREIGN KEY REFERENCES City(CityId) NOT NULL,
	UserPassword NVARCHAR(20) NOT NULL,	
	AmountToPay MONEY NOT NULL DEFAULT 0,
	CustomerNote NVARCHAR(255) NULL,
	DeleteFlag BIT NOT NULL DEFAULT 0
)

GO

IF OBJECT_ID('dbo.[Admin]', 'U') IS NOT NULL
  DROP TABLE dbo.[Admin]
CREATE TABLE [Admin](
	AdminId INT PRIMARY KEY IDENTITY (1,1),
	AdminName NVARCHAR(30) UNIQUE NOT NULL,
	AdminPassword NVARCHAR(255) NOT NULL,
	RoleId NVARCHAR(40) NOT NULL FOREIGN KEY REFERENCES AdminRole(RoleId),
	AdminNote NVARCHAR(255) NULL,
	DeleteFlag BIT NOT NULL DEFAULT 0
)

GO

IF OBJECT_ID('dbo.Facility', 'U') IS NOT NULL
	DROP TABLE dbo.Facility
CREATE TABLE Facility(					
	FacilityId NVARCHAR(40) PRIMARY KEY,
	FacilityName NVARCHAR(255) NOT NULL,
	FacilityTypeId NVARCHAR(40) FOREIGN KEY REFERENCES FacilityType(FacilityTypeId) NOT NULL,	
	FacilityLocation NVARCHAR(255) NOT NULL,
	CityId NVARCHAR(40) FOREIGN KEY REFERENCES City(CityId) NOT NULL,
	FacilityPrice MONEY NOT NULL,
	FacilityQuality FLOAT NOT NULL DEFAULT 1,
	Quantity INT NOT NULL DEFAULT 0,
	FacilityNote NVARCHAR(255) NULL,
	FacilityAvailability BIT NOT NULL DEFAULT 0,
	DeleteFlag BIT NOT NULL DEFAULT 0
)

GO

IF OBJECT_ID('dbo.TouristSpot', 'U') IS NOT NULL
	 DROP TABLE dbo.TouristSpot
CREATE TABLE TouristSpot(
	TouristSpotId NVARCHAR(40) PRIMARY KEY,
	TouristSpotName NVARCHAR(255) NOT NULL,
	CityId NVARCHAR(40) FOREIGN KEY REFERENCES City(CityId) NOT NULL,
	CategoryId NVARCHAR(40) FOREIGN KEY REFERENCES Category(CategoryId) NOT NULL,
	Rating FLOAT NOT NULL DEFAULT 1,
	TouristSpotAvailability BIT NOT NULL DEFAULT 0,
	TouristSpotNote NVARCHAR(255) NULL,
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

IF OBJECT_ID('dbo.TourDetail', 'U') IS NOT NULL
  DROP TABLE dbo.TourDetail
CREATE TABLE TourDetail(
	TourDetailId INT PRIMARY KEY IDENTITY(1,1),
	TourDetailName NVARCHAR(255) NOT NULL,
	Activity NVARCHAR(255) NOT NULL,
	ActivityTimeStart DATETIME NOT NULL,
	ActivityTimeEnd DATETIME NOT NULL,
	TouristSpotId NVARCHAR(40) NOT NULL DEFAULT 0 FOREIGN KEY REFERENCES TouristSpot(TouristSpotId),
	FacilityId NVARCHAR(40) NOT NULL DEFAULT 0 FOREIGN KEY REFERENCES Facility(FacilityId),
	ActivityNote NVARCHAR(255) NULL,
	TourId NVARCHAR(40) NOT NULL FOREIGN KEY REFERENCES Tour(TourId),
	DeleteFlag BIT NOT NULL DEFAULT 0
)

GO

IF OBJECT_ID('dbo.TransactionRecord', 'U') IS NOT NULL
  DROP TABLE dbo.TransactionRecord
CREATE TABLE TransactionRecord(
	TransactionRecordId INT PRIMARY KEY IDENTITY(1,1),
	TransactionTypeId NVARCHAR(40) FOREIGN KEY REFERENCES TransactionType(TransactionTypeId) NOT NULL,
	TourId NVARCHAR(40) FOREIGN KEY REFERENCES Tour(TourId) NOT NULL,
	CustomerID INT FOREIGN KEY REFERENCES Customer(CustomerID) NOT NULL,
	TransactionFee MONEY NOT NULL,
	Paid BIT NOT NULL DEFAULT 0,
	AdminID INT FOREIGN KEY REFERENCES [Admin](AdminID) NOT NULL,
	TransactionNote NVARCHAR(255) NULL,
	DeleteFlag BIT NOT NULL DEFAULT 0
)


ON [PRIMARY]
GO


INSERT INTO Country(CountryId, CountryName, Continent, CountryCode) VALUES
(N'none', N'none', N'none', N'none'),
(N'VN', N'Việt Nam', N'Asia', N'+84')


INSERT INTO City(CityId, CityName, CountryId, PostalCode) VALUES
(N'none', N'none', N'none', 'none'),
(N'VN_HN', N'Hà Nội', N'VN', N'100000'),
(N'VN_CB', N'Cao Bằng', N'VN', N'270000'),
(N'VN_MC', N'Mai Châu', N'VN', N'350000'),
(N'VN_VY', N'Vĩnh Yên', N'VN', N'280000')


INSERT INTO TransactionType(TransactionTypeId, TransactionTypeName, TransactionTypeNote, TransactionPriceRate) VALUES
(N'PURCHSE', N'Purchase tour', N'Normal tour price', 1),
(N'CANCL_EARL', N'Cancel tour early', N'No fee charges, full refund', 0),
(N'CANCL_LATE', N'Cancel tour late', N'(Few days before tour starts) Half refund', 0.5)


INSERT INTO Tour(TourId, TourName, TourAvailability, TourStart, TourEnd, TourPrice, TourNote) VALUES
(N'T_HNtoCB01', N'Hà Nội - Cao Bằng', 1, CAST(N'2022-08-12T06:00:00.000' AS DateTime), CAST(N'2022-08-13T22:00:00.000' AS DateTime), 2350000.0000, NULL),
(N'T_HNtoMC01', N'Hà Nội - Mộc Châu', 1, CAST(N'2022-09-13T06:00:00.000' AS DateTime), CAST(N'2022-09-14T18:00:00.000' AS DateTime), 1580000.0000, NULL),
(N'T_HNtoTD01', N'Hà Nội - Tam Đảo', 1, CAST(N'2022-10-21T07:00:00.000' AS DateTime), CAST(N'2022-10-22T18:00:00.000' AS DateTime), 1350000.0000, NULL)


INSERT INTO FacilityType(FacilityTypeId, FacilityTypeName) VALUES
(N'UNTP', N'Untyped'),
(N'HTEL', N'Hotel'),
(N'RSRT', N'Resort'),
(N'LODG', N'Lodging house'),
(N'RSTR', N'Restaurant'),
(N'SHP', N'Merchandise shop'),
(N'BNK', N'Bank'),
(N'STA_VHC', N'Vehicle station'),
(N'STA_TRN', N'Train station'),
(N'STA_DCK', N'Ships docking station'),
(N'AIRPRT', N'Airport')


INSERT INTO Category(CategoryId, CategoryName, CategoryNote) VALUES
(N'UNCG', N'Uncategorized', NULL),
(N'WTER', N'Bodies of water', N'Rivers, lakes, ponds, waterfalls,...'),
(N'HIST', N'Historical structure', NULL),
(N'CAVE', N'Caves, grottos', NULL),
(N'CULTR', N'Cultural attractions', N'Museums, libraries, old streets,...'),
(N'HGHLN', N'Highlands', N'Mountains, hills')


INSERT INTO TouristSpot(TouristSpotId, TouristSpotName, CityId, CategoryId, Rating, TouristSpotAvailability) VALUES 
(N'none', N'none', N'none', N'UNCG', 0, 1),
(N'TS_CB_BB01', N'Ba Bể Lake', N'VN_CB', N'WTER', 5, 1),
(N'TS_HN_TMC01', N'Trái tim Mộc Châu Tea Hill', N'VN_HN', N'HGHLN', 5, 1),
(N'TS_HN_B01', N'Bạc Waterfall', N'VN_HN', N'WTER', 5, 1),
(N'TS_HN_CC01', N'Cát Cát Village', N'VN_HN', N'CULTR', 5, 1),
(N'TS_CB_PTL01', N'Phật tích Trúc Lâm Pagoda of Bản Giốc', N'VN_CB', N'HIST', 5, 1),
(N'TS_CB_NN01', N'Ngườm Ngao Cave', N'VN_CB', N'CAVE', 5, 1),
(N'TS_CB_BG02', N'Bản Giốc Waterfall', N'VN_CB', N'WTER', 5, 1),
(N'TS_CB_D01', N'Đuổm Buddhist Temple', N'VN_CB', N'HIST', 5, 1),
(N'TS_CB_AM01', N'An Mã Buddhist Temple', N'VN_CB', N'HIST', 5, 1),
(N'TS_HN_P01', N'Puông Cave', N'VN_HN', N'CAVE', 6, 1),
(N'TS_MC_TK01', N'Thung Khe Pass', N'VN_MC', N'HGHLN', 5, 1),
(N'TS_HN_DY01', N'Dải Yếm Waterfall', N'VN_HN', N'WTER', 5, 0)


INSERT INTO Facility(FacilityId, FacilityName, FacilityTypeId, FacilityLocation, CityId, FacilityPrice, FacilityQuality, Quantity, FacilityNote, FacilityAvailability) VALUES
(N'none', N'none', N'UNTP', N'', N'none', 0, 0, 0, NULL, 1),	--8
(N'FC_HN_HV01', N'Hải Vân Bus Station', N'STA_VHC', N'23, Nguyễn Khuyến road', N'VN_HN', 0.0000, 5, 20, N'29 seats per 20 bus', 1),
(N'FC_HN_NN01', N'Nguyễn Nhật Bus Station', N'STA_VHC', N'25 Văn Quán street', N'VN_HN', 0.0000, 3, 7, N'16 seats per 7 bus', 1),
(N'FC_MC_HT01', N'Hợp Thủy Restaurant', N'RSTR', N'Subdivision 2, Mai Châu district', N'VN_MC', 0.0000, 0, 20, N'4 seats per 20 tables', 1),
(N'FC_VY_NA01', N'Nguyệt Anh Hotel', N'HTEL', N'Block 1, Tam Đảo district', N'VN_VY', 0.0000, 0, 12, N'12 single rooms, 15 tables with 4 seats', 1),
(N'FC_VY_GTD01', N'Gió Tam Đảo Cafe', N'RSTR', N'Tam Đảo district, Vĩnh Phúc province', N'VN_VY', 0.0000, 0, 16, N'3 seats per 16 tables', 1),
(N'FC_VY_PN01', N'Phố Núi Restaurant', N'RSTR', N'Near Hợp Châu town''s people''s Committee , Tam Đảo district', N'VN_VY', 0.0000, 0, 6, N'3 seats per 6 tables', 1),
(N'FC_HN_BND01', N'Bảo Ngọc Diamond Hotel', N'HTEL', N'1st floor, 124 Bế Văn Đàn street, Hợp Giang sub-district', N'VN_CB', 0.0000, 3, 10, N'10 double rooms, 12 four seats tables', 1),
(N'FC_CB_TL01', N'Thành Loan Hotel', N'HTEL', N'131 Vườn Cam street', N'VN_CB', 0.0000, 9, 9, N'9 triple rooms, 4 ten seats tables', 1),
(N'FC_HN_TN01', N'Thành Nam Bus Station', N'STA_VHC', N'29 Mộ Lao street', N'VN_HN', 0.0000, 5, 16, N'42 seats per 16 buses', 1),
(N'FC_CB_BG01', N'Bản Giốc Restaurant', N'RSTR', N'Bản Giốc hamlet, Đàm Thủy commune, Trùng Khánh district', N'VN_CB', 0.0000, 4, 18, N'3 seats per 18 tables', 1),
(N'FC_MC_EE01', N'88 Hotel', N'HTEL', N'102 Hoàng Quốc Việt street', N'VN_MC', 0.0000, 0, 5, N'5 double rooms, 2 seats per 5 tables', 1),
(N'FC_MC_BCE01', N'Ba Chị Em Restaurant', N'RSTR', N'Chiềng Sại hamlet', N'VN_MC', 0.0000, 0, 15, N'2 seats per 15 tables', 1)


INSERT INTO TourDetail(TourId, TourDetailName, TouristSpotId, FacilityId, Activity, ActivityTimeStart, ActivityTimeEnd, ActivityNote) VALUES
(N'T_HNtoCB01', N'Check out of the hotel', N'none', N'FC_CB_TL01', N'Return to the hotel and check out, prepare for your return trip', CAST(N'2022-08-12T06:00:00.000' AS DateTime), CAST(N'2022-08-12T06:00:00.000' AS DateTime), NULL),
(N'T_HNtoCB01', N'Board the bus', N'none', N'FC_HN_HV01', N'Gather at Hai Van bus station and prepare to depart', CAST(N'2022-08-12T06:00:00.000' AS DateTime), CAST(N'2022-08-12T06:00:00.000' AS DateTime), NULL),
(N'T_HNtoCB01', N'Visit Puông Cave', N'TS_HN_P01', N'none', N'Sightseeing at Puông Cave', CAST(N'2022-08-12T06:00:00.000' AS DateTime), CAST(N'2022-08-12T06:00:00.000' AS DateTime), NULL),
(N'T_HNtoCB01', N'Get back to the hotel', N'none', N'FC_HN_HV01', N'Return to Thành Loan Hotel on bus', CAST(N'2022-08-12T06:00:00.000' AS DateTime), CAST(N'2022-08-12T06:00:00.000' AS DateTime), NULL),
(N'T_HNtoCB01', N'Have dinner and rest', N'none', N'FC_CB_TL01', N'Have dinner and rest at the Thành Loan Hotel', CAST(N'2022-08-12T06:00:00.000' AS DateTime), CAST(N'2022-08-12T06:00:00.000' AS DateTime), NULL),
(N'T_HNtoCB01', N'Board the bus', N'TS_CB_BG02', N'none', N'Travel to Bản Giốc Waterfall', CAST(N'2022-08-12T06:00:00.000' AS DateTime), CAST(N'2022-08-12T06:00:00.000' AS DateTime), NULL),
(N'T_HNtoCB01', N'Have lunch', N'none', N'FC_CB_BG01', N'Have lunch at Bản Giốc Restaurant', CAST(N'2022-08-12T06:00:00.000' AS DateTime), CAST(N'2022-08-12T06:00:00.000' AS DateTime), NULL),
(N'T_HNtoCB01', N'Visit Bản Giốc Waterfall', N'TS_CB_BG02', N'none', N'Sightseeing at Bản Giốc Waterfall', CAST(N'2022-08-12T06:00:00.000' AS DateTime), CAST(N'2022-08-12T06:00:00.000' AS DateTime), NULL),
(N'T_HNtoCB01', N'Board the bus', N'none', N'FC_HN_HV01', N'Move to Phật tích Trúc Lâm Pagoda', CAST(N'2022-08-12T06:00:00.000' AS DateTime), CAST(N'2022-08-12T06:00:00.000' AS DateTime), NULL),
(N'T_HNtoCB01', N'Visit the Phật tích Trúc Lâm Pagoda', N'TS_CB_PTL01', N'none', N'Freely visit the Phật tích Trúc Lâm pagoda and offer incense', CAST(N'2022-08-12T06:00:00.000' AS DateTime), CAST(N'2022-08-12T06:00:00.000' AS DateTime), NULL),
(N'T_HNtoCB01', N'Board the bus', N'none', N'FC_HN_HV01', N'Move to Ngườm Ngao Cave', CAST(N'2022-08-12T06:00:00.000' AS DateTime), CAST(N'2022-08-12T06:00:00.000' AS DateTime), NULL),
(N'T_HNtoCB01', N'Visit Ngườm Ngao', N'TS_CB_NN01', N'none', N'Sightseeing at Ngườm Ngao Cave', CAST(N'2022-08-12T06:00:00.000' AS DateTime), CAST(N'2022-08-12T06:00:00.000' AS DateTime), NULL),
(N'T_HNtoCB01', N'Visit Đuồm Pagoda', N'TS_CB_D01', N'none', N'Visit Đuồm Pagoda', CAST(N'2022-08-12T06:00:00.000' AS DateTime), CAST(N'2022-08-12T06:00:00.000' AS DateTime), NULL),
(N'T_HNtoCB01', N'Check out of the hotel', N'none', N'FC_CB_TL01', N'Pack your belongings and checkout', CAST(N'2022-08-12T06:00:00.000' AS DateTime), CAST(N'2022-08-12T06:00:00.000' AS DateTime), NULL),
(N'T_HNtoCB01', N'Check in and have lunch at the hotel', N'none', N'FC_CB_TL01', N'Have lunch and check in at Thành Loan Hotel', CAST(N'2022-08-12T06:00:00.000' AS DateTime), CAST(N'2022-08-12T06:00:00.000' AS DateTime), NULL),
(N'T_HNtoCB01', N'Have lunch and rest', N'none', N'FC_HN_BND01', N'Have lunch and rest at the Bảo Ngọc Diamond Hotel', CAST(N'2022-08-12T06:00:00.000' AS DateTime), CAST(N'2022-08-12T06:00:00.000' AS DateTime), NULL),
(N'T_HNtoCB01', N'Board the bus', N'none', N'FC_HN_HV01', N'Make way to Ba Bể Lake ', CAST(N'2022-08-12T06:00:00.000' AS DateTime), CAST(N'2022-08-12T06:00:00.000' AS DateTime), NULL),
(N'T_HNtoCB01', N'Visit Ba Bể Lake', N'TS_CB_BB01', N'none', N'Visit Ba Bể Lake and its surround jungles', CAST(N'2022-08-12T06:00:00.000' AS DateTime), CAST(N'2022-08-12T06:00:00.000' AS DateTime), NULL),
(N'T_HNtoCB01', N'Board the bus', N'none', N'FC_HN_HV01', N'Go to An Mã Pagoda', CAST(N'2022-08-12T06:00:00.000' AS DateTime), CAST(N'2022-08-12T06:00:00.000' AS DateTime), NULL),
(N'T_HNtoCB01', N'Visit An Mã Pagoda', N'TS_CB_AM01', N'none', N'Sightseeing at An Mã Pagoda', CAST(N'2022-08-12T06:00:00.000' AS DateTime), CAST(N'2022-08-12T06:00:00.000' AS DateTime), NULL),
(N'T_HNtoCB01', N'Board the bus', N'none', N'FC_HN_HV01', N'Make way to Puông Cave', CAST(N'2022-08-12T06:00:00.000' AS DateTime), CAST(N'2022-08-12T06:00:00.000' AS DateTime), NULL),
(N'T_HNtoCB01', N'Tour ends', N'none', N'FC_HN_HV01', N'Return to Hà Nội as the end of your jorney', CAST(N'2022-08-12T06:00:00.000' AS DateTime), CAST(N'2022-08-12T06:00:00.000' AS DateTime), NULL),
----
(N'T_HNtoMC01', N'Tour starts', N'none', N'FC_HN_TN01', N'Board the bus and make your way to Thung Khe Pass', CAST(N'2022-09-12T07:00:00.000' AS DateTime), CAST(N'2022-09-12T08:30:00.000' AS DateTime), NULL),
(N'T_HNtoMC01', N'Visit Thung Khe Pass', N'TS_MC_TK01', N'none', N'Sightseeing as you climb through the pass', CAST(N'2022-09-12T08:30:00.000' AS DateTime), CAST(N'2022-09-12T10:00:00.000' AS DateTime), NULL),
(N'T_HNtoMC01', N'Board the bus', N'none', N'FC_HN_TN01', N'Move to Ba Chị Em Restaurant', CAST(N'2022-09-12T10:00:00.000' AS DateTime), CAST(N'2022-09-12T11:00:00.000' AS DateTime), NULL),
(N'T_HNtoMC01', N'Have lunch at a local restaurant', N'none', N'FC_MC_BCE01', N'Have lunch and a short break at Ba Chị Em Restaurant', CAST(N'2022-09-12T11:00:00.000' AS DateTime), CAST(N'2022-09-12T14:00:00.000' AS DateTime), NULL),
(N'T_HNtoMC01', N'Board the bus', N'none', N'FC_HN_TN01', N'Go to Dải Yếm Waterfall', CAST(N'2022-09-12T14:00:00.000' AS DateTime), CAST(N'2022-09-12T14:45:00.000' AS DateTime), NULL),
(N'T_HNtoMC01', N'Visit Dải Yếm Waterfall', N'TS_HN_DY01', N'none', N'Sightseeing at Dải Yếm Waterfalls', CAST(N'2022-09-12T14:45:00.000' AS DateTime), CAST(N'2022-09-12T16:00:00.000' AS DateTime), NULL),
(N'T_HNtoMC01', N'Have dinner and rest', N'none', N'FC_HN_TN01', N'Return to have dinner and stay the night at the 88 Hotel', CAST(N'2022-09-12T16:00:00.000' AS DateTime), CAST(N'2022-09-12T17:00:00.000' AS DateTime), NULL),
(N'T_HNtoMC01', N'Have dinner and rest', N'none', N'FC_MC_EE01', N'Have dinner and stay the night at 88 Hotel', CAST(N'2022-09-12T17:00:00.000' AS DateTime), CAST(N'2022-09-13T07:00:00.000' AS DateTime), NULL),
(N'T_HNtoMC01', N'Board the bus', N'none', N'FC_HN_TN01', N'Check out of the hotel and move to the tea hill', CAST(N'2022-09-13T07:00:00.000' AS DateTime), CAST(N'2022-09-13T08:30:00.000' AS DateTime), NULL),
(N'T_HNtoMC01', N'Visit Trái tim Mộc Châu tea hill', N'TS_HN_TMC01', N'none', N'Sightseeing at take pictures at the Trái tim Mộc Châu tea hill', CAST(N'2022-09-13T08:00:00.000' AS DateTime), CAST(N'2022-09-13T11:00:00.000' AS DateTime), NULL),
(N'T_HNtoMC01', N'Board the bus', N'none', N'FC_HN_TN01', N'Make way to the restaurant for lunch', CAST(N'2022-09-13T11:00:00.000' AS DateTime), CAST(N'2022-09-13T11:45:00.000' AS DateTime), NULL),
(N'T_HNtoMC01', N'Have lunch and rest', N'none', N'FC_MC_HT01', N'Have lunch and a short break at Hợp Thủy Restaurant', CAST(N'2022-09-13T11:45:00.000' AS DateTime), CAST(N'2022-09-13T14:00:00.000' AS DateTime), NULL),
(N'T_HNtoMC01', N'Tour ends', N'none', N'FC_HN_TN01', N'The bus travels back to Hà Nội', CAST(N'2022-09-13T14:00:00.000' AS DateTime), CAST(N'2022-09-13T18:00:00.000' AS DateTime), NULL),
----
(N'T_HNtoTD01', N'Tour starts', N'none', N'FC_HN_NN01', N'Board the bus and make way to the hotel', CAST(N'2022-12-04T06:00:00.000' AS DateTime), CAST(N'2022-12-04T06:00:00.000' AS DateTime), NULL),
(N'T_HNtoTD01', N'Check in the hotel', N'none', N'FC_VY_NA01', N'Check in at Nguyệt Anh Hotel', CAST(N'2022-12-04T06:00:00.000' AS DateTime), CAST(N'2022-12-04T06:00:00.000' AS DateTime), NULL),
(N'T_HNtoTD01', N'Have lunch and rest', N'none', N'FC_VY_NA01', N'Have lunch and a break at Nguyệt Anh Hotel', CAST(N'2022-12-04T06:00:00.000' AS DateTime), CAST(N'2022-12-04T06:00:00.000' AS DateTime), NULL),
(N'T_HNtoTD01', N'Board the bus', N'none', N'FC_HN_NN01', N'Move to Bạc Waterfall', CAST(N'2022-12-04T06:00:00.000' AS DateTime), CAST(N'2022-12-04T06:00:00.000' AS DateTime), NULL),
(N'T_HNtoTD01', N'Visit Bạc Waterfall', N'TS_HN_B01', N'none', N'Sightseeing at Bạc Waterfall', CAST(N'2022-12-04T06:00:00.000' AS DateTime), CAST(N'2022-12-042T06:00:00.000' AS DateTime), NULL),
(N'T_HNtoTD01', N'Board the bus', N'none', N'FC_HN_NN01', N'Return to the hotel', CAST(N'2022-12-04T06:00:00.000' AS DateTime), CAST(N'2022-12-04T06:00:00.000' AS DateTime), NULL),
(N'T_HNtoTD01', N'Have dinner and rest', N'none', N'FC_VY_NA01', N'Have dinner and stay the night at Nguyệt Anh Hotel', CAST(N'2022-12-04T06:00:00.000' AS DateTime), CAST(N'2022-08-12T06:00:00.000' AS DateTime), NULL),
(N'T_HNtoTD01', N'Check out and board the bus', N'none', N'FC_HN_NN01', N'Check out of the hotel and move to Cát Cát Village', CAST(N'2022-12-04T06:00:00.000' AS DateTime), CAST(N'2022-12-04T06:00:00.000' AS DateTime), NULL),
(N'T_HNtoTD01', N'Visit Cát Cát Village', N'TS_HN_CC01', N'none', N'Visit Cát Cát Village', CAST(N'2022-12-04T06:00:00.000' AS DateTime), CAST(N'2022-12-04T06:00:00.000' AS DateTime), NULL),
(N'T_HNtoTD01', N'Board the bus', N'none', N'FC_HN_NN01', N'Go to Phố Núi Restaurant', CAST(N'2022-12-04T06:00:00.000' AS DateTime), CAST(N'2022-12-04T06:00:00.000' AS DateTime), NULL),
(N'T_HNtoTD01', N'Have lunch and rest', N'none', N'FC_VY_PN01', N'Have lunch and a short break at the restaurant', CAST(N'2022-12-04T06:00:00.000' AS DateTime), CAST(N'2022-12-04T06:00:00.000' AS DateTime), NULL),
(N'T_HNtoTD01', N'Board the bus', N'none', N'FC_HN_NN01', N'Make your way to Gió Tam Đảo Cafe', CAST(N'2022-12-04T06:00:00.000' AS DateTime), CAST(N'2022-12-04T06:00:00.000' AS DateTime), NULL),
(N'T_HNtoTD01', N'Enjoy local coffe at Gió Tam Đảo Cafe', N'none', N'FC_VY_GTD01', N'Have coffe, take photos, and sightseeing at the Gió Tam Đảo Cafe', CAST(N'2022-12-04T06:00:00.000' AS DateTime), CAST(N'2022-12-04T06:00:00.000' AS DateTime), NULL),
(N'T_HNtoTD01', N'Tour ends', N'none', N'FC_HN_NN01', N'Return to Hà Nội', CAST(N'2022-12-04T06:00:00.000' AS DateTime), CAST(N'2022-12-04T06:00:00.000' AS DateTime), NULL)


GO


select * from TourDetail where TourId = 'T_HNtoMC01' order by ActivityTimeStart
