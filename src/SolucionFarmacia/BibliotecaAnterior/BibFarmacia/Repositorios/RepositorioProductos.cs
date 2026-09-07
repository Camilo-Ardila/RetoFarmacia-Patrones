using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BibFarmacia.Clases;
using BibFarmacia.Factories;
using BibFarmacia.Interfaces;

namespace BibFarmacia.Repositorios
{
    public class RepositorioProductos : IRepositorio<Producto>
    {
        private readonly IFabricadorProducto fabricadorProducto;

        public RepositorioProductos()
            : this(new FabricaProducto())
        {
        }

        public RepositorioProductos(IFabricadorProducto fabricadorProducto)
        {
            this.fabricadorProducto = fabricadorProducto;
        }

        public List<Producto> Cargar(
            string ruta)
        {
            if (!File.Exists(ruta))
            {
                throw new FileNotFoundException(
                    "Archivo no encontrado");
            }

            List<Producto> productos =
                new List<Producto>();

            string[] lineas =
                File.ReadAllLines(ruta);

            foreach (string linea in lineas)
            {
                if (string.IsNullOrWhiteSpace(linea))
                {
                    continue;
                }

                string[] datos =
                    linea.Split(';');

                Laboratorio laboratorio =
                    new Laboratorio(
                        datos[6],
                        "Medellin",
                        "4444444");

                ProductoRequest request = new ProductoRequest
                {
                    TipoProducto = datos[0].ToLowerInvariant() switch
                    {
                        "capsula" => typeof(MedicamentoCapsula),
                        "liquido" => typeof(MedicamentoLiquido),
                        _ => throw new NotSupportedException(
                            $"Tipo de producto no soportado: {datos[0]}")
                    },
                    Nombre = datos[1],
                    Precio = decimal.Parse(datos[2]),
                    Stock = int.Parse(datos[3]),
                    StockMinimo = int.Parse(datos[4]),
                    FechaVencimiento = DateTime.Parse(datos[5]),
                    Laboratorio = laboratorio,
                    Relleno = new RellenoGel(),
                    Envase = new EnvaseVidrio(),
                    Mililitros = 100
                };

                Producto producto =
                    fabricadorProducto.Crear(request);

                productos.Add(producto);
            }

            return productos;
        }
    }
}
