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
        private readonly IRepositorio<RegistroProducto> repositorio;
        private readonly IFabricaMedicamentos fabricaMedicamentos;

        public ServicioProducto(
            IRepositorio<RegistroProducto> repositorio,
            IFabricaMedicamentos fabricaMedicamentos)
        {
            this.repositorio = repositorio;
            this.fabricaMedicamentos = fabricaMedicamentos;

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

        public string AgregarProducto(
            string nombre,
            decimal precio,
            int stock,
            int stockMinimo,
            DateTime fechaVencimiento,
            Laboratorio laboratorio,
            IRelleno relleno)
        {
            return AgregarProducto(
                fabricaMedicamentos.Crear(
                    nombre,
                    precio,
                    stock,
                    stockMinimo,
                    fechaVencimiento,
                    laboratorio,
                    relleno));
        }

        public string AgregarProducto(
            string nombre,
            decimal precio,
            int stock,
            int stockMinimo,
            DateTime fechaVencimiento,
            Laboratorio laboratorio,
            IEnvase envase,
            int mililitros)
        {
            return AgregarProducto(
                fabricaMedicamentos.Crear(
                    nombre,
                    precio,
                    stock,
                    stockMinimo,
                    fechaVencimiento,
                    laboratorio,
                    envase,
                    mililitros));
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
                foreach (RegistroProducto registro in
                    repositorio.Cargar(ruta))
                {
                    productos.Add(
                        fabricaMedicamentos.Crear(registro));
                }

                return "Productos cargados";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}