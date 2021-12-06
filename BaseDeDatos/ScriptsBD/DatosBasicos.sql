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

EXEC sp_ingresar_Mat_autor 'Gabriel garcia marquez'
EXEC sp_ingresar_Mat_productor 'planeta'
EXEC sp_ingresar_Material '0','0','0',1,1,1,1
EXEC sp_ingresar_persona_usuario '0','0','0','0',1,'0','qAWsKidjruJEHDU78jd'
EXEC sp_ingresar_Reserva '0','0',2,'01/01/2021'




