using MediatR;
using Stocks.Domain.Service.Events;
using static Confluent.Kafka.ConfigPropertyNames;
using System.Threading;
using Microsoft.AspNetCore.Components.Forms;
using ThirdParty.Json.LitJson;
using Newtonsoft.Json;
using VentaWorker.Application.CasosUso.AdministrarProductos.ActualizarProducto;
using Amazon.Runtime.Internal;
using Newtonsoft.Json.Linq;
using System.Net;
using VentaWorker.Domain.Models;
//using Venta.Worker.Repositories;
//using Venta.Domain.Services.WebServices;

namespace Venta.Worker.Workers
{
    public class ActualizarStocksWorker : BackgroundService
    {
        private readonly IConsumerFactory _consumerFactory;
        private readonly IMediator _mediator;

        public ActualizarStocksWorker(IConsumerFactory consumerFactory, IMediator mediator)
        {
            _consumerFactory = consumerFactory;
            _mediator = mediator;

        }
        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var consumer = _consumerFactory.GetConsumer();
            consumer.Subscribe("stocks");

            while (!cancellationToken.IsCancellationRequested)
            {
                var consumeResult = consumer.Consume(cancellationToken);
                //Llamar al handler para actualizar la información del producto
                //, la actualización deberia relizarse llamando una api del
                //microservicio de Ventas
                if (consumeResult.Value != null)
                {
                    var request = consumeResult.Value;

                    var objProducto = JsonConvert.DeserializeObject<ActualizarProductoRequest>(request);
                   
                    var response =  _mediator.Send(objProducto);
                }
            }

            consumer.Close();

            return Task.CompletedTask;
        }
    }
}
