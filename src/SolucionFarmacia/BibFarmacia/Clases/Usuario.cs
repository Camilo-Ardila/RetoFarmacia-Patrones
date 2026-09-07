using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BibFarmacia.Clases
{
    public class Usuario : Persona
    {
        private string userName = string.Empty;
        private string password = string.Empty;

        public string UserName
        {
            get => userName;
            set => userName = !string.IsNullOrWhiteSpace(value)
                ? value.Trim()
                : throw new ArgumentException("El nombre de usuario es obligatorio.");
        }

        public string Password
        {
            get => password;
            set => password = !string.IsNullOrWhiteSpace(value)
                ? value
                : throw new ArgumentException("La contraseña es obligatoria.");
        }

        public Usuario(string nombre, string cedula,
            string telefono, string correo,
            string userName, string password)
            : base(nombre, cedula, telefono, correo)
        {
            UserName = userName;
            Password = password;
        }
    }
}
