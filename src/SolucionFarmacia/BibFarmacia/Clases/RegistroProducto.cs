namespace BibFarmacia.Clases
{
    public class RegistroProducto
    {
        public string Tipo { get; }
        public string Nombre { get; }
        public decimal Precio { get; }
        public int Stock { get; }
        public int StockMinimo { get; }
        public DateTime FechaVencimiento { get; }
        public string Laboratorio { get; }

        public RegistroProducto(
            string tipo,
            string nombre,
            decimal precio,
            int stock,
            int stockMinimo,
            DateTime fechaVencimiento,
            string laboratorio)
        {
            Tipo = tipo;
            Nombre = nombre;
            Precio = precio;
            Stock = stock;
            StockMinimo = stockMinimo;
            FechaVencimiento = fechaVencimiento;
            Laboratorio = laboratorio;
        }
    }
}