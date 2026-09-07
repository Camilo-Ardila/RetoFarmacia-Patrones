using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BibFarmacia.Clases;
using BibFarmacia.Interfaces;

namespace BibFarmacia.Repositorios
{
    public class RepositorioClientes : IRepositorio<Cliente>
    {
        public List<Cliente> Cargar(
            string ruta)
        {
            if (!File.Exists(ruta))
            {
                throw new FileNotFoundException(
                    "Archivo no encontrado");
            }

            List<Cliente> clientes =
                new List<Cliente>();

            string[] lineas =
                File.ReadAllLines(ruta);

            foreach (string linea in lineas)
            {
                string[] datos =
                    linea.Split(';');

                Cliente cliente =
                    new Cliente(
                        datos[0],
                        datos[1],
                        datos[2],
                        datos[3]);

                clientes.Add(cliente);
            }

            return clientes;
        }
    }
}
