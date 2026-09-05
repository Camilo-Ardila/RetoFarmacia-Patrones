using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BibFarmacia.Clases;
using BibFarmacia.Eventos;
using BibFarmacia.Interfaces;

namespace BibFarmacia.Servicios
{
    public class ServicioPuntos
    {
        private readonly IReglaPuntos reglaPuntos;

        public EventoPuntos EventoPuntos;

        public ServicioPuntos(
            IReglaPuntos reglaPuntos)
        {
            this.reglaPuntos = reglaPuntos;

            EventoPuntos = new EventoPuntos();
        }

        public void ProcesarMovimientoRegistrado(
            ContextoVenta contexto)
        {
            try
            {
                int puntosBase =
                    Math.Max(
                        1,
                        (int)Math.Floor(
                            contexto.Total / 1000m));

                AcumularPuntos(
                    contexto.Cliente,
                    puntosBase);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(
                    $"No se pudieron acumular puntos: {ex.Message}");
            }
        }

        public void AcumularPuntos(
            Cliente cliente,
            int puntos)
        {
            int puntosCalculados =
                reglaPuntos.Calcular(puntos);

            cliente.AcumularPuntos(puntosCalculados);

            EventoPuntos.Disparar(
                cliente.Nombre,
                puntosCalculados);
        }
    }
}
