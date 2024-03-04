using System;
using System.Collections.Generic;

namespace Practica20240304.Models
{
    public partial class DetFactura
    {
        public int Id { get; set; }
        public int? IdFactura { get; set; }
        public string? Producto { get; set; }
        public int? Cantidad { get; set; }
        public decimal? Precio { get; set; }

        public virtual Factura? IdFacturaNavigation { get; set; }
    }
}
