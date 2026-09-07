using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BibFarmacia.Interfaces;

namespace BibFarmacia.Servicios
{
    public class ServicioAutenticacion
    {
        private readonly IAutenticador autenticador;

        public ServicioAutenticacion(
            IAutenticador autenticador)
        {
            this.autenticador = autenticador;
        }

        public bool Login(
            string user,
            string password)
        {
            return autenticador.Autenticar(
                user,
                password);
        }
    }
}
