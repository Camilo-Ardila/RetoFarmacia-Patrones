using BibFarmacia.Clases;
using BibFarmacia.Interfaces;

namespace BibFarmacia.Clases.EstadosVenta
{
    public abstract class EstadoVentaBase : IEstadoVenta
    {
        public abstract string Nombre { get; }

        public virtual void Facturar(ContextoVenta contexto)
        {
            throw new InvalidOperationException(
                $"No se puede facturar una venta en estado {Nombre}.");
        }

        public virtual void Procesar(ContextoVenta contexto)
        {
            throw new InvalidOperationException(
                $"No se puede procesar una venta en estado {Nombre}.");
        }

        public virtual void Confirmar(ContextoVenta contexto)
        {
            throw new InvalidOperationException(
                $"No se puede confirmar una venta en estado {Nombre}.");
        }

        public virtual void Fallar(ContextoVenta contexto, string mensaje)
        {
            contexto.CambiarEstado(
                new EstadoVentaFallida(),
                mensaje);
        }
    }
}
