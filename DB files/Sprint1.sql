-- Sprint1.sql
-- Cambios efectuados en la base de datos durante el sprint 1
-- Realizados por Leonardo Villalobos Arias


-- Modulo Productos
-- Agregar nuevos atributos a producto
ALTER TABLE INV_PRODUCTOS
  ADD
  (
    INTENCION VARCHAR2(35),
    PRECIO_C  NUMBER(15,2),
    PRECIO_D  NUMBER(15,2)
  );


-- Modulo Bodega
-- Agregar nuevos atributos a bodega
ALTER TABLE CAT_BODEGA
  ADD
    CAT_INTENCIONUSO NUMBER(2);

CREATE TABLE CAT_INTENCIONUSO
(
  CAT_INTENCIONUSO  NUMBER(2) PRIMARY KEY,
  NOMBRE            VARCHAR(35)
);

INSERT ALL
  INTO CAT_INTENCIONUSO VALUES (0, 'Punto de Venta')
  INTO CAT_INTENCIONUSO VALUES (1, 'Cocina')
  INTO CAT_INTENCIONUSO VALUES (2, 'Almacenamiento')
SELECT * FROM dual;

SELECT * FROM CAT_INTENCIONUSO;

-- Agregar estado a INV_BODEGA_PRODUCTOS
ALTER TABLE INV_BODEGA_PRODUCTOS
  ADD
    ESTADO NUMBER(2);
    

-- Modulo Seguridad
-- Seguridad de base de datos
CREATE TABLE SEG_PERMISOS
(
  SEG_PERMISO VARCHAR2(30) PRIMARY KEY,
  SEG_PERFIL  VARCHAR2(30),
  INTERFAZ    VARCHAR2(45),
  PERMISOS    CHAR(6)      -- Formato XXXXXX, X = 0, 1
  -- Auxiliar, Auxiliar, Desactivar, Modificar, Insertar, Consultar
  -- Permiso en 0 significa que no tiene ese permiso, en 1 significa que si lo tiene
  -- Por ejemplo, si el perfil Supervisor tiene para la interfaz de Bodegas los permisos 001001
    -- entonces el Supervisor puede consultar que bodegas existen y desactivar bodegas
	-- pero no puede insertar nuevas bodegas y modificar los datos de las ya existentes
  -- Se dejan 2 bits auxiliares en caso de ser necesarios
);

-- Listado de permisos por perfil e interfaz de las interfaces del primer sprint
INSERT ALL
-- Administrador global
  INTO SEG_PERMISOS VALUES('27042015180000000000', 'CYCLO105062012115352292015','Catalogo general de productos', '111111')
  INTO SEG_PERMISOS VALUES('27042015180000000001', 'CYCLO105062012115352292015','Categorias de productos', '111111')
  INTO SEG_PERMISOS VALUES('27042015180000000002', 'CYCLO105062012115352292015','Catalogos de productos en bodegas', '111111')
  INTO SEG_PERMISOS VALUES('27042015180000000003', 'CYCLO105062012115352292015','Gestion de bodegas', '111111')
  INTO SEG_PERMISOS VALUES('27042015180000000004', 'CYCLO105062012115352292015','Gestion de actividades', '111111')
-- Administrador local
  INTO SEG_PERMISOS VALUES('27042015180000000005', 'CYCLO114062012084948203080','Catalogo general de productos', '000111')
  INTO SEG_PERMISOS VALUES('27042015180000000006', 'CYCLO114062012084948203080','Categorias de productos', '000001')
  INTO SEG_PERMISOS VALUES('27042015180000000007', 'CYCLO114062012084948203080','Catalogos de productos en bodegas', '001111')
  INTO SEG_PERMISOS VALUES('27042015180000000008', 'CYCLO114062012084948203080','Gestion de bodegas', '000001')
  INTO SEG_PERMISOS VALUES('27042015180000000009', 'CYCLO114062012084948203080','Gestion de actividades', '001111')
-- Supervisor
  INTO SEG_PERMISOS VALUES('27042015180000000010', 'CYCLO112122012150416678002','Catalogo general de productos', '000000')
  INTO SEG_PERMISOS VALUES('27042015180000000011', 'CYCLO112122012150416678002','Categorias de productos', '000000')
  INTO SEG_PERMISOS VALUES('27042015180000000012', 'CYCLO112122012150416678002','Catalogos de productos en bodegas', '000001')
  INTO SEG_PERMISOS VALUES('27042015180000000013', 'CYCLO112122012150416678002','Gestion de bodegas', '000000')
  INTO SEG_PERMISOS VALUES('27042015180000000014', 'CYCLO112122012150416678002','Gestion de actividades', '000001')
-- Vendedor
  INTO SEG_PERMISOS VALUES('27042015180000000015', 'CYCLO112122012150835261069','Catalogo general de productos', '000000')
  INTO SEG_PERMISOS VALUES('27042015180000000016', 'CYCLO112122012150835261069','Categorias de productos', '000000')
  INTO SEG_PERMISOS VALUES('27042015180000000017', 'CYCLO112122012150835261069','Catalogos de productos en bodegas', '000001')
  INTO SEG_PERMISOS VALUES('27042015180000000018', 'CYCLO112122012150835261069','Gestion de bodegas', '000000')
  INTO SEG_PERMISOS VALUES('27042015180000000019', 'CYCLO112122012150835261069','Gestion de actividades', '000001')
SELECT * FROM dual;

SELECT * FROM SEG_PERMISOS;

--Intenciones de uso iniciales para las bodegas existentes
UPDATE CAT_BODEGA SET CAT_INTENCIONUSO=2

-- Usuarios de prueba
-- Estos solo durante el periodo de pruebas, no introducir en version final
INSERT INTO SEG_USUARIO VALUES('1', 'oscar', 'pass', '28-APR-15', 'Administrador global de prueba', 'SII014548761600.7018216447', '01', 'Oscar Esquivel', 1);
INSERT INTO SEG_USUARIO VALUES('2', 'leo', 'pass', '28-APR-15', 'Administrador local de prueba', 'SII014548761600.7018216447', '01', 'Leonardo Villalobos', 1);
INSERT INTO SEG_USUARIO VALUES('3', 'carlos', 'pass', '28-APR-15', 'Supervisor de prueba', 'SII014548761600.7018216447', '01', 'Carlos Sanabria', 1);
INSERT INTO SEG_USUARIO VALUES('4', 'fer', 'pass', '28-APR-15', 'Vendedor de prueba', 'SII014548761600.7018216447', '01', 'Fernando Mata', 1);

INSERT INTO SEG_PERFIL_USUARIO VALUES('1', '1', 'CYCLO105062012115352292015');
INSERT INTO SEG_PERFIL_USUARIO VALUES('2', '2', 'CYCLO114062012084948203080');
INSERT INTO SEG_PERFIL_USUARIO VALUES('3', '3', 'CYCLO112122012150416678002');
INSERT INTO SEG_PERFIL_USUARIO VALUES('4', '4', 'CYCLO112122012150835261069');

-- Cambiar nombres de permisos
UPDATE SEG_PERFIL SET NOMBRE = 'Administrador global' WHERE SEG_PERFIL = 'CYCLO105062012115352292015';
UPDATE SEG_PERFIL SET NOMBRE = 'Administrador local' WHERE SEG_PERFIL = 'CYCLO114062012084948203080';
UPDATE SEG_PERFIL SET NOMBRE = 'Supervisor' WHERE SEG_PERFIL = 'CYCLO112122012150416678002';
UPDATE SEG_PERFIL SET NOMBRE = 'Vendedor' WHERE SEG_PERFIL = 'CYCLO112122012150835261069';