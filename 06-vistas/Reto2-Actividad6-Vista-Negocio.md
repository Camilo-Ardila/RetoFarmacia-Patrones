# RETO 2 — ACTIVIDAD 6

## Vista para el negocio

**Para quién es este documento:** para la Dirección de Ingeniería y para quien aprueba el presupuesto. Explica qué le hicimos al sistema de la farmacia, qué gana el negocio, qué cuesta y qué decisiones necesitamos que se tomen. No hace falta saber programar para leerlo.

---

## 1. Lo que hay que saber en un minuto

El sistema de la farmacia funciona. El problema no era que fallara: era que **cada cosa nueva que el negocio pedía costaba más de lo que debería**, porque la misma decisión estaba escrita en varios sitios a la vez y había que ir a cambiarlos todos, uno por uno, con el riesgo de olvidar alguno.

Lo que hicimos fue **juntar cada decisión en un solo sitio**. Que la regla de cuántos puntos gana una compra viva en un lugar; que la regla de cuánto se le descuenta a cada cliente viva en un lugar; que la forma de armar un producto viva en un lugar; que la lista de a quién se le avisa cuando pasa algo viva en un lugar.

Y hicimos un cambio más, que sí se nota: **ahora la venta calcula lo que se cobra y lo guarda.** Antes no lo hacía, y esa era la pieza que faltaba para poder abrir convenios de descuento.

**La solicitud de convenios está terminada salvo por un dato que solo el negocio puede dar:** la lista de entidades y qué se le descuenta a cada una. En cuanto llegue esa lista, encenderla es cuestión de minutos.

---

## 2. Qué cambia y qué no

Esto va primero porque la condición que la Dirección puso fue que no cambiara nada, y **hay una parte que sí cambió**. La decimos completa.

### Lo que cambió, y por qué estaba autorizado

Todo el cambio está en **la pantalla de vender**, y todo viene de la misma solicitud, la de convenios:

- **Ahora pregunta a nombre de quién es la venta.** Antes no lo preguntaba. Es obligatorio: si el descuento depende del cliente, hay que saber quién compra.
- **Al terminar, dice cuánto se cobró.** Antes solo decía que la venta quedó registrada, sin importe.
- **Si se intenta vender más de lo que hay, ya no se cae la aplicación.** Antes el programa terminaba en mitad de la operación y el operador perdía la sesión. Ahora responde que no se pudo registrar y explica por qué.
- **Cada venta acumula puntos automáticamente.** Antes había que cargarlos a mano desde otra opción del menú.

Los tres primeros son consecuencia directa del cobro; el cuarto es lo que el negocio siempre entendió que hacía el sistema de puntos.

### Lo que no cambió

- **El resto de las pantallas son las mismas.** Las mismas opciones, en el mismo orden, con los mismos textos.
- **Los números son los mismos.** El listado de productos, el historial de movimientos, las alertas de existencias y de vencimiento, el arranque y el ingreso al sistema: idénticos, comparados línea por línea.
- **Los archivos de datos son los mismos.** Ni una columna nueva, ni una columna menos. Nadie tiene que volver a cargar información.
- **No se compra nada.** Ninguna herramienta, ninguna licencia, ningún servicio externo.
- **Nada de lo que quedó encendido cambia un cobro.** Mientras no haya convenios asignados, todo cliente paga lo mismo que pagaba antes.

Para demostrarlo no pedimos que nos crean: guardamos la salida del sistema anterior y la del sistema nuevo, y las comparamos línea por línea. Fuera de la pantalla de venta, tienen que ser idénticas.

### Lo único que hay que capacitar

**Media frase al personal del mostrador:** al vender, ahora se pide el nombre del cliente. Nada más.

---

## 3. Qué le hicimos al sistema

Cinco cambios. Cuatro son por dentro y no se ven; el quinto es el del cobro, que sí se ve.

| # | Qué juntamos en un solo sitio | Para qué sirve mañana |
|---|---|---|
| 1 | **La forma de armar un producto que entra al catálogo.** Antes la decisión de qué se está creando estaba escrita en cuatro sitios distintos del programa. | Que dar de alta una familia de productos nueva deje de ser una revisión de todo el programa. Hoy son dos sitios en vez de cuatro. |
| 2 | **La regla de cuántos puntos gana una compra.** Antes esa regla estaba metida dentro del registro del cliente y no había ningún otro lugar donde pudiera vivir una regla distinta. | Ofrecer una promoción —doble puntos un fin de semana— sin tocar el registro del cliente ni la pantalla de ventas. Es una línea de configuración. |
| 3 | **La regla de cuánto se le descuenta a cada cliente.** Antes existía un único descuento con el porcentaje escrito a mano dentro del programa, y el cliente no tenía ningún dato que dijera a qué entidad pertenece. | Abrir convenios con empresas, bancos, cooperativas, universidades y colegios, cada uno con su propia condición. |
| 4 | **La lista de a quién se le avisa cuando pasa algo.** Antes los avisos iban directo a la pantalla y no había forma de sumar otro destino. | Mandar el mismo aviso a más de un sitio. Ya quedó funcionando con un segundo destino real, un archivo de registro, sin que la pantalla cambie. |
| 5 | **El cobro.** Antes la venta descontaba la existencia y anotaba el movimiento, pero **no calculaba el importe en ningún momento**: la pantalla de historial lo volvía a calcular después, al precio que el producto tuviera en ese instante. | Que el valor que se le cobra a una persona quede calculado una sola vez y **guardado**. Es lo que hace posible aplicar un descuento, y es lo que garantiza que la venta y el historial nunca muestren dos cifras distintas. |

Además, la venta se reorganizó en etapas con nombre —se calcula, se despacha, se anota, se acreditan los puntos— y el sistema sabe en cuál va cada operación. Eso es lo que permite que un tropiezo a mitad de camino termine en un mensaje claro en vez de en una aplicación cerrada.

---

## 4. Dónde se iba el tiempo antes

Estos números no son una impresión: los medimos contando, para cada solicitud, cuántos sitios del programa había que abrir, entender, cambiar y volver a probar. Cada sitio que se toca es una oportunidad de romper algo que funciona.

| Lo que el negocio pide | Sitios que había que tocar | Además |
|---|---|---|
| Vender una familia de productos nueva (cosméticos, comestibles) | **4** | una parte nueva por cada familia |
| Ofrecer una forma distinta de acumular puntos | **3**, uno de ellos el registro del cliente | y otra vez los mismos 3 por cada promoción siguiente |
| Abrir convenios de descuento | **4** | y una parte nueva por cada combinación de entidad y beneficio: con las cinco entidades y los dos beneficios pedidos, **hasta 10** |
| Sumar un destino nuevo para los avisos | **3** como mínimo | 4 si además hay que decir a quién va dirigido |

Y estaba el problema de fondo, que ya no está: **la venta no calculaba lo que se cobra.** Mientras eso siguiera así, no había ningún lugar donde un descuento pudiera aplicarse ni quedar guardado. Ese era el bloqueo, y se levantó.

---

## 5. Qué gana el negocio

Dos cosas medibles: cuánto tarda una solicitud y cuánto riesgo tiene aplicarla.

| Lo que el negocio pide | Antes | Ahora |
|---|---|---|
| Una promoción de puntos | tocar 3 sitios, uno de ellos el registro del cliente | **una parte nueva y una línea de configuración**; el registro del cliente no se toca |
| Un convenio nuevo | tocar 4 sitios, y hasta 10 partes nuevas para cubrir lo pedido | **un solo sitio, y ninguna parte nueva** si la condición es un porcentaje; el cálculo y el guardado ya existen |
| Un destino nuevo para los avisos | tocar 3 sitios | **una parte nueva y una línea**; ya está demostrado con el destino que conectamos |
| Una presentación nueva de un medicamento | tocar 4 sitios | **dos sitios** |
| Una etapa nueva en la venta (reservar, anular) | no existía forma de agregarla sin reescribir la venta | **una parte nueva y una línea**, en el orden que le corresponda |

Sobre el riesgo de romper algo, el razonamiento es directo: **el riesgo se concentra donde se toca el programa.** Si una promoción pasa de tocar tres sitios a tocar uno, quedan dos sitios menos donde equivocarse, y esos dos son partes que el negocio usa todos los días —el registro del cliente y la pantalla de ventas—, no rincones olvidados.

Dos ganancias más que no se ven en la tabla:

- **La venta dejó de ser un camino donde el sistema se cae.** Intentar vender más de lo que hay era, hasta esta entrega, una forma de terminar la aplicación con la existencia ya descontada. Hoy responde y sigue.
- **Ya hay dónde leer cómo se conecta el sistema.** Antes, para saber qué usa qué, había que leerlo entero. Ahora todas las conexiones están escritas en un único sitio, y hay un documento —el que acompaña a este— que le dice a quien llegue nuevo dónde tocar para cada tipo de solicitud.

---

## 6. Qué cuesta

Lo decimos completo, incluido lo que no nos favorece.

- **Tiempo del equipo en trabajo que en su mayor parte no produce ninguna función visible.** Salvo el cobro, el sistema hace lo mismo que antes. Lo que se compra es el precio de las solicitudes siguientes.
- **El sistema quedó con más partes.** Cada regla que sale a vivir por su cuenta es una parte más que existe, que hay que nombrar y que hay que entender. Un sistema con más partes pequeñas es más fácil de cambiar y **más difícil de leer la primera vez**. Lo compensamos con el documento de la vista técnica, que es exactamente el mapa para esa primera vez.
- **La pantalla de vender cambió.** Está detallado en el punto 2. Es media frase de capacitación, pero es un cambio real y hay que decirlo, no esconderlo entre los cambios invisibles.
- **Siguen existiendo dos caminos que terminan la aplicación:** cargar cero puntos a mano y dar de alta un producto con precio cero. Los blindamos donde la solicitud de convenios lo exigía —la venta— y no más allá, porque tocar el resto sería cambiar comportamiento que nadie pidió cambiar. Están declarados y arreglarlos es una solicitud aparte.
- **Lo que NO cuesta:** ninguna compra, ninguna migración de datos, ninguna parada del servicio.

---

## 7. Qué riesgos hay, y cómo nos damos cuenta

En lenguaje de operación. El detalle completo, con ocho riesgos y sus mediciones, está en el análisis de riesgos que acompaña a esta entrega.

| Si pasa esto | Lo que se ve en la operación | Cómo nos enteramos a tiempo |
|---|---|---|
| **Se encienden los convenios antes de arreglar el cálculo del cobro** | Era el riesgo más caro de todos, el único que producía un cobro equivocado: el sistema le cobraba un valor al cliente y mostraba otro distinto en el historial. **Ya no puede pasar:** hoy la venta y el historial muestran el mismo número guardado, no dos cálculos separados. | La prueba sigue vigente y se corre igual: se vende con un cliente con convenio y se comparan los dos totales. Ahora tienen que coincidir siempre. |
| **Se enciende un convenio que el negocio no autorizó, o con el porcentaje equivocado** | Es el riesgo que reemplaza al anterior. Un cliente paga menos de lo que le corresponde, o menos que otro igual a él, y nadie lo nota hasta el cierre. | Los convenios se asignan en un solo sitio y por cédula, así que la lista completa se lee de un vistazo y se compara contra la lista autorizada. **Es una revisión de cinco minutos y hay que hacerla antes de cada entrega.** |
| **Falla el destino donde se escriben los avisos** (el archivo queda tomado, se llena el disco) | Antes, la venta se caía completa por culpa de un aviso, con la existencia ya descontada. **Ya no:** el aviso falla solo, la venta se completa y el operador ve una nota. | El archivo de avisos deja de crecer mientras el historial sigue registrando ventas. |
| **Queda encendida por error una promoción de prueba** | Los clientes acumulan el doble de puntos sin que nadie lo haya autorizado. | La comparación de salidas contra el sistema anterior deja de coincidir. Se corre antes de cada entrega. |
| **Los planos del sistema y el sistema real dejan de coincidir** | El próximo que entre al equipo no va a poder ubicar dónde tocar, y volvemos al punto de partida: hay que leerlo todo. | Se revisa la correspondencia entre el plano y el programa antes de cerrar la entrega, con dos personas y lista en mano. Esta entrega ya obligó a rehacer los dos documentos de vistas, que es la prueba de que el control funciona. |
| **Se cargan cero puntos a mano, o se da de alta un producto con precio cero** | La aplicación termina en mitad de la operación. Sigue pasando y no lo cambiamos en esta entrega. | Queda declarado como deuda conocida. Si el negocio quiere que se arregle, es una solicitud aparte y hay que autorizar el cambio de comportamiento. |

---

## 8. Qué necesitamos del negocio

Decisiones que no puede tomar el equipo técnico. **La primera es la que separa a la solicitud de convenios de estar terminada.**

1. **La lista de entidades con convenio y la condición de cada una.** Nombre de la entidad, qué se le descuenta, y quién queda autorizado a cambiar ese valor después. Es lo único que falta: el sistema ya sabe calcular y guardar un descuento, pero no sabe a quién dárselo.
2. **Definir si el descuento aplica también a los servicios** —inyectología, curaciones, cambio de vendajes— o solo a los productos. Hoy la venta trata igual a los dos.
3. **Definir qué pasa cuando quien compra no está registrado.** Hoy la venta pide el nombre del cliente y, si no lo encuentra, no se registra. Hay que confirmar que ese es el comportamiento deseado y no un obstáculo en el mostrador.
4. **Aceptar el cambio en la pantalla de vender**, detallado en el punto 2, y confirmar que media frase de capacitación es suficiente.
5. **Media hora de alguien del mostrador** para confirmar que el resto de las pantallas y los números son los mismos de siempre. Es la mejor prueba que existe y no cuesta casi nada.
6. **Confirmar que los archivos de datos siguen congelados**: que nadie va a cambiar su formato por fuera, porque todo el trabajo se diseñó para no tocarlos.

---

## 9. Qué pasa si no se completa

El trabajo está hecho; lo que puede quedar sin hacer es el último paso. Si la lista de convenios no llega:

- El sistema queda con la capacidad construida, probada y apagada. Se pagó el costo y no se cobra el beneficio.
- Los convenios que el negocio quiere abrir siguen sin existir, y el riesgo real se desplaza: alguien terminará agregando un descuento por su cuenta, con el porcentaje escrito a mano, sin que nadie haya decidido a quién aplica ni haya revisado la lista.
- Las familias de productos nuevas —cosméticos, comestibles— siguen esperando, aunque su costo ya bajó a la mitad.

Y hay un costo que no se mide en horas: los dos documentos de vistas valen mientras describan el sistema real. Esta entrega ya obligó a rehacerlos completos. Si el código sigue avanzando y los documentos no, la respuesta honesta a cualquier pregunta sobre el sistema vuelve a ser *hay que leerlo todo*, y eso depende de que las mismas personas sigan en el equipo.

---

## 10. Prueba de que esta vista sirve

Esta vista se le presenta a una persona ajena al equipo y sin formación técnica, y se anota qué entendió. La evidencia queda en el video.

Preguntas que se le hacen, sin ayudarle:

| Pregunta | Qué entendió |
|---|---|
| ¿Qué le hicieron al sistema? | |
| ¿Qué va a notar la persona que atiende en el mostrador? | |
| ¿Qué gana la farmacia con esto? | |
| ¿Cuál es el riesgo más caro hoy y cómo se darían cuenta? | |
| ¿Qué falta para que los convenios funcionen, y de quién depende? | |

Si al quitarle las palabras técnicas esta vista se queda sin contenido, es que no había contenido de negocio. Por eso ninguna de las diez secciones nombra una sola pieza del programa.
