using BibFarmacia.Clases;

namespace BibFarmacia.Clases.EstadosVenta
{
    public class EstadoVentaProcesada : EstadoVentaBase
    {
        public override string Nombre => "Procesada";

        public override void Confirmar(ContextoVenta contexto)
        {
            contexto.CambiarEstado(
                new EstadoVentaConfirmada());
        }
    }
}
