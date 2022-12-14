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
	RegionCode NVARCHAR(20) NOT NULL,
	Deleted BIT NOT NULL DEFAULT 0
)

GO

IF OBJECT_ID('dbo.City', 'U') IS NOT NULL
  DROP TABLE dbo.City
CREATE TABLE City(
	CityId NVARCHAR(40) PRIMARY KEY,
	CityName NVARCHAR(255) NOT NULL,
	CountryId NVARCHAR(40) FOREIGN KEY REFERENCES Country(CountryId) NOT NULL,
	PostalCode NVARCHAR(20) NOT NULL,
	CityNote NVARCHAR(255) NULL,
	Deleted BIT NOT NULL DEFAULT 0
)

GO

IF OBJECT_ID('dbo.FacilityType', 'U') IS NOT NULL
  DROP TABLE dbo.FacilityType
CREATE TABLE FacilityType(
	FacilityTypeId NVARCHAR(40) PRIMARY KEY,
	FacilityTypeName NVARCHAR(255) NOT NULL,
	FacilityTypeNote NVARCHAR(255) NULL,
	Deleted BIT NOT NULL DEFAULT 0
)

GO

IF OBJECT_ID('dbo.Category', 'U') IS NOT NULL
  DROP TABLE dbo.Category
CREATE TABLE Category(
	CategoryId NVARCHAR(40) PRIMARY KEY,
	CategoryName NVARCHAR(255) NOT NULL,
	CategoryNote NVARCHAR(255) NULL,
	Deleted BIT NOT NULL DEFAULT 0
)

IF OBJECT_ID('dbo.SubCategory', 'U') IS NOT NULL
  DROP TABLE dbo.SubCategory
CREATE TABLE SubCategory(
	SubCategoryId NVARCHAR(40) PRIMARY KEY,
	SubCategoryName NVARCHAR(255) NOT NULL,
	SubCategoryNote NVARCHAR(255) NULL,
	CategoryId NVARCHAR(40) FOREIGN KEY REFERENCES Category(CategoryId) NOT NULL,
	Deleted BIT NOT NULL DEFAULT 0
)

GO

IF OBJECT_ID('dbo.AdminRole', 'U') IS NOT NULL
  DROP TABLE dbo.AdminRole
CREATE TABLE AdminRole(
	RoleId NVARCHAR(40) PRIMARY KEY,
	RoleName NVARCHAR(50) NOT NULL,
	RoleNote NVARCHAR(255) NULL,
	Deleted BIT NOT NULL DEFAULT 0
)

GO

IF OBJECT_ID('dbo.Bank', 'U') IS NOT NULL
  DROP TABLE dbo.Bank
CREATE TABLE Bank(
	BankId NVARCHAR(40) PRIMARY KEY,
	BankName NVARCHAR(255) NOT NULL,
	SwiftCode NVARCHAR(255) UNIQUE NOT NULL,
	CountryId NVARCHAR(40) FOREIGN KEY REFERENCES Country(CountryId) NOT NULL,
	Deleted BIT NOT NULL DEFAULT 0
)

GO

IF OBJECT_ID('dbo.BankAccount', 'U') IS NOT NULL
  DROP TABLE dbo.BankAccount
CREATE TABLE BankAccount(
	BankAccountId INT PRIMARY KEY IDENTITY (1,1),
	AccountName NVARCHAR(255) NOT NULL,
	AccountNumber NVARCHAR(40) NOT NULL UNIQUE,
	Balance MONEY NOT NULL DEFAULT 0,
	BankId NVARCHAR(40) FOREIGN KEY REFERENCES Bank(BankId) NOT NULL,
	Deleted BIT NOT NULL DEFAULT 0
)

GO

IF OBJECT_ID('dbo.Customer', 'U') IS NOT NULL
  DROP TABLE dbo.Customer
CREATE TABLE Customer(
	CustomerId INT PRIMARY KEY IDENTITY (1,1),
	Username NVARCHAR(255) UNIQUE NOT NULL,
	Email NVARCHAR(255) UNIQUE NOT NULL,
	Phone NVARCHAR(20) UNIQUE NOT NULL,
	BankAccountId INT FOREIGN KEY REFERENCES BankAccount(BankAccountId) NULL,
	CityId NVARCHAR(40) FOREIGN KEY REFERENCES City(CityId) NOT NULL,
	UserPassword NVARCHAR(255) NOT NULL,
	AmountToPay MONEY NOT NULL DEFAULT 0,
	AmountToRefund MONEY NOT NULL DEFAULT 0,
	Violations INT NOT NULL DEFAULT 0,
	CustomerNote NVARCHAR(255) NULL,
	Deleted BIT NOT NULL DEFAULT 0
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
	Deleted BIT NOT NULL DEFAULT 0
)

GO

IF OBJECT_ID('dbo.Facility', 'U') IS NOT NULL
	DROP TABLE dbo.Facility
CREATE TABLE Facility(					
	FacilityId NVARCHAR(40) PRIMARY KEY,
	FacilityName NVARCHAR(255) NOT NULL,
	FacilityTypeId NVARCHAR(40) FOREIGN KEY REFERENCES FacilityType(FacilityTypeId) NOT NULL,	
	FacilityLocation NVARCHAR(255) NOT NULL,
	FacilitySocials NVARCHAR(255) NULL,
	FacilityPhone NVARCHAR(255) NULL,
	FacilityEmail NVARCHAR(255) NULL,
	CityId NVARCHAR(40) FOREIGN KEY REFERENCES City(CityId) NOT NULL,
	Quantity INT NOT NULL DEFAULT 0,
	FacilityImage NVARCHAR(255) NULL,
	ServiceNote NVARCHAR(255) NOT NULL,
	FacilityAvailability BIT NOT NULL DEFAULT 0,
	Deleted BIT NOT NULL DEFAULT 0
)

GO

IF OBJECT_ID('dbo.TouristSpot', 'U') IS NOT NULL
	 DROP TABLE dbo.TouristSpot
CREATE TABLE TouristSpot(
	TouristSpotId NVARCHAR(40) PRIMARY KEY,
	TouristSpotName NVARCHAR(255) NOT NULL,
	CityId NVARCHAR(40) FOREIGN KEY REFERENCES City(CityId) NOT NULL,
	SubCategoryId NVARCHAR(40) FOREIGN KEY REFERENCES SubCategory(SubCategoryId) NOT NULL,
	TouristSpotLocation NVARCHAR(255) NOT NULL,
	TouristSpotRating FLOAT NOT NULL DEFAULT 1,
	OpenHour BIGINT NOT NULL,
	ClosingHour BIGINT NOT NULL,
	TouristSpotAvailability BIT NOT NULL DEFAULT 0,
	TouristSpotImage NVARCHAR(255) NULL,
	Cord_Lat NVARCHAR(20) NULL,
	Cord_Long NVARCHAR(20) NULL,
	TouristSpotNote NVARCHAR(255) NULL,
	Deleted BIT NOT NULL DEFAULT 0
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
	CategoryId1 NVARCHAR(40) FOREIGN KEY REFERENCES Category(CategoryId) NOT NULL,
	CategoryId2 NVARCHAR(40) FOREIGN KEY REFERENCES Category(CategoryId) NULL,
	MaxBooking INT NOT NULL,
	BookTimeLimit INT NOT NULL,
	TourRating FLOAT NOT NULL DEFAULT 1,
	TourImage NVARCHAR(255) NULL,
	TourNote NVARCHAR(255) NULL,
	Deleted BIT NOT NULL DEFAULT 0
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
	TouristSpotId NVARCHAR(40) NOT NULL FOREIGN KEY REFERENCES TouristSpot(TouristSpotId),
	FacilityId NVARCHAR(40) NOT NULL FOREIGN KEY REFERENCES Facility(FacilityId),
	ActivityNote NVARCHAR(255) NULL,
	TourId NVARCHAR(40) NOT NULL FOREIGN KEY REFERENCES Tour(TourId),
	Deleted BIT NOT NULL DEFAULT 0
)

GO

IF OBJECT_ID('dbo.TransactionRecord', 'U') IS NOT NULL
  DROP TABLE dbo.TransactionRecord
CREATE TABLE TransactionRecord(
	TransactionRecordId INT PRIMARY KEY IDENTITY(1,1),
	TourId NVARCHAR(40) FOREIGN KEY REFERENCES Tour(TourId) NOT NULL,
	CustomerID INT FOREIGN KEY REFERENCES Customer(CustomerID) NOT NULL,
	Quantity INT NOT NULL DEFAULT 1,
	TransactionFee MONEY NOT NULL,
	RecordedTime DATETIME NOT NULL,
	DueDate DATETIME NOT NULL,
	Paid BIT NOT NULL DEFAULT 0,
	Canceled BIT NOT NULL DEFAULT 0,
	TransactionNote NVARCHAR(255) NULL,
	Deleted BIT NOT NULL DEFAULT 0
)

ON [PRIMARY]
GO


INSERT INTO Country(CountryId, CountryName, Continent, RegionCode) VALUES
(N'VN', N'The Socialist Republic of Vietnam', N'Asia', N'+84'),
(N'CN', N'People''s Republic of China', N'Asia', N'+86'),
(N'SG', N'Republic of Singapore', N'Asia', N'+65')


INSERT INTO City(CityId, CityName, CountryId, PostalCode) VALUES
(N'none', N'none', N'VN', N'0'),
(N'VN_HN', N'H?? N???i', N'VN', N'100000'),
(N'VN_CB', N'Cao B???ng', N'VN', N'270000'),
(N'VN_MC', N'Mai Ch??u', N'VN', N'350000'),
(N'VN_VY', N'V??nh Y??n', N'VN', N'280000'),
(N'CN_BJ', N'Beijing', N'CN', N'100000'),
(N'SG_SG', N'Singapore', N'SG', N'828603'),
(N'VN_NT', N'Nha Trang', N'VN', N'58000'),
(N'VN_L??', N'L??m ?????ng', N'VN', N'66000'),
(N'VN_KH', N'Kh??nh H??a', N'VN', N'57000'),
(N'VN_HP', N'H???i Ph??ng', N'VN', N'04000'),
(N'VN_??L', N'???? L???t', N'VN', N'66100'),
(N'VN_CR', N'Cam Ranh', N'VN', N'66100')


INSERT INTO Bank(BankId, BankName, SwiftCode, CountryId) VALUES
(N'B_VN_TCB', N'TECHCOMBANK', N'VTCBVNVX', N'VN'),
(N'B_VN_VCB', N'VIETCOMBANK', N'BFTVVNVX', N'VN'),
(N'B_VN_BIVD', N'BIDV', N'BIDVVNVX', N'VN'),
(N'B_CN_JCB', N'JPMORGAN CHASE BANK', N'CHASCN22', N'CN'),
(N'B_CN_BOK', N'BANK OF KUNLUN', N'CKLBCNBJ', N'CN'),
(N'B_CN_BIVD', N'CHINA CONSTRUCTION BANK', N'PCBCCNBJ', N'CN')


INSERT INTO BankAccount(AccountName, AccountNumber, BankId) VALUES
(N'none', '0', N'B_VN_TCB')


INSERT INTO AdminRole(RoleId, RoleName, RoleNote) VALUES
(N'TEMP', N'Temporary account', N'Have no access'),
(N'SALE_MG',N'Saler', N'Manage sales, trasactions and user accounts, cannot access tour database'),
(N'TOUR_MG', N'Tour Manager', N'Manage the tour database, cannot access sales and users'),
(N'SN_MG', N'Senior Manager', N'Have access to all divisions, and can access deleted items')


INSERT INTO [Admin](AdminName, AdminPassword ,RoleId) VALUES
(N'NTA', N'40bd001563085fc35165329ea1ff5c5ecbdbbeef', N'SN_MG'),
(N'VHL', N'51eac6b471a284d3341d8c0c63d0f1a286262a18', N'SALE_MG'),
(N'VCM', N'fc1200c7a7aa52109d762a9f005b149abef01479', N'TOUR_MG')


INSERT INTO FacilityType(FacilityTypeId, FacilityTypeName) VALUES
(N'UNTP', N'Untyped'),
(N'HTEL', N'Hotel'),
(N'RSRT', N'Resort'),
(N'LODG', N'Lodging house'),
(N'RSTR', N'Restaurant'),
(N'SHP', N'Merchandise shop'),
(N'STA_VHC', N'Vehicle station'),
(N'STA_TRN', N'Train station'),
(N'STA_DCK', N'Ships docking station'),
(N'AIRPRT', N'Airport')



INSERT INTO Category(CategoryId, CategoryName, CategoryNote) VALUES
(N'UNCG', N'Uncategorized', NULL),
(N'NATR', N'Natural', N'Water bodies, highlands, caves,...'),
(N'HIST', N'Historical', N'Pagodas, temples, museums...'),
(N'CULTR', N'Cultural', N'Villages, festival sites,...'),
(N'EVENT', N'Special events', N'Markets, festivals ,exhibitions, ,...'),
(N'ARTIF', N'Atifcial attractions', N'Entertainment parks, zoos,..')



INSERT INTO SubCategory(SubCategoryId, SubCategoryName, SubCategoryNote, CategoryId) VALUES
(N'UNSCG', N'Uncategorized', NULL, N'UNCG'),
(N'WTER', N'Water bodies', N'Rivers, lakes, waterfalls,...', N'NATR'),
(N'TEMPL', N'Temples/pagodas/churches', NULL, N'HIST'),
(N'CAVE', N'Caves/grottos', NULL, N'NATR'),
(N'STRC', N'Historical structures', NULL, N'HIST'),
(N'VILLG', N'Villages', NULL, N'CULTR'),
(N'HGHLN', N'Highlands', N'Mountains, hills', N'NATR'),
(N'PARK', N'Parks', N'Natural parks, amusement parks,...', N'ARTIF'),
(N'THEATR', N'Theater', N'Play, muscial theater,...', N'ARTIF')



INSERT INTO Tour(TourId, TourName, TourAvailability, TourStart, TourEnd, TourPrice, TourImage, TourRating, MaxBooking, BookTimeLimit, CategoryId1, CategoryId2) VALUES
(N'T_HNtoCB01', N'H?? N???i - Cao B???ng', 1, CAST(N'2022-08-03T06:00:00.000' AS DateTime), CAST(N'2022-08-04T21:00:00.000' AS DateTime), 100.16, N'/assets/images/T_HNtoCB01.png', 4.2, 40, 60, N'NATR', N'HIST'),
(N'T_HNtoMC01', N'H?? N???i - Mai Ch??u', 1, CAST(N'2022-09-12T07:00:00.000' AS DateTime), CAST(N'2022-09-13T18:00:00.000' AS DateTime), 67.34, N'/assets/images/T_HNtoMC01.png', 3.6, 30, 30, N'CULTR', N'NATR'),
(N'T_HNtoTD01', N'H?? N???i - Tam ?????o', 1, CAST(N'2022-12-04T07:00:00.000' AS DateTime), CAST(N'2022-12-05T18:30:00.000' AS DateTime), 57.54, N'/assets/images/T_HNtoTD01.png', 4.9, 20, 30, N'NATR', N'CULTR'),
----
(N'T_HNtoBJ01', N'H?? N???i - Beijing', 1, CAST(N'2022-11-17T07:00:00.000' AS DateTime), CAST(N'2022-11-19T19:30:00.000' AS DateTime), 289.82, N'/assets/images/T_HNtoBJ01.png', 4.4, 20, 30, N'HIST', N'CULTR'),
(N'T_HNto??L01', N'H?? N???i - ???? L???t', 1, CAST(N'2022-10-23T09:00:00.000' AS DateTime), CAST(N'2022-10-24T19:30:00.000' AS DateTime), 238.68, N'/assets/images/T_HNto??L01.png', 4.3, 30, 20, N'NATR', N'HIST'),
(N'T_HNtoLP01', N'H?? N???i - La Pandora Cruise', 1, CAST(N'2023-07-15T08:00:00.000' AS DateTime), CAST(N'2023-07-16T15:00:00.000' AS DateTime), 98.03, N'/assets/images/T_HNtoLP01.png', 4.6, 30, 30, N'NATR', N'HIST'),
(N'T_HNtoNT01', N'H?? N???i - Nha Trang', 1, CAST(N'2022-10-10T06:00:00.000' AS DateTime), CAST(N'2022-10-11T21:00:00.000' AS DateTime), 247.20, N'/assets/images/T_HNtoNT01.png', 4.5, 20, 30, N'HIST', N'NATR'),
(N'T_HNtoSG01', N'H?? N???i - Singapore', 1, CAST(N'2023-08-26T05:00:00.000' AS DateTime), CAST(N'2023-08-28T19:45:00.000' AS DateTime), 392.12, N'/assets/images/T_HNtoSG01.png', 4.6, 24, 45, N'CULTR', N'NATR')



INSERT INTO TouristSpot(TouristSpotId, TouristSpotName, CityId, SubCategoryId, TouristSpotRating, TouristSpotAvailability, TouristSpotLocation, TouristSpotImage, OpenHour, ClosingHour, Cord_Lat, Cord_Long) VALUES 
(N'none', N'none', N'VN_HN', N'UNSCG', 0, 1, N'none', NULL, 0, 0, NULL, NULL),
(N'TS_CB_BB01', N'Ba B??? Lake', N'VN_CB', N'WTER', 4, 1, N'At the center of The National Garden of Ba B???, Nam M???u Commune, Ba B??? District', N'/assets/images/TS_CB_BB01.png', 252000000000, 684000000000, N'22.413690640121835', N'105.61364174880788'),
(N'TS_HN_TMC01', N'Tr??i tim M???c Ch??u Tea Hill', N'VN_HN', N'HGHLN', 4.5, 1, N'VMRP+7HQ, Unnamed Road, TT. NT M???c Ch??u, M???c Ch??u', N'/assets/images/TS_HN_TMC01.png', 0, 0, N'20.890727609551', N'104.68646126880073'),
(N'TS_HN_B01', N'B???c Waterfall,  Sao stream', N'VN_HN', N'WTER', 4.9, 1, N'XCQR+QXM, Provincial Road 446, Y??n B??nh, Th???ch Th???t', N'/assets/images/TS_HN_B01.png', 252000000000, 648000000000, N'21.453373782682352', N'105.64400081299175'),
(N'TS_HN_CC01', N'C??t C??t Village', N'VN_HN', N'VILLG', 3.6, 1, N'San S??? H???, Sa Pa', N'/assets/images/TS_HN_CC01.png', 216000000000, 648000000000, N'22.33011940702077', N'103.8318837060974'),
(N'TS_CB_PTL01', N'Ph???t t??ch Tr??c L??m Pagoda of B???n Gi???c', N'VN_CB', N'TEMPL', 2.5, 1, N'Old National Highway 3, S??ng Hi???n Ward', N'/assets/images/TS_CB_PTL01.png', 0, 0, N'22.680747374910645', N'106.23360256883093'),
(N'TS_CB_NN01', N'Ng?????m Ngao Cave', N'VN_CB', N'CAVE', 4.2, 1, N'Ng?????m Ngao cave entrance, ????m Thu???, Tr??ng Kh??nh', N'/assets/images/TS_CB_NN01.png', 252000000000, 648000000000, N'22.845199855325074', N'106.70619115534286'),
(N'TS_CB_BG02', N'B???n Gi???c Waterfall', N'VN_CB', N'WTER', 3.8, 1, N'TL 211, ????m Thu???, Tr??ng Kh??nh', N'/assets/images/TS_CB_BG02.png', 0, 0, N'22.854787336930077', N'106.7240985976703'),
(N'TS_CB_D01', N'??u???m Buddhist Temple', N'VN_CB', N'TEMPL', 3.3, 1, N'QP34+4MH, Ch???o village, Ph?? L????ng, Th??i Nguy??n, B???c K???n', N'/assets/images/TS_CB_D01.png', 0, 0, N'21.75296525820971', N'105.70672133997864'),
(N'TS_CB_AM01', N'An M??? Buddhist Temple', N'VN_CB', N'TEMPL', 4.4, 1, N'CJ68+R59, Nam M???u, Ba B???, B???c K???n', N'/assets/images/TS_CB_AM01.png', 0, 0, N'22.41296071581092', N'105.6155436966015'),
(N'TS_HN_P01', N'Pu??ng Cave', N'VN_HN', N'CAVE', 4.7, 1, N'FM63+V9F, Khang Ninh, Ba B???, B???c K???n', N'/assets/images/TS_HN_P01.png',  324000000000, 612000000000, N'22.462386382210422', N'105.65345765533614'),
(N'TS_MC_TK01', N'Thung Khe Pass', N'VN_MC', N'HGHLN', 2.5, 1, N'M47P+XRX, Ph?? C?????ng, T??n L???c, H??a B??nh', N'/assets/images/TS_MC_TK01.png', 432000000000, 863400000000, N'20.66610999129284', N'105.13666031533224'),
(N'TS_HN_DY01', N'D???i Y???m Waterfall', N'VN_HN', N'WTER', 4.1, 1, N'RH8R+3WM, QL43, M?????ng Sang, M???c Ch??u, S??n La', N'/assets/images/TS_HN_DY01.png',  0, 0, N'20.8154682846861', N'104.59237938133306'),
----
(N'TS_BJ_TAM01', N'Tiananmen Square', N'CN_BJ', N'STRC', 4, 1, N'Dongcheng', N'/assets/images/TS_BJ_TAM01.png', 180000000000, 792000000000, N'39.90568700215214', N'116.39767461334093'),
(N'TS_BJ_GWCN01', N'The Great Wall of China', N'CN_BJ', N'STRC', 4.2, 1, N'Huairou District, 101406', N'/assets/images/TS_BJ_GWCN01.png', 324000000000, 594000000000, N'40.43207917650585', N'116.57040708451989'),
(N'TS_??L_DDM01', N'Domain de Marie Church', N'VN_??L', N'TEMPL', 4.5, 1, N'13 Tr???n Ph??, Block 3', N'/assets/images/TS_??L_DDM01.png', 0, 0, N'11.94966070603777', N'108.43027395065502'),
(N'TS_??L_VLOL01', N'???? L???t Valley of Love', N'VN_??L', N'HGHLN', 4, 1, N'1 Ng?? Quy???n, Block 6', N'/assets/images/TS_??L_VLOL01.png', 252000000000, 612000000000, N'11.980327180035628', N'108.45028055515648'),
(N'TS_HP_ST01', N'S??ng-T???i Cave', N'VN_HP', N'CAVE', 4.5, 1, N'R32X+624, Khe S??u, C??t H???i', N'/assets/images/TS_HP_ST01.png', 0, 0, N'20.800691747534163', N'107.09761798409752'),
(N'TS_HP_LH01', N'Lan H??? Bay', N'VN_HP', N'WTER', 4.8, 1, N'C??t H???i', N'/assets/images/TS_HP_LH01.png', 0, 0, N'20.72505291853503', N'107.04664768746605'),
(N'TS_NT_TBS01', N'Th??p B?? mineral hot spring', N'VN_NT', N'WTER', 4, 1, N'438 Ng?? ?????n, Ng???c Hi???p', N'/assets/images/TS_NT_TB01.png', 270000000000, 630000000000, N'12.270467801114426', N'109.17780445515912'),
(N'TS_NT_PT01', N'Ponagar Tower', N'VN_NT', N'STRC', 4.5, 1, N'61 Hai Th??ng T??, V??nh Ph?????c', N'/assets/images/TS_NT_TBP01.png', 288000000000, 648000000000, N'12.265597770003238', N'109.19542668399446'),
(N'TS_SG_E01', N'Esplanade-Theatres on the Bay', N'SG_SG', N'THEATR', 4.5, 1, N'1 Esplanade Dr', N'/assets/images/TS_SG_E01.png', 216000000000, 72000000000, N'1.2899971952797442', N'103.8558380551042'),
(N'TS_SG_GBTB01', N'Gardens by the Bay', N'SG_SG', N'PARK', 4.3, 1, N'18 Marina Gardens Dr', N'/assets/images/TS_SG_GBTB.png', 180000000000, 72000000000, N'1.2818042743959484', N'103.86361319743341'),
(N'TS_SG_MP01', N'Merlion Park', N'SG_SG', N'PARK', 4.5, 1, N'1 Fullerton Rd', N'/assets/images/TS_SG_MP01.png', 0, 0, N'1.2869916000435417', N'103.85441938393953')



INSERT INTO Facility(FacilityId, FacilityName, FacilityTypeId, FacilityLocation, CityId, Quantity, ServiceNote, FacilityAvailability, FacilityImage) VALUES
(N'none', N'none', N'UNTP', N'', N'VN_HN', 0, 'none', 1, NULL),
(N'FC_HN_HV01', N'H???i V??n Bus Station', N'STA_VHC', N'23, Nguy???n Khuy???n road', N'VN_HN', 96, N'24 seats per 4 bus', 1, N'/assets/images/FC_HN_HV01.png'),
(N'FC_HN_NN01', N'Nguy???n Nh???t Bus Station', N'STA_VHC', N'25 V??n Qu??n street', N'VN_HN', 80, N'16 seats per 5 bus', 1, N'/assets/images/FC_HN_NN01.png'),
(N'FC_MC_HT01', N'H???p Th???y Restaurant', N'RSTR', N'Subdivision 2, Mai Ch??u district', N'VN_MC', 48, N'4 seats per 12 tables', 1, N'/assets/images/FC_MC_HT01.png'),
(N'FC_VY_NA01', N'Nguy???t Anh Hotel', N'HTEL', N'Block 1, Tam ?????o district', N'VN_VY', 30, N'15 double rooms, 15 tables with 4 seats', 1, N'/assets/images/FC_VY_NA01.png'),
(N'FC_VY_GTD01', N'Gi?? Tam ?????o Cafe', N'RSTR', N'Tam ?????o district, V??nh Ph??c province', N'VN_VY', 64, N'4 seats per 16 tables', 1, N'/assets/images/FC_VY_GTD01.png'),
(N'FC_VY_PN01', N'Ph??? N??i Restaurant', N'RSTR', N'Near H???p Ch??u town''s people''s Committee , Tam ?????o district', N'VN_VY', 18, N'3 seats per 6 tables', 1, N'/assets/images/FC_VY_PN01.png'),
(N'FC_HN_BND01', N'B???o Ng???c Diamond Hotel', N'HTEL', N'1st floor, 124 B??? V??n ????n street, H???p Giang sub-district', N'VN_CB', 20, N'10 double rooms, 12 four seats tables', 1, N'/assets/images/FC_HN_BND01.png'),
(N'FC_CB_TL01', N'Th??nh Loan Hotel', N'HTEL', N'131 V?????n Cam street', N'VN_CB', 27, N'9 triple rooms, 4 ten seats tables', 1, N'/assets/images/FC_CB_TL01.png'),
(N'FC_HN_TN01', N'Th??nh Nam Bus Station', N'STA_VHC', N'29 M??? Lao street', N'VN_HN', 80, N'40 seats per 2 buses', 1, N'/assets/images/FC_HN_TN01.png'),
(N'FC_CB_BG01', N'B???n Gi???c Restaurant', N'RSTR', N'B???n Gi???c hamlet, ????m Th???y commune, Tr??ng Kh??nh district', N'VN_CB', 54, N'3 seats per 18 tables', 1, N'/assets/images/FC_CB_BG01.png'),
(N'FC_MC_EE01', N'88 Hotel', N'HTEL', N'102 Ho??ng Qu???c Vi???t street', N'VN_MC', 10, N'5 double rooms, 2 seats per 5 tables', 1, N'/assets/images/FC_MC_EE01.png'),
(N'FC_MC_BCE01', N'Ba Ch??? Em Restaurant', N'RSTR', N'Chi???ng S???i hamlet', N'VN_MC', 30, N'2 seats per 15 tables', 1, N'/assets/images/FC_MC_BCE01.png'),
----
(N'FC_BJ_MBT01', N'Mubus Transportation', N'STA_VHC', N'2 Chong Wen Men Nei Street, Dongcheng Dist', N'CN_BJ', 72, N'12 seats per 6 buses', 1, N'/assets/images/FC_BJ_MB01.png'),
(N'FC_BJ_NBP01', N'Novotel Beijing Peace Hotel', N'HTEL', N'6 Jianguo S Rd, Jian Wai Da Jie, Chaoyang', N'CN_BJ', 18, N'9 double rooms, 8 seats per table', 1, N'/assets/images/FC_BJ_NBP01.png'),
(N'FC_BJ_BJITN01', N'Beijing Capital International Airport', N'AIRPRT', N' Shunyi District', N'CN_BJ', 120, N'120 seats, 1 plane', 1, N'/assets/images/FC_BJ_BJITN01.png'),
(N'FC_BJ_XSQTL01', N'Xin Shuang QuanSheng Tai LvYuan Restaurant', N'RSTR', N'HuaiRou District MuTianYu HuanDao East Side', N'CN_BJ', 90, N'3 seats per 30 tables', 1, N'/assets/images/FC_BJ_BJCN01.png'),
(N'FC_??L_BDL01', N'Bonjour Da Lat Hotel', N'HTEL', N'286 Ph?? ?????ng Thi??n V????ng road, Block 8', N'VN_??L', 28, N'14 double rooms, 4 seats per 6 tables', 1, N'/assets/images/FC_??L_BDL01.png'),
(N'FC_??L_MDL01', N'Memory ???? L???t Restaurant', N'RSTR', N'24B H??ng V????ng road, Block 10', N'VN_CB', 18, N'3 seats per 6 tables', 1, N'/assets/images/FC_??L_MDL01.png'),
(N'FC_HN_NB01', N'N???i B??i International Airport', N'AIRPRT', N'Ph?? Minh, S??c S??n', N'VN_HN', 135, N'135 seats, 1 plane', 1, N'/assets/images/FC_??L_NB01.png'),
(N'FC_??L_TB01', N'Th??nh B?????i Travels', N'STA_VHC', N'49-55 Phan B???i Ch??u, Block 1', N'VN_??L', 75, N'25 seats per 3 buses', 1, N'/assets/images/FC_??L_TB01.png'),
(N'FC_HN_LN01', N'L?? Nam Bus Station', N'STA_VHC', N'118 M??? ????nh', N'VN_HN', 72, N'24 seats per 3 buses', 1, N'/assets/images/FC_HN_LN01.png'),
(N'FC_HP_G01', N'G??t Ferry Terminal', N'STA_DCK', N'??T356, TT. C??t H???i, C??t H???i', N'VN_HP', 60, N'Average 30 people or the same weight per 2 ferries', 1, N'/assets/images/FC_HP_G01.png'),
(N'FC_HP_LP01', N'Du Thuy???n La PANDORA', N'STA_DCK', N'Tu???n Ch??u Island, H??? Long', N'VN_HP', 60, N'60 spots in the cruise', 1, N'/assets/images/FC_HP_DT01.png'),
(N'FC_KH_CR01', N'Cam Ranh Airport', N'AIRPRT', N'Nguy???n T???t Th??nh, Cam H???i ????ng, Cam L??m', N'VN_KH', 78, N'78 seats, 1 plane', 1, N'/assets/images/FC_KH_CR01.png'),
(N'FC_L??_LK01', N'Li??n Kh????ng Airport', N'AIRPRT', N'Highway 20, Li??n Ngh??a, ?????c Tr???ng', N'VN_L??', 95, N'95 seats, 1 plane', 1, N'/assets/images/FC_L??_LK01.png'),
(N'FC_NT_M01', N'Majestic Nha Trang Restaurant', N'RSTR', N'105 Nguy???n Th??? Minh Khai, Ph?????c Ho??', N'VN_NT', 30, N'3 seats per 10 tables', 1, N'/assets/images/FC_NT_M01.png'),
(N'FC_NT_PT01', N'Ph????ng Trang Station', N'STA_VHC', N'7 Ho??ng Hoa Th??m, X????ng Hu??n', N'VN_NT', 84, N'42 seats per 2 buses', 1, N'/assets/images/FC_NT_PT01.png'),
(N'FC_NT_VH01', N'Virgo Hotel', N'HTEL', N'39 - 41 Nguyen Thi Minh Khai street', N'VN_CB', 5, N'5 ph??ng ng??? v?? 2 b??n ??n cho 10 ng?????i', 1, N'/assets/images/FC_NT_VH01.png'),
(N'FC_SG_CS01', N'Changi-Singapore National Airport', N'AIRPRT', N'Airport Blvd', N'SG_SG', 115, N'115 seats, 1 plane', 1, N'/assets/images/FC_SG_CS01.png'),
(N'FC_SG_MC01', N'Maxi-Cab Singapore', N'STA_VHC', N'Changi', N'SG_SG', 126, N'42 seats per 3 buses', 1, N'/assets/images/FC_SG_MC01.png'),
(N'FC_SG_MSOS01', N'Mercure Singapore On Stevens Hotel', N'HTEL', N'Orchard District 28 Stevens Road', N'SG_SG', 32, N'16 double rooms, 2 seats per 23 tables', 1, N'/assets/images/FC_SG_MSOS01.png'),
(N'FC_SG_PNR01', N'Perle Noire Resto Bar', N'RSTR', N'40 Lorong Mambong Holland Village', N'SG_SG', 56, N'4 seats per 14 tables', 1, N'/assets/images/FC_SG_PNR01.png'),
(N'FC_CR_CR01', N'Cam Ranh Airport', N'AIRPRT', N'Nguy???n T???t Th??nh, Cam H???i ????ng, Cam L??m, Kh??nh H??a', N'VN_CR', 138, N'138 seats, 1 plane', 1, N'/assets/images/FC_HN_VNAL01.png')



INSERT INTO TourDetail(TourId, TourDetailName, TouristSpotId, FacilityId, Activity, ActivityTimeStart, ActivityTimeEnd, ActivityNote) VALUES
(N'T_HNtoCB01', N'Tour starts', N'none', N'FC_HN_HV01', N'Board the bus and head to ??u???m Pagoda', CAST(N'2022-08-03T06:00:00.000' AS DateTime), CAST(N'2022-08-03T09:30:00.000' AS DateTime), NULL),
(N'T_HNtoCB01', N'Visit ??u???m Pagoda', N'TS_CB_D01', N'none', N'Sightseeing at the ??u???m Buddist Temple', CAST(N'2022-08-03T09:30:00.000' AS DateTime), CAST(N'2022-08-03T11:00:00.000' AS DateTime), NULL),
(N'T_HNtoCB01', N'Move to Th??nh Loan Hotel', N'none', N'FC_HN_HV01', N'Head to Th??nh Loan Hotel', CAST(N'2022-08-03T11:00:00.000' AS DateTime), CAST(N'2022-08-03T11:30:00.000' AS DateTime), NULL),
(N'T_HNtoCB01', N'Have lunch and rest', N'none', N'FC_CB_TL01', N'Check in, have lunch and rest at Th??nh Loan Hotel', CAST(N'2022-08-03T11:30:00.000' AS DateTime), CAST(N'2022-08-03T14:00:00.000' AS DateTime), NULL),
(N'T_HNtoCB01', N'Move to Ba B??? Lake', N'none', N'FC_HN_HV01', N'Move to Ba B??? Lake', CAST(N'2022-08-03T14:00:00.000' AS DateTime), CAST(N'2022-08-03T14:30:00.000' AS DateTime), NULL),
(N'T_HNtoCB01', N'Visit Ba B??? Lake', N'TS_CB_BB01', N'none', N'Enjoy the nature of Ba B??? Lake and its surrounding jungles', CAST(N'2022-08-03T14:30:00.000' AS DateTime), CAST(N'2022-08-03T15:30:00.000' AS DateTime), NULL),
(N'T_HNtoCB01', N'Move to Pu??ng Cave', N'none', N'FC_HN_HV01', N'Make way to Pu??ng Cave', CAST(N'2022-08-03T15:30:00.000' AS DateTime), CAST(N'2022-08-03T16:45:00.000' AS DateTime), NULL),
(N'T_HNtoCB01', N'Visit Pu??ng Cave', N'TS_HN_P01', N'none', N'Sightseeing at Pu??ng Cave', CAST(N'2022-08-03T16:45:00.000' AS DateTime), CAST(N'2022-08-03T18:00:00.000' AS DateTime), NULL),
(N'T_HNtoCB01', N'Return to the hotel', N'none', N'FC_HN_HV01', N'Board the bus and head back to Th??nh Loan Hotel', CAST(N'2022-08-03T18:00:00.000' AS DateTime), CAST(N'2022-08-03T18:30:00.000' AS DateTime), NULL),
(N'T_HNtoCB01', N'Have dinner', N'none', N'FC_CB_TL01', N'Have dinner and stay the night at the hotel', CAST(N'2022-08-03T18:30:00.000' AS DateTime), CAST(N'2022-08-04T05:30:00.000' AS DateTime), NULL),
(N'T_HNtoCB01', N'Move to B???n Gi???c Waterfall', N'none', N'FC_HN_HV01', N'Made way to B???n Gi???c Waterfall, have breakfast on the way', CAST(N'2022-08-04T05:30:00.000' AS DateTime), CAST(N'2022-08-04T10:30:00.000' AS DateTime), NULL),
(N'T_HNtoCB01', N'Visit B???n Gi??c Waterfall', N'TS_CB_BG02', N'none', N'Sightseeing at B???n Gi???c Waterfall', CAST(N'2022-08-04T10:30:00.000' AS DateTime), CAST(N'2022-08-04T11:30:00.000' AS DateTime), NULL),
(N'T_HNtoCB01', N'Have lunch', N'none', N'FC_CB_BG01', N'Have lunch at B???n Gi???c Restaurant', CAST(N'2022-08-04T11:30:00.000' AS DateTime), CAST(N'2022-08-04T12:15:00.000' AS DateTime), NULL),
(N'T_HNtoCB01', N'Move to Ph???t t??ch Tr??c L??m Pagoda', N'none', N'FC_HN_HV01', N'Head to Ph???t t??ch Tr??c L??m Pagoda', CAST(N'2022-08-04T12:15:00.000' AS DateTime), CAST(N'2022-08-04T12:30:00.000' AS DateTime), NULL),
(N'T_HNtoCB01', N'Visit the Ph???t t??ch Tr??c L??m Pagoda', N'TS_CB_PTL01', N'none', N'Freely visit the Ph???t t??ch Tr??c L??m pagoda and offer incense', CAST(N'2022-08-04T12:30:00.000' AS DateTime), CAST(N'2022-08-04T14:00:00.000' AS DateTime), NULL),
(N'T_HNtoCB01', N'Move to Ng?????m Ngao Cave', N'none', N'FC_HN_HV01', N'Move to Ng?????m Ngao Cave', CAST(N'2022-08-04T14:00:00.000' AS DateTime), CAST(N'2022-08-04T14:20:00.000' AS DateTime), NULL),
(N'T_HNtoCB01', N'Visit Ng?????m Ngao Cave', N'TS_CB_NN01', N'none', N'Traverse Ng?????m Ngao Cave', CAST(N'2022-08-04T14:20:00.000' AS DateTime), CAST(N'2022-08-04T15:00:00.000' AS DateTime), NULL),
(N'T_HNtoCB01', N'Move to An M?? Pagoda', N'none', N'FC_HN_HV01', N'Go to An M?? Pagoda', CAST(N'2022-08-04T15:00:00.000' AS DateTime), CAST(N'2022-08-04T15:15:00.000' AS DateTime), NULL),
(N'T_HNtoCB01', N'Visit An M??? Pagoda', N'TS_CB_AM01', N'none', N'Sightseeing at An M?? Pagoda', CAST(N'2022-08-04T15:15:00.000' AS DateTime), CAST(N'2022-08-04T16:30:00.000' AS DateTime), NULL),
(N'T_HNtoCB01', N'Return to the hotel', N'none', N'FC_HN_HV01', N'Return to Th??nh Loan Hotel', CAST(N'2022-08-04T16:30:00.000' AS DateTime), CAST(N'2022-08-04T17:45:00.000' AS DateTime), NULL),
(N'T_HNtoCB01', N'Have dinner and check out', N'none', N'FC_CB_TL01', N'Have your last meal and check out', CAST(N'2022-08-04T15:45:00.000' AS DateTime), CAST(N'2022-08-04T19:00:00.000' AS DateTime), NULL),
(N'T_HNtoCB01', N'Tour ends', N'none', N'FC_HN_HV01', N'Head back to H?? N???i', CAST(N'2022-08-04T19:00:00.000' AS DateTime), CAST(N'2022-08-04T21:00:00.000' AS DateTime), NULL),
----
(N'T_HNtoMC01', N'Tour starts', N'none', N'FC_HN_TN01', N'Board the bus and make your way to Thung Khe Pass', CAST(N'2022-09-12T07:00:00.000' AS DateTime), CAST(N'2022-09-12T08:30:00.000' AS DateTime), NULL),
(N'T_HNtoMC01', N'Visit Thung Khe Pass', N'TS_MC_TK01', N'none', N'Sightseeing as you climb through the pass', CAST(N'2022-09-12T08:30:00.000' AS DateTime), CAST(N'2022-09-12T10:00:00.000' AS DateTime), NULL),
(N'T_HNtoMC01', N'Move to Ba Ch??? Em Restaurant', N'none', N'FC_HN_TN01', N'Move to Ba Ch??? Em Restaurant', CAST(N'2022-09-12T10:00:00.000' AS DateTime), CAST(N'2022-09-12T11:00:00.000' AS DateTime), NULL),
(N'T_HNtoMC01', N'Have lunch at a local restaurant', N'none', N'FC_MC_BCE01', N'Have lunch and a short break at Ba Ch??? Em Restaurant', CAST(N'2022-09-12T11:00:00.000' AS DateTime), CAST(N'2022-09-12T14:00:00.000' AS DateTime), NULL),
(N'T_HNtoMC01', N'Move to D???i Y???m Waterfall', N'none', N'FC_HN_TN01', N'Move to D???i Y???m Waterfall', CAST(N'2022-09-12T14:00:00.000' AS DateTime), CAST(N'2022-09-12T14:45:00.000' AS DateTime), NULL),
(N'T_HNtoMC01', N'Visit D???i Y???m Waterfall', N'TS_HN_DY01', N'none', N'Sightseeing at D???i Y???m Waterfalls', CAST(N'2022-09-12T14:45:00.000' AS DateTime), CAST(N'2022-09-12T16:00:00.000' AS DateTime), NULL),
(N'T_HNtoMC01', N'Move to 88 Hotel', N'none', N'FC_HN_TN01', N'Board the bus and move to 88 Hotel', CAST(N'2022-09-12T16:00:00.000' AS DateTime), CAST(N'2022-09-12T17:00:00.000' AS DateTime), NULL),
(N'T_HNtoMC01', N'Have dinner and rest', N'none', N'FC_MC_EE01', N'Check in, have dinner and stay the night at 88 Hotel', CAST(N'2022-09-12T17:00:00.000' AS DateTime), CAST(N'2022-09-13T07:00:00.000' AS DateTime), NULL),
(N'T_HNtoMC01', N'Move to tea hill', N'none', N'FC_HN_TN01', N'Check out of the hotel and move to the tea hill', CAST(N'2022-09-13T07:00:00.000' AS DateTime), CAST(N'2022-09-13T08:30:00.000' AS DateTime), NULL),
(N'T_HNtoMC01', N'Visit Tr??i tim M???c Ch??u tea hill', N'TS_HN_TMC01', N'none', N'Sightseeing at take pictures at the Tr??i tim M???c Ch??u tea hill', CAST(N'2022-09-13T08:00:00.000' AS DateTime), CAST(N'2022-09-13T11:00:00.000' AS DateTime), NULL),
(N'T_HNtoMC01', N'Move to the restaurant for lunch', N'none', N'FC_HN_TN01', N'Make way to the restaurant for lunch', CAST(N'2022-09-13T11:00:00.000' AS DateTime), CAST(N'2022-09-13T11:45:00.000' AS DateTime), NULL),
(N'T_HNtoMC01', N'Have lunch and rest', N'none', N'FC_MC_HT01', N'Have lunch and a short break at H???p Th???y Restaurant', CAST(N'2022-09-13T11:45:00.000' AS DateTime), CAST(N'2022-09-13T14:00:00.000' AS DateTime), NULL),
(N'T_HNtoMC01', N'Tour ends', N'none', N'FC_HN_TN01', N'The bus travels back to H?? N???i', CAST(N'2022-09-13T14:00:00.000' AS DateTime), CAST(N'2022-09-13T18:00:00.000' AS DateTime), NULL),
----
(N'T_HNtoTD01', N'Tour starts', N'none', N'FC_HN_NN01', N'Board the bus and make way to the hotel', CAST(N'2022-12-04T07:00:00.000' AS DateTime), CAST(N'2022-12-04T09:30:00.000' AS DateTime), NULL),
(N'T_HNtoTD01', N'Check in the hotel and visit the Stone Church', N'none', N'FC_VY_NA01', N'Check in at Nguy???t Anh Hotel and visit the nearby Stone Church', CAST(N'2022-12-04T09:30:00.000' AS DateTime), CAST(N'2022-12-04T11:00:00.000' AS DateTime), NULL),
(N'T_HNtoTD01', N'Have lunch and rest', N'none', N'FC_VY_NA01', N'Have lunch and a break at Nguy???t Anh Hotel', CAST(N'2022-12-04T11:00:00.000' AS DateTime), CAST(N'2022-12-04T14:00:00.000' AS DateTime), NULL),
(N'T_HNtoTD01', N'Move to B???c Waterfall', N'none', N'FC_HN_NN01', N'Move to B???c Waterfall', CAST(N'2022-12-04T14:00:00.000' AS DateTime), CAST(N'2022-12-04T14:30:00.000' AS DateTime), NULL),
(N'T_HNtoTD01', N'Visit B???c Waterfall', N'TS_HN_B01', N'none', N'Sightseeing at B???c Waterfall', CAST(N'2022-12-04T14:30:00.000' AS DateTime), CAST(N'2022-12-04T17:30:00.000' AS DateTime), NULL),
(N'T_HNtoTD01', N'Return to the hotel', N'none', N'FC_HN_NN01', N'Return to the hotel', CAST(N'2022-12-04T17:30:00.000' AS DateTime), CAST(N'2022-12-04T18:00:00.000' AS DateTime), NULL),
(N'T_HNtoTD01', N'Have dinner and rest', N'none', N'FC_VY_NA01', N'Have dinner and stay the night at Nguy???t Anh Hotel', CAST(N'2022-12-04T18:00:00.000' AS DateTime), CAST(N'2022-12-05T07:00:00.000' AS DateTime), NULL),
(N'T_HNtoTD01', N'Check out and board the bus', N'none', N'FC_HN_NN01', N'Check out of the hotel and move to C??t C??t Village', CAST(N'2022-12-05T07:00:00.000' AS DateTime), CAST(N'2022-12-05T08:30:00.000' AS DateTime), NULL),
(N'T_HNtoTD01', N'Visit C??t C??t Village', N'TS_HN_CC01', N'none', N'Visit C??t C??t Village', CAST(N'2022-12-05T08:30:00.000' AS DateTime), CAST(N'2022-12-05T11:30:00.000' AS DateTime), NULL),
(N'T_HNtoTD01', N'Move to Ph??? N??i Restaurant', N'none', N'FC_HN_NN01', N'Go to Ph??? N??i Restaurant', CAST(N'2022-12-05T11:30:00.000' AS DateTime), CAST(N'2022-12-05T12:00:00.000' AS DateTime), NULL),
(N'T_HNtoTD01', N'Have lunch and rest', N'none', N'FC_VY_PN01', N'Have lunch and a short break at the restaurant', CAST(N'2022-12-05T12:00:00.000' AS DateTime), CAST(N'2022-12-05T13:30:00.000' AS DateTime), NULL),
(N'T_HNtoTD01', N'Move to Gi?? Tam ?????o Cafe', N'none', N'FC_HN_NN01', N'Make your way to Gi?? Tam ?????o Cafe', CAST(N'2022-12-05T13:30:00.000' AS DateTime), CAST(N'2022-12-05T13:45:00.000' AS DateTime), NULL),
(N'T_HNtoTD01', N'Enjoy local coffe at Gi?? Tam ?????o Cafe', N'none', N'FC_VY_GTD01', N'Have coffe and sightseeing at the Gi?? Tam ?????o Cafe', CAST(N'2022-12-05T13:45:00.000' AS DateTime), CAST(N'2022-12-05T15:45:00.000' AS DateTime), NULL),
(N'T_HNtoTD01', N'Tour ends', N'none', N'FC_HN_NN01', N'Return to H?? N???i', CAST(N'2022-12-05T15:45:00.000' AS DateTime), CAST(N'2022-12-05T18:30:00.000' AS DateTime), NULL),
----
(N'T_HNtoBJ01', N'Tour starts', N'none', N'FC_HN_TN01', N'Head to N???i B??i Airport', CAST(N'2022-11-17T07:00:00.000' AS DateTime), CAST(N'2022-11-17T09:00:00.000' AS DateTime), NULL),
(N'T_HNtoBJ01', N'Check in airport', N'none', N'FC_HN_NB01', N'Wait for your flight an check in', CAST(N'2022-11-17T09:00:00.000' AS DateTime), CAST(N'2022-11-17T10:00:00.000' AS DateTime), NULL),
(N'T_HNtoBJ01', N'Fly to Beijing International Airport', N'none', N'FC_HN_NB01', N'Board the plane and move to Beijing International Airport', CAST(N'2022-11-17T10:00:00.000' AS DateTime), CAST(N'2022-11-17T14:45:00.000' AS DateTime),  NULL),
(N'T_HNtoBJ01', N'Head to Novotel Beijing Peace Hotel', N'none', N'FC_BJ_MBT01', N'Disembark the plane and head to Beijing Peace Hotel', CAST(N'2022-11-17T14:45:00.000' AS DateTime), CAST(N'2022-11-17T16:00:00.000' AS DateTime), NULL),
(N'T_HNtoBJ01', N'Check in and have dinner', N'none', N'FC_BJ_NBP01', N'Check in and attend the small dinner party at the hotel', CAST(N'2022-11-17T16:00:00.000' AS DateTime), CAST(N'2022-11-17T21:00:00.000' AS DateTime), NULL),
(N'T_HNtoBJ01', N'Rest at the hotel', N'none', N'FC_BJ_NBP01', N'Stay the night and have breakfast the next morning at the hotel''s restaurant', CAST(N'2022-11-17T21:00:00.000' AS DateTime), CAST(N'2022-11-18T07:30:00.000' AS DateTime), NULL),
(N'T_HNtoBJ01', N'Move to The Great Wall of China', N'none', N'FC_BJ_MBT01', N'Move to The Great Wall of China', CAST(N'2022-11-18T07:30:00.000' AS DateTime), CAST(N'2022-11-18T08:45:00.000' AS DateTime), NULL),
(N'T_HNtoBJ01', N'Visit The Great Wall of China', N'TS_BJ_GWCN01', N'none', N'Visit The Great Wall of China', CAST(N'2022-11-18T08:45:00.000' AS DateTime), CAST(N'2022-11-18T11:00:00.000' AS DateTime), NULL),
(N'T_HNtoBJ01', N'Move to the restaurant', N'none', N'FC_BJ_MBT01', N'Move to the Xin Shuang QuanSheng Tai LvYuan Restaurant', CAST(N'2022-11-18T11:00:00.000' AS DateTime), CAST(N'2022-11-18T11:30:00.000' AS DateTime), NULL),
(N'T_HNtoBJ01', N'Have lunch', N'none', N'FC_BJ_XSQTL01', N'Have lunch at Xin Shuang QuanSheng Tai LvYuan Restaurant', CAST(N'2022-11-18T11:30:00.000' AS DateTime), CAST(N'2022-11-18T13:00:00.000' AS DateTime), NULL),
(N'T_HNtoBJ01', N'Move to Tiananmen Square', N'none', N'FC_BJ_MBT01', N'Move to Tiananmen Square', CAST(N'2022-11-18T13:00:00.000' AS DateTime), CAST(N'2022-11-18T14:30:00.000' AS DateTime), NULL),
(N'T_HNtoBJ01', N'Visit Tiananmen Square', N'TS_BJ_TAM01', N'none', N'Visit Tiananmen Square', CAST(N'2022-11-18T14:30:00.000' AS DateTime), CAST(N'2022-11-18T17:00:00.000' AS DateTime), NULL),
(N'T_HNtoBJ01', N'Return to the hotel', N'none', N'FC_BJ_MBT01', N'Return to Novotel Beijing Peace Hotel', CAST(N'2022-11-18T17:00:00.000' AS DateTime), CAST(N'2022-11-18T19:00:00.000' AS DateTime), NULL),
(N'T_HNtoBJ01', N'Stay the night', N'none', N'FC_BJ_NBP01', N'Have dinner, stay the night and can freely shop the next morning', CAST(N'2022-11-18T19:00:00.000' AS DateTime), CAST(N'2022-11-19T10:30:00.000' AS DateTime), NULL),
(N'T_HNtoBJ01', N'Have lunch and check out', N'none', N'FC_BJ_NBP01', N'Have lunch at the hotel and check out', CAST(N'2022-11-19T10:30:00.000' AS DateTime), CAST(N'2022-11-19T12:00:00.000' AS DateTime), NULL),
(N'T_HNtoBJ01', N'Head to Novotel Beijing International Airport', N'none', N'FC_BJ_MBT01', N'Head to Beijing International Airport to return to N???i B??i Airport', CAST(N'2022-11-19T12:00:00.000' AS DateTime), CAST(N'2022-11-19T13:45:00.000' AS DateTime), NULL),
(N'T_HNtoBJ01', N'Check in airport', N'none', N'FC_BJ_BJITN01', N'Wait for your flight and check in', CAST(N'2022-11-19T13:45:00.000' AS DateTime), CAST(N'2022-11-19T14:30:00.000' AS DateTime), NULL),
(N'T_HNtoBJ01', N'Return to N???i B??i Airport', N'none', N'FC_BJ_BJITN01', N'Fly back to N???i B??i Airport', CAST(N'2022-11-19T14:30:00.000' AS DateTime), CAST(N'2022-11-19T17:15:00.000' AS DateTime), NULL),
(N'T_HNtoBJ01', N'Tour ends', N'none', N'FC_HN_TN01', N'Check out, board the bus and leave', CAST(N'2022-11-19T17:15:00.000' AS DateTime), CAST(N'2022-11-19T19:30:00.000' AS DateTime), NULL)



INSERT INTO TourDetail ([TourDetailName], [Activity], [ActivityTimeStart], [ActivityTimeEnd], [TouristSpotId], [FacilityId], [ActivityNote], [TourId]) VALUES
(N'Tour starts', N'Gather at N???i B??i Airport', CAST(N'2022-10-23T09:00:00.000' AS DateTime), CAST(N'2022-10-23T11:00:00.000' AS DateTime), N'none', N'FC_HN_NN01', NULL, N'T_HNto??L01'),
(N'Check in the airport', N'Wait for your flight and check in', CAST(N'2022-10-23T11:00:00.000' AS DateTime), CAST(N'2022-10-23T11:45:00.000' AS DateTime), N'none', N'FC_HN_NB01', NULL, N'T_HNto??L01'),
(N'Fly to Li??n Kh????ng Airport', N'Fly to Li??n Kh????ng Airport and have lunch on the plane', CAST(N'2022-10-23T11:45:00.000' AS DateTime), CAST(N'2022-10-23T14:00:00.000' AS DateTime), N'none', N'FC_HN_NB01', NULL, N'T_HNto??L01'),
(N'Head to Domain de Marie Church', N'Arrive at the Airport and move to the Domain de Marie Church', CAST(N'2022-10-23T14:20:00.000' AS DateTime), CAST(N'2022-10-23T15:20:00.000' AS DateTime), N'none', N'FC_??L_TB01', NULL, N'T_HNto??L01'),
(N'Visit Domain de Marie church', N'Visit the church and give your prayers', CAST(N'2022-10-23T15:20:00.000' AS DateTime), CAST(N'2022-10-23T18:00:00.000' AS DateTime), N'TS_??L_DDM01', N'none', NULL, N'T_HNto??L01'),
(N'Move to Bonjour Da Lat Hotel', N'Move to Bonjour Da Lat Hotel', CAST(N'2022-10-23T18:00:00.000' AS DateTime), CAST(N'2022-10-23T19:00:00.000' AS DateTime), N'none', N'FC_??L_TB01', NULL, N'T_HNto??L01'),
(N'Check in and rest at Bonjour Da Lat Hotel', N'Check in, have dinner and rest at Bonjour Da Lat Hotel and have breakfast and check out the next morning', CAST(N'2022-10-23T19:00:00.000' AS DateTime), CAST(N'2022-10-24T07:30:00.000' AS DateTime), N'none', N'FC_??L_BDL01', NULL, N'T_HNto??L01'),
(N'Move to ???? L???t Valley of Love', N'Move to ???? L???t Valley of Love', CAST(N'2022-10-24T07:30:00.000' AS DateTime), CAST(N'2022-10-24T08:00:00.000' AS DateTime), N'none', N'FC_??L_TB01', NULL, N'T_HNto??L01'),
(N'Visit ???? L???t Valley of Love', N'Visit, takes photos and enjoy the T??y Nguy??n space of Gong Culture show', CAST(N'2022-10-24T08:00:00.000' AS DateTime), CAST(N'2022-10-24T10:30:00.000' AS DateTime), N'TS_??L_VLOL01', N'none', NULL, N'T_HNto??L01'),
(N'Move to Memory ???? L???t Restaurant', N'Move to Memory ???? L???t Restaurant', CAST(N'2022-10-24T10:30:00.000' AS DateTime), CAST(N'2022-10-24T11:00:00.000' AS DateTime), N'none', N'FC_??L_TB01', NULL, N'T_HNto??L01'),
(N'Have Lunch', N'Have lunch and a short break at Memory ???? L???t Restaurant', CAST(N'2022-10-24T11:00:00.000' AS DateTime), CAST(N'2022-10-24T12:30:00.000' AS DateTime), N'none', N'FC_??L_MDL01', NULL, N'T_HNto??L01'),
(N'Return to Li??n Kh????ng Airport', N'Return to Li??n Kh????ng Airport to head back to N???i B??i Airport', CAST(N'2022-10-24T12:30:00.000' AS DateTime), CAST(N'2022-10-24T14:20:00.000' AS DateTime), N'none', N'FC_??L_TB01', NULL, N'T_HNto??L01'),
(N'Check in the airport', N'Wait for your flight and check in', CAST(N'2022-10-24T14:20:00.000' AS DateTime), CAST(N'2022-10-24T15:20:00.000' AS DateTime), N'none', N'FC_L??_LK01', NULL, N'T_HNto??L01'),
(N'Move to N???i B??i airport', N'Fly back to N???i B??i airport', CAST(N'2022-10-24T15:20:00.000' AS DateTime), CAST(N'2022-10-24T17:35:00.000' AS DateTime), N'none', N'FC_L??_LK01', NULL, N'T_HNto??L01'),
(N'Tour ends', N'Leave the airport, board the bus and head back home', CAST(N'2022-10-24T17:35:00.000' AS DateTime), CAST(N'2022-10-24T19:30:00.000' AS DateTime), N'none', N'FC_HN_NN01', NULL, N'T_HNto??L01'),
----
(N'Tour starts', N'The buses gather all tourist at G??t Ferries Terminal', CAST(N'2023-07-15T08:00:00.000' AS DateTime), CAST(N'2023-07-15T11:00:00.000' AS DateTime), N'none', N'FC_HN_LN01', NULL, N'T_HNtoLP01'),
(N'Prepare to board the ship', N'Check on your belongings and complete some procedures', CAST(N'2023-07-15T11:00:00.000' AS DateTime), CAST(N'2023-07-15T12:00:00.000' AS DateTime), N'none', N'FC_HP_G01', NULL, N'T_HNtoLP01'),
(N'Board the cruise ship', N'Board the La Pandora, have lunch and enjoy the view as the cruise ship moves around K9 ???? Ch??ng relic area', CAST(N'2023-07-15T12:00:00.000' AS DateTime), CAST(N'2023-07-15T13:30:00.000' AS DateTime), N'none', N'FC_HP_LP01', N'The lighthouses around the area was built by the French colonial empire 100 years ago', N'T_HNtoLP01'),
(N'Move to Lan H??? bay', N'H??nh kh??ch di chuy???n ?????n khu v???c ??ng C???m c???a v???nh Lan H??? v?? ng???m c???nh ', CAST(N'2023-07-15T13:30:00.000' AS DateTime), CAST(N'2023-07-15T16:00:00.000' AS DateTime), N'none', N'FC_HP_LP01', NULL, N'T_HNtoLP01'),
(N'Have fun at ??ng C???m area of Lan H??? Bay', N'Kayak or freely swim around Lan H??? Bay', CAST(N'2023-07-15T16:00:00.000' AS DateTime), CAST(N'2023-07-15T17:30:00.000' AS DateTime), N'TS_HP_LH01', N'none', NULL, N'T_HNtoLP01'),
(N'Have dinner', N'Have dinner and rest on the cruise ship', CAST(N'2023-07-15T17:30:00.000' AS DateTime), CAST(N'2023-07-15T19:30:00.000' AS DateTime), N'none', N'FC_HP_LP01', NULL, N'T_HNtoLP01'),
(N'Sun set party', N'Join the Sun set party, sightseeing, night squid fishing on the cruise', CAST(N'2023-07-15T19:30:00.000' AS DateTime), CAST(N'2023-07-15T23:00:00.000' AS DateTime), N'none', N'FC_HP_LP01', NULL, N'T_HNtoLP01'),
(N'Rest throught the night', N'Rest throught the night in your room', CAST(N'2023-07-15T23:00:00.000' AS DateTime), CAST(N'2023-07-16T06:00:00.000' AS DateTime), N'none', N'FC_HP_LP01', NULL, N'T_HNtoLP01'),
(N'Tai Chi practice', N'Enjoy the dusk view and join the Tai Chi practice on the ship deck', CAST(N'2023-07-16T06:00:00.000' AS DateTime), CAST(N'2023-07-16T07:00:00.000' AS DateTime), N'none', N'FC_HP_LP01', NULL, N'T_HNtoLP01'),
(N'Have breakfast and head to S??ng-T???i Cave', N'Have breakfast as the ship move to S??ng - T???i Cave', CAST(N'2023-07-16T07:00:00.000' AS DateTime), CAST(N'2023-07-16T08:30:00.000' AS DateTime), N'none', N'FC_HP_LP01', NULL, N'T_HNtoLP01'),
(N'Visit S??ng-T???i Cave area', N'Travese through the cave in small boats', CAST(N'2023-07-16T08:30:00.000' AS DateTime), CAST(N'2023-07-16T10:00:00.000' AS DateTime), N'TS_HP_ST01', N'none', NULL, N'T_HNtoLP01'),
(N'Have lunch', N'Head back to the cruise for lunch', CAST(N'2023-07-16T10:00:00.000' AS DateTime), CAST(N'2023-07-16T11:45:00.000' AS DateTime), N'none', N'FC_HP_LP01', NULL, N'T_HNtoLP01'),
(N'Prepare for depart', N'Check your belongings and prepare to depart the ship', CAST(N'2023-07-16T11:45:00.000' AS DateTime), CAST(N'2023-07-16T12:00:00.000' AS DateTime), N'none', N'FC_HP_LP01', NULL, N'T_HNtoLP01'),
(N'Tour ends', N'Depart the La Pandora, board the bus and return home', CAST(N'2023-07-16T12:00:00.000' AS DateTime), CAST(N'2023-07-16T15:00:00.000' AS DateTime), N'none', N'FC_HN_LN01', NULL, N'T_HNtoLP01'),
----
(N'Tour starts', N'The buses gather at N???i B??i Airport', CAST(N'2022-10-10T06:00:00.000' AS DateTime), CAST(N'2022-10-10T08:00:00.000' AS DateTime), N'none', N'FC_HN_TN01', NULL, N'T_HNtoNT01'),
(N'Wait for your flight', N'Wait for your flight and check in', CAST(N'2022-10-10T08:00:00.000' AS DateTime), CAST(N'2022-10-10T09:00:00.000' AS DateTime), N'none', N'FC_HN_NB01', NULL, N'T_HNtoNT01'),
(N'Move to Cam Ranh Airport', N'Move to Cam Ranh Airport', CAST(N'2022-10-10T09:00:00.000' AS DateTime), CAST(N'2022-10-10T11:00:00.000' AS DateTime), N'none', N'FC_HN_NB01', NULL, N'T_HNtoNT01'),
(N'Arrive at the airport', N'Arrived at Cam Ranh Airport and head to the hotel on bus', CAST(N'2022-10-10T11:00:00.000' AS DateTime), CAST(N'2022-10-10T12:00:00.000' AS DateTime), N'none', N'FC_NT_PT01', NULL, N'T_HNtoNT01'),
(N'Have lunch', N'Check in, have lunch and rest at Virgo Hotel', CAST(N'2022-10-10T12:00:00.000' AS DateTime), CAST(N'2022-10-10T14:00:00.000' AS DateTime), N'none', N'FC_NT_VH01', NULL, N'T_HNtoNT01'),
(N'Move to Th??p B?? mineral hot spring area', N'Move to Th??p B?? mineral hot spring area', CAST(N'2022-10-10T14:00:00.000' AS DateTime), CAST(N'2022-10-10T15:00:00.000' AS DateTime), N'none', N'FC_NT_PT01', NULL, N'T_HNtoNT01'),
(N'Visit Th??p B?? mineral hot spring area', N'Enjoy Th??p B?? mineral hot spring area''s service', CAST(N'2022-10-10T15:00:00.000' AS DateTime), CAST(N'2022-10-10T17:30:00.000' AS DateTime), N'TS_NT_TBS01', N'none', N'Including mud bath, mineral hot spring, herbal bath', N'T_HNtoNT01'),
(N'Return to the hotel', N'Return to the Virgo Hotel', CAST(N'2022-10-10T17:30:00.000' AS DateTime), CAST(N'2022-10-10T18:30:00.000' AS DateTime), N'none', N'FC_NT_PT01', NULL, N'T_HNtoNT01'),
(N'Stay the night and check out', N'Have dinner, stay the night and have breakfast and check out the next morning', CAST(N'2022-10-10T18:30:00.000' AS DateTime), CAST(N'2022-10-11T08:30:00.000' AS DateTime), N'none', N'FC_NT_VH01', NULL, N'T_HNtoNT01'),
(N'Move to Ponagar Tower', N'Move to Ponagar Tower', CAST(N'2022-10-11T08:30:00.000' AS DateTime), CAST(N'2022-10-11T09:30:00.000' AS DateTime), N'none', N'FC_NT_PT01', NULL, N'T_HNtoNT01'),
(N'Visits Ponagar Tower', N'Sightseeing at Ponagar Tower', CAST(N'2022-10-11T09:30:00.000' AS DateTime), CAST(N'2022-10-11T11:30:00.000' AS DateTime), N'TS_NT_PT01', N'none', NULL, N'T_HNtoNT01'),
(N'Move to Majestic Nha Trang Restaurant', N'Move to Majestic Nha Trang Restaurant', CAST(N'2022-10-11T11:30:00.000' AS DateTime), CAST(N'2022-10-11T12:00:00.000' AS DateTime), N'none', N'FC_NT_PT01', NULL, N'T_HNtoNT01'),
(N'Have lunch', N'Have lunch and a short retst at Majestic Nha Trang Restaurant', CAST(N'2022-10-11T12:00:00.000' AS DateTime), CAST(N'2022-10-11T14:00:00.000' AS DateTime), N'none', N'FC_NT_M01', NULL, N'T_HNtoNT01'),
(N'Move to Cam Ranh Airport', N'Move to Cam Ranh Airport', CAST(N'2022-10-11T14:00:00.000' AS DateTime), CAST(N'2022-10-11T16:00:00.000' AS DateTime), N'none', N'FC_NT_PT01', NULL, N'T_HNtoNT01'),
(N'Wait for your flight', N'Wait for your flight and check in at Cam Ranh Airport', CAST(N'2022-10-11T16:00:00.000' AS DateTime), CAST(N'2022-10-11T16:50:00.000' AS DateTime), N'none', N'FC_KH_CR01', NULL, N'T_HNtoNT01'),
(N'Move to N???i B??i Airport', N'Fly back to N???i B??i Airport', CAST(N'2022-10-11T16:50:00.000' AS DateTime), CAST(N'2022-10-11T18:50:00.000' AS DateTime), N'none', N'FC_CR_CR01', NULL, N'T_HNtoNT01'),
(N'Tour ends', N'Arrive at N???i B??i Airport and board the bus to make your way home', CAST(N'2022-10-11T18:50:00.000' AS DateTime), CAST(N'2022-10-11T21:00:00.000' AS DateTime), N'none', N'FC_HN_TN01', NULL, N'T_HNtoNT01'),
----
(N'Tour Start', N'The buses gather at N???i B??i Airport', CAST(N'2023-08-26T05:00:00.000' AS DateTime), CAST(N'2023-08-26T07:00:00.000' AS DateTime), N'none', N'FC_HN_NN01', NULL, N'T_HNtoSG01'),
(N'Wait for your flight', N'Have break fast, Wait for your flight and check in', CAST(N'2023-08-26T07:00:00.000' AS DateTime), CAST(N'2023-08-26T08:00:00.000' AS DateTime), N'none', N'FC_HN_NB01', NULL, N'T_HNtoSG01'),
(N'Have lunch on your way to Changi-Singapore International Airport', N'Have lunch on the plane while waiting for arrival', CAST(N'2023-08-26T08:00:00.000' AS DateTime), CAST(N'2023-08-26T12:50:00.000' AS DateTime), N'none', N'FC_HN_NB01', NULL, N'T_HNtoSG01'),
(N'Complete entry', N'Complete entry procedure at the airport and head to the park', CAST(N'2023-08-26T12:50:00.000' AS DateTime), CAST(N'2023-08-26T14:30:00.000' AS DateTime), N'none', N'FC_SG_MC01', NULL, N'T_HNtoSG01'),
(N'Visit Merlion Park', N'Sightseeing at Merlion Park', CAST(N'2023-08-26T14:30:00.000' AS DateTime), CAST(N'2023-08-26T16:30:00.000' AS DateTime), N'TS_SG_MP01', N'none', NULL, N'T_HNtoSG01'),
(N'Move to Esplanade Theater', N'Move to Esplanade Theater', CAST(N'2023-08-26T16:00:00.000' AS DateTime), CAST(N'2023-08-26T16:30:00.000' AS DateTime), N'none', N'FC_SG_MC01', NULL, N'T_HNtoSG01'),
(N'Visit Esplanade Theater', N'Visit Esplanade - The Theatres on the Bay', CAST(N'2023-08-26T16:30:00.000' AS DateTime), CAST(N'2023-08-26T18:00:00.000' AS DateTime), N'TS_SG_E01', N'none', NULL, N'T_HNtoSG01'),
(N'Move to Mercure Singapore On Stevens Hotel', N'Move to Mercure Singapore On Stevens Hotel', CAST(N'2023-08-26T18:00:00.000' AS DateTime), CAST(N'2023-08-26T18:30:00.000' AS DateTime), N'none', N'FC_SG_MC01', NULL, N'T_HNtoSG01'),
(N'Have dinner and rest', N'Have dinner, a free time period, rest and have breakfast the next morning', CAST(N'2023-08-26T18:30:00.000' AS DateTime), CAST(N'2023-08-27T08:00:00.000' AS DateTime), N'none', N'FC_SG_MSOS01', NULL, N'T_HNtoSG01'),
(N'Move to Gardens by the bay', N'Move to Gardens by the bay', CAST(N'2023-08-27T08:00:00.000' AS DateTime), CAST(N'2023-08-27T09:30:00.000' AS DateTime), N'none', N'FC_SG_MC01', NULL, N'T_HNtoSG01'),
(N'Visit Gardens by the bay', N'Visit the South and East area of the Gardens by the bay', CAST(N'2023-08-27T09:30:00.000' AS DateTime), CAST(N'2023-08-27T11:30:00.000' AS DateTime), N'TS_SG_GBTB01', N'none', NULL, N'T_HNtoSG01'),
(N'Have lunch', N'Have lunch at Perle Noire Resto Bar', CAST(N'2023-08-27T11:30:00.000' AS DateTime), CAST(N'2023-08-27T12:30:00.000' AS DateTime), N'none', N'FC_SG_PNR01', NULL, N'T_HNtoSG01'),
(N'Continue the Garden visit', N'Continue the Garden trip at the Central area', CAST(N'2023-08-27T12:30:00.000' AS DateTime), CAST(N'2023-08-27T15:30:00.000' AS DateTime), N'TS_SG_GBTB01', N'none', NULL, N'T_HNtoSG01'),
(N'Move to Mercure Singapore On Stevens Hotel', N'Return to Mercure Singapore On Stevens Hotel', CAST(N'2023-08-27T15:30:00.000' AS DateTime), CAST(N'2023-08-27T17:00:00.000' AS DateTime), N'none', N'FC_SG_MC01', NULL, N'T_HNtoSG01'),
(N'Have dinner and rest', N'Have dinner and stay the night at Mercure Singapore On Stevens Hotel', CAST(N'2023-08-27T17:00:00.000' AS DateTime), CAST(N'2023-08-28T07:30:00.000' AS DateTime), N'none', N'FC_SG_MSOS01', NULL, N'T_HNtoSG01'),
(N'Have breakfast', N'Have break fast and freely visit the gift shops at the hotel', CAST(N'2023-08-28T07:30:00.000' AS DateTime), CAST(N'2023-08-28T10:45:00.000' AS DateTime), N'none', N'FC_SG_MSOS01', NULL, N'T_HNtoSG01'),
(N'Return to the airport', N'Check out of the hotel and return to Changi - Singapore Airport', CAST(N'2023-08-28T10:45:00.000' AS DateTime), CAST(N'2023-08-28T12:15:00.000' AS DateTime), N'none', N'FC_SG_MC01', NULL, N'T_HNtoSG01'),
(N'Wait for your flight', N'Wait for your flight and check in', CAST(N'2023-08-28T12:15:00.000' AS DateTime), CAST(N'2023-08-28T13:05:00.000' AS DateTime), N'none', N'FC_SG_CS01', NULL, N'T_HNtoSG01'),
(N'Have lunch', N'Have lunch on your flight back to N???i B??i Airport', CAST(N'2023-08-28T13:05:00.000' AS DateTime), CAST(N'2023-08-28T17:25:00.000' AS DateTime), N'none', N'FC_SG_CS01', NULL, N'T_HNtoSG01'),
(N'Tour ends', N'The plane arrives and board the busses to head back home', CAST(N'2023-08-28T17:25:00.000' AS DateTime), CAST(N'2023-08-28T19:45:00.000' AS DateTime), N'none', N'FC_HN_NN01', NULL, N'T_HNtoSG01')

GO