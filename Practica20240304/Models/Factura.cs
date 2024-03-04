using System;
using System.Collections.Generic;

namespace Practica20240304.Models
{
    public partial class Factura
    {
        public Factura()
        {
            DetFacturas = new List<DetFactura>();
        }

        public int Id { get; set; }
        public string? Correlativo { get; set; }
        public DateTime? Fecha { get; set; }

        public virtual IList<DetFactura> DetFacturas { get; set; }
    }
}
