using Microsoft.EntityFrameworkCore.Metadata;
using CatalogService.Messaging.Messages;
using Microsoft.EntityFrameworkCore;
using CatalogService.API;

namespace CatalogService.Messaging
{

    public class CatalogRequestConsumer
    {
        private readonly IModel _channel;
        private readonly CatalogContext _context;

        public CatalogRequestConsumer(IModel channel, CatalogContext context)
        {
            _channel = channel;
            _context = context;
        }

        public void StartConsuming()
        {
            const string deadLetterExchange = "dead-letter-exchange";
            const string deadLetterQueue = "dead-letter-queue";

            //Opret DLQ
            _channel.ExchangeDeclare(exchange: deadLetterExchange, type: ExchangeType.Fanout, durable: true);
            _channel.QueueDeclare(queue: deadLetterQueue, durable: true, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueBind(queue: deadLetterQueue, exchange: deadLetterExchange, routingKey: "");

            var mainQueueArguments = new Dictionary<string, object>
            {
                { "x-dead-letter-exchange", deadLetterExchange }
            };

            // Opret exchange og queue for catalogResponse
            _channel.ExchangeDeclare(exchange: "catalog-exchange", type: ExchangeType.Direct, durable: true);
            _channel.QueueDeclare(queue: "catalog-queue", durable: true, exclusive: false, autoDelete: false, arguments: mainQueueArguments);
            _channel.QueueBind(queue: "catalog-queue", exchange: "catalog-exchange", routingKey: "request-catalog-items");


            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                // Deserialize request
                var catalogRequest = JsonSerializer.Deserialize<CatalogRequest>(message);

                // Process request
                var response = await HandleCatalogRequestAsync(catalogRequest);

                // Send response back via ReplyTo queue
                var props = ea.BasicProperties;

                if (!string.IsNullOrEmpty(props.ReplyTo))
                {
                    var responseBody = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response));

                    var replyProps = _channel.CreateBasicProperties();
                    replyProps.CorrelationId = props.CorrelationId;

                    _channel.BasicPublish(
                        exchange: "",
                        routingKey: props.ReplyTo,
                        basicProperties: replyProps,
                        body: responseBody
                    );
                }

                // Acknowledge the message
                _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            _channel.BasicConsume(queue: "catalog-queue", autoAck: false, consumer: consumer);
        }

        private async Task<List<CatalogItemResponse>> HandleCatalogRequestAsync(CatalogRequest catalogRequest)
        {
            // Hent CatalogItems fra databasen baseret på de IDs, der er i forespørgslen
            var catalogItems = await _context.CatalogItems
                .Where(item => catalogRequest.CatalogItemIds.Contains(item.Id))
                .ToListAsync();

            // Map CatalogItems til CatalogItemResponse
            var response = catalogItems.Select(item => new CatalogItemResponse
            {
                Id = item.Id,
                Name = item.Name, // Dette svarer til ProductName i Order
                PictureUri = item.PictureUri
            }).ToList();

            return response;
        }
    }
    
}
