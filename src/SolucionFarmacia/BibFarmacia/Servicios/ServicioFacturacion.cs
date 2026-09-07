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
    public class ServicioFacturacion
    {
        private readonly EventoVenta eventoVenta;

        public ServicioFacturacion(EventoVenta eventoVenta)
        {
            this.eventoVenta = eventoVenta;
        }

        public void ProcesarVentaSolicitada(ContextoVenta contexto)
        {
            try
            {
                decimal subtotal =
                    CalcularTotal(
                        contexto.Facturable,
                        contexto.Cantidad);

                decimal descuento =
                    contexto.Cliente.Descuento
                        .CalcularDescuento(subtotal);

                if (descuento < 0 || descuento > subtotal)
                {
                    throw new InvalidOperationException(
                        "El descuento no es válido.");
                }

                contexto.Subtotal = subtotal;
                contexto.Descuento = descuento;
                contexto.Total = subtotal - descuento;
                contexto.Facturar();

                eventoVenta
                    .DispararFacturaCalculada(contexto);
            }
            catch (Exception ex)
            {
                contexto.Fallar(ex.Message);
            }
        }

        public decimal CalcularTotal(
            IFacturable facturable,
            int cantidad)
        {
            return facturable.Precio *
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
