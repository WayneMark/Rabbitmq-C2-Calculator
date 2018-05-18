using System.Text;
using RabbitMQ.Client;
namespace ComponentLib {
    public class MessageHandlerContext {
        public IBasicProperties Properties { get; private set; }
        public string ConsumerTag { get; private set; }
        public ulong DeliveryTag { get; private set; }
        public bool Redelivered { get; private set; }
        public string Exchange { get; private set; }
        public string RoutingKey { get; private set; }

        public IModel Channel { get; private set; }

        internal MessageHandlerContext (string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, IModel channel) {
            ConsumerTag = consumerTag;
            DeliveryTag = deliveryTag;
            Redelivered = redelivered;
            Exchange = exchange;
            RoutingKey = routingKey;
            Properties = properties;
            Channel = channel;
        }
        public void SendBack (string msg) {
            var msgProperties = Channel.CreateBasicProperties ();
            msgProperties.CorrelationId = Properties.CorrelationId; //return correlated id
            msgProperties.DeliveryMode = 2; //persistent

            Channel.BasicPublish (Component.COMMON_EXCHANGE_NAME, Properties.ReplyTo, msgProperties, Encoding.UTF8.GetBytes (msg));
        }
    }
}