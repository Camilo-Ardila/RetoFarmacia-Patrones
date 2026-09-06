using BibFarmacia.Clases;
using BibFarmacia.Interfaces;

namespace BibFarmacia.Factories
{
    public class FabricaMedicamentos : IFabricaMedicamentos
    {
        private readonly IRelleno rellenoPorDefecto;
        private readonly IEnvase envasePorDefecto;
        private readonly int mililitrosPorDefecto;

        public FabricaMedicamentos(
            IRelleno rellenoPorDefecto,
            IEnvase envasePorDefecto,
            int mililitrosPorDefecto)
        {
            this.rellenoPorDefecto = rellenoPorDefecto;
            this.envasePorDefecto = envasePorDefecto;
            this.mililitrosPorDefecto = mililitrosPorDefecto;
        }

        public Medicamento Crear(RegistroProducto registro)
        {
            Laboratorio laboratorio =
                new Laboratorio(
                    registro.Laboratorio,
                    "Medellin",
                    "4444444");

            return registro.Tipo switch
            {
                "capsula" => Crear(
                    registro.Nombre,
                    registro.Precio,
                    registro.Stock,
                    registro.StockMinimo,
                    registro.FechaVencimiento,
                    laboratorio,
                    rellenoPorDefecto),
                "liquido" => Crear(
                    registro.Nombre,
                    registro.Precio,
                    registro.Stock,
                    registro.StockMinimo,
                    registro.FechaVencimiento,
                    laboratorio,
                    envasePorDefecto,
                    mililitrosPorDefecto),
                _ => throw new NotSupportedException(
                    $"Tipo de producto no soportado: {registro.Tipo}")
            };
        }

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