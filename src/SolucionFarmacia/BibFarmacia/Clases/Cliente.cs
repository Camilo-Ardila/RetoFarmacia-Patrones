using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BibFarmacia.Interfaces;

namespace BibFarmacia.Clases
{
    public class Cliente : Persona
    {
        private int puntos;

        public int Puntos
        {
            get => puntos;
            set => puntos = value >= 0
                ? value
                : throw new ArgumentOutOfRangeException(nameof(value), "Los puntos no pueden ser negativos.");
        }

        public IDescuento Descuento { get; set; }

        public Cliente(string nombre, string cedula,
            string telefono, string correo)
            : base(nombre, cedula, telefono, correo)
        {
            Puntos = 0;
            Descuento = new SinDescuento();
        }

        public void AcumularPuntos(int puntos)
        {
            Puntos = puntos > 0
                ? Puntos + puntos
                : throw new ArgumentOutOfRangeException(nameof(puntos), "Los puntos a acumular deben ser positivos.");
        }
    }
}
