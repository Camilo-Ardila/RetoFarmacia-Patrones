using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BibFarmacia.Interfaces;

namespace BibFarmacia.Servicios
{
    public class ServicioFacturacion
    {
        public decimal CalcularTotal(
            IFacturable facturable,
            int cantidad)
        {
            return facturable.ObtenerPrecio() *
                cantidad;
        }
    }
}
