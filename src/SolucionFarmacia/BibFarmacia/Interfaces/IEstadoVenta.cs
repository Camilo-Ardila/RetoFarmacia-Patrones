using BibFarmacia.Clases;

namespace BibFarmacia.Interfaces
{
    public interface IEstadoVenta
    {
        string Nombre { get; }

        void Facturar(ContextoVenta contexto);
        void Procesar(ContextoVenta contexto);
        void Confirmar(ContextoVenta contexto);
        void Fallar(ContextoVenta contexto, string mensaje);
    }
}