using BibFarmacia.Interfaces;

namespace BibFarmacia.Clases
{
    public class ProductoRequest
    {
        private Type tipoProducto = null!;
        private string nombre = string.Empty;
        private decimal precio;
        private int stock;
        private int stockMinimo;
        private DateTime fechaVencimiento;
        private Laboratorio laboratorio = null!;

        public Type TipoProducto
        {
            get => tipoProducto;
            set => tipoProducto = (value == typeof(MedicamentoCapsula) ||
                value == typeof(MedicamentoLiquido))
                ? value
                : throw new ArgumentException("El tipo de producto no está soportado.");
        }

        public string Nombre
        {
            get => nombre;
            set => nombre = !string.IsNullOrWhiteSpace(value)
                ? value.Trim()
                : throw new ArgumentException("El nombre del producto es obligatorio.");
        }

        public decimal Precio
        {
            get => precio;
            set => precio = value > 0
                ? value
                : throw new ArgumentOutOfRangeException(nameof(value), "El precio debe ser positivo.");
        }

        public int Stock
        {
            get => stock;
            set => stock = value >= 0
                ? value
                : throw new ArgumentOutOfRangeException(nameof(value), "El stock no puede ser negativo.");
        }

        public int StockMinimo
        {
            get => stockMinimo;
            set => stockMinimo = value >= 0
                ? value
                : throw new ArgumentOutOfRangeException(nameof(value), "El stock mínimo no puede ser negativo.");
        }

        public DateTime FechaVencimiento
        {
            get => fechaVencimiento;
            set => fechaVencimiento = value != DateTime.MinValue
                ? value
                : throw new ArgumentException("La fecha de vencimiento es obligatoria.");
        }

        public Laboratorio Laboratorio
        {
            get => laboratorio;
            set => laboratorio = value is not null
                ? value
                : throw new ArgumentNullException(nameof(value));
        }

        public IRelleno? Relleno { get; set; }
        public IEnvase? Envase { get; set; }
        private int mililitros;

        public int Mililitros
        {
            get => mililitros;
            set => mililitros = value > 0
                ? value
                : throw new ArgumentOutOfRangeException(nameof(value), "Los mililitros deben ser positivos.");
        }
    }
}
