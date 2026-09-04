using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BibFarmacia.Clases;
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

        public decimal CalcularTotal(
            IFacturable facturable,
            int cantidad,
            Cliente cliente)
        {
            decimal subtotal =
                CalcularTotal(facturable, cantidad);

            return subtotal -
                cliente.Descuento.CalcularDescuento(subtotal);
        }
    }
}
