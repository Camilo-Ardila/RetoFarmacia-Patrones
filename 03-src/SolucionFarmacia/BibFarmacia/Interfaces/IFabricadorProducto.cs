using BibFarmacia.Clases;

namespace BibFarmacia.Interfaces
{
    public interface IFabricadorProducto
    {
        Producto Crear(ProductoRequest request);
    }
}
