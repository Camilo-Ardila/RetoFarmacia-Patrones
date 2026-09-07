using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibFarmacia.Interfaces
{
    public interface IInventariable
    {
        int Stock { get; }
        int StockMinimo { get; }
        DateTime FechaVencimiento { get; }

        void DescontarStock(int cantidad);
    }
}
