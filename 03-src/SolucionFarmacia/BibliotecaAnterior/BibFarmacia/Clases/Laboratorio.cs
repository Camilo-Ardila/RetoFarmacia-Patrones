using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibFarmacia.Clases
{
    public class Laboratorio
    {
        private string nombre = string.Empty;
        private string direccion = string.Empty;
        private string telefono = string.Empty;

        public string Nombre
        {
            get => nombre;
            set => nombre = !string.IsNullOrWhiteSpace(value)
                ? value.Trim()
                : throw new ArgumentException("El nombre del laboratorio es obligatorio.");
        }

        public string Direccion
        {
            get => direccion;
            set => direccion = !string.IsNullOrWhiteSpace(value)
                ? value.Trim()
                : throw new ArgumentException("La dirección del laboratorio es obligatoria.");
        }

        public string Telefono
        {
            get => telefono;
            set => telefono = value.Length is >= 7 and <= 15 && value.All(char.IsDigit)
                ? value
                : throw new ArgumentException("El teléfono debe contener entre 7 y 15 dígitos.");
        }

        public Laboratorio(string nombre,
            string direccion,
            string telefono)
        {
            Nombre = nombre;
            Direccion = direccion;
            Telefono = telefono;
        }
    }
}
