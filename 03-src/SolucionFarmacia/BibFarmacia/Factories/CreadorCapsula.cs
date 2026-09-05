using BibFarmacia.Clases;
using BibFarmacia.Interfaces;

namespace BibFarmacia.Factories
{
    public class CreadorCapsula : ICreadorMedicamento
    {
        private readonly IFabricaMedicamentos fabrica;
        private readonly IRelleno relleno;

        public CreadorCapsula(
            IFabricaMedicamentos fabrica,
            IRelleno relleno)
        {
            this.fabrica = fabrica;
            this.relleno = relleno;
        }

        public Medicamento Crear(
            string nombre,
            decimal precio,
            int stock,
            int stockMinimo,
            DateTime fechaVencimiento,
            Laboratorio laboratorio)
        {
            return fabrica.Crear(
                nombre,
                precio,
                stock,
                stockMinimo,
                fechaVencimiento,
                laboratorio,
                relleno);
        }
    }
}