# Demostración ejecutable de los patrones

`./correr-patrones.sh` corre el sistema **actual** y produce las salidas en `salidas/`.

La idea es simple: **si un patrón está bien aplicado, se puede demostrar sin editar
código.** Las dos primeras comprobaciones son un `diff` entre dos ejecuciones del mismo
binario con distinta configuración; si las salidas fueran iguales, el script falla.

## p01 — Strategy sobre `IReglaPuntos`

Misma entrada (50 puntos a Ana), dos corridas:

```
FARMACIA_REGLA_PUNTOS=estandar  →  Cliente Ana acumuló 50 puntos   →  Ana - Puntos: 50
FARMACIA_REGLA_PUNTOS=doble     →  Cliente Ana acumuló 100 puntos  →  Ana - Puntos: 100
```

Lo que demuestra: `ServicioPuntos` no sabe qué regla le tocó, y `Cliente` no cambió.
La decisión vive en el punto de ensamblaje, `Program.cs:74-92`.

## p02 — Strategy sobre `IDescuento`

Misma entrada (2 Dolex a Ana, cédula 456), dos corridas:

```
sin convenios        →  Total: 10000  →  historial 10000  →  Ana - Puntos: 10
FARMACIA_CONVENIOS=1 →  Total: 9000   →  historial 9000   →  Ana - Puntos: 9
```

Lo que demuestra tres cosas de una vez:

1. El descuento se aplica en la venta, no en una sobrecarga sin llamar.
2. **La venta y el historial muestran el mismo número**, porque es el mismo valor
   guardado y no dos cálculos separados. Ese era el riesgo R-01.
3. Los puntos bajan de 10 a 9 solos: se calculan sobre el total real, así que las dos
   estrategias se componen sin conocerse.

## p03 — El registro de estrategias (OCP)

Da de alta tres cápsulas seguidas eligiendo `gel`, `polvo` y `xyz`. Lo que se ve:

- el menú ofrece `Relleno (gel/polvo): ` — **ese texto se arma con las claves del
  diccionario** de `Program.cs:130-135`, no está escrito en el menú;
- `xyz` responde `Relleno no registrado` y no crea nada.

Agregar un relleno nuevo cambia las dos cosas sin tocar el menú.

**Límite honesto:** una vez creado el producto, el relleno **no se ve en ninguna
pantalla** — ni el listado ni la búsqueda lo muestran. Así que esta corrida demuestra
el registro y la validación, no el efecto del relleno sobre el objeto. Eso hay que
mostrarlo en el código, no en la salida.

## p04 — State

Dos ventas que no llegan a término, y después el historial:

- **Amoxicilina x5** con 1 unidad en existencia: la etapa de inventario captura, llama
  `contexto.Fallar(...)` y la venta responde `Venta no registrada: ...`. En el sistema
  anterior esto terminaba la aplicación.
- **Cliente `zzz`**: el menú corta antes de abrir la venta, con `Cliente no encontrado`.

Y lo que cierra el argumento: **el historial queda vacío**. Una venta que terminó en
`Fallida` no dejó movimiento, y la existencia de Amoxicilina sigue en 1. El estado no es
decorativo: es lo que separa una venta a medias de una venta completa.
