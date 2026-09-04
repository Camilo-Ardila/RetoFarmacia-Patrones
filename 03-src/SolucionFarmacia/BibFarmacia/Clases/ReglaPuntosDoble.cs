using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BibFarmacia.Interfaces;

namespace BibFarmacia.Clases
{
    public class ReglaPuntosDoble : IReglaPuntos
    {
        public int Calcular(int puntosBase)
        {
            return puntosBase * 2;
        }
    }
}
