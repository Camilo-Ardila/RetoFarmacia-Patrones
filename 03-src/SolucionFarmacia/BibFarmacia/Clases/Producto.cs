using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BibFarmacia.Interfaces;

namespace BibFarmacia.Clases
{
    public abstract class Producto : IFacturable, IInventariable
    {
        private string nombre = string.Empty;
        private decimal precio;
        private int stock;
        private int stockMinimo;
        private DateTime fechaVencimiento;

        public string Nombre
        {
            get => nombre;
            set => nombre = !string.IsNullOrWhiteSpace(value)
                ? value.Trim()
                : throw new ArgumentException("El nombre del producto es obligatorio.");
        }

        public decimal Precio
        {
            get => precio;
            set => precio = value > 0
                ? value
                : throw new ArgumentOutOfRangeException(nameof(value), "El precio debe ser positivo.");
        }

        public int Stock
        {
            get => stock;
            set => stock = value >= 0
                ? value
                : throw new ArgumentOutOfRangeException(nameof(value), "El stock no puede ser negativo.");
        }

        public int StockMinimo
        {
            get => stockMinimo;
            set => stockMinimo = value >= 0
                ? value
                : throw new ArgumentOutOfRangeException(nameof(value), "El stock mínimo no puede ser negativo.");
        }

        public DateTime FechaVencimiento
        {
            get => fechaVencimiento;
            set => fechaVencimiento = value != DateTime.MinValue
                ? value
                : throw new ArgumentException("La fecha de vencimiento es obligatoria.");
        }

        protected Producto(string nombre,
            decimal precio,
            int stock,
            int stockMinimo,
            DateTime fechaVencimiento)
        {
            Nombre = nombre;
            Precio = precio;
            Stock = stock;
            StockMinimo = stockMinimo;
            FechaVencimiento = fechaVencimiento;
        }

        public void DescontarStock(int cantidad)
        {
            Stock = cantidad > 0 && cantidad <= Stock
                ? Stock - cantidad
                : throw new ArgumentOutOfRangeException(nameof(cantidad), "La cantidad debe ser positiva y no superar el stock disponible.");
        }

    }
}
