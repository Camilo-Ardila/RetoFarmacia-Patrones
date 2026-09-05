using BibFarmacia.Clases;

namespace BibFarmacia.Clases.EstadosVenta
{
    public class EstadoVentaFacturada : EstadoVentaBase
    {
        public override string Nombre => "Facturada";

        public override void Procesar(ContextoVenta contexto)
        {
            contexto.CambiarEstado(
                new EstadoVentaProcesada());
        }
    }
}
