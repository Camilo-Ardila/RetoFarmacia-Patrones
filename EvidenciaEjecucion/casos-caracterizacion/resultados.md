# Casos de caracterización — diseño y resultados

Cada caso es un archivo de entrada (stdin scripteado) ejecutado contra el sistema
**anterior** (worktree del commit `ffaf533`) y contra el sistema **actual**. Las salidas
completas quedan en `salidas-anterior/` y `salidas-actual/`.

Antes de comparar se normaliza lo que varía entre corridas sin ser comportamiento: la
fecha y hora de los movimientos (`DateTime.Now`), las rutas absolutas y las líneas de
traza de una excepción.

**Ejecutado el 2026-09-06** sobre .NET 10.0.10, con `./correr-casos.sh --fijar`.
Línea base: commit `ffaf533`.

**Resultado global: 14/14 invariantes idénticos y 3/3 diferencias contenidas a la
pantalla de venta.** Ninguna regresión.

## Grupo 1 — Invariantes

La salida **debe ser idéntica**. Una diferencia acá es una regresión.

| # | Caso | Qué cubre | Resultado |
|---|---|---|---|
| c01 | login-invalido | Credenciales incorrectas → "Acceso denegado" y fin | ✅ Idéntico |
| c02 | login-salir | Carga de los cuatro archivos, login, alertas de arranque, menú, salir | ✅ Idéntico |
| c03 | ver-productos | Opción 1. Cubre que `FormateadorProducto` produce la misma línea que el antiguo `MostrarInformacion` | ✅ Idéntico |
| c04 | ver-servicios | Opción 2. Cubre que la carga silenciosa de `servicios.txt` no se volvió ruidosa | ✅ Idéntico |
| c05 | ver-clientes | Opción 3 | ✅ Idéntico |
| c06 | ver-movimientos-vacio | Opción 4 sin ventas previas | ✅ Idéntico |
| c07 | ver-alertas | Opción 5: re-verificación de stock mínimo y vencimientos | ✅ Idéntico |
| c08 | buscar-existente | Opción 8 con "dolex": búsqueda parcial insensible a mayúsculas | ✅ Idéntico |
| c09 | buscar-inexistente | Opción 8 con "xyz" → "Producto no encontrado" | ✅ Idéntico |
| c10 | **venta-inexistente** | Opción 9 con un producto que no existe. **El caso más importante del grupo**: prueba que el prompt nuevo de cliente quedó *dentro* del `if`, así que esta rama no lo ve y sigue idéntica | ✅ Idéntico |
| c11 | acumular-puntos | Opción 10: 50 puntos a "ana", evento de puntos, y opción 3 para ver el saldo | ✅ Idéntico |
| c12 | agregar-servicio | Opción 7: alta de un servicio nuevo y listado | ✅ Idéntico |
| c13 | agregar-producto | Opción 6: alta de cápsula con relleno de polvo elegido del registro | ✅ Idéntico |
| c14 | opcion-invalida | Entrada 99 → "Opción inválida" y el menú continúa | ✅ Idéntico |

## Grupo 2 — Cambio autorizado

La salida **debe diferir**. Se verifica que la diferencia sea exactamente la registrada
en `diferencias/*.esperado`. Si crece, el script falla.

Las entradas son distintas entre sistemas porque el actual pide un dato más:
`entradas-autorizados/anterior/` y `entradas-autorizados/actual/`.

| # | Caso | Diferencia esperada | Resultado |
|---|---|---|---|
| a01 | venta-producto | 5 líneas, **un solo bloque**. Pide `Nombre cliente:`, imprime `Cliente Ana acumuló 14 puntos` y cambia `Venta registrada` → `Venta registrada. Total: 14000` | ✅ Contenida |
| a02 | venta-servicio | 5 líneas, un solo bloque. Lo mismo sobre un `Servicio`: `Total: 8000` y 8 puntos. La etapa de inventario lo saltó sin caso especial | ✅ Contenida |
| a03 | **venta-sin-stock** | 56 líneas, un solo bloque, y son casi todas *salida que el anterior nunca llegó a producir*: muere con `Unhandled exception. System.ArgumentOutOfRangeException`, el actual responde `Venta no registrada: ...` y sigue hasta el final | ✅ Contenida |

## Lo que no se compara

- Las rutas nuevas del sistema actual sin equivalente en el anterior: venta a un cliente
  que no existe, y la venta con convenio. No hay contra qué compararlas, así que se
  demuestran en `../patrones/` y en `../demo/`.
- Las alertas de vencimiento dependen de `DateTime.Now`. Ambos sistemas corren en la
  misma sesión, así que el conjunto de alertas es el mismo; si se re-ejecuta en otra
  fecha, ambos cambian igual y siguen coincidiendo.


---

## Las dos evidencias que más pesan

**1. El historial y el listado coinciden en a01.** El `diff` tiene **un solo bloque**, y
está en la pantalla de venta. Todo lo posterior es idéntico:

```
anterior: Ibuprofeno  8  7000   |  FECHA - Venta - Producto - Ibuprofeno x2 - Total: 14000
actual:   Ibuprofeno  8  7000   |  FECHA - Venta - Producto - Ibuprofeno x2 - Total: 14000
```

El número es el mismo, pero **no se obtuvo igual**: el anterior lo recalculaba a precio
del momento (`precio × cantidad`) y el actual lee el valor guardado en el `Movimiento`.
Que coincidan sin convenio es lo que prueba que cerrar P-04 no movió ninguna cifra —
y es exactamente la señal con la que el riesgo **R-01** se declaraba cerrado.

**2. La existencia sobrevive a la venta fallida en a03.** Después del `Venta no
registrada`, el listado del sistema actual sigue mostrando `Amoxicilina 1 12000`: la
venta terminó en estado Fallida **sin consumir stock ni dejar movimiento**. El sistema
anterior ni siquiera llegó a listar, porque ya había abortado.

## Nota sobre `a03` y el "core dumped"

La corrida imprime `Aborted (core dumped)` al ejecutar el sistema anterior en este caso.
**Es el resultado esperado, no un fallo del script**: el sistema anterior no captura la
excepción y el proceso muere. El aviso lo emite el shell, y desde esta versión se
descarta para que la salida se lea limpia; la traza de la aplicación sí queda registrada
en `salidas-anterior/a03-venta-sin-stock.txt`.

---

## Corridas registradas — 2026-09-06, .NET 10.0.10

Las tres suites, en el orden en que se corren. Estas son las salidas de consola tal como
salieron; las salidas completas de cada caso están en los directorios correspondientes.

### `casos-caracterizacion/correr-casos.sh --fijar`

```
Compilando sistema anterior (ffaf533)...
Compilando sistema actual...

--- Invariantes (deben coincidir) ---
OK       c01-login-invalido
OK       c02-login-salir
OK       c03-ver-productos
OK       c04-ver-servicios
OK       c05-ver-clientes
OK       c06-ver-movimientos-vacio
OK       c07-ver-alertas
OK       c08-buscar-existente
OK       c09-buscar-inexistente
OK       c10-venta-inexistente
OK       c11-acumular-puntos
OK       c12-agregar-servicio
OK       c13-agregar-producto
OK       c14-opcion-invalida

--- Cambio autorizado (deben diferir, y solo en lo declarado) ---
FIJADO   a01-venta-producto (5 líneas de diferencia)
FIJADO   a02-venta-servicio (5 líneas de diferencia)
FIJADO   a03-venta-sin-stock (56 líneas de diferencia)

RESULTADO: sin regresiones; el cambio autorizado está contenido.
```

En `a03` el sistema anterior muere de verdad, y el shell lo anuncia con
`Aborted (core dumped)`. **Es el resultado esperado, no un fallo del script**; la versión
actual del script descarta ese aviso para que la salida se lea limpia, y la traza de la
aplicación queda igual en `salidas-anterior/a03-venta-sin-stock.txt`.

`--fijar` solo se usa la primera vez. De aquí en adelante se corre sin él, y cualquier
cambio que altere la venta más allá de lo pactado sale marcado como `CRECIÓ`.

### `patrones/correr-patrones.sh`

```
Compilando...

=== Strategy — IReglaPuntos (misma entrada, dos reglas) ===
  < Nombre cliente: Puntos: Cliente Ana acumuló 50 puntos
  > Nombre cliente: Puntos: Cliente Ana acumuló 100 puntos
  < Ana - Puntos: 50
  > Ana - Puntos: 100

=== Strategy — IDescuento (Ana, cédula 456, convenio del 10 %) ===
  < Tipo de venta: Nombre producto: Cantidad: Nombre cliente: Cliente Ana acumuló 10 puntos
  > Tipo de venta: Nombre producto: Cantidad: Nombre cliente: Cliente Ana acumuló 9 puntos
  < Venta registrada. Total: 10000
  > Venta registrada. Total: 9000
  < FECHA - Venta - Producto - Dolex x2 - Total: 10000
  > FECHA - Venta - Producto - Dolex x2 - Total: 9000
  < Ana - Puntos: 10
  > Ana - Puntos: 9

=== Factory + registros y State: salidas capturadas en salidas/ ===
  p03-registro-rellenos.txt  — dos cápsulas creadas por la misma fábrica, un relleno cada una
  p04-estados-venta.txt      — venta que termina en Fallida y venta sin cliente

RESULTADO: las estrategias son intercambiables sin tocar el código.
```

**Es el mismo binario en las dos corridas de cada bloque.** No se editó ni una línea entre
una y otra: solo cambia la variable de entorno que lee el punto de ensamblaje. Si las dos
salidas fueran iguales, el script falla — esa es la comprobación.

El bloque de `IDescuento` demuestra tres cosas a la vez: el descuento se aplica en la
venta; **la venta y el historial muestran el mismo número** (10000/10000 y 9000/9000), que
es la señal de R-01; y los puntos bajan de 10 a 9 solos, porque se calculan sobre el total
real. Las dos estrategias se componen sin conocerse.

`p04` cierra el argumento del State: su historial queda **vacío**. Ni la venta que falló
por existencia ni la cortada por cliente inexistente dejaron movimiento.

### `demo/correr-demo.sh`

Misma entrada, dos configuraciones. La diferencia es la demostración del convenio:

```
137c137
< Tipo de venta: Nombre producto: Cantidad: Nombre cliente: Cliente Ana acumuló 17 puntos
---
> Tipo de venta: Nombre producto: Cantidad: Nombre cliente: Cliente Ana acumuló 15 puntos
140c140
< Venta registrada. Total: 17000
---
> Venta registrada. Total: 15300
160c160
< Tipo de venta: Nombre servicio: Cantidad: Nombre cliente: Cliente Ana acumuló 15 puntos
---
> Tipo de venta: Nombre servicio: Cantidad: Nombre cliente: Cliente Ana acumuló 13 puntos
163c163
< Venta registrada. Total: 15000
---
> Venta registrada. Total: 13500
243c243
< Ana - Puntos: 82
---
> Ana - Puntos: 78
270,271c270,271
< FECHA - Venta - Producto - ParacetamolForte x2 - Total: 17000
< FECHA - Venta - Servicio - Vacunacion x1 - Total: 15000
---
> FECHA - Venta - Producto - ParacetamolForte x2 - Total: 15300
> FECHA - Venta - Servicio - Vacunacion x1 - Total: 13500
```

Producto (8500 × 2 = 17000 → 15300) y **servicio** (15000 → 13500) reciben el mismo 10 %:
la facturación no distingue entre uno y otro, habla con `IFacturable`. El saldo de puntos
cierra la cadena: 82 sin convenio, 78 con él.

## Defecto encontrado por esta suite

La primera corrida con convenio imprimía `Total: 9000.00` mientras la corrida sin convenio
imprimía `Total: 10000`. No era un redondeo mal hecho: en .NET el tipo `decimal` conserva
la escala, y al multiplicar se suman las escalas — `10000m × 0.10m` da `1000.00m`, así que
la resta arrastraba dos decimales. **Solo se veía con convenio**, porque sin él el
descuento es `0m` de escala 0.

Corregido con el formato `0.##` en los tres puntos donde se imprime un total
(`ServicioVenta.cs:48`, `Program.cs:206` y `Program.cs:454`). Los totales sin convenio no
cambian, así que las diferencias autorizadas ya congeladas siguen siendo válidas.

Vale la pena decirlo en la sustentación: **el defecto lo destapó la propia suite**, en la
única ruta que ninguna comparación contra el sistema anterior podía cubrir, porque el
anterior no sabía aplicar descuentos.
