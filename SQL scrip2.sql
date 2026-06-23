USE [master]
GO
/****** Object:  Database [ConsultorioDental]    Script Date: 6/22/2026 7:31:37 PM ******/
CREATE DATABASE [ConsultorioDental]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ConsultorioDental', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL17.MSSQLSERVER\MSSQL\DATA\ConsultorioDental.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'ConsultorioDental_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL17.MSSQLSERVER\MSSQL\DATA\ConsultorioDental_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [ConsultorioDental] SET COMPATIBILITY_LEVEL = 170
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ConsultorioDental].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ConsultorioDental] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ConsultorioDental] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ConsultorioDental] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ConsultorioDental] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ConsultorioDental] SET ARITHABORT OFF 
GO
ALTER DATABASE [ConsultorioDental] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ConsultorioDental] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ConsultorioDental] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ConsultorioDental] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ConsultorioDental] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ConsultorioDental] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ConsultorioDental] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ConsultorioDental] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ConsultorioDental] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ConsultorioDental] SET  ENABLE_BROKER 
GO
ALTER DATABASE [ConsultorioDental] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ConsultorioDental] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ConsultorioDental] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ConsultorioDental] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ConsultorioDental] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ConsultorioDental] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ConsultorioDental] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ConsultorioDental] SET RECOVERY FULL 
GO
ALTER DATABASE [ConsultorioDental] SET  MULTI_USER 
GO
ALTER DATABASE [ConsultorioDental] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ConsultorioDental] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ConsultorioDental] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ConsultorioDental] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [ConsultorioDental] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [ConsultorioDental] SET OPTIMIZED_LOCKING = OFF 
GO
ALTER DATABASE [ConsultorioDental] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [ConsultorioDental] SET QUERY_STORE = ON
GO
ALTER DATABASE [ConsultorioDental] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [ConsultorioDental]
GO
/****** Object:  Table [dbo].[Citas]    Script Date: 6/22/2026 7:31:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Citas](
	[IdCita] [int] IDENTITY(1,1) NOT NULL,
	[Fecha] [date] NOT NULL,
	[Hora] [time](7) NOT NULL,
	[Estado] [varchar](20) NULL,
	[IdPaciente] [int] NOT NULL,
	[IdDentista] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdCita] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CitasApp]    Script Date: 6/22/2026 7:31:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CitasApp](
	[IdCita] [int] IDENTITY(1,1) NOT NULL,
	[IdPaciente] [int] NOT NULL,
	[FechaHora] [varchar](30) NOT NULL,
	[MotivoConsulta] [varchar](200) NULL,
	[Activa] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[IdCita] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Dentista]    Script Date: 6/22/2026 7:31:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dentista](
	[IdDentista] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
	[Especialidad] [varchar](100) NULL,
	[Telefono] [varchar](20) NULL,
	[Activo] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[IdDentista] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Dentistas]    Script Date: 6/22/2026 7:31:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dentistas](
	[IdDentista] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
	[Especialidad] [varchar](100) NULL,
	[Telefono] [varchar](20) NULL,
	[Activo] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[IdDentista] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HistorialClinico]    Script Date: 6/22/2026 7:31:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HistorialClinico](
	[IdHistorial] [int] IDENTITY(1,1) NOT NULL,
	[IdPaciente] [int] NOT NULL,
	[FechaConsulta] [datetime] NULL,
	[Diagnostico] [varchar](max) NULL,
	[Tratamiento] [varchar](max) NULL,
	[Observaciones] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdHistorial] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pacientes]    Script Date: 6/22/2026 7:31:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pacientes](
	[IdPaciente] [int] IDENTITY(1,1) NOT NULL,
	[Cedula] [varchar](20) NULL,
	[Nombre] [varchar](50) NOT NULL,
	[Apellido] [varchar](50) NOT NULL,
	[Telefono] [varchar](20) NULL,
	[Direccion] [varchar](200) NULL,
	[FechaNacimiento] [date] NULL,
	[FechaRegistro] [datetime] NULL,
	[Activo] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[IdPaciente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PacientesApp]    Script Date: 6/22/2026 7:31:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PacientesApp](
	[IdPaciente] [int] IDENTITY(1,1) NOT NULL,
	[NombreCompleto] [varchar](100) NOT NULL,
	[Telefono] [varchar](20) NOT NULL,
	[FechaNacimiento] [varchar](20) NOT NULL,
	[Correo] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdPaciente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pagos]    Script Date: 6/22/2026 7:31:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pagos](
	[IdPago] [int] IDENTITY(1,1) NOT NULL,
	[IdPaciente] [int] NOT NULL,
	[FechaPago] [datetime] NULL,
	[Monto] [decimal](10, 2) NULL,
	[MetodoPago] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdPago] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Citas] ON 

INSERT [dbo].[Citas] ([IdCita], [Fecha], [Hora], [Estado], [IdPaciente], [IdDentista]) VALUES (1, CAST(N'2026-07-01' AS Date), CAST(N'09:00:00' AS Time), N'Pendiente', 1, 1)
INSERT [dbo].[Citas] ([IdCita], [Fecha], [Hora], [Estado], [IdPaciente], [IdDentista]) VALUES (2, CAST(N'2026-10-16' AS Date), CAST(N'00:00:00' AS Time), N'Cancelada', 2, 1)
INSERT [dbo].[Citas] ([IdCita], [Fecha], [Hora], [Estado], [IdPaciente], [IdDentista]) VALUES (3, CAST(N'2026-11-16' AS Date), CAST(N'00:00:00' AS Time), N'Activa', 3, 1)
SET IDENTITY_INSERT [dbo].[Citas] OFF
GO
SET IDENTITY_INSERT [dbo].[Dentistas] ON 

INSERT [dbo].[Dentistas] ([IdDentista], [Nombre], [Especialidad], [Telefono], [Activo]) VALUES (1, N'Ana Lopez', N'Ortodoncia', N'77777777', 1)
SET IDENTITY_INSERT [dbo].[Dentistas] OFF
GO
SET IDENTITY_INSERT [dbo].[Pacientes] ON 

INSERT [dbo].[Pacientes] ([IdPaciente], [Cedula], [Nombre], [Apellido], [Telefono], [Direccion], [FechaNacimiento], [FechaRegistro], [Activo]) VALUES (1, N'001-123456-0001A', N'Juan', N'Perez', N'88888888', N'Managua', CAST(N'2000-05-15' AS Date), CAST(N'2026-06-21T02:29:43.780' AS DateTime), 1)
INSERT [dbo].[Pacientes] ([IdPaciente], [Cedula], [Nombre], [Apellido], [Telefono], [Direccion], [FechaNacimiento], [FechaRegistro], [Activo]) VALUES (2, N'001 1308899 1003T', N'Ulises', N'Frixione', N'81251748', NULL, NULL, CAST(N'2026-06-22T06:13:51.903' AS DateTime), 1)
INSERT [dbo].[Pacientes] ([IdPaciente], [Cedula], [Nombre], [Apellido], [Telefono], [Direccion], [FechaNacimiento], [FechaRegistro], [Activo]) VALUES (3, N'001 253695 7485 T', N'Manuel', N'Gutierrez', N'87581258', NULL, NULL, CAST(N'2026-06-22T06:15:38.743' AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[Pacientes] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Paciente__B4ADFE3882331695]    Script Date: 6/22/2026 7:31:39 PM ******/
ALTER TABLE [dbo].[Pacientes] ADD UNIQUE NONCLUSTERED 
(
	[Cedula] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Citas] ADD  DEFAULT ('Pendiente') FOR [Estado]
GO
ALTER TABLE [dbo].[CitasApp] ADD  DEFAULT ((1)) FOR [Activa]
GO
ALTER TABLE [dbo].[Dentista] ADD  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[Dentistas] ADD  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[HistorialClinico] ADD  DEFAULT (getdate()) FOR [FechaConsulta]
GO
ALTER TABLE [dbo].[Pacientes] ADD  DEFAULT (getdate()) FOR [FechaRegistro]
GO
ALTER TABLE [dbo].[Pacientes] ADD  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[Pagos] ADD  DEFAULT (getdate()) FOR [FechaPago]
GO
ALTER TABLE [dbo].[Citas]  WITH CHECK ADD FOREIGN KEY([IdDentista])
REFERENCES [dbo].[Dentistas] ([IdDentista])
GO
ALTER TABLE [dbo].[Citas]  WITH CHECK ADD FOREIGN KEY([IdPaciente])
REFERENCES [dbo].[Pacientes] ([IdPaciente])
GO
ALTER TABLE [dbo].[CitasApp]  WITH CHECK ADD  CONSTRAINT [FK_CitasApp_PacientesApp] FOREIGN KEY([IdPaciente])
REFERENCES [dbo].[PacientesApp] ([IdPaciente])
GO
ALTER TABLE [dbo].[CitasApp] CHECK CONSTRAINT [FK_CitasApp_PacientesApp]
GO
ALTER TABLE [dbo].[HistorialClinico]  WITH CHECK ADD FOREIGN KEY([IdPaciente])
REFERENCES [dbo].[Pacientes] ([IdPaciente])
GO
ALTER TABLE [dbo].[Pagos]  WITH CHECK ADD FOREIGN KEY([IdPaciente])
REFERENCES [dbo].[Pacientes] ([IdPaciente])
GO
USE [master]
GO
ALTER DATABASE [ConsultorioDental] SET  READ_WRITE 
GO
