using BibFarmacia.Clases;
using BibFarmacia.Interfaces;

namespace BibFarmacia.Factories
{
    public class CreadorCapsula : ICreadorMedicamento
    {
        private readonly IRelleno relleno;

        public CreadorCapsula(IRelleno relleno)
        {
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
            return new MedicamentoCapsula(
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
