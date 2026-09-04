using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BibFarmacia.Clases;
using BibFarmacia.Interfaces;

namespace BibFarmacia.Repositorios
{
    public class RepositorioUsuarios : IRepositorio<Usuario>
    {
        public List<Usuario> Cargar(
            string ruta)
        {
            if (!File.Exists(ruta))
            {
                throw new FileNotFoundException(
                    "Archivo no encontrado");
            }

            List<Usuario> usuarios =
                new List<Usuario>();

            string[] lineas =
                File.ReadAllLines(ruta);

            foreach (string linea in lineas)
            {
                string[] datos =
                    linea.Split(';');

                Usuario usuario =
                    new Usuario(
                        datos[0],
                        datos[1],
                        datos[2],
                        datos[3],
                        datos[4],
                        datos[5]);

                usuarios.Add(usuario);
            }

            return usuarios;
        }
    }
}
