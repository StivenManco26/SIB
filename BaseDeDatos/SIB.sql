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
fechaReserva DATE NOT NULL,
fechaRegistro DATE NOT NULL)
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
fechaPrestamo DATE NOT NULL,
fechaDevolucion DATE NOT NULL,
fechaRegistro DATE NOT NULL)
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


ALTER TABLE tblPersona ADD FOREIGN KEY (perfil) REFERENCES tblPerfil (perfil)
ALTER TABLE tblUsuario ADD FOREIGN KEY (nit) REFERENCES tblPersona (nit)
ALTER TABLE tblReserva ADD FOREIGN KEY (nit) REFERENCES tblPersona (nit)
ALTER TABLE tblPrestamo ADD FOREIGN KEY (nit) REFERENCES tblPersona (nit)
ALTER TABLE tblDevolucion ADD FOREIGN KEY (nit) REFERENCES tblPersona (nit)
ALTER TABLE tblMaterial ADD FOREIGN KEY (idEstado) REFERENCES tblMaterialEstado (id)
ALTER TABLE tblMaterial ADD FOREIGN KEY (idAutor) REFERENCES tblMaterialAutor (id)
ALTER TABLE tblMaterial ADD FOREIGN KEY (idProductor) REFERENCES tblMaterialProductor (id)
ALTER TABLE tblReserva ADD FOREIGN KEY (idEstado) REFERENCES tblReservaEstado (id)
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
		SELECT 0 AS Rpta, 0 AS nit, 0 AS nombre
	
	--EXEC sp_login 'jcardenas', 'ContraTodo'
END
GO


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