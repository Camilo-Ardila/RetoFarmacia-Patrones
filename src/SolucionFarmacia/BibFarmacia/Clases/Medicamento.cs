using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibFarmacia.Clases
{
    public class Medicamento : Producto
    {
        private Laboratorio laboratorio = null!;

        public Laboratorio Laboratorio
        {
            get => laboratorio;
            set => laboratorio = value is not null
                ? value
                : throw new ArgumentNullException(nameof(value));
        }

        public Medicamento(string nombre,
            decimal precio,
            int stock,
            int stockMinimo,
            DateTime fechaVencimiento,
            Laboratorio laboratorio)
            : base(nombre, precio, stock,
                  stockMinimo, fechaVencimiento)
        {
            Laboratorio = laboratorio;
        }
    }
}
