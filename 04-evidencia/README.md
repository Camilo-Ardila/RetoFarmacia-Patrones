# 04 — Evidencia de ejecución

Tres suites, con tres propósitos distintos. Todas se corren desde este directorio y
todas necesitan `dotnet` en el PATH.

| Carpeta | Qué prueba | Cómo se corre |
|---|---|---|
| `casos-caracterizacion/` | Que el trabajo de patrones **no rompió nada**, y que lo que sí cambió es exactamente lo autorizado | `./casos-caracterizacion/correr-casos.sh` |
| `patrones/` | Que las estrategias son **realmente intercambiables**: misma entrada, distinta salida, sin editar código | `./patrones/correr-patrones.sh` |
| `demo/` | Un recorrido guiado del sistema, para la sustentación | `./demo/correr-demo.sh` |

## Por qué esta vez no es "todo idéntico byte a byte"

En el Reto 1 la comparación era simple: el rediseño no debía cambiar nada, así que
cualquier diferencia era una regresión.

En el Reto 2 eso ya no aplica. La solicitud de convenios exigió que la venta calculara
y conservara el importe (P-04), y eso **cambió la pantalla de venta a propósito**. El
enunciado no prohíbe cambiar el comportamiento observable: prohíbe cambiarlo **sin
solicitud que lo respalde**. Por eso los casos están partidos en dos grupos con
criterios opuestos:

- **Invariantes** — la salida debe ser idéntica. Cualquier diferencia es una regresión.
- **Cambio autorizado** — la salida *debe* diferir. Lo que se verifica es que la
  diferencia siga siendo exactamente la registrada, y no haya crecido.

Ese segundo criterio es el que hace auditable la frase "cambio autorizado y acotado":
si alguien altera la salida de la venta más allá de lo pactado, el script lo marca
como `CRECIÓ`.

## La línea base

El "antes" es el propio repositorio en el commit **`ffaf533`**, el estado inmediatamente
anterior al trabajo de patrones —el que describen el análisis de riesgos y la primera
versión de las vistas: venta sin importe, repositorio que fabrica, ningún convenio.

`correr-casos.sh` lo trae solo, con `git worktree add` sobre un directorio temporal.
No hay que copiar código ni mantener una segunda solución, y no hay riesgo de colisión
de ensamblados con `BibliotecaAnterior/`. Para usar otro commit como base:

```bash
FARMACIA_BASELINE=<sha> ./casos-caracterizacion/correr-casos.sh
```

Al terminar, `./casos-caracterizacion/correr-casos.sh --limpiar` borra el worktree.

## Las dos variables de entorno

El punto de ensamblaje lee dos variables para elegir estrategia. **Sin ellas, el sistema
se comporta exactamente como se entrega**, así que ninguna corrida normal se ve afectada.

| Variable | Valores | Por defecto | Qué cambia |
|---|---|---|---|
| `FARMACIA_REGLA_PUNTOS` | `estandar`, `doble` | `estandar` | La regla de acumulación de puntos (`IReglaPuntos`) |
| `FARMACIA_CONVENIOS` | `1` o sin definir | sin definir | Asigna el convenio del 10 % a la cédula 456 (`IDescuento`) |

Existen por una razón de evidencia, no de producto: **una demostración que exige editar
el código y recompilar entre corridas no es reproducible por quien califica.** Con esto,
la prueba de que Strategy funciona es un `diff` entre dos ejecuciones del mismo binario.

Que los convenios estén apagados por defecto no es un descuido: encenderlos cambia lo
que se le cobra a una persona, y eso espera la lista de entidades autorizada por el
negocio.

## Estado

| Suite | Estado |
|---|---|
| `casos-caracterizacion/` | **Corrida el 2026-09-06 sobre .NET 10.0.10.** 14/14 invariantes idénticos, 3/3 diferencias contenidas. Detalle en `casos-caracterizacion/resultados.md` |
| `patrones/` | Pendiente de correr |
| `demo/` | Pendiente de correr |

La primera corrida de `correr-casos.sh` ya se hizo con `--fijar`, así que las diferencias
autorizadas quedaron congeladas en `diferencias/*.esperado`. De aquí en adelante basta:

```bash
./casos-caracterizacion/correr-casos.sh
```

y cualquier cambio que altere la salida de la venta más allá de lo pactado saldrá
marcado como `CRECIÓ`.

## Sobre el framework

Los proyectos apuntan a **net10.0**. Se retargetearon desde net8.0 porque la imagen trae
solo el runtime 10: el SDK compila net8.0 sin problema, pero no puede ejecutarlo.

La línea base `ffaf533` es anterior a ese cambio y sus proyectos piden net8.0, así que
`correr-casos.sh` le ajusta el framework al worktree después de crearlo. El worktree es
desechable y vive fuera del repositorio: **el commit sigue diciendo net8.0 y no se
reescribe historia**.

Los tres scripts localizan el binario con `bin/Debug/net*/`, así que no vuelven a
romperse si el framework cambia otra vez.
