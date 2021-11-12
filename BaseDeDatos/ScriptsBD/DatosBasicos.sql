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