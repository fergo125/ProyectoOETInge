-- DROP TABLE HISTORIAL_COSTOS 
CREATE TABLE HISTORIAL_COSTOS
(
	INV_BODEGA_PRODUCTOS	  	  VARCHAR2(30),
	FECHA				                TIMESTAMP,
	CANTIDAD			              NUMBER,
	COSTO_UNITARIO_COL	      	NUMBER(15,2),
	COSTO_UNITARIO_DOL	      	NUMBER(15,2),
  
	PRIMARY KEY ( INV_BODEGA_PRODUCTOS, FECHA )
);


-- Procedimientos almacenados


-- Actualizar el costo promedio de un producto
CREATE OR REPLACE PROCEDURE actualizar_costo ( id_producto IN HISTORIAL_COSTOS.INV_BODEGA_PRODUCTOS%TYPE )
AS
  costo_nuevo_col   NUMBER;
  costo_nuevo_dol   NUMBER;
BEGIN
  SELECT ( SELECT SUM(CANTIDAD * COSTO_UNITARIO_COL)
      FROM HISTORIAL_COSTOS
      WHERE INV_BODEGA_PRODUCTOS = id_producto )
      / ( SELECT SUM(CANTIDAD)
            FROM HISTORIAL_COSTOS
            WHERE INV_BODEGA_PRODUCTOS = id_producto )
      INTO costo_nuevo_col FROM DUAL;
  SELECT ( SELECT SUM(CANTIDAD * COSTO_UNITARIO_DOL)
      FROM HISTORIAL_COSTOS
      WHERE INV_BODEGA_PRODUCTOS = id_producto )
      / ( SELECT SUM(CANTIDAD)
            FROM HISTORIAL_COSTOS
            WHERE INV_BODEGA_PRODUCTOS = id_producto )
      INTO costo_nuevo_dol FROM DUAL;
  DBMS_OUTPUT.PUT_LINE( 'costo nuevo: colones-' || costo_nuevo_col || ' dolares-' || costo_nuevo_dol );
  UPDATE INV_BODEGA_PRODUCTOS
    SET COSTO_COLONES = costo_nuevo_col,
      COSTO_DOLARES = costo_nuevo_dol
    WHERE INV_BODEGA_PRODUCTOS = id_producto;
END;

-- Elimina los mas antiguos N costos de un producto, usado para salidas
CREATE OR REPLACE PROCEDURE quitar_historial ( id_producto IN HISTORIAL_COSTOS.INV_BODEGA_PRODUCTOS%TYPE, cantidad_restar IN HISTORIAL_COSTOS.CANTIDAD%TYPE )
AS
	fecha_prod		        TIMESTAMP;
	cantidad_actual			  NUMBER;
  cantidad_faltante     NUMBER;
BEGIN
	cantidad_faltante := cantidad_restar;
	WHILE cantidad_faltante > 0
	LOOP
    SELECT *
    INTO fecha_prod, cantidad_actual
      FROM (
        SELECT FECHA, CANTIDAD
          FROM HISTORIAL_COSTOS
          WHERE INV_BODEGA_PRODUCTOS = id_producto
          ORDER BY FECHA ASC
      )
      WHERE ROWNUM <= 1;
		DBMS_OUTPUT.PUT_LINE( fecha_prod || ' ' || cantidad_actual);
		IF cantidad_faltante >= cantidad_actual THEN
			DBMS_OUTPUT.PUT_LINE( 'Borrar tupla' );
			DELETE FROM HISTORIAL_COSTOS
				WHERE INV_BODEGA_PRODUCTOS = id_producto AND FECHA = fecha_prod;
			cantidad_faltante := cantidad_faltante - cantidad_actual;
		ELSE
			UPDATE HISTORIAL_COSTOS
				SET CANTIDAD = cantidad_actual - cantidad_faltante
				WHERE INV_BODEGA_PRODUCTOS = id_producto AND FECHA = fecha_prod;
			DBMS_OUTPUT.PUT_LINE( 'Actualizar Tupla' );
		cantidad_faltante := 0;
		END IF;
		DBMS_OUTPUT.PUT_LINE( cantidad_faltante );
	END LOOP;
  actualizar_costo( id_producto );
END;

-- Insertar nuevas tuplas a tabla historial
CREATE OR REPLACE PROCEDURE insertar_historial ( id_producto IN HISTORIAL_COSTOS.INV_BODEGA_PRODUCTOS%TYPE
                                                , cantidad_entrada IN HISTORIAL_COSTOS.CANTIDAD%TYPE
                                                , costo_unit_col IN HISTORIAL_COSTOS.COSTO_UNITARIO_COL%TYPE  )
AS
  costo_unit_dol         HISTORIAL_COSTOS.COSTO_UNITARIO_DOL%TYPE;
BEGIN
  SELECT costo_unit_col / ( SELECT * FROM (SELECT COMPRA FROM reservas.TIPOCAMBIO ORDER BY DEL DESC) WHERE ROWNUM <= 1 ) INTO costo_unit_dol FROM DUAL;
  INSERT INTO HISTORIAL_COSTOS
    VALUES( id_producto, (SELECT SYSDATE FROM DUAL), cantidad_entrada, costo_unit_col, costo_unit_dol );
  actualizar_costo( id_producto );
END;

-- Insertar nuevas tuplas a tabla historial, usando costo promedio actual
CREATE OR REPLACE PROCEDURE insertar_historial_promedio ( id_producto IN HISTORIAL_COSTOS.INV_BODEGA_PRODUCTOS%TYPE
                                                          , cantidad_entrada IN HISTORIAL_COSTOS.CANTIDAD%TYPE  )
AS
  costo_unit_col         HISTORIAL_COSTOS.COSTO_UNITARIO_COL%TYPE;
  costo_unit_dol         HISTORIAL_COSTOS.COSTO_UNITARIO_DOL%TYPE;
BEGIN
  SELECT COSTO_COLONES, COSTO_DOLARES
    INTO costo_unit_col, costo_unit_dol
    FROM INV_BODEGA_PRODUCTOS
    WHERE INV_BODEGA_PRODUCTOS = id_producto;
  INSERT INTO HISTORIAL_COSTOS
    VALUES( id_producto, (SELECT SYSDATE FROM DUAL), cantidad_entrada, costo_unit_col, costo_unit_dol );
  actualizar_costo( id_producto );
END;


-- Trasladar productos de una bodega a otra
CREATE OR REPLACE PROCEDURE trasladar_historial ( id_producto_origen IN HISTORIAL_COSTOS.INV_BODEGA_PRODUCTOS%TYPE
                                                , id_producto_destino IN HISTORIAL_COSTOS.INV_BODEGA_PRODUCTOS%TYPE
                                                , cantidad_traslado IN HISTORIAL_COSTOS.CANTIDAD%TYPE )
AS
	fecha_prod		        TIMESTAMP;
	cantidad_actual			  NUMBER;
  cantidad_faltante     NUMBER;
  costo_colones         HISTORIAL_COSTOS.COSTO_UNITARIO_COL%TYPE;
  costo_dolares         HISTORIAL_COSTOS.COSTO_UNITARIO_DOL%TYPE;
  contador              NUMBER := 0;
BEGIN
	cantidad_faltante := cantidad_traslado;
	WHILE cantidad_faltante > 0
	LOOP
    SELECT *
    INTO fecha_prod, cantidad_actual
      FROM (
        SELECT FECHA, CANTIDAD
          FROM HISTORIAL_COSTOS
          WHERE INV_BODEGA_PRODUCTOS = id_producto_origen
          ORDER BY FECHA ASC
      )
      WHERE ROWNUM <= 1;
		DBMS_OUTPUT.PUT_LINE( fecha_prod || ' ' || cantidad_actual);
		IF cantidad_faltante >= cantidad_actual THEN
			DBMS_OUTPUT.PUT_LINE( 'Mover tupla' );
			UPDATE HISTORIAL_COSTOS
        SET INV_BODEGA_PRODUCTOS = id_producto_destino,
            FECHA = (SELECT SYSDATE + contador/(24*60*60) FROM DUAL)
				WHERE INV_BODEGA_PRODUCTOS = id_producto_origen AND FECHA = fecha_prod;
			cantidad_faltante := cantidad_faltante - cantidad_actual;
		ELSE
      SELECT COSTO_UNITARIO_COL, COSTO_UNITARIO_DOL
        INTO costo_colones, costo_dolares
        FROM HISTORIAL_COSTOS
        WHERE INV_BODEGA_PRODUCTOS = id_producto_origen AND FECHA = fecha_prod;
			UPDATE HISTORIAL_COSTOS
				SET CANTIDAD = cantidad_actual - cantidad_faltante
				WHERE INV_BODEGA_PRODUCTOS = id_producto_origen AND FECHA = fecha_prod;
      INSERT INTO HISTORIAL_COSTOS
        VALUES( id_producto_destino, (SELECT SYSDATE + contador/(24*60*60) FROM DUAL), cantidad_faltante, costo_colones, costo_dolares );
			DBMS_OUTPUT.PUT_LINE( 'Actualizar Tupla' );
		cantidad_faltante := 0;
		END IF;
		DBMS_OUTPUT.PUT_LINE( cantidad_faltante );
    contador := contador + 1;
	END LOOP;
  actualizar_costo( id_producto_origen );
  actualizar_costo( id_producto_destino );
END;

-- Inicializar
DELETE FROM HISTORIAL_COSTOS;
UPDATE INV_BODEGA_PRODUCTOS
  SET COSTO_COLONES = 0,
      COSTO_DOLARES = 0,
      SALDO = 0;

-- Pruebas
INSERT INTO HISTORIAL_COSTOS VALUES ('PITAN102022015142627451180', TO_TIMESTAMP('29/07/2015 16:58:00', 'DD/MM/YYYY HH24:MI:SS'), 5, 1300, 2.46 );
INSERT INTO HISTORIAL_COSTOS VALUES ('PITAN102022015142627451180', TO_TIMESTAMP('28/07/2015 12:45:00', 'DD/MM/YYYY HH24:MI:SS'), 10, 1200, 2.27  );
INSERT INTO HISTORIAL_COSTOS VALUES ('PITAN102022015142627451180', TO_TIMESTAMP('27/07/2015 13:15:00', 'DD/MM/YYYY HH24:MI:SS'), 10, 1000, 1.89 );
INSERT INTO HISTORIAL_COSTOS VALUES ('PITAN102022015142627467181', TO_TIMESTAMP('30/06/2015 09:35:00', 'DD/MM/YYYY HH24:MI:SS'), 6, 4000, 7.58 );

SELECT * FROM HISTORIAL_COSTOS;
call quitar_historial( 'PITAN102022015142627451180', 5 );
SELECT * FROM HISTORIAL_COSTOS;
call insertar_historial( 'PITAN102022015142627451180', 5, 1400 );


call actualizar_costo( 'PITAN102022015142627451180' );
SELECT COSTO_COLONES, COSTO_DOLARES FROM INV_BODEGA_PRODUCTOS WHERE INV_BODEGA_PRODUCTOS = 'PITAN102022015142627451180';

--Nuevos permisos para los perfiles, según las interfaces del sprint 3
INSERT INTO SEG_PERMISOS VALUES('30052015180000000036', 'CYCLO105062012115352292015', 'Seguridad', '111111');
INSERT INTO SEG_PERMISOS VALUES('30052015180000000037', 'CYCLO114062012084948203080', 'Seguridad', '000111');
INSERT INTO SEG_PERMISOS VALUES('30052015180000000038', 'CYCLO112122012150416678002', 'Seguridad', '000000');
INSERT INTO SEG_PERMISOS VALUES('30052015180000000039', 'CYCLO112122012150835261069', 'Seguridad', '000000');
INSERT INTO SEG_PERMISOS VALUES('30052015180000000040', 'CYCLO105062012115352292015', 'Reportes', '111111');
INSERT INTO SEG_PERMISOS VALUES('30052015180000000041', 'CYCLO114062012084948203080', 'Reportes', '000111');
INSERT INTO SEG_PERMISOS VALUES('30052015180000000042', 'CYCLO112122012150416678002', 'Reportes', '000011');
INSERT INTO SEG_PERMISOS VALUES('30052015180000000043', 'CYCLO112122012150835261069', 'Reportes', '000000');
--CYCLO105062012115352292015	Administrador global
--CYCLO114062012084948203080	Administrador local
--CYCLO112122012150416678002	Supervisor
--CYCLO112122012150835261069	Vendedor
