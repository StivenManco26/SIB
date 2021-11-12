--MODULO RESERVA------------------------------------------------------
CREATE PROCEDURE sp_consultar_Reserva_estado
AS
BEGIN
	SELECT id as Clave, descripcion as Dato
	FROM tblReservaEstado
	WHERE id<>2
	ORDER BY id ASC 
END
GO


CREATE PROCEDURE sp_cancelar_Reserva
@idReserva INT
AS
BEGIN

IF EXISTS (SELECT * FROM tblReserva WHERE id=@idReserva)
BEGIN

	BEGIN TRANSACTION tx

		UPDATE tblReserva
		SET idEstado=2
		WHERE id=@idReserva

		IF ( @@ERROR > 0 )
		BEGIN
			ROLLBACK TRANSACTION tx
			SELECT 0 AS Rpta
			RETURN
		END

	COMMIT TRANSACTION tx
	SELECT 1 AS Rpta
	RETURN

END
ELSE 
	SELECT 0 AS Rpta
	RETURN
END

--EXEC sp_cancelar_Reserva 1
GO


CREATE PROCEDURE sp_ingresar_Reserva ---HACER CON CLAVE DATO
@codMaterial VARCHAR(30),
@nit VARCHAR(30),
@idEstado INT,
@fechaReserva DATETIME
AS
BEGIN

--Se debe validar que la fecha ingresada sea superior a la fecha actual para poder ejecutar este sp

	DECLARE @MaxPrestamo INT
	DECLARE @CantPrestamo INT
	DECLARE @DiasPrestamo INT

		SELECT @MaxPrestamo= PE.maxCantPrestamo,@DiasPrestamo=PE.diasPrestamo
		FROM tblPersona P
		INNER JOIN tblPerfil PE ON P.perfil=PE.perfil
		WHERE P.nit = @nit

		SELECT @CantPrestamo=MAX(cant)
		FROM (
			SELECT codigoMat,cant=ROW_NUMBER() OVER (PARTITION BY codigoMat ORDER BY codigoMat)
			FROM tblPrestamo
			WHERE nit=@nit ) A

		IF (@CantPrestamo>=@MaxPrestamo)
		BEGIN
			SELECT 0 AS Rpta, 'El usuario excede la cantidad de prestamos permitidos' Msj
			RETURN
		END

	DECLARE @fechaReservaActual AS DATE
	DECLARE @fechaPosibleDevolucion AS DATE
	DECLARE @control INT

	DECLARE validarFecha CURSOR FOR
		SELECT CONVERT(DATE,R.fechaReserva),CONVERT(DATE,DATEADD(DAY,PE.diasPrestamo,R.fechaReserva))
		FROM tblReserva R
		INNER JOIN tblPersona P ON R.nit=P.nit
		INNER JOIN tblPerfil PE ON P.perfil=PE.perfil
		WHERE R.codigoMat=@codMaterial AND R.idEstado=1
	OPEN validarFecha
		FETCH NEXT FROM Cambio_Precio1 INTO @fechaReservaActual,@fechaPosibleDevolucion
		WHILE @@FETCH_STATUS = 0 BEGIN
			IF ((CONVERT(DATE,@fechaReserva) BETWEEN @fechaReservaActual AND @fechaPosibleDevolucion)
				OR (CONVERT(DATE,DATEADD(DAY,@DiasPrestamo,@fechaReserva)) BETWEEN @fechaReservaActual AND @fechaPosibleDevolucion))
			BEGIN
				SET @control=1
			END
			FETCH NEXT FROM validarFecha INTO @fechaReservaActual,@fechaPosibleDevolucion 
		END
	CLOSE validarFecha
	DEALLOCATE validarFecha

	IF (@control=1)
	BEGIN
		SELECT 0 AS Rpta, 'El material se encuentra reservado para la fecha ingresada' Msj
		RETURN
	END

	BEGIN TRANSACTION tx

		INSERT INTO tblReserva(codigoMat,nit,idEstado,fechaReserva,fechaRegistro)
		VALUES (@codMaterial,@nit,@idEstado,@fechaReserva,GETDATE());

		IF ( @@ERROR > 0 )
		BEGIN
			ROLLBACK TRANSACTION tx
			SELECT 0 AS Rpta, 'Error al ingresar' Msj
			RETURN
		END

	COMMIT TRANSACTION tx
	SELECT 1 AS Rpta,'Ingresado correctamente' Msj
	RETURN

	END

	--EXEC sp_ingresar_Reserva 'CAS0001','1152704820',1,'3191234567',1,'01/12/2021'
GO
