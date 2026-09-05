# RETO 2 — ACTIVIDAD 5

## Análisis de Riesgos

Riesgos que hay que mitigar para ejecutar los cambios que supone el TO-BE sobre el código actual (`03-src/SolucionFarmacia/`). Cada riesgo está anclado a una decisión concreta del diseño —los cuatro patrones adoptados en la Actividad 2— o a un punto de dolor que el TO-BE todavía no cierra. No se incluyen riesgos genéricos de proyecto.

### Cómo se leen los números

Las escalas se definen para que la exposición no sea una opinión. Igual que en la Actividad 1, no se acepta "alto / medio / bajo" sin un criterio detrás.

**Probabilidad (P)**

| Valor | Significado |
|---|---|
| 5 | Ya está ocurriendo en el código de hoy |
| 4 | Va a ocurrir salvo que alguien haga algo explícito para impedirlo |
| 3 | Ocurre si se sigue el camino más cómodo al implementar |
| 2 | Exige una decisión equivocada deliberada |
| 1 | Necesitaría una causa externa al equipo |

**Impacto (I)**

| Valor | Significado |
|---|---|
| 5 | Se cobra o se registra un monto equivocado, o se rompe una condición que la Dirección puso como no negociable |
| 4 | Obliga a reabrir el diseño o a rehacer un entregable completo |
| 3 | Retrabajo acotado a un archivo o a una clase |
| 2 | Molestia operativa sin pérdida de información |
| 1 | Cosmético |

**Exposición (Exp) = P × I.** El registro está ordenado de mayor a menor exposición.

---

## Entregable 5.1 — Registro de riesgos

| ID | Riesgo (si ocurre X, entonces Y) | P | I | Exp | Qué hacen para evitarlo | Cómo se enteran de que está pasando |
|---|---|:-:|:-:|:-:|---|---|
| **R-01** | Si se habilita un convenio de descuento (SC-3) antes de que la venta calcule y conserve el importe, entonces el descuento vivirá en una sobrecarga que nadie invoca mientras la pantalla de movimientos sigue recalculando precio × cantidad a precio de hoy: el sistema mostrará un total distinto del que se cobró, y no habrá ningún registro de cuál fue el correcto. | 4 | 5 | **20** | Orden de trabajo obligatorio: primero P-04, después P-03. `RegistrarVenta` recibe la facturación y el cliente, calcula subtotal, descuento y total, y los guarda en el resultado de la venta; la pantalla de movimientos pasa a leer ese total guardado en vez de recalcularlo. Ningún convenio se cablea hasta que ese paso esté hecho. | Vender dos unidades a un cliente con convenio y comparar el total que imprime la venta contra el que muestra "Ver movimientos": si no coinciden, ya está pasando. Señal previa, sin ejecutar nada: la sobrecarga de tres argumentos de `ServicioFacturacion` sigue sin una sola llamada en todo el sistema. |
| **R-02** | Si se implementan las categorías nuevas (cosméticos y comestibles de SC-1) sobre el estado actual, donde el repositorio de productos fabrica por dentro su propia fábrica y sus propias estrategias concretas mientras el punto de ensamblaje crea una segunda instancia de la misma fábrica, entonces cada par repositorio/fábrica nuevo copiará ese molde y el punto de ensamblaje dejará de ser el único lugar donde se decide qué implementación se usa: vuelve P-01, con otro nombre y multiplicado por cada categoría. | 4 | 4 | **16** | Aplicar el arreglo de una línea que la matriz de la Actividad 4 ya declara —quitar el constructor sin parámetros, pasar desde el ensamblaje la instancia de fábrica que ya se crea, y resolver relleno y envase por defecto desde los registros que ya existen en el ensamblaje— **antes** de escribir la primera clase de la categoría nueva. Queda además como regla escrita en la guía de dónde tocar. | Buscar construcciones de clases concretas dentro de la biblioteca: cualquier `new` de una fábrica o de una estrategia que aparezca fuera del punto de ensamblaje es la señal. Hoy hay tres. |
| **R-03** | Si los diagramas del TO-BE se dan por terminados antes que el código y después el código cambia —al aplicar el arreglo de R-02, o al retirar la clase de solicitud de creación que quedó huérfana—, entonces se entrega un diseño que no corresponde al sistema, y la vista técnica deja de servir para lo único que existe: que alguien que no estuvo en las reuniones ubique dónde tocar. | 4 | 4 | **16** | Congelar el código un día antes de exportar los diagramas, y hacer una pasada de correspondencia clase por clase entre la capa TO-BE y los archivos reales de la biblioteca, con dos personas y lista en mano. El diagrama se exporta al final, no al principio. | Comparar el listado de archivos de la biblioteca contra la leyenda del diagrama: cualquier clase dibujada que no exista como archivo, o cualquier archivo nuevo que no esté dibujado, es la señal. |
| **R-04** | Si el canal de notificación por archivo falla al escribir (archivo tomado por otro proceso, permisos, disco lleno) justo cuando se registra una venta, entonces la falla sube por el evento hasta la venta y, como el menú no captura ninguna excepción, la aplicación termina: el stock ya quedó descontado y el operador pierde la sesión por una falla de un canal de aviso que no tiene nada que ver con la venta. | 3 | 5 | **15** | Cada suscriptor absorbe su propia falla: el cuerpo de la suscripción al evento se envuelve en su propio manejo de error en el punto de ensamblaje, de modo que un canal caído degrade el aviso y nunca la operación. La regla queda escrita: ningún observador puede lanzar hacia quien dispara el evento. | El archivo de notificaciones deja de crecer mientras "Ver movimientos" sigue listando ventas nuevas: si el número de líneas del archivo es menor que el número de movimientos de la sesión, un canal está fallando en silencio. La forma ruidosa de enterarse es la aplicación terminando con una traza de error de entrada/salida justo después de un aviso de movimiento registrado. |
| **R-05** | Si al demostrar que las estrategias son intercambiables se deja cableada la regla de puntos doble, o un descuento por defecto distinto de "sin descuento", entonces el sistema entregado produce salidas distintas a las del sistema anterior en un flujo que nadie autorizó: los mismos 50 puntos tecleados pasan a valer 100, y el comportamiento observable —que la Dirección puso como no negociable— cambia sin solicitud que lo respalde. | 3 | 5 | **15** | Las estrategias por defecto quedan fijadas por contrato: regla de puntos estándar y sin descuento. Las alternativas se demuestran cambiando una línea durante la sustentación, pero no se entregan cableadas. Antes de empaquetar se corre la comparación de salidas lado a lado del Entregable 4.2 sobre los casos del Reto 1. | La comparación byte a byte de las salidas antes y después deja de coincidir en algún caso. Verificación directa antes de entregar: en el punto de ensamblaje deben aparecer la regla estándar y ninguna instancia de descuento porcentual. |
| **R-06** | Si durante una operación normal se venden más unidades de las que hay en existencia, o se teclea una cantidad de puntos igual a cero, entonces la validación de la entidad lanza y nadie la captura: la aplicación termina en mitad de la operación. Es el comportamiento de hoy, y el TO-BE agrega caminos nuevos que también lanzan al mover el cálculo del importe dentro de la venta. | 3 | 4 | **12** | No se captura la excepción en el menú, porque hacerlo cambiaría el comportamiento observable que está congelado: la mitigación es de alcance, no de código. Los caminos que abortan se declaran como deuda conocida en la vista técnica, y los casos que se ejecutan en vivo se fijan y se ensayan completos, de principio a fin, con sus entradas escritas. Al cerrar P-04, la validación de cantidad y disponibilidad se hace antes de tocar las existencias, para no dejar una venta a medio aplicar. | Cualquier ensayo previo que termine con una excepción no controlada en la consola. En operación, un movimiento cuyo stock ya bajó sin que exista el movimiento correspondiente en el historial. |
| **R-07** | Si se da de alta un convenio con el porcentaje mal expresado —un 15 % tecleado como 15 en vez de 0,15—, entonces el descuento calculado supera al precio y el total facturado sale negativo: ni la clase de descuento porcentual valida su porcentaje al construirse, ni el contrato de descuento declara que el descuento no puede exceder el precio. | 3 | 4 | **12** | Validar el porcentaje en el constructor (intervalo cerrado de 0 a 1) y declarar en el contrato el límite que hoy no está escrito: el descuento nunca supera el precio. Ambas cosas antes de que exista el primer convenio real, no después. | Un total menor o igual a cero en el historial de movimientos. En verificación: un caso que construya un descuento con porcentaje 1,5 y no falle es la señal de que la validación sigue ausente. |
| **R-08** | Si al cerrar el cálculo del importe se decide introducir una capa de aplicación, un contenedor de inyección de dependencias o una reescritura del flujo de venta, entonces el trabajo deja de ser lo que se encargó —cómo colaboran los objetos del back que ya existe— y se convierte en un cambio de estilo arquitectónico que la Dirección rechazó de entrada. | 2 | 5 | **10** | El cambio de P-04 se acota por escrito: la venta recibe la facturación y el cliente y devuelve un resultado con subtotal, descuento y total. Nada más. El ensamblaje sigue siendo manual, en el mismo archivo de siempre, sin biblioteca externa. Toda propuesta que agregue una capa o una dependencia nueva se rechaza por regla y no por criterio. | Aparece una referencia a paquete nueva en cualquier archivo de proyecto, o un proyecto nuevo en la solución. Ambas cosas se revisan antes de cada entrega. |

**Ocho riesgos registrados** (el mínimo exigido es tres). Exposición total 106; los tres primeros concentran 52.

---

## Anclaje en el código

Evidencia por riesgo, para poder defenderlo con el archivo abierto durante la sustentación. Las líneas corresponden al estado actual de `RetoFarmacia-Patrones/03-src/SolucionFarmacia/`.

| ID | Evidencia |
|---|---|
| R-01 | `Servicios/ServicioFacturacion.cs:22-32` (sobrecarga con `Cliente`, cero llamadas en todo el sistema) · `Servicios/ServicioVenta.cs:31-53` (`RegistrarVenta` descuenta stock y registra el movimiento sin calcular importe) · `AppFarmaciaConsola/Program.cs:375` (el historial recalcula el total en vez de leerlo) · `Clases/Cliente.cs:30` (única asignación de `Descuento`, siempre `SinDescuento`) |
| R-02 | `Repositorios/RepositorioProductos.cs:17-20` (constructor sin parámetros que hace `new FabricaMedicamentos()`) · `RepositorioProductos.cs:67` y `:75` (`new RellenoGel()`, `new EnvaseVidrio()` dentro del repositorio) · `Program.cs:40-41` (segunda instancia de la misma fábrica) · registros de estrategias ya disponibles en `Program.cs:93-105`. Es la celda **DIP = Roto** de la matriz de la Actividad 4 |
| R-03 | `DiagramasUML/TO-BE/` frente a `BibFarmacia/**/*.cs` · `Clases/ProductoRequest.cs` (huérfana: sin una sola referencia desde que la fábrica pasó a parámetros explícitos) |
| R-04 | `Program.cs:154-161` (segunda suscripción a `MovimientoRegistrado`) · `Servicios/NotificacionArchivo.cs:18-23` (`File.AppendAllText`, sin manejo de error) · `Servicios/ServicioMovimiento.cs:26-33` (`Disparar` invoca a los suscriptores dentro de `RegistrarMovimiento`) · `Program.cs` no contiene ningún `try` |
| R-05 | `Program.cs:71` (`new ReglaPuntosEstandar()`, la línea que se cambia para demostrar) · `Program.cs:88` (canal de notificación, que no imprime en consola precisamente para no alterar la salida) · evidencia de salidas comparadas del Entregable 4.2 |
| R-06 | `Clases/Producto.cs:77-82` (`DescontarStock` lanza) · `Clases/Cliente.cs:33-38` (`AcumularPuntos` exige puntos positivos) · `Program.cs:644` y `Program.cs:684` (llamadas sin captura) · asimetría conocida: el alta de servicios sí captura (`ServicioObjetoServicio.cs:29-42`) y el alta de productos no, porque la fábrica se invoca fuera del `try` |
| R-07 | `Clases/DescuentoPorcentual.cs:15-18` (constructor sin validación) · `Interfaces/IDescuento.cs:11` (el contrato no declara ningún límite sobre el resultado). Es la salvedad de LSP ya declarada en la matriz de la Actividad 4 |
| R-08 | `SolucionFarmacia.sln` (dos proyectos) · `BibFarmacia.csproj` y `AppFarmaciaConsola.csproj` (sin ninguna referencia a paquete externo) · `Program.cs:35-36` (comentario que declara el ensamblaje manual) |

---

## Qué se hace primero

El orden no lo decide la gravedad sino la dependencia entre riesgos:

1. **R-01** manda sobre el plan de trabajo: cierra el orden P-04 → P-03 y bloquea el alta de convenios hasta que la venta conserve el importe. Es el único riesgo que, si se ignora, produce un cobro equivocado.
2. **R-02** tiene que resolverse antes de escribir la primera categoría nueva, no después: es el arreglo de una línea que impide que el punto de dolor P-01 se reproduzca en cada categoría.
3. **R-04** y **R-05** se cierran con dos revisiones baratas y repetibles (manejo de error en cada suscriptor; comparación de salidas antes de empaquetar) que conviene dejar como paso fijo de la entrega.
4. **R-03**, **R-06**, **R-07** y **R-08** se vigilan con las señales de la tabla; ninguno exige trabajo por adelantado, pero los cuatro se revisan antes de dar el TO-BE por cerrado.

## Riesgos que se evaluaron y no entraron al registro

Se descartan por no tener anclaje en este diseño, no por ser improbables:

- **Una fachada que absorbe lógica de negocio** y **un punto de acceso global de instancia única**: no se adoptó ninguno de los dos patrones, así que el riesgo no existe en este TO-BE.
- **Una envoltura que cambia el contrato de lo que envuelve**: Decorator se evaluó dos veces en la Actividad 2 (P-01 y P-02) y se descartó las dos, precisamente para no incurrir en él.
- **Pérdida de datos por la migración**: el sistema no escribe nunca en los archivos de datos —la carga es de solo lectura— y el formato de los cuatro archivos está congelado, así que no hay migración que pueda fallar.

---

## Guion de la sustentación (minuto 14 a 17)

Tres riesgos, cada uno con su señal de alerta, en este orden:

1. **R-01**, el que cuesta dinero: *"si habilitamos convenios antes de arreglar el cálculo del importe, cobramos un número y mostramos otro"*. Señal: el total de la venta y el del historial no coinciden.
2. **R-02**, el que devuelve el problema que vinimos a resolver: *"cada categoría nueva copia el molde y volvemos a tener la decisión repartida"*. Señal: aparece un `new` de una clase concreta dentro de la biblioteca.
3. **R-04**, el que tumba la operación por algo que no es la operación: *"un aviso que no se pudo escribir se lleva la venta por delante"*. Señal: el archivo de notificaciones deja de crecer mientras siguen entrando movimientos.
