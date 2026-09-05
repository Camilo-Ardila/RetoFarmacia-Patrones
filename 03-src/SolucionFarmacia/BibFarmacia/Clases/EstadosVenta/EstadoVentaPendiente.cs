using BibFarmacia.Clases;

namespace BibFarmacia.Clases.EstadosVenta
{
    public class EstadoVentaPendiente : EstadoVentaBase
    {
        public override string Nombre => "Pendiente";

        public override void Facturar(ContextoVenta contexto)
        {
            contexto.CambiarEstado(
                new EstadoVentaFacturada());
        }
    }
}
