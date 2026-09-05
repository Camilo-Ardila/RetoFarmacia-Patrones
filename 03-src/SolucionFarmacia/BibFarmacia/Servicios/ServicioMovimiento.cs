using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BibFarmacia.Clases;
using BibFarmacia.Eventos;

namespace BibFarmacia.Servicios
{
    public class ServicioMovimiento
    {
        private List<Movimiento> movimientos;
        private readonly EventoVenta eventoVenta;

        public ServicioMovimiento(EventoVenta eventoVenta)
        {
            this.eventoVenta = eventoVenta;
            movimientos = new List<Movimiento>();
        }

        public void ProcesarVentaProcesada(ContextoVenta contexto)
        {
            Movimiento movimiento =
                new Movimiento(
                    DateTime.Now,
                    contexto.Cantidad,
                    "Venta",
                    contexto.Facturable,
                    contexto.Cliente,
                    contexto.Subtotal,
                    contexto.Descuento,
                    contexto.Total);

            movimientos.Add(movimiento);
            contexto.Estado = EstadoVenta.Confirmada;
            eventoVenta.DispararMovimientoRegistrado(contexto);
        }

        public List<Movimiento>
            ObtenerMovimientos()
        {
            return movimientos;
        }
    }
}