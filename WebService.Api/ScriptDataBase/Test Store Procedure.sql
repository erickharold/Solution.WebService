

USE RuletaApuestas

GO

CREATE PROCEDURE usp_InsertarRuleta
(
	@EstadoAbierto BIT = 0,
	@IdUsuario INT,
	@UsuarioRegistro NVARCHAR(50),
	@FechaRegistro DATETIME,
	@UsuarioActualizacion NVARCHAR(50),
	@FechaActualizacion DATETIME
)
AS
BEGIN
	INSERT INTO Ruleta (EstadoAbierto, IdUsuario, UsuarioRegistro, FechaRegistro, UsuarioActualizacion, FechaActualizacion)
	VALUES (@EstadoAbierto, @IdUsuario, @UsuarioRegistro, @FechaRegistro, @UsuarioActualizacion, @FechaActualizacion)

	SELECT SCOPE_IDENTITY()
END

GO


CREATE PROCEDURE usp_ObtenerRuleta
(
	@IdRuleta INT = 0
)
AS
BEGIN
	SELECT
		IdRuleta, EstadoAbierto, IdUsuario, UsuarioRegistro, FechaRegistro, UsuarioActualizacion, FechaActualizacion
	FROM Ruleta
	WHERE IdRuleta = @IdRuleta
END

GO



CREATE PROCEDURE usp_ActualizarRuleta
(
	@IdRuleta INT = 0,
	@EstadoAbierto BIT = 0,
	@IdUsuario INT,
	@UsuarioRegistro NVARCHAR(50),
	@FechaRegistro DATETIME,
	@UsuarioActualizacion NVARCHAR(50),
	@FechaActualizacion DATETIME
)
AS
BEGIN
	UPDATE Ruleta SET EstadoAbierto = @EstadoAbierto, IdUsuario = @IdUsuario, 
		UsuarioRegistro = @UsuarioRegistro, FechaRegistro = @FechaRegistro,
		UsuarioActualizacion = @UsuarioActualizacion, FechaActualizacion = @FechaActualizacion
	WHERE IdRuleta = @IdRuleta
END

GO



CREATE PROCEDURE usp_InsertarJugadas
(
	@IdRuleta INT = 0,
	@NroJuego INT,
	@IdJugador INT,
	@TipoApuesta VARCHAR(10),
	@ValorTipoApuesta VARCHAR(10),
	@MontoApuesta DECIMAL(9,2),
	@ValorGanador VARCHAR(10),
	@MontoGanado DECIMAL(9,2),
	@Fue_Ganador bit = 0,
	@EstadoJugada bit = 0
)
AS
BEGIN
	INSERT INTO RuletaJuegos
		(
			IdRuleta, NroJuego, IdJugador, TipoApuesta, ValorTipoApuesta, 
			MontoApuesta, ValorGanador, MontoGanado, Fue_Ganador, EstadoJugada
		 )
	VALUES
		(
			@IdRuleta, @NroJuego, @IdJugador, @TipoApuesta, @ValorTipoApuesta, 
			@MontoApuesta, @ValorGanador, @MontoGanado, @Fue_Ganador, @EstadoJugada
		)

	SELECT SCOPE_IDENTITY()
END


GO


CREATE PROCEDURE usp_ListarJugdoresXNroJuego
(
	@IdRuleta INT = 0,
	@NroJuego INT = 0
)
AS
BEGIN
	SELECT * FROM RuletaJuegos
	WHERE IdRuleta = @IdRuleta AND NroJuego = @NroJuego
END


GO



CREATE PROCEDURE usp_ActualizarJugada
(
	@IdJugada INT = 0,
	@IdRuleta INT = 0,
	@NroJuego INT = 0,
	@IdJugador INT = 0,
	@TipoApuesta VARCHAR(10),
	@ValorTipoApuesta VARCHAR(10),
	@MontoApuesta DECIMAL(9,2),
	@ValorGanador VARCHAR(10),
	@MontoGanado DECIMAL(9,2),
	@Fue_Ganador bit = 0,
	@EstadoJugada bit = 0
)
AS
BEGIN
	UPDATE RuletaJuegos SET IdRuleta = @IdRuleta, NroJuego = @NroJuego, IdJugador = @IdJugador,
	TipoApuesta = @TipoApuesta, ValorTipoApuesta = @ValorTipoApuesta, MontoApuesta = @MontoApuesta,
	ValorGanador = @ValorGanador, MontoGanado = @MontoGanado,  Fue_Ganador = @Fue_Ganador, EstadoJugada = @EstadoJugada
	WHERE IdJugada = @IdJugada
END

GO



CREATE PROCEDURE usp_MostrarGanadorJuegoRuleta
(
	@IdRuleta INT = 0
)
AS
BEGIN
	SELECT
		J.IdJugada, J.IdRuleta, J.NroJuego, J.IdJugador, p.Nombre + ' ' + p.Apellido as ClienteNombre , 
		J.TipoApuesta, J.ValorTipoApuesta, J.MontoApuesta, J.ValorGanador,
		J.MontoGanado, J.Fue_Ganador, J.EstadoJugada
	FROM RuletaJuegos J
	INNER JOIN Persona P
	ON j.IdJugador = p.IdPersona
	WHERE j.IdRuleta = @IdRuleta AND j.Fue_Ganador = 1
END

GO


CREATE PROCEDURE usp_ObtenerCreditoPersona
(
	@IdPersona INT = 0
)
AS
BEGIN
	SELECT
		IdCredito, IdPersona, Tarjeta, NroCuenta, Saldo, 
		UsuarioRegistro, FechaRegistro, UsuarioActualizacion, FechaActualizacion
	FROM Credito
	WHERE IdPersona = @IdPersona
END


GO


CREATE PROCEDURE usp_ActualizarCreditoPersona
(
	@IdCredito INT = 0,
	@IdPersona INT = 0,
	@Tarjeta NVARCHAR(50),
	@NroCuenta NVARCHAR(50),
	@Saldo DECIMAL(9,2),
	@UsuarioRegistro NVARCHAR(50),
	@FechaRegistro DATETIME,
	@UsuarioActualizacion NVARCHAR(50),
	@FechaActualizacion DATETIME

)
AS
BEGIN
	UPDATE Credito SET Tarjeta = @Tarjeta, NroCuenta = @NroCuenta, Saldo = @Saldo, 
	UsuarioRegistro = @UsuarioRegistro, FechaRegistro = @FechaRegistro,
	UsuarioActualizacion = @UsuarioActualizacion, FechaActualizacion = @FechaActualizacion
	WHERE IdCredito = @IdCredito AND IdPersona = @IdPersona 
END


GO


CREATE PROCEDURE usp_ListRoulette
AS
BEGIN
	SELECT
		r.IdRuleta, r.EstadoAbierto, 
		'Estado' = (
				CASE
					WHEN r.EstadoAbierto = 0 THEN 'CERRADA'
					WHEN r.EstadoAbierto = 1 THEN 'ABIERTA'
				END),
		r.IdUsuario, 
		r.UsuarioRegistro, r.FechaRegistro, r.UsuarioActualizacion, r.FechaActualizacion
	FROM Ruleta r
END
