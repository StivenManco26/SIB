--MÓDULO MATERIAL------------------------------------------------------

CREATE PROCEDURE sp_consultar_Mat_general
AS
BEGIN
	SELECT M.CODIGO,M.NOMBRE,M.EDICION,E.ESTADO,A.AUTOR,P.PRODUCTOR
	FROM tblMaterial M
	INNER JOIN tblMaterialEstado E ON E.id=M.idEstado
	INNER JOIN tblMaterialAutor A ON A.id=M.idAutor
	INNER JOIN tblMaterialProductor P ON P.id=M.idProductor
	WHERE M.codigo<>'0'
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

CREATE PROCEDURE sp_consultar_Mat_Autor_general
AS
BEGIN
	SELECT id CODIGO,AUTOR
	FROM tblMaterialAutor
	ORDER BY id ASC 
END
GO

CREATE PROCEDURE sp_consultar_autor_puntual
@id INT
AS
BEGIN
	SELECT id CODIGO,AUTOR
	FROM tblMaterialAutor
	WHERE id=@id
END
GO

CREATE PROCEDURE sp_consultar_Mat_Productor_puntual
@id INT
AS
BEGIN
	SELECT id CODIGO,productor
	FROM tblMaterialProductor
	WHERE id=@id
END
GO

CREATE PROCEDURE sp_consultar_Mat_Productor_general
AS
BEGIN
	SELECT id CODIGO,productor EDITORIAL
	FROM tblMaterialProductor
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

IF EXISTS (SELECT codigo FROM tblMaterial WHERE codigo=@codigo)
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