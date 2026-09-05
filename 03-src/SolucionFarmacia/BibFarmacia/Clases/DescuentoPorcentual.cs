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
            this.porcentaje = porcentaje >= 0 && porcentaje <= 1
                ? porcentaje
                : throw new ArgumentOutOfRangeException(
                    nameof(porcentaje),
                    "El porcentaje debe estar entre 0 y 1.");
        }

        public decimal CalcularDescuento(decimal precio)
        {
            return precio * porcentaje;
        }
    }
}
