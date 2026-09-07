using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BibFarmacia.Clases;
using BibFarmacia.Interfaces;

namespace BibFarmacia.Servicios
{
    public class ServicioUsuario
    {
        private List<Usuario> usuarios;
        private readonly IRepositorio<Usuario> repositorio;

        public ServicioUsuario(
            IRepositorio<Usuario> repositorio)
        {
            this.repositorio = repositorio;

            usuarios = new List<Usuario>();
        }

        public void AgregarUsuario(
            Usuario usuario)
        {
            usuarios.Add(usuario);
        }

        public List<Usuario> ObtenerUsuarios()
        {
            return usuarios;
        }

        public string Cargar(
            string ruta)
        {
            try
            {
                usuarios.AddRange(
                    repositorio.Cargar(ruta));

                return "Usuarios cargados";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}