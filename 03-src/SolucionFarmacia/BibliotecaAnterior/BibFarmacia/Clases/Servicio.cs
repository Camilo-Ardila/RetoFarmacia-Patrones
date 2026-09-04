using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BibFarmacia.Interfaces;

namespace BibFarmacia.Clases
{
    public class Servicio : IFacturable
    {
        private string nombre = string.Empty;
        private decimal precio;
        private int duracionMinutos;

        public string Nombre
        {
            get => nombre;
            set => nombre = !string.IsNullOrWhiteSpace(value)
                ? value.Trim()
                : throw new ArgumentException("El nombre del servicio es obligatorio.");
        }

        public decimal Precio
        {
            get => precio;
            set => precio = value > 0
                ? value
                : throw new ArgumentOutOfRangeException(nameof(value), "El precio debe ser positivo.");
        }

        public int DuracionMinutos
        {
            get => duracionMinutos;
            set => duracionMinutos = value > 0
                ? value
                : throw new ArgumentOutOfRangeException(nameof(value), "La duración debe ser positiva.");
        }

        public Servicio(string nombre,
            decimal precio,
            int duracionMinutos)
        {
            Nombre = nombre;
            Precio = precio;
            DuracionMinutos = duracionMinutos;
        }

        public decimal ObtenerPrecio()
        {
            return Precio;
        }

        public string MostrarInformacion()
        {
            return $"{Nombre}\t{Precio}\t" +
                $"{DuracionMinutos} min";
        }
    }
}
