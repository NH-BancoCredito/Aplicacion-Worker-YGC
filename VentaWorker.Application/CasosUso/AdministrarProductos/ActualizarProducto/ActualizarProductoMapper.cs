using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using VentaWorker.Domain.Models;

namespace VentaWorker.Application.CasosUso.AdministrarProductos.ActualizarProducto
{
    public class ActualizarProductoMapper : Profile
    {
        public ActualizarProductoMapper()
        {
            CreateMap<ActualizarProductoRequest, Producto>();
        }
    }
}
