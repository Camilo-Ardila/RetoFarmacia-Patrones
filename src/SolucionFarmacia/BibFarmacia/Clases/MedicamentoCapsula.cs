using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BibFarmacia.Interfaces;

namespace BibFarmacia.Clases
{
    public class MedicamentoCapsula : Medicamento
    {
        private IRelleno relleno = null!;

        public IRelleno Relleno
        {
            get => relleno;
            set => relleno = value is not null
                ? value
                : throw new ArgumentNullException(nameof(value));
        }

        public MedicamentoCapsula(
            string nombre,
            decimal precio,
            int stock,
            int stockMinimo,
            DateTime fechaVencimiento,
            Laboratorio laboratorio,
            IRelleno relleno)
            : base(nombre, precio, stock,
                  stockMinimo, fechaVencimiento,
                  laboratorio)
        {
            Relleno = relleno;
        }
    }
}
