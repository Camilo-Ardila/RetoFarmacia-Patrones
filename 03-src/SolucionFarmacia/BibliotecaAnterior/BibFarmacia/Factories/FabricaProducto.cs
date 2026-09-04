using BibFarmacia.Clases;
using BibFarmacia.Interfaces;

namespace BibFarmacia.Factories
{
    public class FabricaProducto : IFabricadorProducto
    {
        public Producto Crear(ProductoRequest request)
        {
            /*  Este punto de decisión es un mecanismo de adaptación de tipos.
                No representa la lógica de negocio central; por eso permanece
                localizado en la fábrica, donde la extensión de nuevas variantes
                puede hacerse sin modificar el servicio que consume la abstracción. */
            return request.TipoProducto switch
            {
                Type tipo when tipo == typeof(MedicamentoCapsula) => new MedicamentoCapsula(
                    request.Nombre,
                    request.Precio,
                    request.Stock,
                    request.StockMinimo,
                    request.FechaVencimiento,
                    request.Laboratorio,
                    request.Relleno!),

                Type tipo when tipo == typeof(MedicamentoLiquido) => new MedicamentoLiquido(
                    request.Nombre,
                    request.Precio,
                    request.Stock,
                    request.StockMinimo,
                    request.FechaVencimiento,
                    request.Laboratorio,
                    request.Envase!,
                    request.Mililitros),

                _ => throw new NotSupportedException($"Tipo de producto no soportado: {request.TipoProducto.Name}")
            };
        }
    }
}
