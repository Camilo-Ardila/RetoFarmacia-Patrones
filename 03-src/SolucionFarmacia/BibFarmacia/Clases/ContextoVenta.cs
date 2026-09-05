using BibFarmacia.Clases.EstadosVenta;
using BibFarmacia.Interfaces;

namespace BibFarmacia.Clases
{
    public class ContextoVenta
    {
        public Cliente Cliente { get; }
        public IFacturable Facturable { get; }
        public int Cantidad { get; }
        public decimal Subtotal { get; set; }
        public decimal Descuento { get; set; }
        public decimal Total { get; set; }
        public IEstadoVenta Estado { get; private set; }
        public string? Error { get; set; }

        public bool EstaConfirmada =>
            Estado is EstadoVentaConfirmada;

        public ContextoVenta(
            Cliente cliente,
            IFacturable facturable,
            int cantidad)
        {
            Cliente = cliente ?? throw new ArgumentNullException(nameof(cliente));
            Facturable = facturable ?? throw new ArgumentNullException(nameof(facturable));
            Cantidad = cantidad > 0
                ? cantidad
                : throw new ArgumentOutOfRangeException(nameof(cantidad));
            Estado = new EstadoVentaPendiente();
        }

        public void Facturar() => Estado.Facturar(this);

        public void Procesar() => Estado.Procesar(this);

        public void Confirmar() => Estado.Confirmar(this);

        public void Fallar(string mensaje)
        {
            Estado.Fallar(this, mensaje);
        }

        internal void CambiarEstado(
            IEstadoVenta estado,
            string? error = null)
        {
            Estado = estado;
            Error = error;
        }
    }
}