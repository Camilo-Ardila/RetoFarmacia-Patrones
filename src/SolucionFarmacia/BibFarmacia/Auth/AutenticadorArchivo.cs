using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BibFarmacia.Clases;
using BibFarmacia.Interfaces;

namespace BibFarmacia.Auth
{
    public class AutenticadorArchivo : IAutenticador
    {
        private readonly IRepositorio<Usuario> repositorio;
        private readonly string ruta;

        public AutenticadorArchivo(
            IRepositorio<Usuario> repositorio,
            string ruta)
        {
            this.repositorio = repositorio;
            this.ruta = ruta;
        }

        public bool Autenticar(
            string user,
            string password)
        {
            try
            {
                List<Usuario> usuarios =
                    repositorio.Cargar(ruta);

                return usuarios.Any(u =>
                    u.UserName == user &&
                    u.Password == password);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
