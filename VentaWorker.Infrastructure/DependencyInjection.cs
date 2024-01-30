using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Stocks.Domain.Repositories;
//using Stocks.Infrastructure.Repositories;
using MongoDB.Driver;
using Confluent.Kafka;
using Stocks.Domain.Service.Events;
using Stocks.Infrastructure.Services.Events;
using System.Net;
using VentaWorker.Domain.Service.WebServices;
using VentaWorker.Infrastructure.Services.WebServices;
using Polly.Extensions.Http;
using Polly;


namespace Stocks.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfraestructure(
            this IServiceCollection services, string connectionString
            )
        {

            var httpClientBuilder = services.AddHttpClient<IProductoService, ProductoService>(
                options =>
                {
                    options.BaseAddress = new Uri("http://localhost:5297/");
                    //options.Timeout = TimeSpan.FromMilliseconds(30000);
                }
                ).SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddPolicyHandler(GetRetryPolicy());

            services.AddDataBaseFactories(connectionString);
            services.AddRepositories();
            services.AddProducer();
            services.AddEventServices();
            services.AddConsumer();
        }

        private static void AddDataBaseFactories(this IServiceCollection services, string connectionString)
        {            
            services.AddSingleton(mongoDatabase =>
            {
                var mongoClient = new MongoClient(connectionString);
                return mongoClient.GetDatabase("db-productos-stocks");
            });

        }

        private static void AddRepositories(this IServiceCollection services)
        {
            //services.AddScoped<IProductoRepository, ProductoRepository>();

            
        }

        private static IServiceCollection AddProducer(this IServiceCollection services)
        {
            var config = new ProducerConfig
            {
                Acks = Acks.Leader,
                BootstrapServers = "127.0.0.1:9092",
                ClientId = Dns.GetHostName(),
            };

            services.AddSingleton<IPublisherFactory>(sp => new PublisherFactory(config));
            return services;
        }

        private static IServiceCollection AddConsumer(this IServiceCollection services)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "127.0.0.1:9092",
                GroupId = "venta-actualizar-stocks",
                AutoOffsetReset = AutoOffsetReset.Latest
            };

            services.AddSingleton<IConsumerFactory>(sp => new ConsumerFactory(config));
            return services;
        }

        private static void AddEventServices(this IServiceCollection services)
        {
            services.AddSingleton<IEventSender, EventSender>();
        }
        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(2,
                            retryAttempts => TimeSpan.FromSeconds(Math.Pow(2, retryAttempts)));
        }

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            Action<DelegateResult<HttpResponseMessage>, TimeSpan> onBreak = (result, timeSpan) =>
            {
                Console.WriteLine(result);
            }
            ;
            Action onReset = null;
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrTransientHttpError()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30),
                onBreak, onReset
                );


        }
        //private static void SetHttpClient<TClient, TImplementation>(this IServiceCollection services, string constante) where TClient : class where TImplementation : class, TClient
        //{

        //    services.AddHttpClient<TClient, TImplementation>(options =>
        //    {
        //        options.Timeout = TimeSpan.FromMilliseconds(2000);
        //    })
        //    .SetHandlerLifetime(TimeSpan.FromMinutes(30))
        //    .ConfigurePrimaryHttpMessageHandler(() =>
        //    {
        //        var handler = new HttpClientHandler();
        //        //if (EnvironmentVariableProvider.IsDevelopment())
        //        //{
        //        //    handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
        //        //}

        //        return handler;
        //    });
        //}
    }
}
