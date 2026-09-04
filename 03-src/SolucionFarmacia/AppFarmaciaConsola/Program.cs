using BibFarmacia.Auth;
using BibFarmacia.Clases;
using BibFarmacia.Factories;
using BibFarmacia.Interfaces;
using BibFarmacia.Repositorios;
using BibFarmacia.Servicios;

Console.Title = "Sistema Farmacia";

static string ResolverRutaDatos(string nombreArchivo)
{
    string rutaBase = Path.Combine(AppContext.BaseDirectory, nombreArchivo);

    if (File.Exists(rutaBase))
    {
        return rutaBase;
    }

    string rutaActual = Path.Combine(Directory.GetCurrentDirectory(), nombreArchivo);

    if (File.Exists(rutaActual))
    {
        return rutaActual;
    }

    return rutaBase;
}

string rutaProductos = ResolverRutaDatos("productos.txt");
string rutaClientes = ResolverRutaDatos("clientes.txt");
string rutaUsuarios = ResolverRutaDatos("usuarios.txt");
string rutaServicios = ResolverRutaDatos("servicios.txt");
string rutaNotificaciones = ResolverRutaDatos("notificaciones.log");

// Composition root: único lugar donde se
// instancian los adaptadores de persistencia

// Registro de creadores para la carga desde archivo
// (Factory Method — un tipo de medicamento nuevo es
// un creador nuevo más una línea aquí, sin tocar
// RepositorioProductos)
Dictionary<string, ICreadorMedicamento> creadoresMedicamento =
    new Dictionary<string, ICreadorMedicamento>
    {
        ["capsula"] = new CreadorCapsula(new RellenoGel()),
        ["liquido"] = new CreadorLiquido(new EnvaseVidrio(), 100)
    };

ServicioProducto servicioProducto =
    new ServicioProducto(
        new RepositorioProductos(creadoresMedicamento),
        new FabricaMedicamentos());

ServicioCliente servicioCliente =
    new ServicioCliente(
        new RepositorioClientes());

ServicioUsuario servicioUsuario =
    new ServicioUsuario(
        new RepositorioUsuarios());

ServicioMovimiento servicioMovimiento =
    new ServicioMovimiento();

ServicioObjetoServicio servicioObjetoServicio =
    new ServicioObjetoServicio(
        new RepositorioServicios());

ServicioFacturacion servicioFacturacion =
    new ServicioFacturacion();

ServicioInventario servicioInventario =
    new ServicioInventario();

// Regla de puntos vigente: configuración del
// administrador para la promoción del día
// (Strategy — cambiar de ReglaPuntosEstandar a
// ReglaPuntosDoble es una línea aquí, sin tocar
// Cliente, ServicioPuntos ni el menú)
ServicioPuntos servicioPuntos =
    new ServicioPuntos(
        new ReglaPuntosEstandar());

ServicioAutenticacion servicioAutenticacion =
    new ServicioAutenticacion(
        new AutenticadorArchivo(
            new RepositorioUsuarios(),
            rutaUsuarios));

ServicioVenta servicioVenta =
    new ServicioVenta(servicioMovimiento);

// Canal de notificación: observador independiente de
// la venta, suscrito al mismo evento de dominio que
// ya consume la consola. No imprime nada — deja
// evidencia en notificaciones.log — así que no
// altera la salida por consola.
IServicioNotificacion servicioNotificacion =
    new NotificacionArchivo(rutaNotificaciones);

// Registro de estrategias disponibles: agregar un
// relleno/envase nuevo es una línea aquí, sin tocar
// los servicios ni el menú (OCP)
Dictionary<string, Func<IRelleno>> rellenosDisponibles =
    new Dictionary<string, Func<IRelleno>>
    {
        ["gel"] = () => new RellenoGel(),
        ["polvo"] = () => new RellenoPolvo()
    };

Dictionary<string, Func<IEnvase>> envasesDisponibles =
    new Dictionary<string, Func<IEnvase>>
    {
        ["vidrio"] = () => new EnvaseVidrio(),
        ["plastico"] = () => new EnvasePlastico()
    };

// ================= EVENTOS =================

servicioInventario.EventoStock.StockMinimo +=
    mensaje =>
    {
        Console.ForegroundColor =
            ConsoleColor.Red;

        Console.WriteLine(mensaje);

        Console.ResetColor();
    };

servicioInventario.EventoVencimiento.Vencimiento +=
    mensaje =>
    {
        Console.ForegroundColor =
            ConsoleColor.Yellow;

        Console.WriteLine(mensaje);

        Console.ResetColor();
    };

servicioPuntos.EventoPuntos.PuntosAcumulados +=
    mensaje =>
    {
        Console.ForegroundColor =
            ConsoleColor.Green;

        Console.WriteLine(mensaje);

        Console.ResetColor();
    };

servicioMovimiento.EventoMovimiento
    .MovimientoRegistrado +=
    mensaje =>
    {
        Console.ForegroundColor =
            ConsoleColor.Cyan;

        Console.WriteLine(mensaje);

        Console.ResetColor();
    };

// Segundo observador del mismo evento (Observer):
// el canal de notificación no reemplaza a la
// consola, coexiste con ella
servicioMovimiento.EventoMovimiento
    .MovimientoRegistrado +=
    mensaje =>
        servicioNotificacion.EnviarNotificacion(
            mensaje);

// ================= CARGA TXT =================

Console.ForegroundColor =
    ConsoleColor.DarkGreen;

Console.WriteLine(
    "Cargando información del sistema...\n");

Console.ResetColor();

Console.WriteLine(
    servicioProducto.CargarDesdeArchivo(
        rutaProductos));

Console.WriteLine(
    servicioCliente.Cargar(
        rutaClientes));

Console.WriteLine(
    servicioUsuario.Cargar(
        rutaUsuarios));

// SC-2: carga silenciosa para preservar
// la salida original del sistema
servicioObjetoServicio.CargarDesdeArchivo(
    rutaServicios);

Console.WriteLine();

// ================= LOGIN =================

Console.ForegroundColor =
    ConsoleColor.Blue;

Console.WriteLine(
    "=========== LOGIN ===========");

Console.ResetColor();

Console.Write("Usuario: ");
string user =
    Console.ReadLine()!;

Console.Write("Contraseña: ");
string password =
    Console.ReadLine()!;

bool login =
    servicioAutenticacion.Login(
        user,
        password);

if (!login)
{
    Console.ForegroundColor =
        ConsoleColor.Red;

    Console.WriteLine(
        "\nAcceso denegado");

    Console.ResetColor();

    return;
}

Console.ForegroundColor =
    ConsoleColor.Green;

Console.WriteLine(
    "\nLogin correcto");

Console.ResetColor();

// ================= ALERTAS =================

servicioInventario.VerificarStock(
    servicioProducto.ObtenerProductos());

servicioInventario.VerificarVencimiento(
    servicioProducto.ObtenerProductos());

// ================= MENÚ =================

int opcion = 0;

while (opcion != 11)
{
    Console.ForegroundColor =
        ConsoleColor.Magenta;

    Console.WriteLine("\n==============================");
    Console.WriteLine("      SISTEMA FARMACIA");
    Console.WriteLine("==============================");

    Console.ResetColor();

    Console.WriteLine("1. Ver productos");
    Console.WriteLine("2. Ver servicios");
    Console.WriteLine("3. Ver clientes");
    Console.WriteLine("4. Ver movimientos");
    Console.WriteLine("5. Ver alertas");
    Console.WriteLine("6. Agregar producto");
    Console.WriteLine("7. Agregar servicio");
    Console.WriteLine("8. Buscar producto");
    Console.WriteLine("9. Registrar venta");
    Console.WriteLine("10. Acumular puntos");
    Console.WriteLine("11. Salir");

    Console.Write("\nSeleccione opción: ");

    opcion =
        int.Parse(Console.ReadLine()!);

    switch (opcion)
    {
        case 1:

            Console.ForegroundColor =
                ConsoleColor.Cyan;

            Console.WriteLine(
                "\n===== PRODUCTOS =====");

            Console.ResetColor();

            Console.WriteLine(
                "Nombre\t\tStock\tPrecio");

            Console.WriteLine(
                "-----------------------------------");

            foreach (var producto in
                servicioProducto.ObtenerProductos())
            {
                Console.WriteLine(
                    producto.MostrarInformacion());
            }

            break;

        case 2:

            Console.ForegroundColor =
                ConsoleColor.DarkCyan;

            Console.WriteLine(
                "\n===== SERVICIOS =====");

            Console.ResetColor();

            Console.WriteLine(
                "Nombre\t\tPrecio\tDuración");

            Console.WriteLine(
                "-----------------------------------");

            foreach (var servicio in
                servicioObjetoServicio
                .ObtenerServicios())
            {
                Console.WriteLine(
                    servicio.MostrarInformacion());
            }

            break;

        case 3:

            Console.ForegroundColor =
                ConsoleColor.Green;

            Console.WriteLine(
                "\n===== CLIENTES =====");

            Console.ResetColor();

            foreach (var cliente in
                servicioCliente.ObtenerClientes())
            {
                Console.WriteLine(
                    $"{cliente.Nombre} - " +
                    $"Puntos: {cliente.Puntos}");
            }

            break;

        case 4:

            Console.ForegroundColor =
                ConsoleColor.DarkYellow;

            Console.WriteLine(
                "\n===== MOVIMIENTOS =====");

            Console.ResetColor();

            foreach (var movimiento in
                servicioMovimiento
                .ObtenerMovimientos())
            {
                string categoria =
                    movimiento.Facturable
                        is IInventariable
                        ? "Producto"
                        : "Servicio";

                Console.WriteLine(
                    $"{movimiento.Fecha:yyyy-MM-dd HH:mm} - " +
                    $"{movimiento.Tipo} - " +
                    $"{categoria} - " +
                    $"{movimiento.Facturable.Nombre} " +
                    $"x{movimiento.Cantidad} - " +
                    $"Total: {servicioFacturacion.CalcularTotal(movimiento.Facturable, movimiento.Cantidad)}");
            }

            break;

        case 5:

            Console.WriteLine(
                "\nVerificando alertas...");

            servicioInventario
                .VerificarStock(
                    servicioProducto
                    .ObtenerProductos());

            servicioInventario
                .VerificarVencimiento(
                    servicioProducto
                    .ObtenerProductos());

            break;

        case 6:

            Console.Write(
                "\nTipo (capsula/liquido): ");

            string tipoProducto =
                Console.ReadLine()!.ToLower();

            if (tipoProducto != "capsula" &&
                tipoProducto != "liquido")
            {
                Console.WriteLine(
                    "\nTipo no válido");

                break;
            }

            Console.Write("Nombre: ");
            string nombreNuevo =
                Console.ReadLine()!;

            Console.Write("Precio: ");
            decimal precioNuevo =
                decimal.Parse(Console.ReadLine()!);

            Console.Write("Stock: ");
            int stockNuevo =
                int.Parse(Console.ReadLine()!);

            Console.Write("Stock mínimo: ");
            int stockMinimoNuevo =
                int.Parse(Console.ReadLine()!);

            Console.Write(
                "Fecha vencimiento (yyyy-MM-dd): ");
            DateTime fechaNueva =
                DateTime.Parse(Console.ReadLine()!);

            Console.Write("Laboratorio - Nombre: ");
            string laboratorioNombre =
                Console.ReadLine()!;

            Console.Write("Laboratorio - Dirección: ");
            string laboratorioDireccion =
                Console.ReadLine()!;

            Console.Write("Laboratorio - Teléfono: ");
            string laboratorioTelefono =
                Console.ReadLine()!;

            Laboratorio laboratorioNuevo =
                new Laboratorio(
                    laboratorioNombre,
                    laboratorioDireccion,
                    laboratorioTelefono);

            /*  Este punto de decisión del menú solo selecciona la estrategia de
                entrada para el producto. La creación concreta queda delegada al
                servicio y, finalmente, a la fábrica, para cumplir de manera más
                estricta con OCP: no se deben agregar nuevos métodos al servicio
                por cada tipo de producto. */

            if (tipoProducto == "capsula")
            {
                Console.Write(
                    "Relleno (" +
                    string.Join("/",
                        rellenosDisponibles.Keys) +
                    "): ");

                string rellenoElegido =
                    Console.ReadLine()!.ToLower();

                if (!rellenosDisponibles.TryGetValue(
                    rellenoElegido,
                    out var crearRelleno))
                {
                    Console.WriteLine(
                        "\nRelleno no registrado");

                    break;
                }

                Console.WriteLine(
                    "\n" +
                    servicioProducto.AgregarProducto(
                        nombreNuevo,
                        precioNuevo,
                        stockNuevo,
                        stockMinimoNuevo,
                        fechaNueva,
                        laboratorioNuevo,
                        crearRelleno()));
            }
            else
            {
                Console.Write(
                    "Envase (" +
                    string.Join("/",
                        envasesDisponibles.Keys) +
                    "): ");

                string envaseElegido =
                    Console.ReadLine()!.ToLower();

                if (!envasesDisponibles.TryGetValue(
                    envaseElegido,
                    out var crearEnvase))
                {
                    Console.WriteLine(
                        "\nEnvase no registrado");

                    break;
                }

                Console.Write("Mililitros: ");
                int mililitrosNuevos =
                    int.Parse(Console.ReadLine()!);

                Console.WriteLine(
                    "\n" +
                    servicioProducto.AgregarProducto(
                        nombreNuevo,
                        precioNuevo,
                        stockNuevo,
                        stockMinimoNuevo,
                        fechaNueva,
                        laboratorioNuevo,
                        crearEnvase(),
                        mililitrosNuevos));
            }

            break;

        case 7:

            Console.Write("\nNombre: ");
            string nombreServicioNuevo =
                Console.ReadLine()!;

            Console.Write("Precio: ");
            decimal precioServicioNuevo =
                decimal.Parse(Console.ReadLine()!);

            Console.Write("Duración (minutos): ");
            int duracionNueva =
                int.Parse(Console.ReadLine()!);

            Console.WriteLine(
                "\n" +
                servicioObjetoServicio.AgregarServicio(
                    nombreServicioNuevo,
                    precioServicioNuevo,
                    duracionNueva));

            break;

        case 8:

            Console.Write(
                "\nIngrese nombre producto: ");

            string nombre =
                Console.ReadLine()!;

            var productoBuscado =
                servicioProducto
                .ObtenerProductos()
                .FirstOrDefault(p =>
                    p.Nombre.ToLower()
                    .Contains(nombre.ToLower()));

            if (productoBuscado != null)
            {
                Console.WriteLine(
                    $"\nProducto: " +
                    $"{productoBuscado.Nombre}");

                Console.WriteLine(
                    $"Precio: " +
                    $"{productoBuscado.Precio}");

                Console.WriteLine(
                    $"Stock: " +
                    $"{productoBuscado.Stock}");
            }
            else
            {
                Console.WriteLine(
                    "\nProducto no encontrado");
            }

            break;

        case 9:

            Console.WriteLine(
                "\n1. Producto");
            Console.WriteLine(
                "2. Servicio");

            Console.Write(
                "Tipo de venta: ");

            string tipoVenta =
                Console.ReadLine()!;

            if (tipoVenta != "1" &&
                tipoVenta != "2")
            {
                Console.WriteLine(
                    "\nOpción inválida");

                break;
            }

            IEnumerable<IFacturable> candidatos =
                tipoVenta == "1"
                    ? servicioProducto
                        .ObtenerProductos()
                    : servicioObjetoServicio
                        .ObtenerServicios();

            Console.Write(
                tipoVenta == "1"
                    ? "Nombre producto: "
                    : "Nombre servicio: ");

            string nombreVenta =
                Console.ReadLine()!;

            var facturableVenta =
                servicioVenta.BuscarFacturable(
                    candidatos,
                    nombreVenta);

            if (facturableVenta != null)
            {
                Console.Write(
                    "Cantidad: ");

                int cantidad =
                    int.Parse(
                        Console.ReadLine()!);

                Console.WriteLine(
                    "\n" +
                    servicioVenta.RegistrarVenta(
                        facturableVenta,
                        cantidad));
            }
            else
            {
                Console.WriteLine(
                    tipoVenta == "1"
                        ? "\nProducto no encontrado"
                        : "\nServicio no encontrado");
            }

            break;

        case 10:

            Console.Write(
                "\nNombre cliente: ");

            string nombreCliente =
                Console.ReadLine()!;

            var clientePuntos =
                servicioCliente
                .ObtenerClientes()
                .FirstOrDefault(c =>
                    c.Nombre.ToLower()
                    .Contains(
                        nombreCliente.ToLower()));

            if (clientePuntos != null)
            {
                Console.Write(
                    "Puntos: ");

                int puntos =
                    int.Parse(
                        Console.ReadLine()!);

                servicioPuntos
                    .AcumularPuntos(
                        clientePuntos,
                        puntos);
            }
            else
            {
                Console.WriteLine(
                    "\nCliente no encontrado");
            }

            break;

        case 11:

            Console.ForegroundColor =
                ConsoleColor.Red;

            Console.WriteLine(
                "\nSaliendo del sistema...");

            Console.ResetColor();

            break;

        default:

            Console.WriteLine(
                "\nOpción inválida");

            break;
    }
}

Console.WriteLine(
    "\nFIN DEL SISTEMA");
