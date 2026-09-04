using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BibFarmacia.Interfaces;

namespace BibFarmacia.Clases
{
    public class RellenoPolvo : IRelleno
    {
        public string Nombre => "Polvo";

        public string InstruccionesConservacion()
        {
            return "Mantener en lugar seco, protegido de la humedad";
        }
    }
}
