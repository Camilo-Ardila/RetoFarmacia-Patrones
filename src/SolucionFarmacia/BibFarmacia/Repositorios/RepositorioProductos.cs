using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BibFarmacia.Clases;
using BibFarmacia.Interfaces;

namespace BibFarmacia.Repositorios
{
    public class RepositorioProductos : IRepositorio<RegistroProducto>
    {
        public List<RegistroProducto> Cargar(
            string ruta)
        {
            if (!File.Exists(ruta))
            {
                throw new FileNotFoundException(
                    "Archivo no encontrado");
            }

            List<RegistroProducto> registros =
                new List<RegistroProducto>();

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

                registros.Add(
                    new RegistroProducto(
                    datos[0].ToLowerInvariant(),
                    datos[1],
                    decimal.Parse(datos[2]),
                    int.Parse(datos[3]),
                    int.Parse(datos[4]),
                    DateTime.Parse(datos[5]),
                    datos[6]));
            }

            return registros;
        }
    }
}
