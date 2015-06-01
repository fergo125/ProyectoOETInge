--- XYZ) Update para dejar la base de datos consistente. Toma las existencias de cada producto en TODAS LAS BODEGAS y las suma
---dicha suma se convertira en la existencia total del producto (SALDO) en el catalogo global de productos
update INV_PRODUCTOS
set    SALDO = ( select SUM (B.SALDO)
                              from INV_BODEGA_PRODUCTOS B
                              GROUP BY B.INV_PRODUCTOS
                              HAVING inv_productos.inv_productos = b.inv_productos );

--- Ejmeplo del funcionamiento de la consulta anterior
---- El producto PITAN130012015110927803059 tiene una existencia total en todas las bodegas de 2500
select INV_PRODUCTOS, SUM (B.SALDO)
from INV_BODEGA_PRODUCTOS B
GROUP BY B.INV_PRODUCTOS
HAVING inv_productos = 'PITAN130012015110927803059';

---Al correr el UPDATE XYZ) se debe observar que el 2500 obtenido en el select anterior debe ser la existencia global en el catalogo global

SELECT INV_PRODUCTOS, SALDO
FROM inv_productos
where inv_productos = 'PITAN130012015110927803059';


--Trigger que actualiza la existencia(saldo) de cada producto en catalogo global cuando se modifica la existencia del mismo producto en el catalogo local 
                              
create or replace trigger "GRUPO02"."INV_PRODUCTOS_TRIG_SALDO" AFTER UPDATE ON INV_BODEGA_PRODUCTOS 

  FOR EACH ROW 
  DECLARE
      var_diferencia number(15,2);
      var_actual number(15,2);
  BEGIN
      -- Se obtiene la diferencia entre la existencia nueva y vieja del catalogo local
      var_diferencia := :new.saldo - :old.saldo;
      
      SELECT saldo INTO var_actual
      FROM inv_productos p
      WHERE p.inv_productos = :old.inv_productos; 
      
      UPDATE inv_productos 
      SET saldo = var_actual + var_diferencia
      where inv_productos = :old.inv_productos;
END;

--PRUEBA1: Se modifica localmente la cantidad de un producto (que solo esta en una bodega) -Actualmente tiene 1500-
UPDATE INV_BODEGA_PRODUCTOS
SET SALDO = 1500
WHERE inv_productos = 'PITAN130012015110927803059';

-- Se ve el cambio LOCALMENTE
SELECT *
FROM INV_BODEGA_PRODUCTOS
where  inv_productos = 'PITAN130012015110927803059'
-- Se ve el cambio GLOBALMENTE
SELECT INV_PRODUCTOS, SALDO
FROM inv_productos
where inv_productos = 'PITAN130012015110927803059';



SELECT *
FROM INV_BODEGA_PRODUCTOS
ORDER BY inv_productos

--PRUEBA2: Se modifica localmente la cantidad de un producto (que esta en varias bodega)

---PRODUCTO QUE TIENE ESTA EN VARIAS BODEGAS tiene 1000 en una y 28 en otra
select INV_PRODUCTOS, SUM (B.SALDO)
from INV_BODEGA_PRODUCTOS B
GROUP BY B.INV_PRODUCTOS
HAVING inv_productos = 'PITAN130012015092529441001';


--Cambio la existencia en UNA de las bodegas (pasar de 1000 a 500)
UPDATE INV_BODEGA_PRODUCTOS
SET SALDO = 500
WHERE inv_bodega_productos = ' 13052015102054872002';
---Se checkea el cambio GLOBALMENTE
SELECT INV_PRODUCTOS, SALDO
FROM inv_productos
where inv_productos = 'PITAN130012015092529441001';
---Se checkea el cambio LOCALMENTE
SELECT INV_PRODUCTOS, SALDO
FROM inv_bodega_productos
where inv_productos = 'PITAN130012015092529441001';


-- Creación de Tabla para modulo de Entradas
CREATE TABLE CAT_ENTRADAS
(
  CAT_BODEGA      VARCHAR2(30)    PRIMARY KEY,
  FACTURA         VARCHAR2(30),   -- No se sabe
  SEG_USUARIO     VARCHAR2(30),
  SEG_BODEGA      VARCHAR2(30),
  FECHA           DATE
);

CREATE TABLE CAT_ENTRADAS_PRODUCTOS
(
  CAT_ENTRADAS_PRODUCTOS    VARCHAR2(30)    PRIMARY KEY,
  CAT_ENTRADAS              VARCHAR2(30),
  CAT_PRODUCTOS             VARCHAR2(30),
  CANTIDAD                  NUMBER(10),
  PRECIO_UNITARIO           NUMBER(10)
);

--Asignación de códigos internos únicos para cada perfil para que el sistema
--sea menos hard-coded y se puedan modificar los nombres de los perfiles sin
--afectar la funcionalidad. No se elimina lo hard-coded por completo, los
--códigos de cada perfil seguirán en el código fuente.
--Se agregarán de esta manera: 1,2,3,4.
ALTER TABLE SEG_PERFIL
  ADD
    (
      CODIGO  NUMBER(15,2)
    );
UPDATE SEG_PERFIL SET CODIGO=1 WHERE NOMBRE='Administrador global';
UPDATE SEG_PERFIL SET CODIGO=2 WHERE NOMBRE='Administrador local';
UPDATE SEG_PERFIL SET CODIGO=3 WHERE NOMBRE='Supervisor';
UPDATE SEG_PERFIL SET CODIGO=4 WHERE NOMBRE='Vendedor';


---Creación de tabla para Ajustes
create table  AJUSTES(
	id_ajustes              varchar2(30),
	cat_tipo_movimiento     varchar2(30),
	fecha                   date,
	usuario_bodega          varchar2(30),
	idBodega                varchar2(30),
	notas                   varchar2(30)
);
 
create table DETALLES_AJUSTES (
	id_ajustes                  varchar2(30),
	inv_bodega_productos        varchar2(30),
	cambio                      number(15,2)
);

---Creacion de tablas para facturas


CREATE TABLE REGISTRO_FACTURAS_VENTA(
consecutivo int NOT NULL PRIMARY KEY,
fecha DATE,
bodega varchar(30),
estacion varchar2(30),
compañia varchar2(30),
actividad varchar2(30),
vendedor varchar2(40),
cliente varchar2(40),
tipoMoneda varchar2(10),
metodoPago varchar2(30),
montoTotal float
);

CREATE TABLE REGISTRO_DETALLES_FACTURAS(
idFactura int REFERENCES REGISTRO_FACTURAS_VENTA(consecutivo),
idProducto varchar2(20),
cantidad int,
precioUnitarioColones float, 
precioUnitarioDolares float,
descuento int
);


SELECT  *
FROM    INV_PRODUCTOS
WHERE   CODIGO = 'CRO00186' AND NOMBRE = 'Servilletas (paquetes)' AND ESTADO = 1

SELECT  ESTADO
FROM    INV_BODEGA_PRODUCTOS
WHERE   INV_PRODUCTOS = 'PITAN130012015150520764106'


--Nuevos permisos para los perfiles, según las interfaces del sprint 2
INSERT INTO SEG_PERMISOS VALUES('30052015180000000020', 'CYCLO105062012115352292015', 'Entradas de inventario',   '111111');
INSERT INTO SEG_PERMISOS VALUES('30052015180000000021', 'CYCLO105062012115352292015', 'Traslados de inventario',  '111111');
INSERT INTO SEG_PERMISOS VALUES('30052015180000000022', 'CYCLO105062012115352292015', 'Ajustes de inventario',    '111111');
INSERT INTO SEG_PERMISOS VALUES('30052015180000000023', 'CYCLO105062012115352292015', 'Facturacion',              '111111');
INSERT INTO SEG_PERMISOS VALUES('30052015180000000024', 'CYCLO114062012084948203080', 'Entradas de inventario',   '000111');
INSERT INTO SEG_PERMISOS VALUES('30052015180000000025', 'CYCLO114062012084948203080', 'Traslados de inventario',  '000111');
INSERT INTO SEG_PERMISOS VALUES('30052015180000000026', 'CYCLO114062012084948203080', 'Ajustes de inventario',    '000011');
INSERT INTO SEG_PERMISOS VALUES('30052015180000000027', 'CYCLO114062012084948203080', 'Facturacion',              '000111');
INSERT INTO SEG_PERMISOS VALUES('30052015180000000028', 'CYCLO112122012150416678002', 'Entradas de inventario',   '000011');
INSERT INTO SEG_PERMISOS VALUES('30052015180000000029', 'CYCLO112122012150416678002', 'Traslados de inventario',  '000011');
INSERT INTO SEG_PERMISOS VALUES('30052015180000000030', 'CYCLO112122012150416678002', 'Ajustes de inventario',    '000011');
INSERT INTO SEG_PERMISOS VALUES('30052015180000000031', 'CYCLO112122012150416678002', 'Facturacion',              '000111');
INSERT INTO SEG_PERMISOS VALUES('30052015180000000032', 'CYCLO112122012150835261069', 'Entradas de inventario',   '000000');
INSERT INTO SEG_PERMISOS VALUES('30052015180000000033', 'CYCLO112122012150835261069', 'Traslados de inventario',  '000000');
INSERT INTO SEG_PERMISOS VALUES('30052015180000000034', 'CYCLO112122012150835261069', 'Ajustes de inventario',    '000000');
INSERT INTO SEG_PERMISOS VALUES('30052015180000000035', 'CYCLO112122012150835261069', 'Facturacion',              '000011');
--CYCLO105062012115352292015	Administrador global
--CYCLO114062012084948203080	Administrador local
--CYCLO112122012150416678002	Supervisor
--CYCLO112122012150835261069	Vendedor










-----PARTE DE TRASLADOS

create table  TRASLADOS(
	id_traslado               varchar2(30),
	fecha                     date,
	usuario_bodega            varchar2(30),
	idBodegaOrigen            varchar2(30),
	idBodegaDestino           varchar2(30),
  estado                    number(1),
	notas                     varchar2(30)
);
 
create table DETALLES_TRASLADO (
	id_traslado                   varchar2(30),
	inv_bodega_productosDestino   varchar2(30),
  inv_bodega_productosOrigen    varchar2(30),
	traslado                      number(15,2)
);


ALTER TABLE INV_BODEGA_PRODUCTOS ADD SaldoCongelado number(15,2);

Update INV_BODEGA_PRODUCTOS
SET SaldoCongelado = 0

INSERT INTO TRASLADOS (ID_TRASLADO, FECHA, USUARIO_BODEGA, IDBODEGAORIGEN, IDBODEGADESTINO, ESTADO, NOTAS) 
VALUES ('1111', TO_DATE('2003/07/09', 'yyyy/mm/dd') , '4', 'PITAN129012015101713605001', 'CYCLO128122012112950388004', 0, 'PrimerConsultar');
DELETE FROM TRASLADOS WHERE ID_TRASLADO = '1111';


select *
from inv_bodega_productos 
order by inv_productos


--
--Actualización de SEG_USUARIO_BODEGA
ALTER TABLE SEG_USUARIO_BODEGA ADD ESTACION VARCHAR2(30);
UPDATE SEG_USUARIO_BODEGA S SET ESTACION = (SELECT E.ID FROM RESERVAS.ESTACION E WHERE E.ID = (SELECT B.ESTACION FROM CAT_BODEGA B WHERE B.CAT_BODEGA = S.CAT_BODEGA))
--DELETE FROM SEG_USUARIO_BODEGA WHERE ESTACION = '' --Todas las bodegas deberían estar asociadas a una estación


---Productos que estan en ambas bodegas PITAN130012015105015574038 PITAN130012015104745304036



SELECT T.ID_TRASLADO, T.NOTAS, T.FECHA, U.NOMBRE 
FROM Inventarios.TRASLADOS T, Inventarios.SEG_USUARIO U
WHERE T.USUARIO_BODEGA = U.SEG_USUARIO  
AND ( T.IDBODEGAORIGEN = 'PITAN129012015101713605001' OR T.IDBODEGADESTINO = 'PITAN129012015101713605001') 
ORDER BY T.FECHA DESC

commit

SELECT *
FROM TRASLADOS;



