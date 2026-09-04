using BibFarmacia.Clases;
using BibFarmacia.Interfaces;

namespace BibFarmacia.Factories
{
    public class FabricaMedicamentos : IFabricaMedicamentos
    {
        public MedicamentoCapsula Crear(
            string nombre,
            decimal precio,
            int stock,
            int stockMinimo,
            DateTime fechaVencimiento,
            Laboratorio laboratorio,
            IRelleno relleno)
        {
            return new MedicamentoCapsula(
                nombre,
                precio,
                stock,
                stockMinimo,
                fechaVencimiento,
                laboratorio,
                relleno);
        }

        public MedicamentoLiquido Crear(
            string nombre,
            decimal precio,
            int stock,
            int stockMinimo,
            DateTime fechaVencimiento,
            Laboratorio laboratorio,
            IEnvase envase,
            int mililitros)
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