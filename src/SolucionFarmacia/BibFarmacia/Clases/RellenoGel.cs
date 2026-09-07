using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BibFarmacia.Interfaces;

namespace BibFarmacia.Clases
{
    public class RellenoGel : IRelleno
    {
        public string Nombre => "Gel";

        public string InstruccionesConservacion()
        {
            return "Mantener por debajo de 25°C";
        }
    }
}
