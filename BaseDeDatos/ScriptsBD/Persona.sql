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
	SELECT P.nit,P.nombre,P.correo,P.celular,P.perfil
	FROM tblPersona P
	--INNER JOIN tblPerfil PE ON P.perfil=PE.perfil
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