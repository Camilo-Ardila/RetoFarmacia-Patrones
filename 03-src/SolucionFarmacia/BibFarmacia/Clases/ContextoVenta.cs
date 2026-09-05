using BibFarmacia.Interfaces;

namespace BibFarmacia.Clases
{
    public enum EstadoVenta
    {
        Pendiente,
        Facturada,
        Procesada,
        Confirmada,
        Fallida
    }

    public class ContextoVenta
    {
        public Cliente Cliente { get; }
        public IFacturable Facturable { get; }
        public int Cantidad { get; }
        public decimal Subtotal { get; set; }
        public decimal Descuento { get; set; }
        public decimal Total { get; set; }
        public EstadoVenta Estado { get; set; }
        public string? Error { get; set; }

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
            Estado = EstadoVenta.Pendiente;
        }

        public void Fallar(string mensaje)
        {
            Error = mensaje;
            Estado = EstadoVenta.Fallida;
        }
    }
}