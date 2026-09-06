using BibFarmacia.Clases;

namespace BibFarmacia.Clases.EstadosVenta
{
    public class Facturada : Base
    {
        public override string Nombre => "Facturada";

        public override void Procesar(ContextoVenta contexto)
        {
            contexto.CambiarEstado(
                new Procesada());
        }
    }
}
