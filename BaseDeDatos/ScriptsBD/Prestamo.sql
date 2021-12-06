USE [SIB]
GO


CREATE PROCEDURE [dbo].[sp_Contar_Prestamo]
@nit VARCHAR(30)
AS
BEGIN
	SELECT COUNT(nit) 
	FROM tblPrestamo
	WHERE nit = @nit
	--EXEC sp_contar_Prestamo '1152704820'
END
GO


CREATE PROCEDURE [dbo].[sp_buscar_prestamo_persona]
@nit VARCHAR(30)
AS
BEGIN
	SELECT P.id,P.nit,PE.nombre,P.codigoMat,ME.estado,ISNULL(P.idReserva,0) idReserva,
	CONVERT(DATE,P.fechaPrestamo,3) fechaPrestamo,CONVERT(DATE,P.fechaDevolucion,1) fechaDevolucion,
	CONVERT(DATE,P.fechaRegistro,3) fechaRegistro
	FROM tblPrestamo P
	INNER JOIN tblPersona PE ON P.nit=PE.nit
	INNER JOIN tblMaterialEstado ME ON P.idEstadoMat=ME.id
	WHERE P.nit = @nit
	--EXEC sp_buscar_prestamo_persona '1152704820'
END
GO


CREATE PROCEDURE [dbo].[sp_Ingresar_Prestamo]
@codigoMat VARCHAR(30),
@idEstadoMat INT,
@nit VARCHAR(30),
@idReserva INT,
@FechaDevolucion DATETIME
AS
BEGIN

 IF EXISTS (SELECT nit,SUM(monto) FROM tblSancion WHERE nit=@nit GROUP BY nit HAVING SUM(monto)>0)
BEGIN
SELECT 2 AS Rpta --Mostrar mensaje de bloqueo por no pago de sanción
RETURN
END

 BEGIN TRANSACTION tx

 INSERT INTO tblPrestamo(codigoMat,idEstadoMat,nit,idReserva,fechaPrestamo,fechaDevolucion,fechaRegistro)
VALUES (@codigoMat,@idEstadoMat,@nit,@idReserva,GETDATE(),CONVERT(DATE,@FechaDevolucion,3),GETDATE())

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
GO

CREATE PROCEDURE sp_validar_reserva_prestamo
@codigoMat VARCHAR(30),
@nit VARCHAR(30)
AS
BEGIN

DECLARE @fechaReserva DATE
DECLARE @fechaDevolucion DATE

SELECT @fechaDevolucion=CONVERT(DATE,DATEADD(DAY,PE.diasPrestamo,GETDATE()))
FROM tblPersona P
INNER JOIN tblPerfil PE ON P.perfil=PE.id 
WHERE P.nit=@nit

SELECT @fechaReserva=CONVERT(DATE,DATEADD(DAY,PE.diasPrestamo,R.fechaReserva))
FROM tblReserva R
INNER JOIN tblPersona P ON R.nit=P.nit
INNER JOIN tblPerfil PE ON P.perfil=PE.id
WHERE R.codigoMat=@codigoMat AND R.idEstado=1 AND P.nit=@nit

IF(@fechaReserva BETWEEN CONVERT(DATE,GETDATE()) AND @fechaDevolucion)
BEGIN
	SELECT 0 AS Rpta
	RETURN
END

SELECT 1 AS Rpta
RETURN

END
GO

CREATE PROCEDURE sp_validar_prestamo
@codigoMat VARCHAR(30)
AS
BEGIN

IF EXISTS (SELECT id FROM tblPrestamo WHERE codigoMat=@codigoMat)
BEGIN
	SELECT 0 AS Rpta
	RETURN
END

SELECT 1 AS Rpta
RETURN

END
GO

CREATE PROCEDURE sp_cargar_reserva_persona
@codigoMat VARCHAR(30),
@nit VARCHAR(30)
AS
BEGIN
	SELECT Reserva=R.id,Material=M.nombre,[Fecha reserva]=R.fechaReserva,Estado=E.descripcion
	FROM tblReserva R
	INNER JOIN tblReservaEstado E ON R.idEstado=E.id
	INNER JOIN tblMaterial M ON R.codigoMat=M.codigo
	WHERE R.codigoMat=@codigoMat AND R.nit=@nit AND R.idEstado=1

	--EXEC sp_cargar_reserva_persona 'CAS0001','1152704820'
END
GO
