USE [master]
GO
/****** Object:  Database [YizitTurbo]    Script Date: 2021/10/22 9:53:08 ******/
CREATE DATABASE [YizitTurbo]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'BaseDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\BaseDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'BaseDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\BaseDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [YizitTurbo] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [YizitTurbo].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [YizitTurbo] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [YizitTurbo] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [YizitTurbo] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [YizitTurbo] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [YizitTurbo] SET ARITHABORT OFF 
GO
ALTER DATABASE [YizitTurbo] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [YizitTurbo] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [YizitTurbo] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [YizitTurbo] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [YizitTurbo] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [YizitTurbo] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [YizitTurbo] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [YizitTurbo] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [YizitTurbo] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [YizitTurbo] SET  DISABLE_BROKER 
GO
ALTER DATABASE [YizitTurbo] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [YizitTurbo] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [YizitTurbo] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [YizitTurbo] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [YizitTurbo] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [YizitTurbo] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [YizitTurbo] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [YizitTurbo] SET RECOVERY FULL 
GO
ALTER DATABASE [YizitTurbo] SET  MULTI_USER 
GO
ALTER DATABASE [YizitTurbo] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [YizitTurbo] SET DB_CHAINING OFF 
GO
ALTER DATABASE [YizitTurbo] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [YizitTurbo] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [YizitTurbo] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'YizitTurbo', N'ON'
GO
ALTER DATABASE [YizitTurbo] SET QUERY_STORE = OFF
GO
USE [YizitTurbo]
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
USE [YizitTurbo]
GO
/****** Object:  Table [dbo].[Account]    Script Date: 2021/10/22 9:53:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[ID] [varchar](36) NOT NULL,
	[LoginName] [nvarchar](512) NOT NULL,
	[Password] [nvarchar](128) NULL,
	[Account_Type] [int] NOT NULL,
	[User_Id] [varchar](36) NULL,
	[Company_Id] [varchar](36) NOT NULL,
	[creator] [varchar](36) NULL,
	[creation_time] [bigint] NULL,
	[modifier] [varchar](36) NULL,
	[modification_time] [bigint] NULL,
	[deleted_by] [varchar](36) NULL,
	[deleted_time] [bigint] NULL,
	[deleted] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Company]    Script Date: 2021/10/22 9:53:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Company](
	[ID] [varchar](36) NOT NULL,
	[Name] [nvarchar](512) NOT NULL,
	[No] [nvarchar](128) NULL,
	[Desc] [nvarchar](1024) NULL,
	[TimeZone_Id] [varchar](256) NULL,
	[Manager] [nvarchar](128) NULL,
	[Company_Schema] [nvarchar](128) NULL,
	[Company_DbConnection] [nvarchar](128) NULL,
	[creator] [varchar](36) NULL,
	[creation_time] [bigint] NULL,
	[modifier] [varchar](36) NULL,
	[modification_time] [bigint] NULL,
	[deleted_by] [varchar](36) NULL,
	[deleted_time] [bigint] NULL,
	[deleted] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Company_Applicable_Privileges]    Script Date: 2021/10/22 9:53:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Company_Applicable_Privileges](
	[Enterprise_Id] [varchar](36) NOT NULL,
	[Privilege_Code] [nvarchar](256) NOT NULL,
	[Code_Type] [int] NOT NULL,
	[Desc] [nvarchar](256) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NotficationScope]    Script Date: 2021/10/22 9:53:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NotficationScope](
	[Notification_Id] [varchar](36) NOT NULL,
	[Scope_Type] [int] NOT NULL,
	[Scope_Id] [varchar](36) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notification_Base]    Script Date: 2021/10/22 9:53:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notification_Base](
	[ID] [varchar](36) NOT NULL,
	[PublishType] [int] NOT NULL,
	[PublishTime] [datetime] NOT NULL,
	[NotficationType] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[creator] [varchar](36) NULL,
	[creation_time] [bigint] NULL,
	[modifier] [varchar](36) NULL,
	[modification_time] [bigint] NULL,
	[deleted_by] [varchar](36) NULL,
	[deleted_time] [bigint] NULL,
	[deleted] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NT_Maintenance]    Script Date: 2021/10/22 9:53:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NT_Maintenance](
	[ID] [varchar](36) NOT NULL,
	[Notification_Id] [varchar](36) NOT NULL,
	[Time] [datetime] NOT NULL,
	[Content] [nvarchar](1024) NULL,
	[Title] [nvarchar](128) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NT_Other]    Script Date: 2021/10/22 9:53:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NT_Other](
	[ID] [varchar](36) NOT NULL,
	[Notification_Id] [varchar](36) NOT NULL,
	[Time] [datetime] NOT NULL,
	[Content] [nvarchar](1024) NULL,
	[Title] [nvarchar](128) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NT_Release]    Script Date: 2021/10/22 9:53:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NT_Release](
	[ID] [varchar](36) NOT NULL,
	[Notification_Id] [varchar](36) NOT NULL,
	[Time] [datetime] NOT NULL,
	[Version_No] [nvarchar](128) NOT NULL,
	[Version_Name] [nvarchar](128) NOT NULL,
	[Content] [nvarchar](1024) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NT_Release_Detail]    Script Date: 2021/10/22 9:53:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NT_Release_Detail](
	[ID] [varchar](36) NOT NULL,
	[Release_Id] [varchar](36) NOT NULL,
	[Notification_Id] [varchar](36) NOT NULL,
	[Type] [int] NOT NULL,
	[Content] [nvarchar](1024) NOT NULL,
 CONSTRAINT [PK_NT_Release_Detail] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 2021/10/22 9:53:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[ID] [varchar](36) NOT NULL,
	[Name] [nvarchar](512) NOT NULL,
	[No] [nvarchar](128) NULL,
	[Company_Id] [varchar](36) NOT NULL,
	[creator] [varchar](36) NULL,
	[creation_time] [bigint] NULL,
	[modifier] [varchar](36) NULL,
	[modification_time] [bigint] NULL,
	[deleted_by] [varchar](36) NULL,
	[deleted_time] [bigint] NULL,
	[deleted] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role_Privileges]    Script Date: 2021/10/22 9:53:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role_Privileges](
	[Role_Id] [varchar](36) NOT NULL,
	[Privilege_Code] [nvarchar](256) NOT NULL,
	[Code_Type] [int] NOT NULL,
	[Company_Id] [varchar](36) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Staff]    Script Date: 2021/10/22 9:53:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Staff](
	[ID] [varchar](36) NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
	[No] [nvarchar](128) NULL,
	[Phone] [varchar](36) NULL,
	[Email] [nvarchar](128) NULL,
	[User_Id] [varchar](36) NOT NULL,
	[Company_Id] [varchar](36) NOT NULL,
	[creator] [varchar](36) NULL,
	[creation_time] [bigint] NULL,
	[modifier] [varchar](36) NULL,
	[modification_time] [bigint] NULL,
	[deleted_by] [varchar](36) NULL,
	[deleted_time] [bigint] NULL,
	[deleted] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 2021/10/22 9:53:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[ID] [varchar](36) NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
	[User_Type] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[Company_Id] [varchar](36) NOT NULL,
	[creator] [varchar](36) NULL,
	[creation_time] [bigint] NULL,
	[modifier] [varchar](36) NULL,
	[modification_time] [bigint] NULL,
	[deleted_by] [varchar](36) NULL,
	[deleted_time] [bigint] NULL,
	[deleted] [int] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User_Preference_Notification]    Script Date: 2021/10/22 9:53:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User_Preference_Notification](
	[User_Id] [varchar](36) NOT NULL,
	[Notification_Id] [varchar](36) NOT NULL,
	[creation_time] [bigint] NOT NULL,
	[deleted_time] [bigint] NULL,
	[deleted] [int] NOT NULL,
 CONSTRAINT [PK_User_Preference_Notification] PRIMARY KEY CLUSTERED 
(
	[User_Id] ASC,
	[Notification_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRoles]    Script Date: 2021/10/22 9:53:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoles](
	[User_Id] [varchar](36) NOT NULL,
	[Role_Id] [varchar](36) NOT NULL,
	[Company_Id] [varchar](36) NOT NULL
) ON [PRIMARY]
GO
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'C11CC901-DF52-4401-9055-42CBA50544D7', N'9', N'96e79218965eb72c92a549dd5a330112', 0, N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', NULL, 1618820988390, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'3A5C81BF-1211-44CD-A383-573C4D2177BD', N'admin', N'96e79218965eb72c92a549dd5a330112', 0, N'38AC9911-9406-4D71-9C90-17E3CBF48050', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', NULL, 1618820988390, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783578074, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'ccba112d-1d77-4cb2-833a-838294d12f87', N'testModify11', N'96e79218965eb72c92a549dd5a330112', 0, N'27d2c853-d410-404a-b7a8-8db921c4ec79', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 0, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1630054965609, NULL, NULL, 1)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'6206919a-94f6-4952-8716-1fa69bb2325c', N'testCJL', N'96e79218965eb72c92a549dd5a330112', 0, N'616df909-6eda-4ac6-99d0-359e3666923f', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 0, N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1632303744060, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'be85e41b-5406-4b1a-aa69-820ce109f51b', N'xuliang', N'96e79218965eb72c92a549dd5a330112', 0, N'd3aaf2f7-2e54-4bb7-b019-e44a1674d3f1', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1629793963529, N'd3aaf2f7-2e54-4bb7-b019-e44a1674d3f1', 1630054739824, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'C11CC901-DF52-4401-9055-42CBA50544D8', N'9', N'96e79218965eb72c92a549dd5a330112', 0, N'32FBB7F4-3656-4990-B0D1-EF1680C90F18', N'69A7016A-CB64-42C9-94A2-174A8AA51A55', NULL, 1618820988390, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'82f0e618-d84f-4847-82af-c67c779dd4ed', N'admin', N'96e79218965eb72c92a549dd5a330112', 0, N'5cc2687c-edf8-4e26-9598-1b2fe7fcc646', N'cd5a4b03-942e-4e59-95b9-e2dd4bf01796', N'00000000-0000-0000-0000-000000000000', 1630047766131, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'a5103946-0148-4d6b-a336-36304eba33b5', N'1', N'96e79218965eb72c92a549dd5a330112', 0, N'5587b248-0ca5-40f4-8e5b-e0fbba4a8a61', N'e892704f-ef88-4c6f-a066-5b12b06d7241', N'00000000-0000-0000-0000-000000000000', 1630047877608, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'3df212ba-fe64-4a4e-8d8a-2fbe2d423029', N'0623', N'96e79218965eb72c92a549dd5a330112', 0, N'060e5b9f-69e3-4b4f-856d-50a6f56eca5f', N'30935dd2-438f-4eb7-a6b0-8f6256a9c8e6', N'00000000-0000-0000-0000-000000000000', 1630047932472, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'4e2d92be-2180-468d-a503-413b0921a6ba', N'bb', N'96e79218965eb72c92a549dd5a330112', 0, N'a8d4111a-cd09-4d82-9765-fc3ea3659034', N'b3248e1a-cb4e-4362-939d-b7045afd8b65', N'00000000-0000-0000-0000-000000000000', 1630051299971, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'fd2ad2ca-a578-4280-b982-a502415c6c44', N'xuliang', N'e3ceb5881a0a1fdaad01296d7554868d', 0, N'8e22695e-f69a-498d-a53f-0cff408e396b', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1630051592892, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1631696956028, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'8772a0a6-6a6b-4580-99a3-24bef15be3b6', N'laoxu', N'7fa8282ad93047a4d6fe6111c93b308a', 0, N'64d98fdc-1d6a-40a1-a77d-f1597e13111f', N'dee15935-682b-4ad0-8c5c-5a2f7c9aca37', N'00000000-0000-0000-0000-000000000000', 1630052465239, N'64d98fdc-1d6a-40a1-a77d-f1597e13111f', 1630052816130, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'4db40433-1062-42f1-878e-f94ce0239247', N'laoxu', N'96e79218965eb72c92a549dd5a330112', 0, N'a7059ff2-6710-450f-8b23-80f52df2cb63', N'fc898bc4-0f19-47a8-a692-83d1e56733ef', N'00000000-0000-0000-0000-000000000000', 1630052515975, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'63a59a74-d6c8-4143-bf55-629d1ab2d916', N'jiadm1', N'96e79218965eb72c92a549dd5a330112', 0, N'97744a5f-58e7-4a0a-b60f-4adfbf9cbb1f', N'fc898bc4-0f19-47a8-a692-83d1e56733ef', N'a7059ff2-6710-450f-8b23-80f52df2cb63', 1630053154723, N'a7059ff2-6710-450f-8b23-80f52df2cb63', 1630054043333, NULL, NULL, 1)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'0ec454e5-8a46-4d2c-9dcd-2e43f5cf7a00', N'liuqy', N'7fa8282ad93047a4d6fe6111c93b308a', 0, N'844beb89-1f8a-45a6-875e-3de1ea09694f', N'fc898bc4-0f19-47a8-a692-83d1e56733ef', N'a7059ff2-6710-450f-8b23-80f52df2cb63', 1630053184739, N'a7059ff2-6710-450f-8b23-80f52df2cb63', 1630054186265, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'C11CC901-DF52-4401-9055-42CBA5054400', N'jiadm', N'', 0, N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', NULL, 1618820988390, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'6f212cc3-b550-481c-8446-f51b54b410aa', N'admin', NULL, 1, N'1000000', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'1000023', 1634722425241, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783503932, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'7a826e43-36be-4a3d-885c-5ad6d117f38a', N'zhulf', NULL, 1, N'1000001', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'1000023', 1631841235149, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783503994, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'e7790d02-0bad-4405-8eef-ff439d468c47', N'taojun', NULL, 1, N'1000019', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'd3aaf2f7-2e54-4bb7-b019-e44a1674d3f1', 1631943296132, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504706, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'3893172f-7e0c-45a8-ac47-0ebb0de4c449', N'yezj', NULL, 1, N'1000007', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'1000023', 1631943433967, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504250, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'f354e898-4110-40aa-a128-87e7c4f888fe', N'yeyc', NULL, 1, N'1000046', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'1000023', 1634779221152, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783505541, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'09758d3d-e605-4623-946c-fa8983a271c9', N'沈', N'96e79218965eb72c92a549dd5a330112', 0, N'42724610-76ae-4674-8c8d-471d7c3235b1', N'226f0026-6fe2-4429-bc46-38ff8eae2c46', N'00000000-0000-0000-0000-000000000000', 1630046451865, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'c9020bf3-90d0-432a-a9e1-4c59aaecc62b', N'xuliang', N'96e79218965eb72c92a549dd5a330112', 0, N'10539492-cad8-47c4-8810-68971beaf208', N'fc898bc4-0f19-47a8-a692-83d1e56733ef', N'a7059ff2-6710-450f-8b23-80f52df2cb63', 1630052947165, N'a7059ff2-6710-450f-8b23-80f52df2cb63', 1631697051008, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'06d30032-cd6e-4f6e-8e7e-aa11660ee4c0', N'jiadm1', N'96e79218965eb72c92a549dd5a330112', 0, N'ebcca99f-e1ca-409d-ba1e-504028fd3775', N'fc898bc4-0f19-47a8-a692-83d1e56733ef', N'a7059ff2-6710-450f-8b23-80f52df2cb63', 1630053988837, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'32fdafc2-f6f2-4419-bfc3-12dffa52afee', N'admin', NULL, 1, N'1000000', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631677969440, N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1632302671011, NULL, NULL, 1)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'cbf360ba-a423-408d-9489-fded65cf1655', N'zhulf', NULL, 1, N'1000001', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631677976317, N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1632302677508, NULL, NULL, 1)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'74ec9041-c6f2-4e4e-bab7-342a708997d6', N'liuqy', NULL, 1, N'1000004', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678229756, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504073, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'6f7f839c-5102-400f-be9e-60e3b65e7df5', N'shenzj', NULL, 1, N'1000005', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678229852, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504127, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'b9eedc78-ca90-4d5a-8399-c08c1a0c71ef', N'shenchong', NULL, 1, N'1000006', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678229928, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504184, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'd45abda2-d10a-44e2-b12a-42930a691f37', N'yezj', NULL, 1, N'1000007', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678230009, N'1000023', 1631943452131, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'15ef3208-7864-47c2-8ce7-e6f4ee335ca4', N'gubh', NULL, 1, N'1000010', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678230093, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504309, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'9dca6bb2-a540-41a3-9112-b60e8b058f14', N'dingpp', NULL, 1, N'1000011', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678230176, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504363, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'6e5b3233-e6bd-468c-9774-c3e53424e65a', N'dongwj', NULL, 1, N'1000012', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678230265, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504413, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'd6c3b90a-a7d2-4ed7-a62c-d5988a50e2ea', N'huanglw', NULL, 1, N'1000013', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678230361, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504468, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'0c9cd788-1fb9-4f85-8a2a-b4a475379b27', N'lingkb', NULL, 1, N'1000015', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678230509, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504519, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'151f1764-be34-435d-864c-4d27733b46d7', N'liuln', NULL, 1, N'1000016', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678230647, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504564, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'fe375136-17ea-48b8-b246-83b639141bc2', N'shenmq', NULL, 1, N'1000017', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678230737, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504608, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'aa03a36f-881a-407c-a082-b16a8fc10ba4', N'luming', NULL, 1, N'1000018', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678230816, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504657, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'f5e29aeb-74cc-4621-98ec-0ec63cbaf7e0', N'taojun', NULL, 1, N'1000019', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678230895, N'1000023', 1631943452525, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'e92abab3-9727-4a75-8491-7a149057ecf2', N'wuchen', NULL, 1, N'1000020', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678230968, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504749, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'1c7057b4-adef-4b3f-88f8-8c84b39e6293', N'xuliang', NULL, 1, N'1000021', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678231045, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504809, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'5e4ec9bf-5186-4fcf-939e-a75b705d3f32', N'wangyu', NULL, 1, N'1000022', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678231119, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504855, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'9275ec27-3697-4a49-99d5-3e970a04906e', N'jiadm', NULL, 1, N'1000023', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678231196, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504913, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'f9b4b9a4-b99e-4971-944e-7660d5d632e3', N'shengxy', NULL, 1, N'1000024', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678231286, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504953, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'f8efe50a-7a66-461a-87ed-24dd23a8f5b4', N'chenjie', NULL, 1, N'1000025', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678231363, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504990, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'fdce3974-346b-4db3-84ae-3fa71741ec6f', N'wanghao', NULL, 1, N'1000026', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678231442, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783505026, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'96ff4182-2920-40d8-90b8-ede00ae7a138', N'suzh', NULL, 1, N'1000028', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678231521, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783505067, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'78d1c328-6649-4029-849b-a8920c409994', N'qianjin', NULL, 1, N'1000029', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678231606, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783505109, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'd33f7dbe-58f3-4b85-a91e-888a70b5949c', N'juxj', NULL, 1, N'1000030', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678231689, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783505151, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'501de3d8-ea07-4dc4-aaae-1c7e810b2c50', N'jiling', NULL, 1, N'1000031', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678231833, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783505201, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'cd491c86-38f4-4589-b1a1-5f7dae999cf4', N'hongjb', NULL, 1, N'1000032', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678231929, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783505241, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'4f1eb151-1800-4eec-8082-aebf68de80f4', N'jibo', NULL, 1, N'1000033', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678232015, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783505269, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'b6f09b8b-40fc-421f-9894-6d9037bd2aa7', N'tju', NULL, 1, N'1000034', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678232095, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783505295, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'db69d2e5-cd99-4021-9c88-60c5d5c9b216', N'shengzb', NULL, 1, N'1000035', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678232170, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783505322, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'7e90a5a8-fee2-4898-a1df-950136fa3af8', N'jenkins', NULL, 1, N'1000036', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678232251, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783505349, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'1440c568-f378-42c3-9786-247a732eb165', N'zhangwb', NULL, 1, N'1000037', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678232335, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783505377, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'c1319a1f-1cf0-4d29-9725-e5e2b5824ce8', N'zhourun', NULL, 1, N'1000039', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678232416, N'1000023', 1631947728532, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'024a8309-de01-4b6d-bde0-70dd8a80c58a', N'jianghj', NULL, 1, N'1000040', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678232494, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783505405, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'5323d6b2-a12d-419e-b542-1460e96f6d73', N'chenly', NULL, 1, N'1000041', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678232573, N'1000023', 1631946030567, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'91ab098a-ce44-4c60-8295-f115cb6667ca', N'wangyj', NULL, 1, N'1000042', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678232653, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783505433, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'c22efc87-bc28-4703-9813-74258a804ee2', N'huangqy', NULL, 1, N'1000043', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678232738, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783505463, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'41a1912d-0975-41d5-abbe-1b8f588aa11a', N'xiongwei', NULL, 1, N'1000044', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678232823, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783505489, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'14d99663-c2ca-4b80-b3c3-2828c588f3c9', N'zhangyue', NULL, 1, N'1000045', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'd3aaf2f7-2e54-4bb7-b019-e44a1674d3f1', 1631782961749, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783505515, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'4724edf1-6fd0-4896-a433-11cb197293c4', N'lingkb', NULL, 0, N'1000015', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'1000023', 1631944122502, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[Account] ([ID], [LoginName], [Password], [Account_Type], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'3f7ecd6d-fc85-4f5e-97af-aceeaf82a350', N'贾东明', N'96e79218965eb72c92a549dd5a330112', 0, N'bb186ced-30fa-4a32-ad5b-afdb7321abc9', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1631948356116, N'1000023', 1631949950352, NULL, NULL, 0)
INSERT [dbo].[Company] ([ID], [Name], [No], [Desc], [TimeZone_Id], [Manager], [Company_Schema], [Company_DbConnection], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'Yizit JX', N'44', N'444', N'Hawaiian Standard Time', N'admin', NULL, NULL, NULL, 1618820988390, N'00000000-0000-0000-0000-000000000000', 1634784866273, NULL, NULL, 0)
INSERT [dbo].[Company] ([ID], [Name], [No], [Desc], [TimeZone_Id], [Manager], [Company_Schema], [Company_DbConnection], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'b3248e1a-cb4e-4362-939d-b7045afd8b65', N'Yizit MX', N'MX', N'墨西哥分部', N'Pacific Standard Time (Mexico)', N'bb', NULL, NULL, N'00000000-0000-0000-0000-000000000000', 1630051299572, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[Company] ([ID], [Name], [No], [Desc], [TimeZone_Id], [Manager], [Company_Schema], [Company_DbConnection], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'dee15935-682b-4ad0-8c5c-5a2f7c9aca37', N'老徐大世界', N'Laoxu', N'111', N'China Standard Time', N'laoxu', NULL, NULL, N'00000000-0000-0000-0000-000000000000', 1630052465226, N'00000000-0000-0000-0000-000000000000', 1630052478664, NULL, NULL, 0)
INSERT [dbo].[Company] ([ID], [Name], [No], [Desc], [TimeZone_Id], [Manager], [Company_Schema], [Company_DbConnection], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'fc898bc4-0f19-47a8-a692-83d1e56733ef', N'老徐大世界1', N'Laoxu', NULL, N'Hawaiian Standard Time', N'laoxu', NULL, NULL, N'00000000-0000-0000-0000-000000000000', 1630052515806, N'00000000-0000-0000-0000-000000000000', 1630052675784, NULL, NULL, 0)
INSERT [dbo].[Company_Applicable_Privileges] ([Enterprise_Id], [Privilege_Code], [Code_Type], [Desc]) VALUES (N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'menu.sys-conf.authority.user', 0, NULL)
INSERT [dbo].[Company_Applicable_Privileges] ([Enterprise_Id], [Privilege_Code], [Code_Type], [Desc]) VALUES (N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'menu.sys-conf.authority.role', 0, NULL)
INSERT [dbo].[Company_Applicable_Privileges] ([Enterprise_Id], [Privilege_Code], [Code_Type], [Desc]) VALUES (N'69A7016A-CB64-42C9-94A2-174A8AA51A55', N'menu.sys-conf.authority.role', 0, NULL)
INSERT [dbo].[Company_Applicable_Privileges] ([Enterprise_Id], [Privilege_Code], [Code_Type], [Desc]) VALUES (N'226f0026-6fe2-4429-bc46-38ff8eae2c46', N'menu.sys-conf.authority.user', 0, NULL)
INSERT [dbo].[Company_Applicable_Privileges] ([Enterprise_Id], [Privilege_Code], [Code_Type], [Desc]) VALUES (N'fc898bc4-0f19-47a8-a692-83d1e56733ef', N'menu.sys-conf.authority.user', 0, NULL)
INSERT [dbo].[Company_Applicable_Privileges] ([Enterprise_Id], [Privilege_Code], [Code_Type], [Desc]) VALUES (N'69A7016A-CB64-42C9-94A2-174A8AA51A55', N'menu.sys-conf.authority.user', 0, NULL)
INSERT [dbo].[Company_Applicable_Privileges] ([Enterprise_Id], [Privilege_Code], [Code_Type], [Desc]) VALUES (N'e6707fba-c5e8-4338-89be-3314a889cad2', N'menu.sys-conf.authority.role', 0, NULL)
INSERT [dbo].[Company_Applicable_Privileges] ([Enterprise_Id], [Privilege_Code], [Code_Type], [Desc]) VALUES (N'e6707fba-c5e8-4338-89be-3314a889cad2', N'menu.sys-conf.authority.user', 0, NULL)
INSERT [dbo].[Company_Applicable_Privileges] ([Enterprise_Id], [Privilege_Code], [Code_Type], [Desc]) VALUES (N'b69fd8a4-4dff-4064-80c9-17ea0baae9af', N'menu.sys-conf.authority.role', 0, NULL)
INSERT [dbo].[Company_Applicable_Privileges] ([Enterprise_Id], [Privilege_Code], [Code_Type], [Desc]) VALUES (N'b69fd8a4-4dff-4064-80c9-17ea0baae9af', N'menu.sys-conf.authority.user', 0, NULL)
INSERT [dbo].[Company_Applicable_Privileges] ([Enterprise_Id], [Privilege_Code], [Code_Type], [Desc]) VALUES (N'463f3576-d175-45d3-aabc-d9fce8f44ff3', N'menu.sys-conf.authority.role', 0, NULL)
INSERT [dbo].[Company_Applicable_Privileges] ([Enterprise_Id], [Privilege_Code], [Code_Type], [Desc]) VALUES (N'463f3576-d175-45d3-aabc-d9fce8f44ff3', N'menu.sys-conf.authority.user', 0, NULL)
INSERT [dbo].[Company_Applicable_Privileges] ([Enterprise_Id], [Privilege_Code], [Code_Type], [Desc]) VALUES (N'b3248e1a-cb4e-4362-939d-b7045afd8b65', N'menu.sys-conf.authority.role', 0, NULL)
INSERT [dbo].[Company_Applicable_Privileges] ([Enterprise_Id], [Privilege_Code], [Code_Type], [Desc]) VALUES (N'b3248e1a-cb4e-4362-939d-b7045afd8b65', N'menu.sys-conf.authority.user', 0, NULL)
INSERT [dbo].[Notification_Base] ([ID], [PublishType], [PublishTime], [NotficationType], [Status], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'ffcf362f-3f5b-4431-9b12-abdbb11c8c85', 0, CAST(N'2021-09-26T02:25:06.000' AS DateTime), 1, 0, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1632623209229, NULL, 1632623211219, NULL, NULL, 0)
INSERT [dbo].[Notification_Base] ([ID], [PublishType], [PublishTime], [NotficationType], [Status], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'329f9369-e9cd-46cd-a806-aee11dd8921c', 0, CAST(N'2021-09-26T02:25:06.000' AS DateTime), 1, 0, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1632623282927, NULL, 1632623284962, NULL, NULL, 0)
INSERT [dbo].[Notification_Base] ([ID], [PublishType], [PublishTime], [NotficationType], [Status], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'c096be34-d269-429a-a610-11337befe25a', 1, CAST(N'2021-10-27T11:47:33.000' AS DateTime), 1, -1, N'00000000-0000-0000-0000-000000000000', 1634615756890, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[Notification_Base] ([ID], [PublishType], [PublishTime], [NotficationType], [Status], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'2d62a196-918b-48d9-8552-7c18da9ad440', 0, CAST(N'2021-10-19T04:04:05.000' AS DateTime), 1, 0, N'00000000-0000-0000-0000-000000000000', 1634616245697, NULL, 1634616247708, NULL, NULL, 0)
INSERT [dbo].[Notification_Base] ([ID], [PublishType], [PublishTime], [NotficationType], [Status], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'4b2b36de-826c-4219-a0a5-b0ff1ff69376', 0, CAST(N'2021-10-19T08:12:13.000' AS DateTime), 0, 1, N'00000000-0000-0000-0000-000000000000', 1634631136131, N'00000000-0000-0000-0000-000000000000', 1634711663466, NULL, NULL, 0)
INSERT [dbo].[Notification_Base] ([ID], [PublishType], [PublishTime], [NotficationType], [Status], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'9c505244-c1ec-42ea-aef3-911e7ebe8e1d', 0, CAST(N'2021-10-19T08:12:46.000' AS DateTime), 0, 0, N'00000000-0000-0000-0000-000000000000', 1634631166216, N'00000000-0000-0000-0000-000000000000', 1634709618253, N'00000000-0000-0000-0000-000000000000', 1634709618253, 1)
INSERT [dbo].[Notification_Base] ([ID], [PublishType], [PublishTime], [NotficationType], [Status], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'591ad055-1121-42fd-a4d7-500204960323', 1, CAST(N'2021-10-30T13:59:37.000' AS DateTime), 0, 1, N'00000000-0000-0000-0000-000000000000', 1634709644743, N'00000000-0000-0000-0000-000000000000', 1634711677047, NULL, NULL, 0)
INSERT [dbo].[Notification_Base] ([ID], [PublishType], [PublishTime], [NotficationType], [Status], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'569358da-e66b-4653-b82a-9a4daf06a33a', 1, CAST(N'2021-10-28T14:00:26.000' AS DateTime), 0, 1, N'00000000-0000-0000-0000-000000000000', 1634709692518, N'00000000-0000-0000-0000-000000000000', 1634710031335, NULL, NULL, 0)
INSERT [dbo].[Notification_Base] ([ID], [PublishType], [PublishTime], [NotficationType], [Status], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'0f484946-49e3-42da-9625-c9bc6a962d0b', 1, CAST(N'2021-10-22T14:01:33.000' AS DateTime), 0, 1, N'00000000-0000-0000-0000-000000000000', 1634709758910, N'00000000-0000-0000-0000-000000000000', 1634710912122, NULL, NULL, 0)
INSERT [dbo].[Notification_Base] ([ID], [PublishType], [PublishTime], [NotficationType], [Status], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'547630d2-8499-4df2-8961-85d184112221', 0, CAST(N'2021-10-20T06:05:52.000' AS DateTime), 0, 0, N'00000000-0000-0000-0000-000000000000', 1634709952560, N'00000000-0000-0000-0000-000000000000', 1634710896270, N'00000000-0000-0000-0000-000000000000', 1634710896270, 1)
INSERT [dbo].[Notification_Base] ([ID], [PublishType], [PublishTime], [NotficationType], [Status], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'294f93eb-0b4a-4ce0-8c33-075dda608225', 0, CAST(N'2021-10-20T06:07:00.000' AS DateTime), 0, 0, N'00000000-0000-0000-0000-000000000000', 1634710020524, NULL, 1634710022557, NULL, NULL, 0)
INSERT [dbo].[Notification_Base] ([ID], [PublishType], [PublishTime], [NotficationType], [Status], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'6f44b119-d747-48b8-b596-7fc5e1d292c6', 1, CAST(N'2021-10-31T14:33:53.000' AS DateTime), 0, -1, N'00000000-0000-0000-0000-000000000000', 1634711702581, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[Notification_Base] ([ID], [PublishType], [PublishTime], [NotficationType], [Status], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'6395b611-b78b-44fa-a306-fb56699c3757', 0, CAST(N'2021-10-21T05:51:33.000' AS DateTime), 0, 0, N'00000000-0000-0000-0000-000000000000', 1634795493788, NULL, 1634795495804, NULL, NULL, 0)
INSERT [dbo].[Notification_Base] ([ID], [PublishType], [PublishTime], [NotficationType], [Status], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'2c9c340e-6764-4833-abd7-b82064f3bd89', 0, CAST(N'2021-10-21T09:42:01.000' AS DateTime), 0, 0, N'00000000-0000-0000-0000-000000000000', 1634809321834, NULL, 1634809323869, NULL, NULL, 0)
INSERT [dbo].[Notification_Base] ([ID], [PublishType], [PublishTime], [NotficationType], [Status], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'3840ea9d-f7ad-4ee9-85e0-2e36925190cf', 1, CAST(N'2021-09-23T09:16:44.000' AS DateTime), 2, 0, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1632359688337, NULL, 1632359808510, NULL, NULL, 0)
INSERT [dbo].[Notification_Base] ([ID], [PublishType], [PublishTime], [NotficationType], [Status], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'a6a118e8-8ca3-497b-aac9-a81f34c0f303', 0, CAST(N'2021-09-23T07:09:07.000' AS DateTime), 2, 0, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1632381342677, NULL, 1632381344872, NULL, NULL, 0)
INSERT [dbo].[Notification_Base] ([ID], [PublishType], [PublishTime], [NotficationType], [Status], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'cd38b8b3-8db1-4b31-b5ee-5a66f2dab5cc', 0, CAST(N'2021-09-22T00:00:00.000' AS DateTime), 0, 0, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1632381350920, NULL, 1632381353042, NULL, NULL, 0)
INSERT [dbo].[Notification_Base] ([ID], [PublishType], [PublishTime], [NotficationType], [Status], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'1a0c63b4-36b5-4411-9a62-09d9035fc03d', 0, CAST(N'2021-10-21T02:41:51.000' AS DateTime), 0, 0, N'00000000-0000-0000-0000-000000000000', 1634784111785, NULL, 1634784113797, NULL, NULL, 0)
INSERT [dbo].[Notification_Base] ([ID], [PublishType], [PublishTime], [NotficationType], [Status], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'2e4e796e-4d24-4a91-9da1-06fa251dc81d', 0, CAST(N'2021-10-21T03:07:32.000' AS DateTime), 0, 0, N'00000000-0000-0000-0000-000000000000', 1634785652015, NULL, 1634785654038, NULL, NULL, 0)
INSERT [dbo].[Notification_Base] ([ID], [PublishType], [PublishTime], [NotficationType], [Status], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'484307bb-b0c9-47cf-acf5-7f7a9a7c2e44', 0, CAST(N'2021-10-20T05:47:54.000' AS DateTime), 0, 0, N'00000000-0000-0000-0000-000000000000', 1634708874935, N'00000000-0000-0000-0000-000000000000', 1634709612701, N'00000000-0000-0000-0000-000000000000', 1634709612700, 1)
INSERT [dbo].[Notification_Base] ([ID], [PublishType], [PublishTime], [NotficationType], [Status], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'a9f8b1b9-4744-4e3d-a332-3ce99fe0bd99', 0, CAST(N'2021-10-21T02:22:38.000' AS DateTime), 0, 0, N'00000000-0000-0000-0000-000000000000', 1634782958634, NULL, 1634782960692, NULL, NULL, 0)
INSERT [dbo].[Notification_Base] ([ID], [PublishType], [PublishTime], [NotficationType], [Status], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'0296ace0-0207-4366-a295-48181dc08e24', 0, CAST(N'2021-10-21T02:22:59.000' AS DateTime), 0, 0, N'00000000-0000-0000-0000-000000000000', 1634782979466, NULL, 1634782981477, NULL, NULL, 0)
INSERT [dbo].[Notification_Base] ([ID], [PublishType], [PublishTime], [NotficationType], [Status], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'2903d5e6-c794-4893-adea-22e79491f16f', 0, CAST(N'2021-10-21T02:24:16.000' AS DateTime), 0, 0, N'00000000-0000-0000-0000-000000000000', 1634783056990, NULL, 1634783059005, NULL, NULL, 0)
INSERT [dbo].[Notification_Base] ([ID], [PublishType], [PublishTime], [NotficationType], [Status], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'60983bd1-592d-4490-b46a-302baaff928c', 0, CAST(N'2021-10-21T02:25:45.000' AS DateTime), 0, 0, N'00000000-0000-0000-0000-000000000000', 1634783145471, NULL, 1634783147493, NULL, NULL, 0)
INSERT [dbo].[Notification_Base] ([ID], [PublishType], [PublishTime], [NotficationType], [Status], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'a7374305-6443-40f6-8c57-8b46f494eec6', 0, CAST(N'2021-10-21T05:16:29.000' AS DateTime), 0, 0, N'00000000-0000-0000-0000-000000000000', 1634793389851, NULL, 1634793391864, NULL, NULL, 0)
INSERT [dbo].[NT_Maintenance] ([ID], [Notification_Id], [Time], [Content], [Title]) VALUES (N'5d42496f-32b4-41ca-9424-7a99499e8763', N'3840ea9d-f7ad-4ee9-85e0-2e36925190cf', CAST(N'2021-09-23T01:12:44.593' AS DateTime), N'维护内容', N'这是一个维护通知')
INSERT [dbo].[NT_Maintenance] ([ID], [Notification_Id], [Time], [Content], [Title]) VALUES (N'7c3c8dd8-6891-434d-b085-525051895446', N'a6a118e8-8ca3-497b-aac9-a81f34c0f303', CAST(N'2021-09-23T07:09:07.253' AS DateTime), N'这是维护通知', N'维护通知')
INSERT [dbo].[NT_Other] ([ID], [Notification_Id], [Time], [Content], [Title]) VALUES (N'0dae590a-3c1c-4fbf-a9b7-c8904c7a0286', N'4b2b36de-826c-4219-a0a5-b0ff1ff69376', CAST(N'2021-10-19T08:12:15.980' AS DateTime), N'f 水电费第三方都是', N'士大夫')
INSERT [dbo].[NT_Other] ([ID], [Notification_Id], [Time], [Content], [Title]) VALUES (N'08c4bd78-ea66-4bb5-8c46-268171c0ded6', N'9c505244-c1ec-42ea-aef3-911e7ebe8e1d', CAST(N'2021-10-19T08:12:46.217' AS DateTime), N'AS的', N'阿萨德ASD')
INSERT [dbo].[NT_Other] ([ID], [Notification_Id], [Time], [Content], [Title]) VALUES (N'1628588d-07d4-4d85-9bf9-936cd0e0e1ca', N'484307bb-b0c9-47cf-acf5-7f7a9a7c2e44', CAST(N'2021-10-20T05:47:54.933' AS DateTime), N'asdsad', N'sadsad')
INSERT [dbo].[NT_Other] ([ID], [Notification_Id], [Time], [Content], [Title]) VALUES (N'fee5545f-a6cb-4425-89e1-fa11ba590059', N'591ad055-1121-42fd-a4d7-500204960323', CAST(N'2021-10-20T06:03:47.063' AS DateTime), N'fdgfdgsdfadsf', N'fgfdg')
INSERT [dbo].[NT_Other] ([ID], [Notification_Id], [Time], [Content], [Title]) VALUES (N'4ee60ea9-a5ed-407f-9241-bfa6ecfc1ae2', N'569358da-e66b-4653-b82a-9a4daf06a33a', CAST(N'2021-10-20T06:01:32.517' AS DateTime), N'sdafdsa', N'dsfasdf')
INSERT [dbo].[NT_Other] ([ID], [Notification_Id], [Time], [Content], [Title]) VALUES (N'843b053f-6b77-473b-810c-1837bd9d5397', N'0f484946-49e3-42da-9625-c9bc6a962d0b', CAST(N'2021-10-20T06:02:38.910' AS DateTime), N'fdsafdsaf', N'sadfdsa')
INSERT [dbo].[NT_Other] ([ID], [Notification_Id], [Time], [Content], [Title]) VALUES (N'407efe3d-c8f7-43ed-a0dc-b410a7219408', N'547630d2-8499-4df2-8961-85d184112221', CAST(N'2021-10-20T06:05:52.560' AS DateTime), N'fsdfdsf', N'dsfdsfds')
INSERT [dbo].[NT_Other] ([ID], [Notification_Id], [Time], [Content], [Title]) VALUES (N'00d0b04f-e5e1-4b0f-96e0-bb866c31b030', N'294f93eb-0b4a-4ce0-8c33-075dda608225', CAST(N'2021-10-20T06:07:00.523' AS DateTime), N'sdfasdf', N'sadfsdaf')
INSERT [dbo].[NT_Other] ([ID], [Notification_Id], [Time], [Content], [Title]) VALUES (N'd97ea3cf-a78b-46aa-921c-f5e19c599020', N'6f44b119-d747-48b8-b596-7fc5e1d292c6', CAST(N'2021-10-20T06:35:02.580' AS DateTime), N'111', N'111')
INSERT [dbo].[NT_Other] ([ID], [Notification_Id], [Time], [Content], [Title]) VALUES (N'cf5ff1d1-1f26-46ba-a9fe-3c48bfdacf7d', N'a9f8b1b9-4744-4e3d-a332-3ce99fe0bd99', CAST(N'2021-10-21T02:22:38.567' AS DateTime), N'fghgfh', N'gfhgfh')
INSERT [dbo].[NT_Other] ([ID], [Notification_Id], [Time], [Content], [Title]) VALUES (N'76f839a6-28dc-4fae-99c7-3b0910874a76', N'0296ace0-0207-4366-a295-48181dc08e24', CAST(N'2021-10-21T02:22:59.467' AS DateTime), N'dfgfdg', N'dfgfdg')
INSERT [dbo].[NT_Other] ([ID], [Notification_Id], [Time], [Content], [Title]) VALUES (N'37b1c0e0-6a4b-4b5f-a147-33f7778b0e8e', N'2903d5e6-c794-4893-adea-22e79491f16f', CAST(N'2021-10-21T02:24:16.990' AS DateTime), N'gfhgf', N'fghfgh')
INSERT [dbo].[NT_Other] ([ID], [Notification_Id], [Time], [Content], [Title]) VALUES (N'f65dd389-9efc-4bc9-9342-179031e0bcb6', N'60983bd1-592d-4490-b46a-302baaff928c', CAST(N'2021-10-21T02:25:45.470' AS DateTime), N'fdsfasdfasdfdsf', N'sdfsda')
INSERT [dbo].[NT_Other] ([ID], [Notification_Id], [Time], [Content], [Title]) VALUES (N'5433a1d2-f0f3-4822-9fdf-ab4d06d2237f', N'1a0c63b4-36b5-4411-9a62-09d9035fc03d', CAST(N'2021-10-21T02:41:51.780' AS DateTime), N'gfhfghgfh', N'ffghfgh')
INSERT [dbo].[NT_Other] ([ID], [Notification_Id], [Time], [Content], [Title]) VALUES (N'79cd28ab-add9-4eff-a858-f5632bb2417d', N'2e4e796e-4d24-4a91-9da1-06fa251dc81d', CAST(N'2021-10-21T03:07:32.013' AS DateTime), N'dsfdsfsd', N'sdfdsf')
INSERT [dbo].[NT_Other] ([ID], [Notification_Id], [Time], [Content], [Title]) VALUES (N'd02b214f-506c-4a3b-9a9c-64954dd3523e', N'a7374305-6443-40f6-8c57-8b46f494eec6', CAST(N'2021-10-21T05:16:29.850' AS DateTime), N'g电饭锅', N'讽德诵功缩放的')
INSERT [dbo].[NT_Other] ([ID], [Notification_Id], [Time], [Content], [Title]) VALUES (N'bc47df99-b291-41bd-afcb-70dee2c892f2', N'6395b611-b78b-44fa-a306-fb56699c3757', CAST(N'2021-10-21T05:51:33.787' AS DateTime), N'asdfdsf', N'sdfdsaf')
INSERT [dbo].[NT_Other] ([ID], [Notification_Id], [Time], [Content], [Title]) VALUES (N'51898e77-1e74-4637-97d8-b2de9a1d189c', N'2c9c340e-6764-4833-abd7-b82064f3bd89', CAST(N'2021-10-21T09:42:01.833' AS DateTime), N'fadsfdsafdsaf', N'adsfdas')
INSERT [dbo].[NT_Other] ([ID], [Notification_Id], [Time], [Content], [Title]) VALUES (N'235aefe1-a831-4ad7-915f-64d0da21baa1', N'cd38b8b3-8db1-4b31-b5ee-5a66f2dab5cc', CAST(N'2021-09-23T15:15:50.920' AS DateTime), N'通知测试', N'其他通知')
INSERT [dbo].[NT_Release] ([ID], [Notification_Id], [Time], [Version_No], [Version_Name], [Content]) VALUES (N'87ecb060-4b1a-4d53-bb2b-27e22b60ecb9', N'ffcf362f-3f5b-4431-9b12-abdbb11c8c85', CAST(N'2021-09-26T02:25:06.047' AS DateTime), N'V1.0', N'应用框架版本1', N'Fix: 修复了XXXX bug,Feature: 完成了新功能YYYY')
INSERT [dbo].[NT_Release] ([ID], [Notification_Id], [Time], [Version_No], [Version_Name], [Content]) VALUES (N'65b86054-75c4-4a2b-9558-b717cfb19012', N'329f9369-e9cd-46cd-a806-aee11dd8921c', CAST(N'2021-09-26T02:25:06.047' AS DateTime), N'V2.0', N'应用框架版本2', N'Fix: 修复了登录 bug,Feature: 完成了实时同喜,Improvement: 改进了用户集成')
INSERT [dbo].[NT_Release] ([ID], [Notification_Id], [Time], [Version_No], [Version_Name], [Content]) VALUES (N'e622fa59-6b80-4e75-8899-c76bb0019c90', N'c096be34-d269-429a-a610-11337befe25a', CAST(N'2021-10-19T03:55:56.700' AS DateTime), N'1', N'查克拉', N'Fix: 放的地方个梵蒂冈')
INSERT [dbo].[NT_Release] ([ID], [Notification_Id], [Time], [Version_No], [Version_Name], [Content]) VALUES (N'8baa6476-1964-4ea8-ab3b-f37a2467505b', N'2d62a196-918b-48d9-8552-7c18da9ad440', CAST(N'2021-10-19T04:04:05.690' AS DateTime), N'v222', N'达伊的大冒险', N'Feature: 龙骑士')
INSERT [dbo].[NT_Release_Detail] ([ID], [Release_Id], [Notification_Id], [Type], [Content]) VALUES (N'08e29c4a-5547-4642-b49c-c6db4bd83168', N'87ecb060-4b1a-4d53-bb2b-27e22b60ecb9', N'ffcf362f-3f5b-4431-9b12-abdbb11c8c85', 1, N'完成了新功能YYYY')
INSERT [dbo].[NT_Release_Detail] ([ID], [Release_Id], [Notification_Id], [Type], [Content]) VALUES (N'36c76b2a-2944-49ba-9a65-21079e9e1b45', N'87ecb060-4b1a-4d53-bb2b-27e22b60ecb9', N'ffcf362f-3f5b-4431-9b12-abdbb11c8c85', 0, N'修复了XXXX bug')
INSERT [dbo].[NT_Release_Detail] ([ID], [Release_Id], [Notification_Id], [Type], [Content]) VALUES (N'57ca60ef-6edb-48ba-bc02-1ea9ee50cfe1', N'8baa6476-1964-4ea8-ab3b-f37a2467505b', N'2d62a196-918b-48d9-8552-7c18da9ad440', 1, N'龙骑士')
INSERT [dbo].[NT_Release_Detail] ([ID], [Release_Id], [Notification_Id], [Type], [Content]) VALUES (N'60b3492e-2f87-4cb6-b7ba-173326c221ea', N'e622fa59-6b80-4e75-8899-c76bb0019c90', N'c096be34-d269-429a-a610-11337befe25a', 0, N'放的地方个梵蒂冈')
INSERT [dbo].[NT_Release_Detail] ([ID], [Release_Id], [Notification_Id], [Type], [Content]) VALUES (N'812b4123-887c-42c7-b5fa-14b1da208f98', N'65b86054-75c4-4a2b-9558-b717cfb19012', N'329f9369-e9cd-46cd-a806-aee11dd8921c', 2, N'改进了用户集成')
INSERT [dbo].[NT_Release_Detail] ([ID], [Release_Id], [Notification_Id], [Type], [Content]) VALUES (N'8f2c7193-30dd-49da-887c-1ff353a397aa', N'65b86054-75c4-4a2b-9558-b717cfb19012', N'329f9369-e9cd-46cd-a806-aee11dd8921c', 0, N'修复了登录 bug')
INSERT [dbo].[NT_Release_Detail] ([ID], [Release_Id], [Notification_Id], [Type], [Content]) VALUES (N'c6f18c46-b4aa-4289-8383-dca7d60614a6', N'65b86054-75c4-4a2b-9558-b717cfb19012', N'329f9369-e9cd-46cd-a806-aee11dd8921c', 1, N'完成了实时同喜')
INSERT [dbo].[Role] ([ID], [Name], [No], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'24E2E3CD-E8CE-41D9-97F4-4766D94642AC', N'测试角色1', NULL, N'59A7016A-CB64-42C9-94A2-174A8AA51A54', NULL, 1618820988390, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634782935486, NULL, NULL, 0)
INSERT [dbo].[Role] ([ID], [Name], [No], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'4BB6F281-4EB1-4913-BA2F-B2DEC79022B8', N'测试角色3', NULL, N'59A7016A-CB64-42C9-94A2-174A8AA51A54', NULL, 1618820988390, N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631588068509, NULL, NULL, 0)
INSERT [dbo].[Role] ([ID], [Name], [No], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'c9cf30dc-c604-4e86-9d8f-6c64e2fb80a1', N'测试角色1XXXXX-1', NULL, N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1630040016736, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1630040028544, NULL, NULL, 1)
INSERT [dbo].[Role] ([ID], [Name], [No], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'18592587-55ac-4921-9835-c7b838016b32', N'管理员', NULL, N'fc898bc4-0f19-47a8-a692-83d1e56733ef', N'a7059ff2-6710-450f-8b23-80f52df2cb63', 1630053044879, N'a7059ff2-6710-450f-8b23-80f52df2cb63', 1630055709248, NULL, NULL, 0)
INSERT [dbo].[Role] ([ID], [Name], [No], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'567cd613-7258-48c4-983c-71e20cd4d5e1', N'用户管理', NULL, N'fc898bc4-0f19-47a8-a692-83d1e56733ef', N'a7059ff2-6710-450f-8b23-80f52df2cb63', 1630053084931, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[Role] ([ID], [Name], [No], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'10e530c5-eef4-40a3-89a3-9b53d045d90a', N'测试角色2', NULL, N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634782950331, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634782954476, NULL, NULL, 1)
INSERT [dbo].[Role] ([ID], [Name], [No], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'7da6a3e6-ebf5-44bb-989a-2dc0f3714ec3', N'角色管理', NULL, N'fc898bc4-0f19-47a8-a692-83d1e56733ef', N'a7059ff2-6710-450f-8b23-80f52df2cb63', 1630053097822, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[Role_Privileges] ([Role_Id], [Privilege_Code], [Code_Type], [Company_Id]) VALUES (N'24E2E3CD-E8CE-41D9-97F4-4766D94642AC', N'entity.yizit-account-sync', 1, N'59A7016A-CB64-42C9-94A2-174A8AA51A54')
INSERT [dbo].[Role_Privileges] ([Role_Id], [Privilege_Code], [Code_Type], [Company_Id]) VALUES (N'24E2E3CD-E8CE-41D9-97F4-4766D94642AC', N'menu.sys-conf.authority.role', 0, N'59A7016A-CB64-42C9-94A2-174A8AA51A54')
INSERT [dbo].[Role_Privileges] ([Role_Id], [Privilege_Code], [Code_Type], [Company_Id]) VALUES (N'4BB6F281-4EB1-4913-BA2F-B2DEC79022B8', N'K-01-01', 0, N'59A7016A-CB64-42C9-94A2-174A8AA51A54')
INSERT [dbo].[Role_Privileges] ([Role_Id], [Privilege_Code], [Code_Type], [Company_Id]) VALUES (N'4BB6F281-4EB1-4913-BA2F-B2DEC79022B8', N'00-01-01', 0, N'59A7016A-CB64-42C9-94A2-174A8AA51A54')
INSERT [dbo].[Role_Privileges] ([Role_Id], [Privilege_Code], [Code_Type], [Company_Id]) VALUES (N'4BB6F281-4EB1-4913-BA2F-B2DEC79022B8', N'00-02-01', 0, N'59A7016A-CB64-42C9-94A2-174A8AA51A54')
INSERT [dbo].[Role_Privileges] ([Role_Id], [Privilege_Code], [Code_Type], [Company_Id]) VALUES (N'4BB6F281-4EB1-4913-BA2F-B2DEC79022B8', N'00-04-02', 0, N'59A7016A-CB64-42C9-94A2-174A8AA51A54')
INSERT [dbo].[Role_Privileges] ([Role_Id], [Privilege_Code], [Code_Type], [Company_Id]) VALUES (N'4BB6F281-4EB1-4913-BA2F-B2DEC79022B8', N'00-02-02-04', 0, N'59A7016A-CB64-42C9-94A2-174A8AA51A54')
INSERT [dbo].[Role_Privileges] ([Role_Id], [Privilege_Code], [Code_Type], [Company_Id]) VALUES (N'4BB6F281-4EB1-4913-BA2F-B2DEC79022B8', N'menu.sys-conf.authority.user', 0, N'59A7016A-CB64-42C9-94A2-174A8AA51A54')
INSERT [dbo].[Role_Privileges] ([Role_Id], [Privilege_Code], [Code_Type], [Company_Id]) VALUES (N'c9cf30dc-c604-4e86-9d8f-6c64e2fb80a1', N'J-01-01', 0, N'59A7016A-CB64-42C9-94A2-174A8AA51A54')
INSERT [dbo].[Role_Privileges] ([Role_Id], [Privilege_Code], [Code_Type], [Company_Id]) VALUES (N'c9cf30dc-c604-4e86-9d8f-6c64e2fb80a1', N'00-02-04', 0, N'59A7016A-CB64-42C9-94A2-174A8AA51A54')
INSERT [dbo].[Role_Privileges] ([Role_Id], [Privilege_Code], [Code_Type], [Company_Id]) VALUES (N'c9cf30dc-c604-4e86-9d8f-6c64e2fb80a1', N'00-01-02', 0, N'59A7016A-CB64-42C9-94A2-174A8AA51A54')
INSERT [dbo].[Role_Privileges] ([Role_Id], [Privilege_Code], [Code_Type], [Company_Id]) VALUES (N'c9cf30dc-c604-4e86-9d8f-6c64e2fb80a1', N'K-01-04', 0, N'59A7016A-CB64-42C9-94A2-174A8AA51A54')
INSERT [dbo].[Role_Privileges] ([Role_Id], [Privilege_Code], [Code_Type], [Company_Id]) VALUES (N'c9cf30dc-c604-4e86-9d8f-6c64e2fb80a1', N'K-02-05', 0, N'59A7016A-CB64-42C9-94A2-174A8AA51A54')
INSERT [dbo].[Role_Privileges] ([Role_Id], [Privilege_Code], [Code_Type], [Company_Id]) VALUES (N'c9cf30dc-c604-4e86-9d8f-6c64e2fb80a1', N'00-02-02', 0, N'59A7016A-CB64-42C9-94A2-174A8AA51A54')
INSERT [dbo].[Role_Privileges] ([Role_Id], [Privilege_Code], [Code_Type], [Company_Id]) VALUES (N'c9cf30dc-c604-4e86-9d8f-6c64e2fb80a1', N'K-02-04', 0, N'59A7016A-CB64-42C9-94A2-174A8AA51A54')
INSERT [dbo].[Role_Privileges] ([Role_Id], [Privilege_Code], [Code_Type], [Company_Id]) VALUES (N'24E2E3CD-E8CE-41D9-97F4-4766D94642AC', N'menu.sys-conf.authority.user', 0, N'59A7016A-CB64-42C9-94A2-174A8AA51A54')
INSERT [dbo].[Role_Privileges] ([Role_Id], [Privilege_Code], [Code_Type], [Company_Id]) VALUES (N'c9cf30dc-c604-4e86-9d8f-6c64e2fb80a1', N'11-02-04', 1, N'59A7016A-CB64-42C9-94A2-174A8AA51A54')
INSERT [dbo].[Role_Privileges] ([Role_Id], [Privilege_Code], [Code_Type], [Company_Id]) VALUES (N'c9cf30dc-c604-4e86-9d8f-6c64e2fb80a1', N'11-01-02', 1, N'59A7016A-CB64-42C9-94A2-174A8AA51A54')
INSERT [dbo].[Role_Privileges] ([Role_Id], [Privilege_Code], [Code_Type], [Company_Id]) VALUES (N'c9cf30dc-c604-4e86-9d8f-6c64e2fb80a1', N'10-01-01', 1, N'59A7016A-CB64-42C9-94A2-174A8AA51A54')
INSERT [dbo].[Role_Privileges] ([Role_Id], [Privilege_Code], [Code_Type], [Company_Id]) VALUES (N'c9cf30dc-c604-4e86-9d8f-6c64e2fb80a1', N'10-01-04', 1, N'59A7016A-CB64-42C9-94A2-174A8AA51A54')
INSERT [dbo].[Role_Privileges] ([Role_Id], [Privilege_Code], [Code_Type], [Company_Id]) VALUES (N'24E2E3CD-E8CE-41D9-97F4-4766D94642AC', N'entity.domain-account-sync', 1, N'59A7016A-CB64-42C9-94A2-174A8AA51A54')
INSERT [dbo].[Role_Privileges] ([Role_Id], [Privilege_Code], [Code_Type], [Company_Id]) VALUES (N'18592587-55ac-4921-9835-c7b838016b32', N'menu.sys-conf.authority.user', 0, N'fc898bc4-0f19-47a8-a692-83d1e56733ef')
INSERT [dbo].[Role_Privileges] ([Role_Id], [Privilege_Code], [Code_Type], [Company_Id]) VALUES (N'567cd613-7258-48c4-983c-71e20cd4d5e1', N'menu.sys-conf.authority.user', 0, N'fc898bc4-0f19-47a8-a692-83d1e56733ef')
INSERT [dbo].[Role_Privileges] ([Role_Id], [Privilege_Code], [Code_Type], [Company_Id]) VALUES (N'7da6a3e6-ebf5-44bb-989a-2dc0f3714ec3', N'menu.sys-conf.authority.role', 0, N'fc898bc4-0f19-47a8-a692-83d1e56733ef')
INSERT [dbo].[Role_Privileges] ([Role_Id], [Privilege_Code], [Code_Type], [Company_Id]) VALUES (N'18592587-55ac-4921-9835-c7b838016b32', N'menu.sys-conf.authority.role', 0, N'fc898bc4-0f19-47a8-a692-83d1e56733ef')
INSERT [dbo].[Role_Privileges] ([Role_Id], [Privilege_Code], [Code_Type], [Company_Id]) VALUES (N'4BB6F281-4EB1-4913-BA2F-B2DEC79022B8', N'menu.sys-conf.authority.role', 0, N'59A7016A-CB64-42C9-94A2-174A8AA51A54')
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'7A572496-AFEB-43BC-A33B-B42C58BC4D43', N'9', NULL, NULL, NULL, N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', NULL, 1618820988390, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'D540447A-64C0-4C4F-8116-C8A34A821F74', N'admin', NULL, NULL, NULL, N'38AC9911-9406-4D71-9C90-17E3CBF48050', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', NULL, 1618820988390, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'7f6b15d1-1f1d-4055-b397-2bc15cc97312', N'jiadm', N'testjiadm', N'13732561943', N'jiadm@yizit.cn', N'27d2c853-d410-404a-b7a8-8db921c4ec79', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 0, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1630054965609, NULL, NULL, 1)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'bb25a1ce-0de4-43ec-8adc-994dd4224fc2', N'chenjl', N'xx', N'bbccc', N'xxx@yizit.cn', N'616df909-6eda-4ac6-99d0-359e3666923f', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 0, N'1000023', 1631946049942, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'78ecb719-8024-4d6a-bbec-f6155c9fe8b3', N'徐良', N'xl', N'18988887777', N'xuliang@yizit.cn', N'd3aaf2f7-2e54-4bb7-b019-e44a1674d3f1', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1629793962578, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1630043092719, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'7A572496-AFEB-43BC-A33B-B42C58BC4D45', N'9', NULL, NULL, NULL, N'32FBB7F4-3656-4990-B0D1-EF1680C90F18', N'69A7016A-CB64-42C9-94A2-174A8AA51A55', NULL, 1618820988390, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'e726de22-2fed-4733-9da9-e2ff33011796', N'沈', NULL, NULL, NULL, N'42724610-76ae-4674-8c8d-471d7c3235b1', N'226f0026-6fe2-4429-bc46-38ff8eae2c46', N'00000000-0000-0000-0000-000000000000', 1630046450437, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'6017d5ef-1856-4833-854e-cbd0343dbbe3', N'admin', NULL, NULL, NULL, N'5cc2687c-edf8-4e26-9598-1b2fe7fcc646', N'cd5a4b03-942e-4e59-95b9-e2dd4bf01796', N'00000000-0000-0000-0000-000000000000', 1630047766130, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'6cf4aeee-4d7d-41bb-98bd-682d865b7ca6', N'1', NULL, NULL, NULL, N'5587b248-0ca5-40f4-8e5b-e0fbba4a8a61', N'e892704f-ef88-4c6f-a066-5b12b06d7241', N'00000000-0000-0000-0000-000000000000', 1630047877608, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'feaeed32-225f-457e-a903-0396bf2bda18', N'0623', NULL, NULL, NULL, N'060e5b9f-69e3-4b4f-856d-50a6f56eca5f', N'30935dd2-438f-4eb7-a6b0-8f6256a9c8e6', N'00000000-0000-0000-0000-000000000000', 1630047932472, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'dfe6e49c-6495-4bd4-8ceb-1bb95bd04cd6', N'bb', NULL, NULL, NULL, N'a8d4111a-cd09-4d82-9765-fc3ea3659034', N'b3248e1a-cb4e-4362-939d-b7045afd8b65', N'00000000-0000-0000-0000-000000000000', 1630051299970, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'48514183-47ab-4110-b341-f036875be98f', N'老徐', NULL, N'18777775555', N'xuliang@yizit.cn', N'8e22695e-f69a-498d-a53f-0cff408e396b', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1630051592892, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1631696956041, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'eb37e3ac-4106-4ea3-9616-fa8e61d01f4f', N'laoxu', NULL, NULL, NULL, N'64d98fdc-1d6a-40a1-a77d-f1597e13111f', N'dee15935-682b-4ad0-8c5c-5a2f7c9aca37', N'00000000-0000-0000-0000-000000000000', 1630052465239, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'c7aa63e0-f39c-4c61-868a-6759e4f1be1e', N'laoxu', NULL, NULL, NULL, N'a7059ff2-6710-450f-8b23-80f52df2cb63', N'fc898bc4-0f19-47a8-a692-83d1e56733ef', N'00000000-0000-0000-0000-000000000000', 1630052515975, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'69e4f2cd-3fec-4579-a31a-8cc8884819ac', N'老徐', N'00001', N'18768144102', NULL, N'10539492-cad8-47c4-8810-68971beaf208', N'fc898bc4-0f19-47a8-a692-83d1e56733ef', N'a7059ff2-6710-450f-8b23-80f52df2cb63', 1630052947165, N'a7059ff2-6710-450f-8b23-80f52df2cb63', 1631697051008, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'90440983-181a-4ba2-b98b-68723e5f7826', N'老贾1', N'00002', NULL, NULL, N'97744a5f-58e7-4a0a-b60f-4adfbf9cbb1f', N'fc898bc4-0f19-47a8-a692-83d1e56733ef', N'a7059ff2-6710-450f-8b23-80f52df2cb63', 1630053154723, N'a7059ff2-6710-450f-8b23-80f52df2cb63', 1630054043333, NULL, NULL, 1)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'e8187e2b-c2c7-43b6-a0f0-20078a8cc1eb', N'钱宝宝1', N'00003', NULL, NULL, N'844beb89-1f8a-45a6-875e-3de1ea09694f', N'fc898bc4-0f19-47a8-a692-83d1e56733ef', N'a7059ff2-6710-450f-8b23-80f52df2cb63', 1630053184739, N'a7059ff2-6710-450f-8b23-80f52df2cb63', 1630054186265, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'55a4fa29-d4fe-4053-9da2-11a23bbcfd1a', N'老贾1', N'00002', NULL, NULL, N'ebcca99f-e1ca-409d-ba1e-504028fd3775', N'fc898bc4-0f19-47a8-a692-83d1e56733ef', N'a7059ff2-6710-450f-8b23-80f52df2cb63', 1630053988837, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'b95427c4-79fc-4fe7-b043-b4821de8f75d', N'系统管理员', NULL, NULL, N'', N'1000000', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631677969440, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783503932, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'96aa9115-2508-4b6c-9948-0f1c39bc235d', N'系统管理员', NULL, NULL, N'', N'1000000', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'1000023', 1634722425230, N'1000023', 1634778357075, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'350c3b2f-ae7c-41ad-83df-6a9872155fe7', N'叶雨辰', NULL, NULL, N'yeyc@yizit.cn', N'1000046', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'1000023', 1634779221151, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783505541, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'dd219121-e41a-4146-872f-bf6ef35c2a6a', N'诸利锋', N'111', N'', N'zhulf@yizit.cn', N'1000001', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631677976317, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783503994, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'afaed6d7-5148-4445-ad68-7fcf4638d5c1', N'刘倩云', NULL, NULL, N'liuqy@yizit.cn', N'1000004', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678229755, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504073, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'1db244aa-8a2f-4636-a4ae-d27cebd881ec', N'沈忠杰', NULL, NULL, N'shenzj@yizit.cn', N'1000005', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678229852, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504127, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'8566c161-7692-43d1-a26f-2a37f118ef65', N'沈冲', NULL, NULL, N'shenchong@yizit.cn', N'1000006', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678229928, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504184, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'948c8f30-a272-4b9d-86ae-e916f13d0d5b', N'叶宗剑', NULL, NULL, N'yezj@yizit.cn', N'1000007', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678230009, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504250, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'18d85c5b-e826-4e1f-8e67-08c8a4653b97', N'顾斌宏', NULL, NULL, N'gubh@yizit.cn', N'1000010', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678230093, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504309, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'24a2227f-a0b9-4529-b2ef-78398a85ba4e', N'丁朋朋', NULL, NULL, N'dingpp@yizit.cn', N'1000011', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678230176, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504363, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'78da5062-0677-47b3-86d0-2fcc3614b436', N'董文洁', NULL, NULL, N'dongwj@yizit.cn', N'1000012', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678230265, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504413, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'd3f1735d-49b1-41b9-8d76-ae85912acbb9', N'黄卢文', NULL, NULL, N'huanglw@yizit.cn', N'1000013', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678230361, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504468, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'b1a1358f-131c-4352-9f0c-dc9b332f2027', N'凌恺彬', NULL, NULL, N'lingkb@yizit.cn', N'1000015', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678230509, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504519, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'eac02346-2db9-4853-9c59-125ab224fb55', N'刘鲁宁', NULL, NULL, N'liuln@yizit.cn', N'1000016', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678230647, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504564, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'a1e74db6-ddfc-44cd-b73d-a337e17df88e', N'沈敏强', NULL, NULL, N'shenmq@yizit.cn', N'1000017', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678230737, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504608, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'f07f2ecf-30c9-4c78-9c22-6905048fa5fa', N'卢明', NULL, NULL, N'luming@yizit.cn', N'1000018', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678230816, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504657, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'1df58c4d-942d-4cf9-9997-2144ad95eb29', N'陶钧', NULL, NULL, N'taojun@yizit.cn', N'1000019', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678230895, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504706, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'5c041639-17b0-4fbe-bf46-d8943817ba61', N'吴晨', NULL, NULL, N'wuchen@yizit.cn', N'1000020', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678230968, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504749, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'caa8b6ff-0f0b-4d2f-9439-6f0efb06d44a', N'徐良', NULL, NULL, N'xuliang@yizit.cn', N'1000021', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678231045, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504809, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'd330c272-dbd1-44df-9f33-19e6960b9e80', N'王郁', NULL, NULL, N'wangyu@yizit.cn', N'1000022', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678231119, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504855, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'a2a9de77-e31c-4788-bf25-24df550dfc60', N'贾东明', NULL, NULL, N'jiadm@yizit.cn', N'1000023', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678231196, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504913, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'def5359e-e909-43ae-86d8-1585139895f5', N'圣小燕', NULL, NULL, N'shengxy@yizit.cn', N'1000024', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678231286, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504953, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'e5bd56f1-6404-4fdf-8cfd-f7c12cca5fb7', N'陈洁', NULL, NULL, N'chenjie@yizit.cn', N'1000025', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678231363, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504990, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'cf426311-f861-44dd-ab76-23d5a192c791', N'王灏', NULL, NULL, N'wanghao@yizit.cn', N'1000026', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678231442, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783505026, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'2d76a3da-a91a-42b5-8390-65e0e7dfc8e4', N'苏宗浩', NULL, NULL, N'suzh@yizit.cn', N'1000028', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678231521, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783505067, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'26572b17-8e9e-4020-8286-c23a626b20b9', N'钱锦', NULL, NULL, N'qianjin@yizit.cn', N'1000029', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678231606, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783505109, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'a6eee6a4-f76e-4702-8be6-b6c059df7ca5', N'居向军', NULL, NULL, N'juxj@yizit.cn', N'1000030', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678231689, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783505151, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'8237e49b-39a2-4ded-aebb-a29102ec4fcf', N'纪玲', NULL, NULL, N'jiling@yizit.cn', N'1000031', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678231833, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783505201, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'ccbe78c4-afb8-4eaa-ae6e-cad4a92c227f', N'洪金波', NULL, NULL, N'hongjb@yizit.cn', N'1000032', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678231929, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783505241, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'656f194f-20b6-4f21-a319-4f37c87412b7', N'纪波', NULL, NULL, N'jibo@yizit.cn', N'1000033', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678232015, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783505269, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'efe6224d-16ee-4cac-ae64-e123709c9a87', N'tju', NULL, NULL, N'tju@yizit.cn', N'1000034', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678232095, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783505295, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'1d44e39a-181b-46d4-af24-6203a40d7230', N'盛仲缤', NULL, NULL, N'shengzb@yizit.cn', N'1000035', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678232170, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783505322, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'0bf62f16-f7a8-4763-a447-025a28d68e9b', N'jenkins', NULL, NULL, N'jenkins@yizit.cn', N'1000036', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678232251, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783505349, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'22c42713-771b-450b-ab39-fe5dedc08a14', N'张文博', NULL, NULL, N'zhangwb@yizit.cn', N'1000037', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678232335, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783505377, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'6c127df0-c286-4500-a7e0-44752dd3feeb', N'周润', NULL, N'111', N'zhourun@yizit.cn', N'1000039', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678232416, N'1000023', 1631947728533, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'fc68b890-7e07-41f1-9e19-d592869563cd', N'江虹艽', NULL, NULL, N'jianghj@yizit.cn', N'1000040', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678232494, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783505405, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'e508832b-82f4-40b6-9426-8b5d916c6f5d', N'陈良玉', NULL, NULL, N'chenly@yizit.cn', N'1000041', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678232573, N'1000023', 1631946030567, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'24d4a704-5924-4fa4-be48-c9d06463d6c1', N'王逸君', NULL, NULL, N'wangyj@yizit.cn', N'1000042', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678232653, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783505433, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'29ec05dd-3f08-41a6-a0e1-a5a8fc811684', N'黄钦烨', NULL, NULL, N'huangqy@yizit.cn', N'1000043', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678232738, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783505463, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'b7990331-5c06-48af-9ce4-0173e76809f1', N'熊伟', NULL, NULL, N'xiongwei@yizit.cn', N'1000044', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678232823, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783505489, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'0ac177fb-9e83-4e6d-9680-3d00f6e5cec8', N'贾东明', N'', N'13732561943', N'3335683442@qq.com', N'bb186ced-30fa-4a32-ad5b-afdb7321abc9', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1631948356116, N'1000023', 1631949950352, NULL, NULL, 0)
INSERT [dbo].[Staff] ([ID], [Name], [No], [Phone], [Email], [User_Id], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'5d9f561e-aa65-4c7b-accb-c29864484449', N'张悦', NULL, N'555222', N'zhangyue@yizit.cn', N'1000045', N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'd3aaf2f7-2e54-4bb7-b019-e44a1674d3f1', 1631782961748, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783505515, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'029fa040-01d4-40fe-87f6-cffda1dce5a9', N'jiadm', 0, 1, N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1629711903133, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1629787184491, NULL, NULL, 1)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'060e5b9f-69e3-4b4f-856d-50a6f56eca5f', N'0623', 1, 1, N'30935dd2-438f-4eb7-a6b0-8f6256a9c8e6', N'00000000-0000-0000-0000-000000000000', 1630047932459, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'1000000', N'admin', 0, 1, N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'1000023', 1634722424687, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783503932, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'1000001', N'zhulf', 0, 0, N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631677976295, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783503994, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'1000004', N'liuqy', 0, 1, N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678229716, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504073, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'1000005', N'shenzj', 0, 1, N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678229834, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504127, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'1000006', N'shenchong', 0, 1, N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678229912, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504184, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'1000007', N'yezj', 0, 1, N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678229993, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504250, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'1000010', N'gubh', 0, 1, N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678230074, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504309, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'1000011', N'dingpp', 0, 1, N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678230159, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504363, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'1000012', N'dongwj', 0, 1, N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678230250, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504413, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'1000013', N'huanglw', 0, 1, N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678230343, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504468, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'1000015', N'lingkb', 0, 1, N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678230495, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504519, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'1000016', N'liuln', 0, 1, N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678230627, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504564, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'1000017', N'shenmq', 0, 1, N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678230723, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504608, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'1000018', N'luming', 0, 1, N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678230800, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504657, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'1000019', N'taojun', 0, 0, N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678230879, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504706, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'1000020', N'wuchen', 0, 0, N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678230954, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504749, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'1000021', N'xuliang', 0, 1, N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678231031, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504809, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'1000022', N'wangyu', 0, 1, N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678231103, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504855, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'1000023', N'jiadm', 0, 1, N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678231182, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504913, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'1000024', N'shengxy', 0, 1, N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678231261, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504953, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'1000025', N'chenjie', 0, 1, N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678231348, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783504990, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'1000026', N'wanghao', 0, 1, N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678231426, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783505026, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'1000028', N'suzh', 0, 1, N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678231505, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783505067, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'1000029', N'qianjin', 0, 0, N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678231586, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783505109, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'1000030', N'juxj', 0, 1, N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678231670, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783505151, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'1000031', N'jiling', 0, 1, N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678231785, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783505201, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'1000032', N'hongjb', 0, 1, N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678231899, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783505241, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'1000033', N'jibo', 0, 1, N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678232001, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783505269, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'1000034', N'tju', 0, 1, N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678232081, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783505295, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'1000035', N'shengzb', 0, 1, N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678232156, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783505322, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'1000036', N'jenkins', 0, 1, N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678232233, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783505349, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'1000037', N'zhangwb', 0, 0, N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678232319, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783505377, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'1000039', N'zhourun', 0, 1, N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678232400, N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1632302338057, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'1000040', N'jianghj', 0, 1, N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678232478, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783505405, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'1000041', N'chenly', 0, 1, N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678232558, N'1000023', 1631946030567, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'1000042', N'wangyj', 0, 1, N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678232638, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783505433, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'1000043', N'huangqy', 0, 1, N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678232720, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783505463, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'1000044', N'xiongwei', 0, 1, N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1631678232803, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783505489, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'1000045', N'zhangyue', 0, 1, N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'd3aaf2f7-2e54-4bb7-b019-e44a1674d3f1', 1631782960611, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783505515, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'1000046', N'yeyc', 0, 1, N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'1000023', 1634779221075, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1634783505541, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'10539492-cad8-47c4-8810-68971beaf208', N'xuliang', 0, 1, N'fc898bc4-0f19-47a8-a692-83d1e56733ef', N'a7059ff2-6710-450f-8b23-80f52df2cb63', 1630052947157, N'a7059ff2-6710-450f-8b23-80f52df2cb63', 1631697051008, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'27d2c853-d410-404a-b7a8-8db921c4ec79', N'testModify11', 0, 0, N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 0, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1630054965609, NULL, NULL, 1)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', N'9', 0, 1, N'59A7016A-CB64-42C9-94A2-174A8AA51A54', NULL, 1618820988390, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'32FBB7F4-3656-4990-B0D1-EF1680C90F18', N'9', 0, 1, N'69A7016A-CB64-42C9-94A2-174A8AA51A55', NULL, 1618820988390, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'38AC9911-9406-4D71-9C90-17E3CBF48050', N'admin', 1, 1, N'59A7016A-CB64-42C9-94A2-174A8AA51A54', NULL, 0, N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1629782474781, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'42724610-76ae-4674-8c8d-471d7c3235b1', N'沈', 1, 1, N'226f0026-6fe2-4429-bc46-38ff8eae2c46', N'00000000-0000-0000-0000-000000000000', 1630046449390, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'5587b248-0ca5-40f4-8e5b-e0fbba4a8a61', N'1', 1, 1, N'e892704f-ef88-4c6f-a066-5b12b06d7241', N'00000000-0000-0000-0000-000000000000', 1630047877598, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'5cc2687c-edf8-4e26-9598-1b2fe7fcc646', N'admin', 1, 1, N'cd5a4b03-942e-4e59-95b9-e2dd4bf01796', N'00000000-0000-0000-0000-000000000000', 1630047766108, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'616df909-6eda-4ac6-99d0-359e3666923f', N'testCJL', 0, 0, N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 0, N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1632302220633, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'64d98fdc-1d6a-40a1-a77d-f1597e13111f', N'laoxu', 1, 1, N'dee15935-682b-4ad0-8c5c-5a2f7c9aca37', N'00000000-0000-0000-0000-000000000000', 1630052465233, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'844beb89-1f8a-45a6-875e-3de1ea09694f', N'liuqy', 0, 1, N'fc898bc4-0f19-47a8-a692-83d1e56733ef', N'a7059ff2-6710-450f-8b23-80f52df2cb63', 1630053184734, N'a7059ff2-6710-450f-8b23-80f52df2cb63', 1631586057352, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'8e22695e-f69a-498d-a53f-0cff408e396b', N'xuliang', 0, 1, N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1630051592881, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1631696956020, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'97744a5f-58e7-4a0a-b60f-4adfbf9cbb1f', N'jiadm', 0, 1, N'fc898bc4-0f19-47a8-a692-83d1e56733ef', N'a7059ff2-6710-450f-8b23-80f52df2cb63', 1630053154716, N'a7059ff2-6710-450f-8b23-80f52df2cb63', 1630054043333, NULL, NULL, 1)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'a7059ff2-6710-450f-8b23-80f52df2cb63', N'laoxu', 1, 1, N'fc898bc4-0f19-47a8-a692-83d1e56733ef', N'00000000-0000-0000-0000-000000000000', 1630052515967, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'a8d4111a-cd09-4d82-9765-fc3ea3659034', N'bb', 1, 1, N'b3248e1a-cb4e-4362-939d-b7045afd8b65', N'00000000-0000-0000-0000-000000000000', 1630051299898, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'bb186ced-30fa-4a32-ad5b-afdb7321abc9', N'贾东明', 0, 1, N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1631948355851, N'1000023', 1631949950352, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'd3aaf2f7-2e54-4bb7-b019-e44a1674d3f1', N'xuliang', 0, 0, N'59A7016A-CB64-42C9-94A2-174A8AA51A54', N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', 1629793961139, N'38AC9911-9406-4D71-9C90-17E3CBF48050', 1631936863543, NULL, NULL, 0)
INSERT [dbo].[User] ([ID], [Name], [User_Type], [Status], [Company_Id], [creator], [creation_time], [modifier], [modification_time], [deleted_by], [deleted_time], [deleted]) VALUES (N'ebcca99f-e1ca-409d-ba1e-504028fd3775', N'jiadm1', 0, 1, N'fc898bc4-0f19-47a8-a692-83d1e56733ef', N'a7059ff2-6710-450f-8b23-80f52df2cb63', 1630053988831, NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[User_Preference_Notification] ([User_Id], [Notification_Id], [creation_time], [deleted_time], [deleted]) VALUES (N'38AC9911-9406-4D71-9C90-17E3CBF48050', N'3840ea9d-f7ad-4ee9-85e0-2e36925190cf', 1632449819346, NULL, 0)
INSERT [dbo].[User_Preference_Notification] ([User_Id], [Notification_Id], [creation_time], [deleted_time], [deleted]) VALUES (N'38AC9911-9406-4D71-9C90-17E3CBF48050', N'a6a118e8-8ca3-497b-aac9-a81f34c0f303', 1632449819353, NULL, 0)
INSERT [dbo].[User_Preference_Notification] ([User_Id], [Notification_Id], [creation_time], [deleted_time], [deleted]) VALUES (N'38AC9911-9406-4D71-9C90-17E3CBF48050', N'cd38b8b3-8db1-4b31-b5ee-5a66f2dab5cc', 1632466244937, NULL, 0)
INSERT [dbo].[UserRoles] ([User_Id], [Role_Id], [Company_Id]) VALUES (N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', N'24E2E3CD-E8CE-41D9-97F4-4766D94642AC', N'59A7016A-CB64-42C9-94A2-174A8AA51A54')
INSERT [dbo].[UserRoles] ([User_Id], [Role_Id], [Company_Id]) VALUES (N'32FBB7F4-3656-4990-B0D1-EF1680C90F17', N'4BB6F281-4EB1-4913-BA2F-B2DEC79022B8', N'59A7016A-CB64-42C9-94A2-174A8AA51A54')
INSERT [dbo].[UserRoles] ([User_Id], [Role_Id], [Company_Id]) VALUES (N'38AC9911-9406-4D71-9C90-17E3CBF48050', N'24E2E3CD-E8CE-41D9-97F4-4766D94642AC', N'59A7016A-CB64-42C9-94A2-174A8AA51A54')
INSERT [dbo].[UserRoles] ([User_Id], [Role_Id], [Company_Id]) VALUES (N'616df909-6eda-4ac6-99d0-359e3666923f', N'24E2E3CD-E8CE-41D9-97F4-4766D94642AC', N'59A7016A-CB64-42C9-94A2-174A8AA51A54')
INSERT [dbo].[UserRoles] ([User_Id], [Role_Id], [Company_Id]) VALUES (N'1000023', N'24E2E3CD-E8CE-41D9-97F4-4766D94642AC', N'59A7016A-CB64-42C9-94A2-174A8AA51A54')
INSERT [dbo].[UserRoles] ([User_Id], [Role_Id], [Company_Id]) VALUES (N'd3aaf2f7-2e54-4bb7-b019-e44a1674d3f1', N'24E2E3CD-E8CE-41D9-97F4-4766D94642AC', N'59A7016A-CB64-42C9-94A2-174A8AA51A54')
INSERT [dbo].[UserRoles] ([User_Id], [Role_Id], [Company_Id]) VALUES (N'27d2c853-d410-404a-b7a8-8db921c4ec79', N'24E2E3CD-E8CE-41D9-97F4-4766D94642AC', N'59A7016A-CB64-42C9-94A2-174A8AA51A54')
INSERT [dbo].[UserRoles] ([User_Id], [Role_Id], [Company_Id]) VALUES (N'27d2c853-d410-404a-b7a8-8db921c4ec79', N'4BB6F281-4EB1-4913-BA2F-B2DEC79022B8', N'59A7016A-CB64-42C9-94A2-174A8AA51A54')
INSERT [dbo].[UserRoles] ([User_Id], [Role_Id], [Company_Id]) VALUES (N'616df909-6eda-4ac6-99d0-359e3666923f', N'4BB6F281-4EB1-4913-BA2F-B2DEC79022B8', N'59A7016A-CB64-42C9-94A2-174A8AA51A54')
INSERT [dbo].[UserRoles] ([User_Id], [Role_Id], [Company_Id]) VALUES (N'3333', N'111', N'59A7016A-CB64-42C9-94A2-174A8AA51A54')
INSERT [dbo].[UserRoles] ([User_Id], [Role_Id], [Company_Id]) VALUES (N'3333', N'222', N'59A7016A-CB64-42C9-94A2-174A8AA51A54')
INSERT [dbo].[UserRoles] ([User_Id], [Role_Id], [Company_Id]) VALUES (N'3333', N'24E2E3CD-E8CE-41D9-97F4-4766D94642AC', N'59A7016A-CB64-42C9-94A2-174A8AA51A54')
INSERT [dbo].[UserRoles] ([User_Id], [Role_Id], [Company_Id]) VALUES (N'10539492-cad8-47c4-8810-68971beaf208', N'7da6a3e6-ebf5-44bb-989a-2dc0f3714ec3', N'fc898bc4-0f19-47a8-a692-83d1e56733ef')
INSERT [dbo].[UserRoles] ([User_Id], [Role_Id], [Company_Id]) VALUES (N'97744a5f-58e7-4a0a-b60f-4adfbf9cbb1f', N'567cd613-7258-48c4-983c-71e20cd4d5e1', N'fc898bc4-0f19-47a8-a692-83d1e56733ef')
INSERT [dbo].[UserRoles] ([User_Id], [Role_Id], [Company_Id]) VALUES (N'844beb89-1f8a-45a6-875e-3de1ea09694f', N'18592587-55ac-4921-9835-c7b838016b32', N'fc898bc4-0f19-47a8-a692-83d1e56733ef')
INSERT [dbo].[UserRoles] ([User_Id], [Role_Id], [Company_Id]) VALUES (N'ebcca99f-e1ca-409d-ba1e-504028fd3775', N'18592587-55ac-4921-9835-c7b838016b32', N'fc898bc4-0f19-47a8-a692-83d1e56733ef')
INSERT [dbo].[UserRoles] ([User_Id], [Role_Id], [Company_Id]) VALUES (N'8e22695e-f69a-498d-a53f-0cff408e396b', N'4BB6F281-4EB1-4913-BA2F-B2DEC79022B8', N'59A7016A-CB64-42C9-94A2-174A8AA51A54')
INSERT [dbo].[UserRoles] ([User_Id], [Role_Id], [Company_Id]) VALUES (N'8e22695e-f69a-498d-a53f-0cff408e396b', N'24E2E3CD-E8CE-41D9-97F4-4766D94642AC', N'59A7016A-CB64-42C9-94A2-174A8AA51A54')
SET ANSI_PADDING ON
GO
/****** Object:  Index [PK_ACCOUNT]    Script Date: 2021/10/22 9:53:15 ******/
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [PK_ACCOUNT] PRIMARY KEY NONCLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [PK_COMPANY]    Script Date: 2021/10/22 9:53:15 ******/
ALTER TABLE [dbo].[Company] ADD  CONSTRAINT [PK_COMPANY] PRIMARY KEY NONCLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [PK_COMPANY_APPLICABLE_PRIVILEG]    Script Date: 2021/10/22 9:53:15 ******/
ALTER TABLE [dbo].[Company_Applicable_Privileges] ADD  CONSTRAINT [PK_COMPANY_APPLICABLE_PRIVILEG] PRIMARY KEY NONCLUSTERED 
(
	[Enterprise_Id] ASC,
	[Privilege_Code] ASC,
	[Code_Type] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [PK_NOTFICATIONSCOPE]    Script Date: 2021/10/22 9:53:15 ******/
ALTER TABLE [dbo].[NotficationScope] ADD  CONSTRAINT [PK_NOTFICATIONSCOPE] PRIMARY KEY NONCLUSTERED 
(
	[Notification_Id] ASC,
	[Scope_Type] ASC,
	[Scope_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [PK_NOTIFICATION_BASE]    Script Date: 2021/10/22 9:53:15 ******/
ALTER TABLE [dbo].[Notification_Base] ADD  CONSTRAINT [PK_NOTIFICATION_BASE] PRIMARY KEY NONCLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [PK_NT_MAINTENANCE]    Script Date: 2021/10/22 9:53:15 ******/
ALTER TABLE [dbo].[NT_Maintenance] ADD  CONSTRAINT [PK_NT_MAINTENANCE] PRIMARY KEY NONCLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [PK_NT_OTHER]    Script Date: 2021/10/22 9:53:15 ******/
ALTER TABLE [dbo].[NT_Other] ADD  CONSTRAINT [PK_NT_OTHER] PRIMARY KEY NONCLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [PK_NT_RELEASE]    Script Date: 2021/10/22 9:53:15 ******/
ALTER TABLE [dbo].[NT_Release] ADD  CONSTRAINT [PK_NT_RELEASE] PRIMARY KEY NONCLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [PK_ROLE]    Script Date: 2021/10/22 9:53:15 ******/
ALTER TABLE [dbo].[Role] ADD  CONSTRAINT [PK_ROLE] PRIMARY KEY NONCLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [PK_ROLE_PRIVILEGES]    Script Date: 2021/10/22 9:53:15 ******/
ALTER TABLE [dbo].[Role_Privileges] ADD  CONSTRAINT [PK_ROLE_PRIVILEGES] PRIMARY KEY NONCLUSTERED 
(
	[Role_Id] ASC,
	[Privilege_Code] ASC,
	[Code_Type] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [PK_STAFF]    Script Date: 2021/10/22 9:53:15 ******/
ALTER TABLE [dbo].[Staff] ADD  CONSTRAINT [PK_STAFF] PRIMARY KEY NONCLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [PK_USERROLES]    Script Date: 2021/10/22 9:53:15 ******/
ALTER TABLE [dbo].[UserRoles] ADD  CONSTRAINT [PK_USERROLES] PRIMARY KEY NONCLUSTERED 
(
	[User_Id] ASC,
	[Role_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Company_Applicable_Privileges] ADD  CONSTRAINT [DF_Company_Applicable_Privileges_Code_Type]  DEFAULT ((0)) FOR [Code_Type]
GO
ALTER TABLE [dbo].[Role_Privileges] ADD  CONSTRAINT [DF_Role_Privileges_Code_Type]  DEFAULT ((0)) FOR [Code_Type]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'账户唯一ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Account', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登录账号（同企业内不允许重复）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Account', @level2type=N'COLUMN',@level2name=N'LoginName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'密码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Account', @level2type=N'COLUMN',@level2name=N'Password'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0 本地用户；1： LDAP 用户；2 微信用户；3 钉钉用户；-1 因致账户' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Account', @level2type=N'COLUMN',@level2name=N'Account_Type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Account', @level2type=N'COLUMN',@level2name=N'User_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'企业id,多租户考虑' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Account', @level2type=N'COLUMN',@level2name=N'Company_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Account', @level2type=N'COLUMN',@level2name=N'creator'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Account', @level2type=N'COLUMN',@level2name=N'creation_time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Account', @level2type=N'COLUMN',@level2name=N'modifier'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Account', @level2type=N'COLUMN',@level2name=N'modification_time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Account', @level2type=N'COLUMN',@level2name=N'deleted_by'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Account', @level2type=N'COLUMN',@level2name=N'deleted_time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除标记' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Account', @level2type=N'COLUMN',@level2name=N'deleted'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'账户信息表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Account'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'企业唯一编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'企业名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'企业编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company', @level2type=N'COLUMN',@level2name=N'No'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'企业描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company', @level2type=N'COLUMN',@level2name=N'Desc'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'时区id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company', @level2type=N'COLUMN',@level2name=N'TimeZone_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'企业管理员名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company', @level2type=N'COLUMN',@level2name=N'Manager'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'企业Schema（多租户考虑-共享database，独立schema时）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company', @level2type=N'COLUMN',@level2name=N'Company_Schema'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'企业连接库信息（多租户考虑-独立database时）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company', @level2type=N'COLUMN',@level2name=N'Company_DbConnection'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company', @level2type=N'COLUMN',@level2name=N'creator'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company', @level2type=N'COLUMN',@level2name=N'creation_time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company', @level2type=N'COLUMN',@level2name=N'modifier'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company', @level2type=N'COLUMN',@level2name=N'modification_time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company', @level2type=N'COLUMN',@level2name=N'deleted_by'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company', @level2type=N'COLUMN',@level2name=N'deleted_time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除标记' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company', @level2type=N'COLUMN',@level2name=N'deleted'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'企业信息表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'企业id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company_Applicable_Privileges', @level2type=N'COLUMN',@level2name=N'Enterprise_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'权限唯一Code' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company_Applicable_Privileges', @level2type=N'COLUMN',@level2name=N'Privilege_Code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'code所属，0：menus；1：entities' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company_Applicable_Privileges', @level2type=N'COLUMN',@level2name=N'Code_Type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'权限描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company_Applicable_Privileges', @level2type=N'COLUMN',@level2name=N'Desc'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'企业可用权限列表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company_Applicable_Privileges'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'通知id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'NotficationScope', @level2type=N'COLUMN',@level2name=N'Notification_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'通过范围类型 （0： 企业； 1： 角色； 2： 用户）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'NotficationScope', @level2type=N'COLUMN',@level2name=N'Scope_Type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'根据类型，分别是企业id，角色id，用户id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'NotficationScope', @level2type=N'COLUMN',@level2name=N'Scope_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消息通知范围' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'NotficationScope'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色唯一编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Notification_Base', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发布方式 0 立即发布 1 定时发布' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Notification_Base', @level2type=N'COLUMN',@level2name=N'PublishType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发布时间 立即发布是当前创建时间； 定时发布时，是指定的时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Notification_Base', @level2type=N'COLUMN',@level2name=N'PublishTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系统消息类型 （系统默认支持 0 其他；1 发布；2 维护）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Notification_Base', @level2type=N'COLUMN',@level2name=N'NotficationType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'-1 初始（未发送）； 0 已发送； 1： 撤回' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Notification_Base', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Notification_Base', @level2type=N'COLUMN',@level2name=N'creator'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Notification_Base', @level2type=N'COLUMN',@level2name=N'creation_time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Notification_Base', @level2type=N'COLUMN',@level2name=N'modifier'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Notification_Base', @level2type=N'COLUMN',@level2name=N'modification_time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Notification_Base', @level2type=N'COLUMN',@level2name=N'deleted_by'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Notification_Base', @level2type=N'COLUMN',@level2name=N'deleted_time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除标记' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Notification_Base', @level2type=N'COLUMN',@level2name=N'deleted'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消息通知基本信息表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Notification_Base'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'版本唯一id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'NT_Maintenance', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属通知id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'NT_Maintenance', @level2type=N'COLUMN',@level2name=N'Notification_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'维护时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'NT_Maintenance', @level2type=N'COLUMN',@level2name=N'Time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'维护内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'NT_Maintenance', @level2type=N'COLUMN',@level2name=N'Content'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标题' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'NT_Maintenance', @level2type=N'COLUMN',@level2name=N'Title'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'通知内容-维护' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'NT_Maintenance'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'版本唯一id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'NT_Other', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属通知id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'NT_Other', @level2type=N'COLUMN',@level2name=N'Notification_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'NT_Other', @level2type=N'COLUMN',@level2name=N'Time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'NT_Other', @level2type=N'COLUMN',@level2name=N'Content'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标题' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'NT_Other', @level2type=N'COLUMN',@level2name=N'Title'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'通知内容-其他' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'NT_Other'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'版本唯一id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'NT_Release', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属通知id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'NT_Release', @level2type=N'COLUMN',@level2name=N'Notification_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'NT_Release', @level2type=N'COLUMN',@level2name=N'Time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'版本号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'NT_Release', @level2type=N'COLUMN',@level2name=N'Version_No'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'版本名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'NT_Release', @level2type=N'COLUMN',@level2name=N'Version_Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'NT_Release', @level2type=N'COLUMN',@level2name=N'Content'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'通知内容-版本更新' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'NT_Release'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'版本唯一id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'NT_Release_Detail', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属版本更新id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'NT_Release_Detail', @level2type=N'COLUMN',@level2name=N'Release_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属通知id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'NT_Release_Detail', @level2type=N'COLUMN',@level2name=N'Notification_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新类型 （0： fix; 1: feature；2: improvement; 3: design ; 4:doc）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'NT_Release_Detail', @level2type=N'COLUMN',@level2name=N'Type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'NT_Release_Detail', @level2type=N'COLUMN',@level2name=N'Content'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'版本更新内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'NT_Release_Detail'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色唯一编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Role', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Role', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Role', @level2type=N'COLUMN',@level2name=N'No'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'企业id,多租户考虑' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Role', @level2type=N'COLUMN',@level2name=N'Company_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Role', @level2type=N'COLUMN',@level2name=N'creator'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Role', @level2type=N'COLUMN',@level2name=N'creation_time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Role', @level2type=N'COLUMN',@level2name=N'modifier'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Role', @level2type=N'COLUMN',@level2name=N'modification_time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Role', @level2type=N'COLUMN',@level2name=N'deleted_by'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Role', @level2type=N'COLUMN',@level2name=N'deleted_time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除标记' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Role', @level2type=N'COLUMN',@level2name=N'deleted'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色信息表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Role'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Role_Privileges', @level2type=N'COLUMN',@level2name=N'Role_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'权限唯一Code' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Role_Privileges', @level2type=N'COLUMN',@level2name=N'Privilege_Code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'code所属，0：menus；1：entities' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Role_Privileges', @level2type=N'COLUMN',@level2name=N'Code_Type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属企业id,多租户考虑' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Role_Privileges', @level2type=N'COLUMN',@level2name=N'Company_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色权限表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Role_Privileges'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'员工唯一ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Staff', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Staff', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'员工编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Staff', @level2type=N'COLUMN',@level2name=N'No'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'手机号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Staff', @level2type=N'COLUMN',@level2name=N'Phone'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'邮箱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Staff', @level2type=N'COLUMN',@level2name=N'Email'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属用户ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Staff', @level2type=N'COLUMN',@level2name=N'User_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属企业id,多租户考虑' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Staff', @level2type=N'COLUMN',@level2name=N'Company_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Staff', @level2type=N'COLUMN',@level2name=N'creator'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Staff', @level2type=N'COLUMN',@level2name=N'creation_time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Staff', @level2type=N'COLUMN',@level2name=N'modifier'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Staff', @level2type=N'COLUMN',@level2name=N'modification_time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Staff', @level2type=N'COLUMN',@level2name=N'deleted_by'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Staff', @level2type=N'COLUMN',@level2name=N'deleted_time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除标记' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Staff', @level2type=N'COLUMN',@level2name=N'deleted'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'员工信息表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Staff'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户唯一ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0 普通用户；1：企业管理员；' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User', @level2type=N'COLUMN',@level2name=N'User_Type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1 正常；0 停用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属企业id,多租户考虑' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User', @level2type=N'COLUMN',@level2name=N'Company_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User', @level2type=N'COLUMN',@level2name=N'creator'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User', @level2type=N'COLUMN',@level2name=N'creation_time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User', @level2type=N'COLUMN',@level2name=N'modifier'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User', @level2type=N'COLUMN',@level2name=N'modification_time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User', @level2type=N'COLUMN',@level2name=N'deleted_by'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User', @level2type=N'COLUMN',@level2name=N'deleted_time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除标记' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User', @level2type=N'COLUMN',@level2name=N'deleted'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户信息表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User_Preference_Notification', @level2type=N'COLUMN',@level2name=N'User_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'通知id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User_Preference_Notification', @level2type=N'COLUMN',@level2name=N'Notification_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'记录已读时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User_Preference_Notification', @level2type=N'COLUMN',@level2name=N'creation_time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User_Preference_Notification', @level2type=N'COLUMN',@level2name=N'deleted_time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除标记（代表用户不想再接收该通知）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User_Preference_Notification', @level2type=N'COLUMN',@level2name=N'deleted'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户d' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserRoles', @level2type=N'COLUMN',@level2name=N'User_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属角色Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserRoles', @level2type=N'COLUMN',@level2name=N'Role_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属企业id,多租户考虑' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserRoles', @level2type=N'COLUMN',@level2name=N'Company_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户角色关系表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserRoles'
GO
USE [master]
GO
ALTER DATABASE [YizitTurbo] SET  READ_WRITE 
GO
