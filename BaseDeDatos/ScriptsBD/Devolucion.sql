--MÓDULO DE DEVOLUCIÓN---------------------------

CREATE PROCEDURE sp_Ingresar_Sancion
@codigoMat VARCHAR(30),
@nit VARCHAR(30),
@idDevolu INT,
@monto MONEY,
@notas VARCHAR(300)
AS
BEGIN
	BEGIN TRANSACTION tx

	INSERT INTO tblSancion (codigoMat,nit,idDevolucion,monto,notas)
	VALUES (@codigoMat,@nit,@idDevolu,@monto,@notas)

	IF ( @@ERROR > 0 )
	BEGIN
		ROLLBACK TRANSACTION tx
		--SELECT 0 AS Rpta
		RETURN
	END
	COMMIT TRANSACTION tx
	--SELECT 1 AS Rpta
	RETURN
END
GO

CREATE PROCEDURE sp_Ingresar_Devolucion
@codigoMat VARCHAR(30),
@idEstadoMat INT,
@nit VARCHAR(30),
@nota VARCHAR(300)
AS 
BEGIN	
	DECLARE @monto MONEY=0
	DECLARE @idEstadoPrestamo INT
	DECLARE @fechaPrestamo DATE

	SELECT @idEstadoPrestamo=P.idEstadoMat,@fechaPrestamo=CONVERT(DATE,P.fechaPrestamo)
	FROM tblPrestamo P
	WHERE P.nit=@nit AND P.codigoMat=@codigoMat

	IF (@fechaPrestamo>CONVERT(DATE,GETDATE()))
	BEGIN
		SELECT @monto=@monto+(DATEDIFF(DAY,CONVERT(DATE,GETDATE()),@fechaPrestamo)*1000) --Se registrarán COP$1.000 por cada día de mora
	END

	IF (@idEstadoMat<>@idEstadoPrestamo)
	BEGIN
		SELECT @monto=@monto+30000 --Se registrarán COP$30.000 por el daño del material
	END

	BEGIN TRANSACTION tx

	INSERT INTO tblDevolucion(codigoMat,idEstadoMatPrestamo,idEstadoMatDevolucion,nit,idReserva,fechaPrestamo,fechaDevolucion,fechaDevolucionReal)
	SELECT P.codigoMat,P.idEstadoMat,@idEstadoMat,P.nit,ISNULL(P.idReserva,0),P.fechaPrestamo,P.fechaDevolucion,GETDATE()
	FROM tblPrestamo P
	WHERE P.nit=@nit AND P.codigoMat=@codigoMat

	EXEC sp_Ingresar_Sancion @codigoMat,@nit,@@IDENTITY,@monto,@nota
	
	DELETE 
	FROM tblPrestamo
	WHERE nit=@nit AND codigoMat=@codigoMat

	IF ( @@ERROR > 0 )
	BEGIN
		ROLLBACK TRANSACTION tx
		SELECT 0 AS Rpta
		RETURN
	END
	COMMIT TRANSACTION tx
	SELECT @monto AS Rpta
	RETURN

END
GO




