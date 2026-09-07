using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BibFarmacia.Clases;
using BibFarmacia.Interfaces;

namespace BibFarmacia.Servicios
{
    public class ServicioProducto
    {
        private readonly List<Producto> productos;
        private readonly IRepositorio<Producto> repositorio;
        private readonly IFabricadorProducto fabricadorProducto;

        public ServicioProducto(
            IRepositorio<Producto> repositorio,
            IFabricadorProducto fabricadorProducto)
        {
            this.repositorio = repositorio;
            this.fabricadorProducto = fabricadorProducto;

            productos = new List<Producto>();
        }

        public string AgregarProducto(
            Producto producto)
        {
            try
            {
                productos.Add(producto);

                return "Producto agregado";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string AgregarProductoDesdeRequest(
            ProductoRequest request)
        {
            return AgregarProducto(
                fabricadorProducto.Crear(request));
        }

        public List<Producto> ObtenerProductos()
        {
            return productos;
        }

        public string CargarDesdeArchivo(
            string ruta)
        {
            try
            {
                productos.AddRange(
                    repositorio.Cargar(ruta));

                return "Productos cargados";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}