using BibFarmacia.Clases;
using BibFarmacia.Interfaces;

namespace BibFarmacia.Factories
{
    public class CreadorLiquido : ICreadorMedicamento
    {
        private readonly IEnvase envase;
        private readonly int mililitros;

        public CreadorLiquido(IEnvase envase, int mililitros)
        {
            this.envase = envase;
            this.mililitros = mililitros;
        }

        public Medicamento Crear(
            string nombre,
            decimal precio,
            int stock,
            int stockMinimo,
            DateTime fechaVencimiento,
            Laboratorio laboratorio)
        {
            return new MedicamentoLiquido(
                nombre,
                precio,
                stock,
                stockMinimo,
                fechaVencimiento,
                laboratorio,
                envase,
                mililitros);
        }
    }
}
