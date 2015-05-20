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


