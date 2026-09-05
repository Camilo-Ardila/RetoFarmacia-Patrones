using BibFarmacia.Clases;

namespace BibFarmacia.Servicios
{
    public class FormateadorProducto
    {
        public string Formatear(Producto producto)
        {
            return $"{producto.Nombre}\t\t{producto.Stock}\t{producto.Precio}";
        }
    }
}