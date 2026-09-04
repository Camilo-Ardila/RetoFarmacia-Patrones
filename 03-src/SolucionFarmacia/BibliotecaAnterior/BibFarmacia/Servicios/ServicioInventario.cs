using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BibFarmacia.Clases;
using BibFarmacia.Eventos;

namespace BibFarmacia.Servicios
{
    public class ServicioInventario
    {
        public EventoStockMinimo EventoStock;
        public EventoVencimiento EventoVencimiento;

        public ServicioInventario()
        {
            EventoStock = new EventoStockMinimo();
            EventoVencimiento = new EventoVencimiento();
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
