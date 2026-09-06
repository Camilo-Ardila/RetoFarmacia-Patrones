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

> **Vigencia.** El registro de la sección 5.1 se levantó **antes** de ejecutar la implementación (commit `123b525`) y **no se reescribe hacia atrás**: sus valores de P e I documentan lo que se sabía en ese momento, y fue ese orden el que mandó sobre el plan de trabajo. Lo que ocurrió después se registra en la sección **Seguimiento al cierre de la implementación**, junto con la exposición residual y los riesgos nuevos.

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

**Ocho riesgos registrados** en el análisis inicial (el mínimo exigido es tres). Exposición total 106; los tres primeros concentran 52. El seguimiento agrega dos más, R-09 y R-10, surgidos de la implementación.

---

## Anclaje en el código, en el momento del análisis

Evidencia por riesgo tal como se levantó. Las líneas corresponden al estado del código en el commit `123b525`, **no al actual**: varias de ellas ya no existen, precisamente porque las mitigaciones se ejecutaron. El anclaje vigente está en la columna de evidencia del seguimiento.

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

## Seguimiento al cierre de la implementación

Estado de cada riesgo tras ejecutar los cambios (commits `d7e7d12`, `84bf4d5`, `e994403`). Las líneas de la columna de evidencia corresponden al **código actual**.

| ID | Estado | Qué pasó | Evidencia vigente | Exp |
|---|---|---|---|:-:|
| **R-01** | **Cerrado** | Se respetó el orden que el riesgo imponía: primero P-04, después P-03. La venta calcula subtotal, descuento y total, el movimiento los conserva y el historial lee el total guardado en vez de recalcularlo. La sobrecarga que no tenía llamadas hoy se invoca en toda venta. | `Servicios/ServicioFacturacion.cs:22-53` · `Clases/Movimiento.cs:59-61` · `Program.cs:410` | 20 → **5** |
| **R-02** | **Cerrado** | El repositorio de productos dejó de fabricar: devuelve un registro plano y quien decide qué clase nace es la fábrica, que recibe sus valores por defecto del ensamblaje. Ya no hay una sola construcción de clase concreta fuera del punto de ensamblaje. | `Repositorios/RepositorioProductos.cs` (sin un solo `new` de dominio) · `Servicios/ServicioProducto.cs:98` · `Program.cs:42-45` | 16 → **4** |
| **R-03** | **Se materializó, y se corrigió** | Ocurrió exactamente como estaba descrito: el código avanzó después de escritas las dos vistas y los documentos quedaron describiendo un sistema que ya no existía. La revisión de correspondencia lo detectó y las dos vistas se rehicieron completas. **Los diagramas `.dia` del TO-BE siguen sin revisar contra el código actual**, y el código incorporó un patrón nuevo que no está dibujado. | `DiagramasUML/TO-BE/` frente a `BibFarmacia/**/*.cs` · `Clases/EstadosVenta/` y `Eventos/EventoVenta.cs`, sin representación en el diagrama | **16** |
| **R-04** | **Cerrado** | Cada suscriptor absorbe su propia falla, y los servicios de la cadena convierten el error en un estado de venta fallida en vez de dejarlo subir. Un canal de avisos caído degrada el aviso, nunca la operación. | `Program.cs:182-195` (suscripción con su propio `try/catch`) · `Servicios/ServicioPuntos.cs:42-46` · `Servicios/ServicioFacturacion.cs:49-52` · `Servicios/ServicioInventario.cs:39-42` | 15 → **5** |
| **R-05** | **Vigente, y ampliado** | Sigue vigente para la regla de puntos, que se entrega en su versión estándar. Pero apareció una arista nueva: la salida de la pantalla de venta **sí cambió**, con autorización de la solicitud de convenios, así que la comparación de salidas dejó de ser "todo idéntico". El riesgo ya no es solo dejar algo cableado por error, sino **que una diferencia no autorizada se confunda con una autorizada**. | `Program.cs:81` (regla estándar) · las cuatro diferencias autorizadas están enumeradas una por una en la regla 1 de la vista técnica | **15** |
| **R-06** | **Parcialmente cerrado** | El camino caro se blindó: vender más de lo que hay ya no termina la aplicación, responde y sigue, y la existencia no queda a medio descontar porque la validación ocurre antes. Siguen abiertos los otros dos —acumular cero puntos a mano y alta de producto con precio 0— y el menú sigue sin validar la entrada numérica. | Cerrado: `Servicios/ServicioInventario.cs:31-42` · Abierto: `Program.cs:741-744` y `Program.cs:432-565`; `Program.cs` sigue sin un solo `try` | 12 → **9** |
| **R-07** | **Cerrado** | El porcentaje se valida al construirse, en el intervalo cerrado de 0 a 1, y además la facturación rechaza cualquier descuento negativo o mayor que el subtotal, venga de donde venga. Un 15 tecleado en vez de 0,15 falla al crearse, no al cobrarse. | `Clases/DescuentoPorcentual.cs:17-21` · `Servicios/ServicioFacturacion.cs:35-39` | 12 → **4** |
| **R-08** | **Cerrado por hecho consumado** | El cambio se acotó como estaba escrito. No se agregó ninguna capa, ningún contenedor y ninguna referencia a paquete externo; la solución sigue con dos proyectos y el ensamblaje sigue siendo manual y en el mismo archivo. | `SolucionFarmacia.sln` (dos proyectos) · los dos `.csproj` sin referencias externas · `Program.cs:39-40` | 10 → **5** |

### Riesgos nuevos, surgidos de la implementación

| ID | Riesgo (si ocurre X, entonces Y) | P | I | Exp | Qué hacen para evitarlo | Cómo se enteran de que está pasando |
|---|---|:-:|:-:|:-:|---|---|
| **R-09** | Si se asigna un convenio a una cédula equivocada, o se cablea uno que el negocio no autorizó, entonces un cliente paga menos de lo que le corresponde —o menos que otro igual a él— y no hay forma de notarlo desde la operación: el sistema se comporta con normalidad y el descuento queda correctamente calculado y guardado. Es el riesgo que **reemplaza** a R-01: ya no se cobra un número y se muestra otro, ahora se cobra bien un descuento que no debía existir. | 3 | 5 | **15** | La asignación vive en un solo sitio del ensamblaje y por cédula, así que la lista completa se lee de un vistazo. Se compara contra la lista de entidades autorizada por el negocio antes de cada entrega, y esa lista es un requisito previo a encender el primer convenio. Mientras no llegue, se entrega sin ningún convenio asignado. | Leer el registro de convenios del punto de ensamblaje y contrastarlo, línea por línea, con la lista autorizada. Cualquier cédula que esté en el código y no en la lista, o con distinto porcentaje, es la señal. |
| **R-10** | Si al agregar una etapa nueva a la venta —reservar, anular— se la coloca después del descuento de existencias y esa etapa falla, entonces la venta termina marcada como fallida pero la existencia ya quedó descontada: el sistema reporta que no vendió y el inventario dice que sí. | 2 | 4 | **8** | Decidir, **antes** de agregar la primera etapa nueva, si el descuento de existencias se mueve al final de la cadena o si el paso a estado fallido devuelve lo descontado. Está declarado en la vista técnica y la fila de la guía de dónde tocar lo exige como condición previa. | Una venta que responde "no registrada" y deja la existencia del producto por debajo de la que tenía antes de intentarla. Se verifica listando productos antes y después de una venta fallida. |

**Exposición residual total: 86.** Los tres primeros de la lista original sumaban 52 y hoy suman 25; el peso se movió a R-03, R-05 y R-09, que son los tres que quedan por encima de 10.

---

## Qué se hace primero

El orden no lo decide la gravedad sino la dependencia entre riesgos. **Este apartado se reordenó al cierre**; el orden original se conserva abajo porque es lo que efectivamente guió el trabajo.

**Lo que queda por hacer, en orden:**

1. **R-03** es hoy el de mayor exposición y el único que ya se materializó una vez. Los diagramas del TO-BE no incluyen el patrón de estados ni la cadena de eventos. Hay que rehacerlos y volver a pasar la revisión de correspondencia **antes** de dar la entrega por cerrada.
2. **R-09** bloquea el último paso de la solicitud de convenios: no se enciende ninguno hasta tener la lista autorizada por el negocio, y la comparación contra esa lista queda como paso fijo de la entrega.
3. **R-05** cambió de forma y hay que actualizar el procedimiento: la comparación de salidas se corre contra la lista explícita de las cuatro diferencias autorizadas, no contra "todo idéntico".
4. **R-06** y **R-10** se vigilan con sus señales. Ninguno exige trabajo por adelantado, pero R-10 hay que resolverlo antes de agregar cualquier etapa nueva a la venta.
5. **R-01, R-02, R-04, R-07 y R-08** quedan cerrados. Se revisan una vez más antes de entregar, con la evidencia de la tabla de seguimiento a la vista.

**El orden original, que sí se siguió:**

1. **R-01** mandó sobre el plan de trabajo: fijó el orden P-04 → P-03 y bloqueó el alta de convenios hasta que la venta conservara el importe. Fue el único riesgo que, si se ignoraba, producía un cobro equivocado. Se ejecutó en ese orden.
2. **R-02** se resolvió antes de escribir la primera categoría nueva, como exigía: el punto de dolor P-01 no llegó a reproducirse.
3. **R-04** y **R-05** se cerraron con dos revisiones baratas y repetibles que quedaron como paso fijo de la entrega.
4. **R-03**, **R-06**, **R-07** y **R-08** se vigilaron con las señales de la tabla.

## Riesgos que se evaluaron y no entraron al registro

Se descartan por no tener anclaje en este diseño, no por ser improbables:

- **Una fachada que absorbe lógica de negocio** y **un punto de acceso global de instancia única**: no se adoptó ninguno de los dos patrones, así que el riesgo no existe en este TO-BE.
- **Una envoltura que cambia el contrato de lo que envuelve**: Decorator se evaluó dos veces en la Actividad 2 (P-01 y P-02) y se descartó las dos, precisamente para no incurrir en él.
- **Pérdida de datos por la migración**: el sistema no escribe nunca en los archivos de datos —la carga es de solo lectura— y el formato de los cuatro archivos está congelado, así que no hay migración que pueda fallar.

---

## Guion de la sustentación (minuto 14 a 17)

Tres riesgos, en este orden, contando qué pasó con cada uno. El registro se usó, no se archivó:

1. **R-01, el que cuesta dinero — cerrado.** *"Si habilitábamos convenios antes de arreglar el cálculo del importe, cobrábamos un número y mostrábamos otro."* Era el de mayor exposición y por eso mandó sobre el orden de trabajo: primero el cobro, después el descuento. Se muestra el código: la venta calcula y guarda, el historial lee lo guardado. La señal —que los dos totales no coincidan— ya no puede dispararse porque **es el mismo número**, no dos cálculos.
2. **R-09, el que lo reemplaza — vigente.** Cerrar R-01 no eliminó el riesgo de cobrar mal, lo cambió de forma: *"ahora el descuento se calcula bien; lo que puede estar mal es a quién se lo damos"*. Y es más silencioso, porque el sistema se comporta con normalidad. Señal: leer el registro de convenios del ensamblaje contra la lista autorizada por el negocio. Por eso no se entrega ningún convenio encendido.
3. **R-03, el que se materializó — corregido, y todavía vigente.** *"Si el código avanza y los documentos no, la vista técnica deja de servir para lo único que existe."* Pasó tal cual: el código incorporó un patrón nuevo y las dos vistas quedaron describiendo un sistema que ya no existía. La revisión de correspondencia lo detectó y se rehicieron completas. **Sigue abierto en los diagramas**, y es hoy el de mayor exposición del registro.
