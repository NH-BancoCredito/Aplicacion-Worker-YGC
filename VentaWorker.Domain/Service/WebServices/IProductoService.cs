using Stocks.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentaWorker.Domain.Models;

namespace VentaWorker.Domain.Service.WebServices
{
    public interface IProductoService : IRepository
    {
        Task<bool> ActualizarProducto(Producto producto);
    }
}
