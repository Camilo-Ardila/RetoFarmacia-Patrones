using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BibFarmacia.Interfaces;

namespace BibFarmacia.Clases
{
    public class MedicamentoLiquido : Medicamento
    {
        private IEnvase envase = null!;
        private int mililitros;

        public IEnvase Envase
        {
            get => envase;
            set => envase = value is not null
                ? value
                : throw new ArgumentNullException(nameof(value));
        }

        public int Mililitros
        {
            get => mililitros;
            set => mililitros = value > 0
                ? value
                : throw new ArgumentOutOfRangeException(nameof(value), "Los mililitros deben ser positivos.");
        }

        public MedicamentoLiquido(
            string nombre,
            decimal precio,
            int stock,
            int stockMinimo,
            DateTime fechaVencimiento,
            Laboratorio laboratorio,
            IEnvase envase,
            int mililitros)
            : base(nombre, precio, stock,
                  stockMinimo, fechaVencimiento,
                  laboratorio)
        {
            Envase = envase;
            Mililitros = mililitros;
        }
    }
}
