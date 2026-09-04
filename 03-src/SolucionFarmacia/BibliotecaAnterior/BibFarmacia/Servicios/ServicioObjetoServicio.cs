using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BibFarmacia.Clases;
using BibFarmacia.Interfaces;

namespace BibFarmacia.Servicios
{
    public class ServicioObjetoServicio
    {
        private readonly List<Servicio> servicios;
        private readonly IRepositorio<Servicio> repositorio;

        public ServicioObjetoServicio(
            IRepositorio<Servicio> repositorio)
        {
            this.repositorio = repositorio;

            servicios = new List<Servicio>();
        }

        public string AgregarServicio(
            string nombre,
            decimal precio,
            int duracionMinutos)
        {
            try
            {
                servicios.Add(
                    new Servicio(
                        nombre,
                        precio,
                        duracionMinutos));

                return "Servicio agregado";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public List<Servicio> ObtenerServicios()
        {
            return servicios;
        }

        public string CargarDesdeArchivo(
            string ruta)
        {
            try
            {
                servicios.AddRange(
                    repositorio.Cargar(ruta));

                return "Servicios cargados";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
