using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BibFarmacia.Interfaces;

namespace BibFarmacia.Servicios
{
    public class NotificacionArchivo : IServicioNotificacion
    {
        private readonly string ruta;

        public NotificacionArchivo(string ruta)
        {
            this.ruta = ruta;
        }

        public void EnviarNotificacion(string mensaje)
        {
            File.AppendAllText(
                ruta,
                $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {mensaje}{Environment.NewLine}");
        }
    }
}
