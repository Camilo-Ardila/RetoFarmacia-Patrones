using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BibFarmacia.Interfaces;

namespace BibFarmacia.Clases
{
    public class DescuentoPorcentual : IDescuento
    {
        private readonly decimal porcentaje;

        public DescuentoPorcentual(decimal porcentaje)
        {
            this.porcentaje = porcentaje;
        }

        public decimal CalcularDescuento(decimal precio)
        {
            return precio * porcentaje;
        }
    }
}
