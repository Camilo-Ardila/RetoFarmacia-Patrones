using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BibFarmacia.Clases;
using BibFarmacia.Eventos;
using BibFarmacia.Interfaces;

namespace BibFarmacia.Servicios
{
    public class ServicioVenta
    {
        private readonly EventoVenta eventoVenta;

        public ServicioVenta(EventoVenta eventoVenta)
        {
            this.eventoVenta = eventoVenta;
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
            Cliente cliente,
            IFacturable facturable,
            int cantidad)
        {
            try
            {
                ContextoVenta contexto =
                    new ContextoVenta(
                        cliente,
                        facturable,
                        cantidad);

                eventoVenta
                    .DispararVentaSolicitada(contexto);

                return contexto.Estado == EstadoVenta.Confirmada
                    ? $"Venta registrada. Total: {contexto.Total}"
                    : $"Venta no registrada: {contexto.Error}";
            }
            catch (Exception ex)
            {
                return $"Venta no registrada: {ex.Message}";
            }
        }
    }
}
