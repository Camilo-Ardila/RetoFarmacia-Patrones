# RETO 2 — ACTIVIDAD 6

## Vista para el negocio

**Para quién es este documento:** para la Dirección de Ingeniería y para quien aprueba el presupuesto. Explica qué le vamos a hacer al sistema de la farmacia, qué gana el negocio, qué cuesta y qué decisiones necesitamos que se tomen. No hace falta saber programar para leerlo.

---

## 1. Lo que hay que saber en un minuto

El sistema de la farmacia funciona. El problema no es que falle: es que **cada cosa nueva que el negocio pide cuesta más de lo que debería**, porque la misma decisión está escrita en varios sitios a la vez y hay que ir a cambiarlos todos, uno por uno, con el riesgo de olvidar alguno.

Lo que vamos a hacer es **juntar cada decisión en un solo sitio**. Que la regla de cuántos puntos gana una compra viva en un lugar; que la regla de cuánto se le descuenta a cada cliente viva en un lugar; que la forma de armar un producto viva en un lugar; que la lista de a quién se le avisa cuando pasa algo viva en un lugar.

No agregamos ninguna función nueva visible en esta entrega. La farmacia va a operar exactamente igual el día después. Lo que cambia es **el precio de la siguiente solicitud**.

---

## 2. Qué NO cambia

Esto es lo primero porque es la condición que la Dirección puso y que aceptamos sin discusión:

- **Las pantallas son las mismas.** Las mismas opciones, en el mismo orden, con los mismos textos.
- **Los números son los mismos.** El mismo total, los mismos puntos, las mismas alertas de existencias y de vencimiento.
- **Los archivos de datos son los mismos.** Ni una columna nueva, ni una columna menos. Nadie tiene que volver a cargar información.
- **Nadie en el mostrador se entera de nada.** No hay que capacitar a ninguna persona ni cambiar ningún procedimiento.
- **No se compra nada.** Ninguna herramienta, ninguna licencia, ningún servicio externo.

Para demostrarlo no pedimos que nos crean: guardamos la salida del sistema anterior y la del sistema nuevo, y las comparamos línea por línea. Tienen que ser idénticas. Si en algún caso no lo son, ese caso no se entrega.

---

## 3. Qué le vamos a hacer al sistema

Cuatro cambios, todos por dentro. Ninguno se ve desde el mostrador.

| # | Qué juntamos en un solo sitio | Para qué sirve mañana |
|---|---|---|
| 1 | **La forma de armar un producto que entra al catálogo.** Hoy la decisión de qué se está creando está escrita en cuatro sitios distintos del programa. | Que dar de alta una familia de productos nueva deje de ser una revisión de todo el programa. |
| 2 | **La regla de cuántos puntos gana una compra.** Hoy esa regla está metida dentro del registro del cliente y no hay ningún otro lugar donde pueda vivir una regla distinta. | Poder ofrecer una promoción —doble puntos un fin de semana— sin tocar el registro del cliente ni la pantalla de ventas. |
| 3 | **La regla de cuánto se le descuenta a cada cliente.** Hoy existe un único descuento con el porcentaje escrito a mano dentro del programa, y el cliente no tiene ningún dato que diga a qué entidad pertenece. | Poder abrir convenios con empresas, bancos, cooperativas, universidades y colegios, cada uno con su propia condición. |
| 4 | **La lista de a quién se le avisa cuando pasa algo.** Hoy los avisos van directo a la pantalla y no hay forma de sumar otro destino. | Poder mandar el mismo aviso a más de un sitio. Ya lo dejamos funcionando con un segundo destino real, un archivo de registro, sin que la pantalla cambie. |

**De las tres solicitudes pendientes, esta entrega ataca la de convenios.** La parte que permite que cada cliente tenga su propia condición de descuento ya está construida y probada. Falta un paso previo del que hablamos en el punto 7, y por eso todavía no está encendida.

---

## 4. Dónde se va hoy el tiempo

Estos números no son una impresión: los medimos contando, para cada solicitud, cuántos sitios del programa hay que abrir, entender, cambiar y volver a probar. Cada sitio que se toca es una oportunidad de romper algo que hoy funciona.

| Lo que el negocio pide | Sitios que hay que tocar hoy | Además |
|---|---|---|
| Vender una familia de productos nueva (cosméticos, comestibles) | **4** | una parte nueva por cada familia |
| Ofrecer una forma distinta de acumular puntos | **3**, uno de ellos el registro del cliente | y otra vez los mismos 3 por cada promoción siguiente |
| Abrir convenios de descuento | **4** | y una parte nueva por cada combinación de entidad y beneficio: con las cinco entidades y los dos beneficios pedidos, **hasta 10** |
| Sumar un destino nuevo para los avisos | **3** como mínimo | 4 si además hay que decir a quién va dirigido |

Y hay un dato que conviene decir con todas sus letras: **hoy la venta no calcula lo que se cobra.** Descuenta la existencia y registra el movimiento, pero el importe no se calcula en ningún momento de la operación; la pantalla de historial lo vuelve a calcular después, al precio que el producto tenga en ese instante. Mientras eso siga así, no hay ningún lugar donde un descuento pueda aplicarse ni quedar guardado.

---

## 5. Qué gana el negocio

Dos cosas medibles: cuánto tarda una solicitud y cuánto riesgo tiene aplicarla.

| Lo que el negocio pide | Antes | Después |
|---|---|---|
| Una promoción de puntos | tocar 3 sitios, uno de ellos el registro del cliente | **una parte nueva y una línea de configuración**; el registro del cliente no se toca |
| Un convenio nuevo | tocar 4 sitios, y hasta 10 partes nuevas para cubrir lo pedido | **una parte nueva por convenio**; nadie más se toca |
| Un destino nuevo para los avisos | tocar 3 sitios | **una parte nueva y una línea**; ya está demostrado con el destino que conectamos |
| Una presentación nueva de un medicamento | tocar 4 sitios | **un solo sitio** |

Sobre el riesgo de romper algo, el razonamiento es directo: **el riesgo se concentra donde se toca el programa.** Si una promoción pasa de tocar tres sitios a tocar uno, quedan dos sitios menos donde equivocarse, y esos dos son partes que el negocio usa todos los días —el registro del cliente y la pantalla de ventas—, no rincones olvidados.

Hay una ganancia que no se ve en la tabla y conviene nombrarla: **hoy no hay dónde leer cómo se conecta el sistema.** Para saber qué usa qué, hay que leerlo entero. Después de este cambio, todas las conexiones quedan escritas en un único sitio, y hay un documento —el que acompaña a este— que le dice a quien llegue nuevo dónde tocar para cada tipo de solicitud.

---

## 6. Qué cuesta

Lo decimos completo, incluido lo que no nos favorece.

- **Tiempo del equipo en trabajo que no produce ninguna función visible.** Al terminar, el sistema hace exactamente lo mismo que antes. Lo que se compra es el precio de las solicitudes siguientes, no una función nueva.
- **El sistema queda con más partes.** Cada regla que sale a vivir por su cuenta es una parte más que existe, que hay que nombrar y que hay que entender. Un sistema con más partes pequeñas es más fácil de cambiar y **más difícil de leer la primera vez**. Lo compensamos con el documento de la vista técnica, que es exactamente el mapa para esa primera vez.
- **Un orden obligatorio que retrasa los convenios.** No se pueden encender los convenios antes de arreglar el cálculo del cobro. Eso significa que la solicitud de convenios se entrega en dos pasos y no en uno.
- **Una corrección pendiente que reconocemos.** Al revisar nuestro propio trabajo encontramos que en un punto no cumplimos la regla que nos pusimos: hay una parte del programa que se construye sus propias piezas por dentro en vez de recibirlas del sitio único de conexiones. Es un arreglo pequeño, está declarado, y hay que hacerlo antes de empezar con las familias de productos nuevas —si no, cada familia nueva copia el error.
- **Lo que NO cuesta:** ninguna compra, ninguna migración de datos, ninguna capacitación, ninguna parada del servicio.

---

## 7. Qué riesgos hay, y cómo nos damos cuenta

En lenguaje de operación. El detalle completo, con ocho riesgos y sus mediciones, está en el análisis de riesgos que acompaña a esta entrega.

| Si pasa esto | Lo que se ve en la operación | Cómo nos enteramos a tiempo |
|---|---|---|
| **Se encienden los convenios antes de arreglar el cálculo del cobro** | El sistema le cobra un valor al cliente y muestra otro distinto en el historial, y no queda registro de cuál fue el correcto. Es el riesgo más caro de todos: es el único que produce un cobro equivocado. | Se vende con un cliente con convenio y se compara el total de la venta contra el del historial. Si no coinciden, está pasando. |
| **Falla el destino donde se escriben los avisos** (el archivo queda tomado, se llena el disco) | Hoy, la venta se cae completa por culpa de un aviso. La existencia ya se descontó y el operador pierde la sesión. | El archivo de avisos deja de crecer mientras el historial sigue registrando ventas. |
| **Queda encendida por error una promoción de prueba** | Los clientes acumulan el doble de puntos sin que nadie lo haya autorizado, y el sistema entregado deja de comportarse como el anterior. | La comparación de salidas contra el sistema anterior deja de coincidir. Se corre antes de cada entrega. |
| **Los planos del sistema y el sistema real dejan de coincidir** | El próximo que entre al equipo no va a poder ubicar dónde tocar, y volvemos al punto de partida: hay que leerlo todo. | Se revisa la correspondencia entre el plano y el programa antes de cerrar la entrega, con dos personas y lista en mano. |
| **Se vende más de lo que hay en existencia, o se cargan cero puntos** | La aplicación termina en mitad de la operación. Esto ya pasa hoy y no lo vamos a cambiar en esta entrega, porque arreglarlo cambiaría el comportamiento que está congelado. | Queda declarado como deuda conocida. Si el negocio quiere que se arregle, es una solicitud aparte y hay que autorizar el cambio de comportamiento. |

---

## 8. Qué necesitamos del negocio

Decisiones que no puede tomar el equipo técnico:

1. **Aceptar el orden.** Primero el cálculo del cobro, después los convenios. Esto significa aceptar que los convenios no arrancan en la primera entrega.
2. **La lista de entidades con convenio y la condición de cada una.** Nombre de la entidad y qué se le descuenta. Y quién queda autorizado a cambiar ese valor después.
3. **Definir si el descuento aplica también a los servicios** —inyectología, curaciones, cambio de vendajes— o solo a los productos.
4. **Definir qué pasa cuando un cliente con convenio compra sin identificarse.** Hoy la venta no le pregunta a nadie quién compra; si el descuento depende del cliente, alguien tiene que decidir qué hacer cuando no hay cliente.
5. **Media hora de alguien del mostrador** para confirmar que la pantalla y los números son los mismos de siempre. Es la mejor prueba que existe y no cuesta casi nada.
6. **Confirmar que los archivos de datos siguen congelados**: que nadie va a cambiar su formato por fuera, porque todo el trabajo se diseñó para no tocarlos.

---

## 9. Qué pasa si no se hace

No pasa nada malo mañana. El sistema sigue funcionando igual. Lo que pasa es que **la cuenta se sigue acumulando**:

- Los convenios que el negocio quiere abrir siguen costando hasta diez partes nuevas en vez de una por convenio, y cada una con su porcentaje escrito a mano dentro del programa.
- Las familias de productos nuevas —cosméticos, comestibles— obligan a repetir la misma decisión en cuatro sitios, y esa repetición ya demostró que se olvida uno.
- Cada promoción de puntos vuelve a tocar el registro del cliente, que es la parte que menos conviene tocar porque la usa todo lo demás.
- El riesgo de cobrar un valor y mostrar otro no desaparece por no hacer el cambio: aparece igual el día que alguien agregue un descuento por su cuenta, pero sin que nadie haya decidido el orden ni haya avisado del riesgo.

Y hay un costo que no se mide en horas: mientras no exista un sitio donde esté escrito cómo se conecta el sistema, la respuesta honesta a cualquier pregunta sobre él seguirá siendo *hay que leerlo todo*, y eso depende de que las mismas personas sigan en el equipo.

---

## 10. Prueba de que esta vista sirve

Esta vista se le presenta a una persona ajena al equipo y sin formación técnica, y se anota qué entendió. La evidencia queda en el video.

Preguntas que se le hacen, sin ayudarle:

| Pregunta | Qué entendió |
|---|---|
| ¿Qué le van a hacer al sistema? | |
| ¿Qué va a notar la persona que atiende en el mostrador? | |
| ¿Qué gana la farmacia con esto? | |
| ¿Cuál es el riesgo más caro y cómo se darían cuenta? | |
| ¿Qué le están pidiendo al negocio? | |

Si al quitarle las palabras técnicas esta vista se queda sin contenido, es que no había contenido de negocio. Por eso ninguna de las diez secciones nombra una sola pieza del programa.
