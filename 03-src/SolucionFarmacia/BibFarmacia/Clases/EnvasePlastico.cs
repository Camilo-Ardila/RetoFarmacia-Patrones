using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BibFarmacia.Interfaces;

namespace BibFarmacia.Clases
{
    public class EnvasePlastico : IEnvase
    {
        public string Material => "Plastico";

        public bool EsRetornable()
        {
            return false;
        }
    }
}
