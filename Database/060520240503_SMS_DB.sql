USE [SMS_DB]
GO
ALTER TABLE [dbo].[Teacher_Subject_Allocation] DROP CONSTRAINT [FK__Teacher_S__Teach__412EB0B6]
GO
ALTER TABLE [dbo].[Teacher_Subject_Allocation] DROP CONSTRAINT [FK__Teacher_S__Subje__4222D4EF]
GO
ALTER TABLE [dbo].[Student_Subject_Teacher_Allocation] DROP CONSTRAINT [FK__Student_S__Subje__45F365D3]
GO
ALTER TABLE [dbo].[Student_Subject_Teacher_Allocation] DROP CONSTRAINT [FK__Student_S__Stude__46E78A0C]
GO
ALTER TABLE [dbo].[Teacher] DROP CONSTRAINT [DF__Teacher__IsEnabl__37A5467C]
GO
ALTER TABLE [dbo].[Subject] DROP CONSTRAINT [DF__Subject__IsEnabl__3D5E1FD2]
GO
ALTER TABLE [dbo].[Student] DROP CONSTRAINT [DF__Student__IsEnabl__3A81B327]
GO
/****** Object:  Index [UQ__Teacher___7733E37DE935FFB0]    Script Date: 5/23/2024 9:24:18 AM ******/
ALTER TABLE [dbo].[Teacher_Subject_Allocation] DROP CONSTRAINT [UQ__Teacher___7733E37DE935FFB0]
GO
/****** Object:  Index [UQ__Student___58646DF98E20C7E0]    Script Date: 5/23/2024 9:24:18 AM ******/
ALTER TABLE [dbo].[Student_Subject_Teacher_Allocation] DROP CONSTRAINT [UQ__Student___58646DF98E20C7E0]
GO
/****** Object:  Table [dbo].[Teacher_Subject_Allocation]    Script Date: 5/23/2024 9:24:18 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Teacher_Subject_Allocation]') AND type in (N'U'))
DROP TABLE [dbo].[Teacher_Subject_Allocation]
GO
/****** Object:  Table [dbo].[Teacher]    Script Date: 5/23/2024 9:24:18 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Teacher]') AND type in (N'U'))
DROP TABLE [dbo].[Teacher]
GO
/****** Object:  Table [dbo].[Subject]    Script Date: 5/23/2024 9:24:18 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Subject]') AND type in (N'U'))
DROP TABLE [dbo].[Subject]
GO
/****** Object:  Table [dbo].[Student_Subject_Teacher_Allocation]    Script Date: 5/23/2024 9:24:18 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Student_Subject_Teacher_Allocation]') AND type in (N'U'))
DROP TABLE [dbo].[Student_Subject_Teacher_Allocation]
GO
/****** Object:  Table [dbo].[Student]    Script Date: 5/23/2024 9:24:18 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Student]') AND type in (N'U'))
DROP TABLE [dbo].[Student]
GO
USE [master]
GO
/****** Object:  Database [SMS_DB]    Script Date: 5/23/2024 9:24:18 AM ******/
DROP DATABASE [SMS_DB]
GO
/****** Object:  Database [SMS_DB]    Script Date: 5/23/2024 9:24:18 AM ******/
CREATE DATABASE [SMS_DB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'SMS_DB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\SMS_DB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'SMS_DB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\SMS_DB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [SMS_DB] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [SMS_DB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [SMS_DB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [SMS_DB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [SMS_DB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [SMS_DB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [SMS_DB] SET ARITHABORT OFF 
GO
ALTER DATABASE [SMS_DB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [SMS_DB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [SMS_DB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [SMS_DB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [SMS_DB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [SMS_DB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [SMS_DB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [SMS_DB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [SMS_DB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [SMS_DB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [SMS_DB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [SMS_DB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [SMS_DB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [SMS_DB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [SMS_DB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [SMS_DB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [SMS_DB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [SMS_DB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [SMS_DB] SET  MULTI_USER 
GO
ALTER DATABASE [SMS_DB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [SMS_DB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [SMS_DB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [SMS_DB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [SMS_DB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [SMS_DB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [SMS_DB] SET QUERY_STORE = ON
GO
ALTER DATABASE [SMS_DB] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [SMS_DB]
GO
/****** Object:  Table [dbo].[Student]    Script Date: 5/23/2024 9:24:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Student](
	[StudentID] [bigint] IDENTITY(1,1) NOT NULL,
	[StudentRegNo] [nvarchar](10) NOT NULL,
	[FirstName] [nvarchar](20) NOT NULL,
	[MiddleName] [nvarchar](20) NULL,
	[LastName] [nvarchar](20) NOT NULL,
	[DisplayName] [nvarchar](20) NOT NULL,
	[Email] [nvarchar](30) NOT NULL,
	[Gender] [nvarchar](10) NOT NULL,
	[DOB] [date] NOT NULL,
	[Address] [nvarchar](50) NOT NULL,
	[ContactNo] [nvarchar](15) NOT NULL,
	[IsEnable] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[StudentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Student_Subject_Teacher_Allocation]    Script Date: 5/23/2024 9:24:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Student_Subject_Teacher_Allocation](
	[StudentAllocationID] [bigint] IDENTITY(1,1) NOT NULL,
	[StudentID] [bigint] NOT NULL,
	[SubjectAllocationID] [bigint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[StudentAllocationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Subject]    Script Date: 5/23/2024 9:24:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Subject](
	[SubjectID] [bigint] IDENTITY(1,1) NOT NULL,
	[SubjectCode] [nvarchar](20) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[IsEnable] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[SubjectID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Teacher]    Script Date: 5/23/2024 9:24:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Teacher](
	[TeacherID] [bigint] IDENTITY(1,1) NOT NULL,
	[TeacherRegNo] [nvarchar](10) NOT NULL,
	[FirstName] [nvarchar](20) NOT NULL,
	[MiddleName] [nvarchar](20) NULL,
	[LastName] [nvarchar](20) NOT NULL,
	[DisplayName] [nvarchar](20) NOT NULL,
	[Email] [nvarchar](30) NOT NULL,
	[Gender] [nvarchar](10) NOT NULL,
	[DOB] [date] NOT NULL,
	[Address] [nvarchar](50) NOT NULL,
	[ContactNo] [nvarchar](15) NOT NULL,
	[IsEnable] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[TeacherID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Teacher_Subject_Allocation]    Script Date: 5/23/2024 9:24:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Teacher_Subject_Allocation](
	[SubjectAllocationID] [bigint] IDENTITY(1,1) NOT NULL,
	[TeacherID] [bigint] NOT NULL,
	[SubjectID] [bigint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[SubjectAllocationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Student] ON 

INSERT [dbo].[Student] ([StudentID], [StudentRegNo], [FirstName], [MiddleName], [LastName], [DisplayName], [Email], [Gender], [DOB], [Address], [ContactNo], [IsEnable]) VALUES (1, N'ST001', N'Michael', N'James', N'Anderson', N'Michael Anderson', N'michael.anderson@example.com', N'Male', CAST(N'1990-02-28' AS Date), N'789 Elm St', N'555-234-5678', 1)
INSERT [dbo].[Student] ([StudentID], [StudentRegNo], [FirstName], [MiddleName], [LastName], [DisplayName], [Email], [Gender], [DOB], [Address], [ContactNo], [IsEnable]) VALUES (2, N'ST002', N'Emma', N'Grace', N'Williams', N'Emma Williams', N'emma.williams@example.com', N'Female', CAST(N'1992-11-10' AS Date), N'567 Pine Rd', N'555-876-5432', 1)
SET IDENTITY_INSERT [dbo].[Student] OFF
GO
SET IDENTITY_INSERT [dbo].[Student_Subject_Teacher_Allocation] ON 

INSERT [dbo].[Student_Subject_Teacher_Allocation] ([StudentAllocationID], [StudentID], [SubjectAllocationID]) VALUES (1, 1, 1)
INSERT [dbo].[Student_Subject_Teacher_Allocation] ([StudentAllocationID], [StudentID], [SubjectAllocationID]) VALUES (3, 1, 2)
INSERT [dbo].[Student_Subject_Teacher_Allocation] ([StudentAllocationID], [StudentID], [SubjectAllocationID]) VALUES (4, 2, 1)
INSERT [dbo].[Student_Subject_Teacher_Allocation] ([StudentAllocationID], [StudentID], [SubjectAllocationID]) VALUES (2, 2, 2)
SET IDENTITY_INSERT [dbo].[Student_Subject_Teacher_Allocation] OFF
GO
SET IDENTITY_INSERT [dbo].[Subject] ON 

INSERT [dbo].[Subject] ([SubjectID], [SubjectCode], [Name], [IsEnable]) VALUES (1, N'M101', N'Mathematics', 1)
INSERT [dbo].[Subject] ([SubjectID], [SubjectCode], [Name], [IsEnable]) VALUES (2, N'S102', N'Science', 1)
SET IDENTITY_INSERT [dbo].[Subject] OFF
GO
SET IDENTITY_INSERT [dbo].[Teacher] ON 

INSERT [dbo].[Teacher] ([TeacherID], [TeacherRegNo], [FirstName], [MiddleName], [LastName], [DisplayName], [Email], [Gender], [DOB], [Address], [ContactNo], [IsEnable]) VALUES (1, N'TR001', N'John', N'William', N'Smith', N'John Smith', N'john.smith@example.com', N'Male', CAST(N'1985-08-15' AS Date), N'123 Main St', N'555-123-4567', 1)
INSERT [dbo].[Teacher] ([TeacherID], [TeacherRegNo], [FirstName], [MiddleName], [LastName], [DisplayName], [Email], [Gender], [DOB], [Address], [ContactNo], [IsEnable]) VALUES (2, N'TR002', N'Emily', N'Rose', N'Johnson', N'Emily Johnson', N'emily.johnson@example.com', N'Female', CAST(N'1980-05-20' AS Date), N'456 Oak Ave', N'555-987-6543', 1)
SET IDENTITY_INSERT [dbo].[Teacher] OFF
GO
SET IDENTITY_INSERT [dbo].[Teacher_Subject_Allocation] ON 

INSERT [dbo].[Teacher_Subject_Allocation] ([SubjectAllocationID], [TeacherID], [SubjectID]) VALUES (1, 1, 1)
INSERT [dbo].[Teacher_Subject_Allocation] ([SubjectAllocationID], [TeacherID], [SubjectID]) VALUES (2, 2, 2)
SET IDENTITY_INSERT [dbo].[Teacher_Subject_Allocation] OFF
GO
/****** Object:  Index [UQ__Student___58646DF98E20C7E0]    Script Date: 5/23/2024 9:24:18 AM ******/
ALTER TABLE [dbo].[Student_Subject_Teacher_Allocation] ADD UNIQUE NONCLUSTERED 
(
	[StudentID] ASC,
	[SubjectAllocationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UQ__Teacher___7733E37DE935FFB0]    Script Date: 5/23/2024 9:24:18 AM ******/
ALTER TABLE [dbo].[Teacher_Subject_Allocation] ADD UNIQUE NONCLUSTERED 
(
	[TeacherID] ASC,
	[SubjectID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Student] ADD  DEFAULT ((1)) FOR [IsEnable]
GO
ALTER TABLE [dbo].[Subject] ADD  DEFAULT ((1)) FOR [IsEnable]
GO
ALTER TABLE [dbo].[Teacher] ADD  DEFAULT ((1)) FOR [IsEnable]
GO
ALTER TABLE [dbo].[Student_Subject_Teacher_Allocation]  WITH CHECK ADD FOREIGN KEY([StudentID])
REFERENCES [dbo].[Student] ([StudentID])
GO
ALTER TABLE [dbo].[Student_Subject_Teacher_Allocation]  WITH CHECK ADD FOREIGN KEY([SubjectAllocationID])
REFERENCES [dbo].[Teacher_Subject_Allocation] ([SubjectAllocationID])
GO
ALTER TABLE [dbo].[Teacher_Subject_Allocation]  WITH CHECK ADD FOREIGN KEY([SubjectID])
REFERENCES [dbo].[Subject] ([SubjectID])
GO
ALTER TABLE [dbo].[Teacher_Subject_Allocation]  WITH CHECK ADD FOREIGN KEY([TeacherID])
REFERENCES [dbo].[Teacher] ([TeacherID])
GO
USE [master]
GO
ALTER DATABASE [SMS_DB] SET  READ_WRITE 
GO
