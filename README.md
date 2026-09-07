# Reto Farmacia — Patrones de diseño arquitectónico

Segunda fase del proyecto de modernización arquitectónica del sistema heredado de
Farmacia para la asignatura Arquitectura de Software.

**Video de sustentación:** [YouTube](https://youtu.be/W5O4OEReH7s)

El objetivo es incorporar los cambios solicitados en el enunciado manteniendo el
comportamiento existente que no fue autorizado a cambiar. La solución aplica patrones
para encapsular reglas variables, controlar el flujo de una venta y permitir la
extensión de productos sin modificar el menú principal.

## Integrantes

| Integrante | Rol y responsabilidad |
|---|---|
| Camilo José Ardila Restrepo | **Arquitecto Líder:** detección de los puntos rígidos, selección y descarte de patrones, y diseño del TO-BE. |
| David Berrío Martínez | **Arquitecto de Riesgos y Despliegue:** análisis de riesgos y elaboración del plan de cambio. |
| Miguel Alejandro Ramírez Rueda | **Arquitecto de Verificación:** demostración de que SOLID se mantiene y de que el comportamiento no cambió. |

La comunicación y la coordinación del proyecto fueron responsabilidad compartida de
los tres integrantes; no se asignó un Arquitecto de Comunicación independiente.

## Alcance de la solución

- **Strategy:** permite intercambiar la regla de acumulación de puntos y la estrategia
	de descuentos sin modificar los servicios que las utilizan.
- **State:** modela las etapas de una venta (`Pendiente`, `Confirmada`, `Facturada`,
	`Procesada` y `Fallida`) y evita que una venta fallida deje movimientos o consuma
	inventario.
- **Factory Method y registro de productos:** centraliza la creación de medicamentos
	y permite registrar rellenos de cápsula (`gel`, `polvo` u otros) sin editar el menú.
- **Observer:** conserva la notificación de eventos de ventas, puntos, movimientos,
	vencimientos y stock mínimo.
- **Responsabilidades separadas:** repositorios, servicios, autenticación,
	facturación, inventario y presentación se mantienen desacoplados mediante interfaces.

## Estructura del repositorio

```text
.
├── DiagramasUML/
│   ├── AS-IS/                 # Diseño del sistema heredado
│   └── TO-BE/                 # Diseño propuesto para esta fase
├── EvidenciaEjecucion/
│   ├── casos-caracterizacion/ # Comparación contra la línea base
│   ├── patrones/              # Demostraciones reproducibles de los patrones
│   └── demo/                  # Recorrido para la sustentación
├── src/SolucionFarmacia/
│   ├── BibFarmacia/           # Biblioteca de dominio y servicios
│   ├── AppFarmaciaConsola/    # Punto de entrada de la aplicación
│   └── BibliotecaAnterior/    # Referencia de la implementación previa
├── Reto2_Patrones_Enunciado_y_Rubrica.pdf
└── Reto2-Documentacion-finalizado.pdf
```

## Requisitos

- .NET SDK 8.0 o una versión compatible con proyectos `net8.0`.
- Bash para ejecutar las suites de evidencia (`Git Bash` funciona en Windows).
- Git, necesario para que la suite de caracterización cree la línea base con
	`git worktree`.

## Compilar y ejecutar

Desde la raíz del repositorio:

```bash
dotnet build src/SolucionFarmacia/SolucionFarmacia.sln
dotnet run --project src/SolucionFarmacia/AppFarmaciaConsola/AppFarmaciaConsola.csproj
```

La aplicación de consola carga los archivos de datos incluidos en el proyecto y ofrece
las opciones de autenticación, consulta de productos, servicios, clientes,
movimientos, alertas, altas y ventas.

## Evidencia de ejecución

Las tres suites se ejecutan desde `EvidenciaEjecucion/`:

```bash
cd EvidenciaEjecucion
./casos-caracterizacion/correr-casos.sh
./patrones/correr-patrones.sh
./demo/correr-demo.sh
```

La suite de caracterización compara el sistema actual con el commit anterior a la
incorporación de patrones. Las invariantes deben permanecer idénticas; las diferencias
de la pantalla de venta están registradas como cambios autorizados. Para crear o
actualizar esas referencias se usa únicamente la primera vez:

```bash
./casos-caracterizacion/correr-casos.sh --fijar
```

La evidencia registrada el 6 de septiembre de 2026 obtuvo `14/14` invariantes
idénticos y `3/3` cambios autorizados contenidos. El detalle de los casos y sus
resultados está en [EvidenciaEjecucion/README.md](EvidenciaEjecucion/README.md) y
[EvidenciaEjecucion/casos-caracterizacion/resultados.md](EvidenciaEjecucion/casos-caracterizacion/resultados.md).

## Demostración de los patrones

La suite de patrones usa el mismo binario con configuraciones distintas, sin editar el
código entre ejecuciones:

- `FARMACIA_REGLA_PUNTOS=estandar` y `doble` demuestran Strategy sobre los puntos.
- `FARMACIA_CONVENIOS=1` demuestra Strategy sobre descuentos y la conservación del
	mismo total en la venta y el historial.
- El registro de rellenos demuestra Factory y extensibilidad mediante configuración.
- Las ventas sin stock o sin cliente demuestran el estado `Fallida` y que no se crea
	un movimiento incompleto.

El detalle de cada comprobación está en
[EvidenciaEjecucion/patrones/README.md](EvidenciaEjecucion/patrones/README.md).

## Documentación y diagramas

- [Enunciado y rúbrica](Reto2_Patrones_Enunciado_y_Rubrica.pdf)
- [Documentación final](Reto2-Documentacion-finalizado.pdf)
- [Diagrama AS-IS](DiagramasUML/AS-IS/BIbliotecaFarmacia_AS-IS.png)
- [Diagrama TO-BE](DiagramasUML/TO-BE/BIbliotecaFarmacia_TO-BE.dia.png)


