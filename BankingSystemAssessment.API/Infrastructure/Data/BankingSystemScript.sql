USE [master]
GO
/****** Object:  Database [BankingSystem]    Script Date: 5/19/2022 9:01:00 AM ******/
CREATE DATABASE [BankingSystem]
GO
ALTER DATABASE [BankingSystem] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [BankingSystem].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [BankingSystem] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [BankingSystem] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [BankingSystem] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [BankingSystem] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [BankingSystem] SET ARITHABORT OFF 
GO
ALTER DATABASE [BankingSystem] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [BankingSystem] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [BankingSystem] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [BankingSystem] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [BankingSystem] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [BankingSystem] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [BankingSystem] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [BankingSystem] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [BankingSystem] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [BankingSystem] SET  ENABLE_BROKER 
GO
ALTER DATABASE [BankingSystem] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [BankingSystem] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [BankingSystem] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [BankingSystem] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [BankingSystem] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [BankingSystem] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [BankingSystem] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [BankingSystem] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [BankingSystem] SET  MULTI_USER 
GO
ALTER DATABASE [BankingSystem] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [BankingSystem] SET DB_CHAINING OFF 
GO
ALTER DATABASE [BankingSystem] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [BankingSystem] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [BankingSystem] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [BankingSystem] SET QUERY_STORE = OFF
GO
USE [BankingSystem]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 5/19/2022 9:01:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Account]    Script Date: 5/19/2022 9:01:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NOT NULL,
	[AccountNumber] [nvarchar](12) NOT NULL,
	[Balance] [decimal](18, 2) NOT NULL,
	[Currency] [nvarchar](10) NOT NULL,
	[Status] [nvarchar](20) NOT NULL,
	[CreatedDate] [date] NOT NULL,
 CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 5/19/2022 9:01:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerID] [nvarchar](16) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[Mobile] [nvarchar](11) NOT NULL,
	[Email] [nvarchar](256) NOT NULL,
	[Address] [nvarchar](500) NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transaction]    Script Date: 5/19/2022 9:01:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transaction](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AccountId] [int] NOT NULL,
	[ReferenceNumber] [nvarchar](20) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[Debit] [decimal](18, 2) NOT NULL,
	[Credit] [decimal](18, 2) NOT NULL,
	[TransactionDate] [datetimeoffset](7) NOT NULL,
	[TranscationType] [nvarchar](50) NOT NULL,
	[BalanceAfter] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_Transaction] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20220517134454_Baseline', N'5.0.17')
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20220518204947_AddCustomerIDColumnInCustomer', N'5.0.17')
GO
SET IDENTITY_INSERT [dbo].[Account] ON 
GO
INSERT [dbo].[Account] ([Id], [CustomerId], [AccountNumber], [Balance], [Currency], [Status], [CreatedDate]) VALUES (1, 1, N'100048648888', CAST(1000.00 AS Decimal(18, 2)), N'EGP', N'Active', CAST(N'2021-05-16' AS Date))
GO
INSERT [dbo].[Account] ([Id], [CustomerId], [AccountNumber], [Balance], [Currency], [Status], [CreatedDate]) VALUES (2, 2, N'100078592250', CAST(9600.00 AS Decimal(18, 2)), N'USD', N'Active', CAST(N'2020-05-16' AS Date))
GO
INSERT [dbo].[Account] ([Id], [CustomerId], [AccountNumber], [Balance], [Currency], [Status], [CreatedDate]) VALUES (3, 2, N'100078592255', CAST(5000.00 AS Decimal(18, 2)), N'EGP', N'Active', CAST(N'2022-05-16' AS Date))
GO
INSERT [dbo].[Account] ([Id], [CustomerId], [AccountNumber], [Balance], [Currency], [Status], [CreatedDate]) VALUES (4, 2, N'100078592255', CAST(0.00 AS Decimal(18, 2)), N'EGP', N'Suspended', CAST(N'2022-05-16' AS Date))
GO
SET IDENTITY_INSERT [dbo].[Account] OFF
GO
SET IDENTITY_INSERT [dbo].[Customer] ON 
GO
INSERT [dbo].[Customer] ([Id], [CustomerID], [FirstName], [LastName], [Mobile], [Email], [Address]) VALUES (1, N'1234542612688859', N'Doha', N'Abdelkarim', N'01022515455', N'doha.hamed.mohamed@gmail.com', N'Nasr city')
GO
INSERT [dbo].[Customer] ([Id], [CustomerID], [FirstName], [LastName], [Mobile], [Email], [Address]) VALUES (2, N'1230542612688857', N'Peter', N'Adel', N'01188961227', N'Peter.Adel@gmail.com', N'New cairo')
GO
SET IDENTITY_INSERT [dbo].[Customer] OFF
GO
SET IDENTITY_INSERT [dbo].[Transaction] ON 
GO
INSERT [dbo].[Transaction] ([Id], [AccountId], [ReferenceNumber], [Description], [Debit], [Credit], [TransactionDate], [TranscationType], [BalanceAfter]) VALUES (1, 1, N'FT1519991872', N'Deposit 1000 EGP', CAST(0.00 AS Decimal(18, 2)), CAST(1000.00 AS Decimal(18, 2)), CAST(N'2019-05-16T12:10:00.0000000+02:00' AS DateTimeOffset), N'Deposit', CAST(1000.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Transaction] ([Id], [AccountId], [ReferenceNumber], [Description], [Debit], [Credit], [TransactionDate], [TranscationType], [BalanceAfter]) VALUES (2, 1, N'FT1529991872', N'Withdrawal 1000 EGP', CAST(1000.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(N'2019-08-17T00:00:00.0000000+02:00' AS DateTimeOffset), N'Withdrawal', CAST(1000.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Transaction] ([Id], [AccountId], [ReferenceNumber], [Description], [Debit], [Credit], [TransactionDate], [TranscationType], [BalanceAfter]) VALUES (3, 2, N'FT1539991872', N'Deposit 10000 EGP', CAST(0.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)), CAST(N'2020-05-16T00:00:00.0000000+02:00' AS DateTimeOffset), N'Deposit', CAST(10000.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Transaction] ([Id], [AccountId], [ReferenceNumber], [Description], [Debit], [Credit], [TransactionDate], [TranscationType], [BalanceAfter]) VALUES (4, 2, N'FT1514991872', N'Withdrawal 1000 EGP', CAST(1000.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-08-16T00:00:00.0000000+02:00' AS DateTimeOffset), N'Withdrawal', CAST(9000.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Transaction] ([Id], [AccountId], [ReferenceNumber], [Description], [Debit], [Credit], [TransactionDate], [TranscationType], [BalanceAfter]) VALUES (5, 2, N'FT1519591872', N'Deposit 600 EGP', CAST(0.00 AS Decimal(18, 2)), CAST(600.00 AS Decimal(18, 2)), CAST(N'2022-05-16T00:00:00.0000000+02:00' AS DateTimeOffset), N'Deposit', CAST(9600.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Transaction] ([Id], [AccountId], [ReferenceNumber], [Description], [Debit], [Credit], [TransactionDate], [TranscationType], [BalanceAfter]) VALUES (6, 3, N'FT1519971872', N'Deposit 5000 USD', CAST(0.00 AS Decimal(18, 2)), CAST(5000.00 AS Decimal(18, 2)), CAST(N'2022-05-16T00:00:00.0000000+02:00' AS DateTimeOffset), N'Deposit', CAST(5000.00 AS Decimal(18, 2)))
GO
SET IDENTITY_INSERT [dbo].[Transaction] OFF
GO
/****** Object:  Index [IX_Account_CustomerId]    Script Date: 5/19/2022 9:01:00 AM ******/
CREATE NONCLUSTERED INDEX [IX_Account_CustomerId] ON [dbo].[Account]
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Transaction_AccountId]    Script Date: 5/19/2022 9:01:00 AM ******/
CREATE NONCLUSTERED INDEX [IX_Transaction_AccountId] ON [dbo].[Transaction]
(
	[AccountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Account]  WITH CHECK ADD  CONSTRAINT [FK_Account_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
GO
ALTER TABLE [dbo].[Account] CHECK CONSTRAINT [FK_Account_Customer]
GO
ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD  CONSTRAINT [FK_Transaction_Account] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[Transaction] CHECK CONSTRAINT [FK_Transaction_Account]
GO
USE [master]
GO
ALTER DATABASE [BankingSystem] SET  READ_WRITE 
GO
