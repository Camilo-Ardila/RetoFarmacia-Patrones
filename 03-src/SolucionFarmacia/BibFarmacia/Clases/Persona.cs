using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibFarmacia.Clases
{
    public abstract class Persona
    {
        private string nombre = string.Empty;
        private string cedula = string.Empty;
        private string telefono = string.Empty;
        private string correo = string.Empty;

        public string Nombre
        {
            get => nombre;
            set => nombre = !string.IsNullOrWhiteSpace(value)
                ? value.Trim()
                : throw new ArgumentException("El nombre es obligatorio.");
        }

        public string Cedula
        {
            get => cedula;
            set => cedula = value.Length >= 3 && value.All(char.IsDigit)
                ? value
                : throw new ArgumentException("La cédula debe contener al menos tres dígitos.");
        }

        public string Telefono
        {
            get => telefono;
            set => telefono = value.Length is >= 7 and <= 15 && value.All(char.IsDigit)
                ? value
                : throw new ArgumentException("El teléfono debe contener entre 7 y 15 dígitos.");
        }

        public string Correo
        {
            get => correo;
            set => correo = !string.IsNullOrWhiteSpace(value) &&
                value.Contains('@') && value.LastIndexOf('.') > value.IndexOf('@')
                ? value.Trim()
                : throw new ArgumentException("El correo no tiene un formato válido.");
        }

        protected Persona(string nombre, string cedula,
            string telefono, string correo)
        {
            Nombre = nombre;
            Cedula = cedula;
            Telefono = telefono;
            Correo = correo;
        }
    }
}
