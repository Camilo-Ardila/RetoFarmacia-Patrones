using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BibFarmacia.Interfaces;

namespace BibFarmacia.Clases
{
    public class Movimiento
    {
        private DateTime fecha;
        private int cantidad;
        private string tipo = string.Empty;
        private IFacturable facturable = null!;

        public DateTime Fecha
        {
            get => fecha;
            set => fecha = value != DateTime.MinValue
                ? value
                : throw new ArgumentException("La fecha del movimiento es obligatoria.");
        }

        public int Cantidad
        {
            get => cantidad;
            set => cantidad = value > 0
                ? value
                : throw new ArgumentOutOfRangeException(nameof(value), "La cantidad debe ser positiva.");
        }

        public string Tipo
        {
            get => tipo;
            set => tipo = !string.IsNullOrWhiteSpace(value)
                ? value.Trim()
                : throw new ArgumentException("El tipo de movimiento es obligatorio.");
        }

        public IFacturable Facturable
        {
            get => facturable;
            set => facturable = value is not null
                ? value
                : throw new ArgumentNullException(nameof(value));
        }

        public Movimiento(DateTime fecha,
            int cantidad,
            string tipo,
            IFacturable facturable)
        {
            Fecha = fecha;
            Cantidad = cantidad;
            Tipo = tipo;
            Facturable = facturable;
        }
    }
}
