# Bitacora de IA - Camilo

## Proposito

Este documento registra las decisiones, correcciones y cambios discutidos entre Camilo y el agente de IA durante la evolucion de `BibFarmacia`.

La bitacora no pretende registrar cada mensaje, sino las decisiones tecnicas relevantes, las propuestas rechazadas y el razonamiento usado para seleccionar los patrones.

## 1. Analisis inicial de la factory de productos

### Situacion encontrada

La biblioteca creaba medicamentos mediante:

- `ProductoRequest` como objeto de transporte;
- `FabricaProducto.Crear(ProductoRequest)`;
- una lista blanca basada en `Type`;
- un `switch` para distinguir capsulas y liquidos;
- consumidores repartidos entre `Program.cs`, `RepositorioProductos` y `ServicioProducto`.

Esto hacia que una nueva presentacion de medicamento obligara a modificar varios archivos y permitia solicitudes con combinaciones de propiedades que no correspondian al tipo elegido.

### Primera propuesta

Se propuso eliminar `ProductoRequest` y crear una interfaz de factory con metodos `Crear` sobrecargados:

```text
Crear(datos comunes, IRelleno) -> MedicamentoCapsula
Crear(datos comunes, IEnvase, mililitros) -> MedicamentoLiquido
```

La decision buscaba expresar las necesidades de cada medicamento mediante la firma y eliminar la inspeccion de `Type`.

### Aclaracion sobre Abstract Factory

Camilo propuso una factory abstracta con dos factories hijas, una para medicamentos liquidos y otra para capsulas. Se evaluo que esa estructura no era una Abstract Factory clasica, porque no existian familias de productos relacionados.

La conclusion fue:

- una factory por capsula y liquido se acercaria mas a Factory Method;
- una sola factory con sobrecargas seria Simple Factory;
- Abstract Factory solo tendria sentido si existieran familias completas, por ejemplo lineas economica y premium que crearan medicamentos, cosmeticos y comestibles compatibles.

Se adopto la solucion pragmatica de `IFabricaMedicamentos` y `FabricaMedicamentos` con sobrecargas.

## 2. Implementacion inicial de la factory de medicamentos

Se agregaron:

- `Interfaces/IFabricaMedicamentos.cs`;
- `Factories/FabricaMedicamentos.cs`.

Se modificaron:

- `ServicioProducto`;
- `RepositorioProductos`;
- `Program.cs`.

Se eliminaron de la implementacion activa:

- `ProductoRequest`;
- `FabricaProducto`;
- `IFabricadorProducto`.

La factory recibia `IRelleno` para capsulas e `IEnvase` mas mililitros para liquidos.

### Correccion de nomenclatura

Durante una revision posterior aparecio temporalmente una factory antigua renombrada como `FabricaMedicamento` y un contrato `IFabricadorMedicamento`. Se comprobo que no tenia consumidores activos y se elimino para evitar dos soluciones concurrentes.

La implementacion conservada fue `FabricaMedicamentos`/`IFabricaMedicamentos`.

## 3. Integracion de cambios del companero

Despues de un `pull`, el commit del companero agrego cambios funcionales y una implementacion alternativa de creadores.

### Cambios funcionales conservados

Se conservaron:

- `IDescuento` y estrategias de descuento;
- `IReglaPuntos`;
- `ReglaPuntosEstandar`;
- `ReglaPuntosDoble`;
- notificacion mediante archivo;
- cambios en `Cliente`;
- cambios en `ServicioFacturacion`;
- cambios en `ServicioPuntos`.

### Alternativa de factory descartada

El companero agrego:

- `ICreadorMedicamento`;
- `CreadorCapsula`;
- `CreadorLiquido`;
- un registro de creadores en `Program.cs`;
- cambios en `RepositorioProductos` para usar el registro.

Inicialmente se revirtieron para conservar la factory con sobrecargas. Posteriormente se concluyo que ambas soluciones atendian entradas distintas:

- Simple Factory para el alta interactiva, donde el administrador elige relleno o envase en tiempo de ejecucion;
- Factory Method para la carga desde archivo, donde la primera columna decide el creador.

Por eso se recuperaron `ICreadorMedicamento`, `CreadorCapsula` y `CreadorLiquido`, haciendo que delegaran en `IFabricaMedicamentos` en lugar de duplicar la construccion concreta.

El repositorio dejo de instanciar factories, rellenos y envases. La instancia compartida y el registro se configuran en `Program.cs`.

## 4. Revision de puntos de dolor y patrones

Se reviso el documento de puntos de dolor P-01 a P-08 y se comparo con el codigo real.

### P-01: creacion de medicamentos

Se concluyo que la factory centraliza parte de la decision, pero que el `switch` del repositorio mantenia un costo OCP y una dependencia concreta. La solucion final separa:

- alta interactiva: `FabricaMedicamentos` con sobrecargas;
- carga desde archivo: Factory Method mediante creadores registrados.

### P-02: acumulacion de puntos

Se acepto Strategy:

- `IReglaPuntos` define `Calcular`;
- `ReglaPuntosEstandar` conserva el comportamiento original;
- `ReglaPuntosDoble` demuestra una alternativa;
- `ServicioPuntos` recibe la estrategia por constructor.

Se identifico que pedir puntos manualmente desde el menu no resolvia completamente el punto de dolor. La decision posterior fue calcularlos desde una venta confirmada.

### P-03: descuentos

Se acepto Strategy para descuentos:

- `IDescuento`;
- `SinDescuento`;
- `DescuentoPorcentual`;
- `Cliente.Descuento`.

Se detecto que la estrategia estaba preparada, pero la sobrecarga de facturacion con cliente no era llamada por el flujo de venta. Esto se marco como una deuda que debia corregirse integrando la factura a la venta real.

### P-04: facturacion

La revision evidencio que la venta original solo descontaba stock y registraba movimiento. El total se recalculaba despues, al mostrar el historial.

Se decidio que la venta debia calcular una sola vez:

- subtotal;
- descuento;
- total final.

Estos valores debian conservarse en el movimiento.

### P-05: categorias futuras

Se concluyo que cosmeticos y comestibles no debian agregarse a `IFabricaMedicamentos`. La alternativa recomendada fue un servicio y una factory por categoria.

Abstract Factory se mantuvo descartada hasta que existan familias reales de productos compatibles.

### P-06: promociones

Se propuso Decorator para modificar el precio facturado sin mutar `Producto.Precio` ni afectar inventario o alertas.

La implementacion del Decorator quedo como fase posterior.

### P-07: menu

Se acepto no modificar estructuralmente el `switch` del menu. Solo se autorizo cambiar los metodos invocados por sus casos.

### P-08: notificaciones

Se comprobo que Observer ya existia parcialmente mediante eventos de dominio y lambdas de consola. Se conecto `NotificacionArchivo` al mismo evento de movimiento, separando el canal de salida del evento.

## 5. Primer plan de implementacion

Se documento un plan con:

- Strategy para puntos y descuentos;
- Observer para notificaciones;
- Factory Method y Simple Factory para medicamentos;
- Decorator para promociones;
- factories y servicios separados para futuras categorias;
- refactorizacion de `Producto` para cumplir SRP.

La separacion de responsabilidades propuesta para `Producto` fue:

- conservar estado e invariantes;
- mover presentacion a un formateador;
- mover facturacion al servicio correspondiente;
- dejar el inventario bajo una responsabilidad de servicio, manteniendo las invariantes protegidas.

## 6. Flujo de venta orientado a eventos

Camilo aclaro que no queria servicios inyectando otros servicios. Se reviso el flujo y se propuso un encadenamiento mediante Observer y handlers conectados desde `Program.cs`:

```text
VentaSolicitada
-> FacturaCalculada
-> VentaProcesada
-> MovimientoRegistrado
-> PuntosAcumulados / Notificaciones
```

Cada servicio quedo responsable de una reaccion:

- `ServicioVenta` publica la solicitud;
- `ServicioFacturacion` calcula subtotal, descuento y total;
- `ServicioInventario` descuenta stock;
- `ServicioMovimiento` crea y conserva el movimiento;
- `ServicioPuntos` reacciona al movimiento y aplica Strategy;
- consola y archivo reaccionan como observadores.

Se agregaron:

- `ContextoVenta`;
- `EventoVenta`.

`Movimiento` paso a conservar cliente, subtotal, descuento y total.

El flujo fue ejecutado desde `Program.cs` con una venta real de Dolex. El resultado verificado fue:

- venta registrada;
- total `5000`;
- cinco puntos acumulados;
- movimiento registrado;
- historial mostrando el total almacenado.

## 7. Estado de la entidad Producto y SRP

Se reviso `Producto` y se eliminaron responsabilidades que no correspondian a la entidad:

- se elimino `ObtenerPrecio` y la facturacion usa `IFacturable.Precio`;
- se elimino `MostrarInformacion` de `Producto`;
- se agrego `FormateadorProducto` para la salida de consola;
- se mantuvo la proteccion de stock necesaria para sus invariantes.

El objetivo fue que `Producto` conservara estado y validaciones, sin conocer formatos de salida ni coordinacion del caso de uso de venta.

## 8. EstadoVenta: propuesta inicial y correccion mediante State

Durante la implementacion del flujo de venta, el agente introdujo un `enum EstadoVenta` para simplificar las etapas:

```text
Pendiente
Facturada
Procesada
Confirmada
Fallida
```

Camilo cuestiono esa decision porque queria aplicar el patron State, no manejar las transiciones mediante un enumerable y condiciones.

Se acordo reemplazar el enum por:

- `IEstadoVenta`;
- `EstadoVentaBase`;
- `EstadoPendiente`;
- `EstadoFacturada`;
- `EstadoProcesada`;
- `EstadoConfirmada`;
- `EstadoFallida`.

`ContextoVenta` paso a contener el estado actual y a delegar las transiciones en el objeto State. Cada estado controla las operaciones validas y evita transiciones incoherentes, por ejemplo facturar una venta ya confirmada.

Esta correccion fue una decision de Camilo sobre la propuesta inicial del agente y mejoro la correspondencia entre la implementacion y la teoria del patron State.

## 9. Organizacion del codigo y limpieza

Se organizaron los estados de venta en:

```text
BibFarmacia/Clases/EstadosVenta/
```

Se actualizaron namespaces y referencias para evitar aglomerar todas las clases de dominio en `Clases`.

Tambien se verifico que `ProductoRequest` estuviera muerto:

- no tenia referencias en el codigo activo;
- solo aparecia en historial, documentacion y `BibliotecaAnterior`.

Se elimino `BibFarmacia/Clases/ProductoRequest.cs` de la biblioteca actual. No se elimino de `BibliotecaAnterior` ni de documentos historicos, porque esos elementos sirven como evidencia de la arquitectura anterior.

## 10. Documentacion UML

Se creo el reporte:

```text
COMPARACION-BIBLIOTECA-ACTUAL-VS-ANTERIOR.md
```

El reporte especifica:

- archivos agregados y eliminados;
- metodos y atributos nuevos;
- relaciones de herencia y realizacion;
- dependencias de factories, servicios y eventos;
- flujo de venta;
- clases que deben incorporarse al UML;
- elementos generados que no deben dibujarse.

Los planes de implementacion tambien fueron documentados en:

```text
PLAN-IMPLEMENTACION-PATRONES.md
PLAN-FLUJO-VENTA-EVENTOS.md
```

Estos documentos se agregaron al `.gitignore` junto con el reporte UML para mantenerlos como material local de trabajo.

## 11. Validaciones realizadas

Se verifico en diferentes etapas:

- compilacion de la solucion con `dotnet build`;
- cero errores y cero advertencias en la implementacion del flujo;
- ausencia de referencias activas a `ProductoRequest` antes de eliminarlo;
- ejecucion real de login, carga de archivos, venta, movimiento, puntos y notificacion;
- total persistido en el movimiento y mostrado en el historial;
- carga de medicamentos mediante el registro de creadores;
- aislamiento de fallos secundarios de notificacion y puntos.

## 12. Resultado y pendientes

### Resultado alcanzado

La biblioteca actual cuenta con una separacion mas clara entre:

- entidades y sus invariantes;
- factories y creadores;
- estrategias de descuento y puntos;
- eventos de dominio;
- handlers del flujo de venta;
- canales de notificacion;
- formateo de consultas.

### Pendientes declarados

- implementar Decorator para promociones de producto;
- incorporar cosmeticos y comestibles cuando el requisito sea activado;
- agregar pruebas automatizadas o una prueba de flujo reproducible mas formal;
- revisar si la salida de errores de los event handlers debe persistirse en un canal dedicado;
- decidir si `DescontarStock` debe trasladarse completamente a `ServicioInventario` o permanecer como operacion protegida de la entidad para preservar sus invariantes.

## Cierre

La implementacion evoluciono mediante varias correcciones de rumbo. El agente propuso simplificaciones iniciales, como la factory unica y el `enum` para el estado de venta. Camilo reviso esas decisiones desde la teoria de patrones y las restricciones del dominio, solicito State para las transiciones y exigio que el flujo real se conectara mediante Observer sin inyeccion de servicios entre si.

El resultado documentado aqui distingue las propuestas del agente de las decisiones aceptadas por Camilo y deja trazabilidad suficiente para continuar el trabajo o actualizar el UML.