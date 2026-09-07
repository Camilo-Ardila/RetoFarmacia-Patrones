using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BibFarmacia.Clases;
using BibFarmacia.Interfaces;

namespace BibFarmacia.Repositorios
{
    public class RepositorioServicios : IRepositorio<Servicio>
    {
        public List<Servicio> Cargar(
            string ruta)
        {
            if (!File.Exists(ruta))
            {
                throw new FileNotFoundException(
                    "Archivo no encontrado");
            }

            List<Servicio> servicios =
                new List<Servicio>();

            string[] lineas =
                File.ReadAllLines(ruta);

            foreach (string linea in lineas)
            {
                string[] datos =
                    linea.Split(';');

                Servicio servicio =
                    new Servicio(
                        datos[0],
                        decimal.Parse(datos[1]),
                        int.Parse(datos[2]));

                servicios.Add(servicio);
            }

            return servicios;
        }
    }
}
