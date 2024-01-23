using MediatR;
using Stocks.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VentaWorker.Application.CasosUso.AdministrarProductos.ActualizarProducto
{
    public class ActualizarProductoRequest : IRequest<IResult>
    {
        public object Id { get; set; }
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public int Stock { get; set; }
        public int StockMinimo { get; set; }
        public decimal PrecioUnitario { get; set; }

        public int IdCategoria { get; set; }
    }
}
