using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BibFarmacia.Clases;
using BibFarmacia.Eventos;

namespace BibFarmacia.Servicios
{
    public class ServicioPuntos
    {
        public EventoPuntos EventoPuntos;

        public ServicioPuntos()
        {
            EventoPuntos = new EventoPuntos();
        }

        public void AcumularPuntos(
            Cliente cliente,
            int puntos)
        {
            cliente.AcumularPuntos(puntos);

            EventoPuntos.Disparar(
                cliente.Nombre,
                puntos);
        }
    }
}
