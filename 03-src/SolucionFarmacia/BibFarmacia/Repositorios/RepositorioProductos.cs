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
        private readonly IReadOnlyDictionary<string, ICreadorMedicamento> creadores;

        public RepositorioProductos(
            IReadOnlyDictionary<string, ICreadorMedicamento> creadores)
        {
            this.creadores = creadores;
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

                if (!creadores.TryGetValue(
                    datos[0].ToLowerInvariant(),
                    out ICreadorMedicamento? creador))
                {
                    throw new NotSupportedException(
                        $"Tipo de producto no soportado: {datos[0]}");
                }

                Producto producto = creador.Crear(
                    datos[1],
                    decimal.Parse(datos[2]),
                    int.Parse(datos[3]),
                    int.Parse(datos[4]),
                    DateTime.Parse(datos[5]),
                    laboratorio);

                productos.Add(producto);
            }

            return productos;
        }
    }
}
