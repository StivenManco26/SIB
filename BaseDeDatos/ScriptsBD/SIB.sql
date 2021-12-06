---CREACIÓN DE BASE DE DATOS Y TABLAS DEL SISTEMA------------------------------------------------------

CREATE DATABASE SIB
GO

USE SIB
GO

CREATE TABLE tblPersona
(nit VARCHAR(30) PRIMARY KEY NOT NULL,
nombre VARCHAR(100) NOT NULL,
correo VARCHAR(100) NOT NULL,
celular VARCHAR(10) NOT NULL,
perfil INT NOT NULL,
id INT IDENTITY(1,1) NOT NULL)
GO

CREATE TABLE tblUsuario
(usuario VARCHAR(30) PRIMARY KEY NOT NULL,
contrasena VARCHAR(100) NOT NULL,
nit VARCHAR(30) NOT NULL,
activo BIT NOT NULL,
id INT IDENTITY(1,1) NOT NULL)
GO

CREATE TABLE tblPerfil
(perfil INT PRIMARY KEY NOT NULL,
descripcion VARCHAR(100) NOT NULL,
maxCantPrestamo INT NOT NULL,
diasPrestamo INT NOT NULL,
maxCantRenovacion INT NOT NULL,
id INT IDENTITY(1,1) NOT NULL)
GO

CREATE TABLE tblMaterial
(codigo VARCHAR (30) PRIMARY KEY NOT NULL,
nombre VARCHAR (200) NOT NULL,
edicion VARCHAR(10) NOT NULL,
cantidad INT NOT NULL,
idEstado INT NOT NULL,
idAutor INT NOT NULL,
idProductor INT NOT NULL)
GO

CREATE TABLE tblMaterialEstado
(id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
estado VARCHAR(200) NOT NULL)
GO

CREATE TABLE tblMaterialAutor
(id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
autor VARCHAR(200) NOT NULL)
GO

CREATE TABLE tblMaterialProductor
(id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
productor VARCHAR(200) NOT NULL)
GO

CREATE TABLE tblReserva
(id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
codigoMat VARCHAR(30) NOT NULL,
nit VARCHAR(30) NOT NULL,
idEstado INT NOT NULL,
fechaReserva DATETIME NOT NULL,
fechaRegistro DATETIME NOT NULL)
GO

CREATE TABLE tblReservaEstado
(id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
descripcion VARCHAR(30) NOT NULL)
GO

CREATE TABLE tblPrestamo
(id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
codigoMat VARCHAR(30) NOT NULL,
idEstadoMat INT NOT NULL,
nit VARCHAR(30) NOT NULL,
idReserva INT,
fechaPrestamo DATETIME NOT NULL,
fechaDevolucion DATETIME NOT NULL,
fechaRegistro DATETIME NOT NULL)
GO

CREATE TABLE tblDevolucion
(id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
codigoMat VARCHAR(30) NOT NULL,
idEstadoMatPrestamo INT NOT NULL,
idEstadoMatDevolucion INT NOT NULL,
nit VARCHAR(30) NOT NULL,
idReserva INT,
fechaPrestamo DATE NOT NULL,
fechaDevolucion DATE NOT NULL,
fechaDevolucionReal DATE NOT NULL)
GO

CREATE TABLE tblSancion
(id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
codigoMat VARCHAR(30) NOT NULL,
nit VARCHAR(30) NOT NULL,
idDevolucion INT NOT NULL,
monto MONEY NOT NULL,
notas VARCHAR(300))
GO


--CREACIÓN DE RELACIONES ENTRE LAS TABLAS------------------------------------------------------
ALTER TABLE tblPersona ADD FOREIGN KEY (perfil) REFERENCES tblPerfil (perfil)
ALTER TABLE tblUsuario ADD FOREIGN KEY (nit) REFERENCES tblPersona (nit)
ALTER TABLE tblReserva ADD FOREIGN KEY (nit) REFERENCES tblPersona (nit)
ALTER TABLE tblPrestamo ADD FOREIGN KEY (nit) REFERENCES tblPersona (nit)
ALTER TABLE tblDevolucion ADD FOREIGN KEY (nit) REFERENCES tblPersona (nit)
ALTER TABLE tblMaterial ADD FOREIGN KEY (idEstado) REFERENCES tblMaterialEstado (id)
ALTER TABLE tblMaterial ADD FOREIGN KEY (idAutor) REFERENCES tblMaterialAutor (id)
ALTER TABLE tblMaterial ADD FOREIGN KEY (idProductor) REFERENCES tblMaterialProductor (id)
ALTER TABLE tblReserva ADD FOREIGN KEY (idEstado) REFERENCES tblReservaEstado (id)
ALTER TABLE tblReserva ADD FOREIGN KEY (codigoMat) REFERENCES tblMaterial (codigo)
ALTER TABLE tblPrestamo ADD FOREIGN KEY (codigoMat) REFERENCES tblMaterial (codigo)
ALTER TABLE tblPrestamo ADD FOREIGN KEY (idEstadoMat) REFERENCES tblMaterialEstado (id)
ALTER TABLE tblPrestamo ADD FOREIGN KEY (idReserva) REFERENCES tblReserva (id)
ALTER TABLE tblDevolucion ADD FOREIGN KEY (codigoMat) REFERENCES tblMaterial (codigo)
ALTER TABLE tblDevolucion ADD FOREIGN KEY (idEstadoMatPrestamo) REFERENCES tblMaterialEstado (id)
ALTER TABLE tblDevolucion ADD FOREIGN KEY (idEstadoMatDevolucion) REFERENCES tblMaterialEstado (id)
ALTER TABLE tblDevolucion ADD FOREIGN KEY (idReserva) REFERENCES tblReserva (id)
ALTER TABLE tblSancion ADD FOREIGN KEY (codigoMat) REFERENCES tblMaterial (codigo)
ALTER TABLE tblSancion ADD FOREIGN KEY (idDevolucion) REFERENCES tblDevolucion (id)
ALTER TABLE tblSancion ADD FOREIGN KEY (nit) REFERENCES tblPersona (nit)
GO


--INSERCIÓN DE DATOS BÁSICOS DEL SISTEMA------------------------------------------------------
INSERT INTO tblPerfil (perfil,descripcion,maxCantPrestamo,diasPrestamo,maxCantRenovacion)
VALUES (1,'ESTUDIANTE ITM',3,8,2),
(2,'ESTUDIANTE EXTERNO',1,5,0),
(3,'DOCENTE ITM',5,8,3),
(4,'DOCENTE EXTERNO',3,5,1),
(5,'EMPLEADOS',1,3,0)
GO

INSERT INTO tblMaterialEstado (estado)
VALUES ('EXCELENTE'),
('ACEPTABLE'),
('DETERIORADO')
GO

INSERT INTO tblReservaEstado (descripcion)
VALUES ('VIGENTE'),
('PROCESADO'),
('CANCELADO')
GO


--LOGIN------------------------------------------------------
CREATE PROCEDURE sp_login
@USER VARCHAR(20),
@PASS VARCHAR(40)
AS
BEGIN
DECLARE @LOGIN INT

	SELECT @LOGIN=COUNT(usuario) FROM tblUsuario
	WHERE usuario=@USER and contrasena COLLATE Latin1_General_CS_AS=@PASS and activo=1
	IF(@LOGIN>0)
	BEGIN
		SELECT 1 AS Rpta, P.nit, nombre
		FROM tblUsuario U
		INNER JOIN tblPersona  P ON U.nit=P.nit and U.usuario=@USER
	END
	ELSE
		SELECT 0 AS Rpta, '0' AS nit, 0 AS nombre
	
	--EXEC sp_login 'operador1', 'Opera1'
END
GO


--MÓDULO PERSONA------------------------------------------------------
CREATE PROCEDURE sp_ingresar_persona_usuario
@nit VARCHAR(30),
@nombre VARCHAR(100),
@correo VARCHAR(100),
@celular VARCHAR(10),
@perfil INT,
@usuario VARCHAR(30),
@contrasena VARCHAR(100)
AS
BEGIN

IF (NOT EXISTS (SELECT nit FROM tblPersona WHERE nit=@nit) AND NOT EXISTS (SELECT nit FROM tblUsuario WHERE nit=@nit))
BEGIN

	BEGIN TRANSACTION tx

		INSERT INTO tblPersona (nit,nombre,correo,celular,perfil)
		VALUES (@nit,UPPER(@nombre),UPPER(@correo),@celular,UPPER(@perfil));

		INSERT INTO tblUsuario (usuario,contrasena,nit,activo)
		VALUES (UPPER(@usuario),@contrasena,@nit,1)

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

	--EXEC sp_ingresar_persona_usuario '1152704820','Juan Cárdenas','juancardenas284825@correo.itm.edu.co','3191234567',1,'jcardenas','ContraTodo'
GO

CREATE PROCEDURE sp_modificar_contrasena
@nit VARCHAR(30),
@usuario VARCHAR(30),
@contrasena VARCHAR(100)
AS
BEGIN
	IF EXISTS (SELECT nit FROM tblUsuario WHERE nit=@nit AND usuario=@usuario)
	BEGIN

		BEGIN TRANSACTION tx

			UPDATE tblUsuario 
			SET contrasena=@contrasena
			WHERE nit=@nit AND usuario=@usuario;


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
	--EXEC sp_modificar_contrasena '1152704820','jcardenas','ContraCorriente'
GO

CREATE PROCEDURE sp_modificar_persona
@nit VARCHAR(30),
@nombre VARCHAR(100),
@correo VARCHAR(100),
@celular VARCHAR(10),
@perfil INT
AS
BEGIN

IF EXISTS (SELECT nit FROM tblPersona WHERE nit=@nit)
BEGIN

	BEGIN TRANSACTION tx

		UPDATE tblPersona 
		SET nit=@nit,nombre=UPPER(@nombre),correo=UPPER(@correo),celular=@celular,perfil=UPPER(@perfil)
		WHERE nit=@nit;


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
	--EXEC sp_modificar_persona '1152704820','Juan Cárdenas','juancardenas284825@correo.itm.edu.co','3197654321',2
GO

CREATE PROCEDURE sp_buscar_persona
@nit VARCHAR(30)
AS
BEGIN
	SELECT P.nit,P.nombre,P.correo,P.celular,PE.descripcion  
	FROM tblPersona P
	INNER JOIN tblPerfil PE ON P.perfil=PE.perfil
	WHERE nit = @nit
	--EXEC sp_buscar_persona '1152704820'
END
GO

CREATE PROCEDURE sp_buscar_persona_perfil
@nit VARCHAR(30)
AS
BEGIN
	SELECT P.nit,PE.maxCantPrestamo,PE.diasPrestamo,PE.maxCantRenovacion
	FROM tblPersona P
	INNER JOIN tblPerfil PE ON P.perfil=PE.perfil
	WHERE nit = @nit
	--EXEC sp_buscar_persona_perfil '1152704820'
END
GO

CREATE PROCEDURE sp_buscar_perfil
AS
BEGIN
	SELECT perfil as Clave, descripcion as Dato
	FROM tblPerfil
	ORDER BY perfil DESC 
END
GO


--MÓDULO MATERIAL------------------------------------------------------
CREATE PROCEDURE sp_consultar_Mat_general
AS
BEGIN
	SELECT *
	FROM tblMaterial
	ORDER BY codigo ASC 
END
GO


CREATE PROCEDURE sp_consultar_Mat_puntual
@codigo VARCHAR(30)
AS
BEGIN
	SELECT *
	FROM tblMaterial
	WHERE codigo=@codigo
	ORDER BY codigo ASC 
END
GO


CREATE PROCEDURE sp_consultar_Mat_Estado
AS
BEGIN
	SELECT id as Clave, estado as Dato
	FROM tblMaterialEstado
	ORDER BY id ASC 
END
GO


CREATE PROCEDURE sp_consultar_Mat_Productor
AS
BEGIN
	SELECT id as Clave, productor as Dato
	FROM tblMaterialProductor
	ORDER BY id ASC 
END
GO


CREATE PROCEDURE sp_consultar_Mat_Autor 
AS
BEGIN
	SELECT id as Clave, autor as Dato
	FROM tblMaterialAutor
	ORDER BY id ASC 
END
GO


CREATE PROCEDURE sp_ingresar_Mat_autor
@nombre VARCHAR(200)
AS
BEGIN

IF NOT EXISTS (SELECT autor FROM tblMaterialAutor WHERE autor=@nombre)
BEGIN

	BEGIN TRANSACTION tx

		INSERT INTO tblMaterialAutor (autor)
		VALUES (UPPER(@nombre));

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

--EXEC sp_ingresar_Mat_autor 'Gabriel garcia marquez'
GO


CREATE PROCEDURE sp_ingresar_Mat_productor
@nombre VARCHAR(200)
AS
BEGIN

IF NOT EXISTS (SELECT productor FROM tblMaterialProductor WHERE productor=@nombre)
BEGIN

	BEGIN TRANSACTION tx

		INSERT INTO tblMaterialProductor (productor)
		VALUES (UPPER(@nombre));

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

--EXEC sp_ingresar_Mat_productor 'planeta'
GO


CREATE PROCEDURE sp_ingresar_Material
@codigo VARCHAR(30),
@nombre VARCHAR(200),
@edicion VARCHAR(10),
@cantidad INT,
@idEstado INT,
@idAutor INT,
@idProductor INT
AS
BEGIN

IF NOT EXISTS (SELECT codigo FROM tblMaterial WHERE codigo=@codigo)
BEGIN

	BEGIN TRANSACTION tx

		INSERT INTO tblMaterial(codigo,nombre,edicion,cantidad,idEstado,idAutor,idProductor)
		VALUES (UPPER(@codigo),UPPER(@nombre),UPPER(@edicion),@cantidad,@idEstado,@idAutor,@idProductor);

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

--EXEC sp_ingresar_Material 'CaS0001','CIEN año DE SOLedad','2.0',2,1,1,1
GO


CREATE PROCEDURE sp_actualizar_Material
@codigo VARCHAR(30),
@nombre VARCHAR(200),
@edicion VARCHAR(10),
@cantidad INT,
@idEstado INT,
@idAutor INT,
@idProductor INT
AS
BEGIN

IF NOT EXISTS (SELECT codigo FROM tblMaterial WHERE codigo=@codigo)
BEGIN

	BEGIN TRANSACTION tx

		UPDATE tblMaterial
		SET nombre=UPPER(@nombre),edicion=UPPER(@edicion),cantidad=@cantidad,idEstado=@idEstado,idAutor=@idAutor,idProductor=@idProductor
		WHERE codigo=@codigo;

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

--EXEC sp_actualizar_Material 'CaS0001','CIEN añoS DE SOLedad','3.0',2,1,1,1
GO


CREATE PROCEDURE sp_actualizar_autor
@id INT,
@nombre VARCHAR(200)
AS
BEGIN

IF EXISTS (SELECT id FROM tblMaterialAutor WHERE id=@id)
BEGIN

	BEGIN TRANSACTION tx

		UPDATE tblMaterialAutor
		SET autor=UPPER(@nombre)
		WHERE id=@id;

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

--EXEC sp_actualizar_autor 1,'GABO'
GO


CREATE PROCEDURE sp_actualizar_productor
@id INT,
@nombre VARCHAR(200)
AS
BEGIN

IF EXISTS (SELECT id FROM tblMaterialProductor WHERE id=@id)
BEGIN

	BEGIN TRANSACTION tx

		UPDATE tblMaterialProductor
		SET productor=UPPER(@nombre)
		WHERE id=@id;

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

--EXEC sp_actualizar_productor 1,'Editorial Planeta S.A.S'
GO


--MOODULO RESERVA------------------------------------------------------
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



--MODULO PRESTAMO ----------------------------

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
	INNER JOIN tblMaterialEstado ME on P.idEstadoMat=ME.id
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

	IF EXISTS (SELECT nit FROM tblSancion WHERE nit=@nit GROUP BY nit)
	BEGIN 
		SELECT 0 AS Rpta
		RETURN
	END

	BEGIN TRANSACTION tx

	INSERT INTO tblPrestamo(codigoMat,idEstadoMat,nit,idReserva,fechaPrestamo,fechaDevolucion,fechaRegistro)
	VALUES (@codigoMat,@idEstadoMat,@nit,@idReserva,GETDATE(),CONVERT(DATE,@FechaDevolucion,3),GETDATE())

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

SELECT @fechaReserva=CONVERT(DATE,DATEADD(DAY,PE.diasPrestamo,R.fechaReserva))
FROM tblReserva R
INNER JOIN tblPersona P ON R.nit=P.nit
INNER JOIN tblPerfil PE ON P.perfil=PE.id
WHERE R.codigoMat=@codigoMat AND R.idEstado=1

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
		SELECT 0 AS Rpta
		RETURN
	END
	COMMIT TRANSACTION tx
	SELECT 1 AS Rpta
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

