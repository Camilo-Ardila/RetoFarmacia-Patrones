using BibFarmacia.Clases;

namespace BibFarmacia.Interfaces
{
    public interface IFabricaMedicamentos
    {
        Medicamento Crear(RegistroProducto registro);

        MedicamentoCapsula Crear(
            string nombre,
            decimal precio,
            int stock,
            int stockMinimo,
            DateTime fechaVencimiento,
            Laboratorio laboratorio,
            IRelleno relleno);

        MedicamentoLiquido Crear(
            string nombre,
            decimal precio,
            int stock,
            int stockMinimo,
            DateTime fechaVencimiento,
            Laboratorio laboratorio,
            IEnvase envase,
            int mililitros);
    }
}