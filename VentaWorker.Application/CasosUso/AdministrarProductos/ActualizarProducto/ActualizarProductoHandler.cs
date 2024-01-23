using AutoMapper;
using MediatR;
using Stocks.Application.Common;
using Stocks.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentaWorker.Application;
using VentaWorker.Application.CasosUso.AdministrarProductos.ActualizarProducto;
using VentaWorker.Domain.Models;
using VentaWorker.Domain.Service;
using VentaWorker.Domain.Service.WebServices;

namespace Venta.Application.CasosUso.AdministrarProductos.ActualizarProducto
{
        /* 
         1 - Deberia verificar si el productos
         Si existe , entoces actualizar en la table de producto
         Si no existe, crear un nuevo registro

         */    
    public class ActualizarProductoHandler :
       IRequestHandler<ActualizarProductoRequest, IResult>
    {
        private readonly IProductoService _productoService;
        private readonly IMapper _mapper;

        public ActualizarProductoHandler(IProductoService productoService, IMapper mapper)
        {
            _productoService = productoService;
            _mapper = mapper;
        }


        public async Task<IResult> Handle(ActualizarProductoRequest request, CancellationToken cancellationToken)
        {

            IResult response = null;
            bool result = false;

            try
            {
                var producto = _mapper.Map<Producto>(request);
                await _productoService.ActualizarProducto(producto);
              
                if (result)
                {
                    return new SuccessResult();
                }
                else
                    return new FailureResult();

            }
            catch (Exception ex)
            {
                response = new FailureResult();
                return response;
            }
        }
    }
}
