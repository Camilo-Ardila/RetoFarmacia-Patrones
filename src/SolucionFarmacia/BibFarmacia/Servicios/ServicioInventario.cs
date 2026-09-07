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
    public class ServicioInventario
    {
        private readonly EventoVenta eventoVenta;

        public EventoStockMinimo EventoStock;
        public EventoVencimiento EventoVencimiento;

        public ServicioInventario(EventoVenta eventoVenta)
        {
            this.eventoVenta = eventoVenta;
            EventoStock = new EventoStockMinimo();
            EventoVencimiento = new EventoVencimiento();
        }

        public void ProcesarFacturaCalculada(ContextoVenta contexto)
        {
            try
            {
                if (contexto.Facturable is IInventariable inventariable)
                {
                    inventariable.DescontarStock(contexto.Cantidad);
                }

                contexto.Procesar();
                eventoVenta.DispararVentaProcesada(contexto);
            }
            catch (Exception ex)
            {
                contexto.Fallar(ex.Message);
            }
        }

        public void VerificarStock(
            List<Producto> productos)
        {
            foreach (var producto in productos)
            {
                if (producto.Stock <=
                    producto.StockMinimo)
                {
                    EventoStock.Disparar(producto);
                }
            }
        }

        public void VerificarVencimiento(
            List<Producto> productos)
        {
            foreach (var producto in productos)
            {
                int dias =
                    (producto.FechaVencimiento -
                    DateTime.Now).Days;

                if (dias <= 30)
                {
                    EventoVencimiento
                        .Disparar(producto);
                }
            }
        }
    }
}
