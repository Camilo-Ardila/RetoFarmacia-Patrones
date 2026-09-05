using BibFarmacia.Clases;

namespace BibFarmacia.Eventos
{
    public class EventoVenta
    {
        public event Action<ContextoVenta>? VentaSolicitada;
        public event Action<ContextoVenta>? FacturaCalculada;
        public event Action<ContextoVenta>? VentaProcesada;
        public event Action<ContextoVenta>? MovimientoRegistrado;

        public void DispararVentaSolicitada(ContextoVenta contexto)
        {
            VentaSolicitada?.Invoke(contexto);
        }

        public void DispararFacturaCalculada(ContextoVenta contexto)
        {
            FacturaCalculada?.Invoke(contexto);
        }

        public void DispararVentaProcesada(ContextoVenta contexto)
        {
            VentaProcesada?.Invoke(contexto);
        }

        public void DispararMovimientoRegistrado(ContextoVenta contexto)
        {
            MovimientoRegistrado?.Invoke(contexto);
        }
    }
}