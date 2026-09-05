using BibFarmacia.Clases;

namespace BibFarmacia.Interfaces
{
    public interface ICreadorMedicamento
    {
        Medicamento Crear(
            string nombre,
            decimal precio,
            int stock,
            int stockMinimo,
            DateTime fechaVencimiento,
            Laboratorio laboratorio);
    }
}