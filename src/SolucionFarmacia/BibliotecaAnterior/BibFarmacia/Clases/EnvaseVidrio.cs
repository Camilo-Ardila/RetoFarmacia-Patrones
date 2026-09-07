using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BibFarmacia.Interfaces;

namespace BibFarmacia.Clases
{
    public class EnvaseVidrio : IEnvase
    {
        public string Material => "Vidrio";

        public bool EsRetornable()
        {
            return true;
        }
    }
}
