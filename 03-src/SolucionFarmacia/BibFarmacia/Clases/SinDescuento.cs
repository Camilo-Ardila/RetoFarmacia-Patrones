using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BibFarmacia.Interfaces;

namespace BibFarmacia.Clases
{
    public class SinDescuento : IDescuento
    {
        public decimal CalcularDescuento(decimal precio)
        {
            return 0m;
        }
    }
}
