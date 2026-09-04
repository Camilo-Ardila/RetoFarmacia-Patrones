using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BibFarmacia.Clases;
using BibFarmacia.Interfaces;

namespace BibFarmacia.Servicios
{
    public class ServicioVenta
    {
        private readonly IRegistroMovimientos registroMovimientos;

        public ServicioVenta(
            IRegistroMovimientos registroMovimientos)
        {
            this.registroMovimientos = registroMovimientos;
        }

        public IFacturable? BuscarFacturable(
            IEnumerable<IFacturable> facturables,
            string nombre)
        {
            return facturables.FirstOrDefault(f =>
                f.Nombre.ToLower()
                .Contains(nombre.ToLower()));
        }

        public string RegistrarVenta(
            IFacturable facturable,
            int cantidad)
        {
            if (facturable is
                IInventariable inventariable)
            {
                inventariable
                    .DescontarStock(cantidad);
            }

            Movimiento venta =
                new Movimiento(
                    DateTime.Now,
                    cantidad,
                    "Venta",
                    facturable);

            registroMovimientos
                .RegistrarMovimiento(venta);

            return "Venta registrada";
        }
    }
}
