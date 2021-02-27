



CREATE DATABASE RuletaApuestas

GO

USE RuletaApuestas

GO

CREATE TABLE Persona
(
	IdPersona INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
	Nombre NVARCHAR(250),
	Apellido NVARCHAR(250),
	UsuarioRegistro NVARCHAR(50),
	FechaRegistro DATETIME,
	UsuarioActualizacion NVARCHAR(50),
	FechaActualizacion DATETIME
)

GO

INSERT INTO Persona (Nombre, Apellido, UsuarioRegistro, FechaRegistro, UsuarioActualizacion, FechaActualizacion)
VALUES
(	
	'ERICK', 'ARIAS', 'ADMIN', GETDATE(), 'ADMIN', GETDATE()
),
(
	'JUAN', 'ROMERO', 'ADMIN', GETDATE(), 'ADMIN', GETDATE()
),
(	
	'JORGE', 'PEREZ', 'ADMIN', GETDATE(), 'ADMIN', GETDATE()
),
(
	'PABLO', 'LEON', 'ADMIN', GETDATE(), 'ADMIN', GETDATE()
),
(	
	'PAOLO', 'SANCHEZ', 'ADMIN', GETDATE(), 'ADMIN', GETDATE()
)

GO

CREATE TABLE Usuario
(
	IdUsuario INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
	IdPersona INT NOT NULL,
	Rol NVARCHAR(250),
	UsuarioRegistro NVARCHAR(50),
	FechaRegistro DATETIME,
	UsuarioActualizacion NVARCHAR(50),
	FechaActualizacion DATETIME,
	FOREIGN KEY (IdPersona) REFERENCES Persona (IdPersona)
)

GO

INSERT INTO Usuario(IdPersona, Rol, UsuarioRegistro, FechaRegistro, UsuarioActualizacion, FechaActualizacion)
VALUES
	(	
		1, 'ADMIN', 'ADMIN', GETDATE(), 'ADMIN', GETDATE()
	),
	(
		2, 'CLIENTE', 'ADMIN', GETDATE(), 'ADMIN', GETDATE()
	),
	(	
		3, 'CLIENTE', 'ADMIN', GETDATE(), 'ADMIN', GETDATE()
	),
	(
		4, 'CLIENTE', 'ADMIN', GETDATE(), 'ADMIN', GETDATE()
	),
	(	
		5, 'CLIENTE', 'ADMIN', GETDATE(), 'ADMIN', GETDATE()
	)

GO

CREATE TABLE Credito
(
	IdCredito INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
	IdPersona INT NOT NULL,
	Tarjeta NVARCHAR(50),
	NroCuenta NVARCHAR(50),
	Saldo decimal(9,2),
	UsuarioRegistro NVARCHAR(50),
	FechaRegistro DATETIME,
	UsuarioActualizacion NVARCHAR(50),
	FechaActualizacion DATETIME,
	FOREIGN KEY (IdPersona) REFERENCES Persona (IdPersona)
)

GO

INSERT INTO Credito (IdPersona, Tarjeta, NroCuenta, Saldo, UsuarioRegistro, FechaRegistro, UsuarioActualizacion, FechaActualizacion)
VALUES
(
	1, 'BCP', '1234567898', '1000000', 'ADMIN', GETDATE(), 'ADMIN', GETDATE()
),
(
	2, 'BCP', '7827492749', '1000000', 'ADMIN', GETDATE(), 'ADMIN', GETDATE()
),
(
	3, 'BCP', '1998473934', '1000000', 'ADMIN', GETDATE(), 'ADMIN', GETDATE()
),
(
	4, 'BCP', '8392048292', '1000000', 'ADMIN', GETDATE(), 'ADMIN', GETDATE()
),
(
	5, 'BCP', '1738299984', '1000000', 'ADMIN', GETDATE(), 'ADMIN', GETDATE()
)


GO

CREATE TABLE Ruleta
(
	IdRuleta INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
	EstadoAbierto bit,
	IdUsuario INT NOT NULL,
	UsuarioRegistro NVARCHAR(50),
	FechaRegistro DATETIME,
	UsuarioActualizacion NVARCHAR(50),
	FechaActualizacion DATETIME,
	FOREIGN KEY (IdUsuario) REFERENCES Usuario (IdUsuario)
)

GO

GO

CREATE TABLE RuletaJuegos
(
	IdJugada INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
	IdRuleta INT NOT NULL,
	NroJuego INT,
	IdJugador INT NOT NULL,
	TipoApuesta VARCHAR(10),
	ValorTipoApuesta VARCHAR(10),
	MontoApuesta DECIMAL(9,2),
	ValorGanador VARCHAR(10),
	MontoGanado DECIMAL(9,2),
	Fue_Ganador bit,
	EstadoJugada bit,
	FOREIGN KEY (IdRuleta) REFERENCES Ruleta (IdRuleta),
	FOREIGN KEY (IdJugador) REFERENCES Persona (IdPersona)
)

GO

