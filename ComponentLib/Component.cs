using System;
using System.Text;
using RabbitMQ.Client;

namespace ComponentLib{
    public class Component : IDisposable {

        //consumer class
        class Consumer : DefaultBasicConsumer {
            private Component m_component;
            public override void HandleBasicDeliver (string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, byte[] body) {
                var context = new MessageHandlerContext (consumerTag, deliveryTag, redelivered, exchange, routingKey, properties, m_component.m_channel);
                m_component.onRecieve (Encoding.UTF8.GetString (body), context); //invoke onRecieve function for callback
                m_component.m_channel.BasicAck (context.DeliveryTag, false); //ack
            }

            public Consumer (Component component) : base (component.m_channel) { m_component = component; }
        }

        public static string COMMON_EXCHANGE_NAME { get; private set; } //common exchange's name
        static Component () {
            COMMON_EXCHANGE_NAME = "cal_exchange";
        }

        private ConnectionFactory m_factory;
        private IConnection m_connection;
        protected IModel m_channel;

        public string RoutingKey { get; private set; }
        public string QueueName { get; private set; }

        public Component (string routingKey, string queueName, string rbmqUsername, string rbmqPassword) {
            RoutingKey = routingKey;
            QueueName = queueName??Guid.NewGuid ().ToString ();
            m_factory = new ConnectionFactory ();
            m_factory.Endpoint = new AmqpTcpEndpoint ("localhost");
            m_factory.UserName = rbmqUsername;
            m_factory.Password = rbmqPassword;
        }

        #region initialization private fuction

        private Component openChannel () {
            m_connection = m_factory.CreateConnection ();
            m_channel = m_connection.CreateModel ();
            return this;
        }

        private Component initQueue () {
            m_channel.QueueDeclare (QueueName, true, false, false, null); //declare a queue
            m_channel.ExchangeDeclare (COMMON_EXCHANGE_NAME, ExchangeType.Topic, true, false, null); //declare the common exchange
            m_channel.QueueBind (QueueName, COMMON_EXCHANGE_NAME, RoutingKey); //bing the queue to the exchage
            return this;
        }

        private Component registerConsumer () {
            IBasicConsumer consumer = new Consumer (this);
            m_channel.BasicConsume (QueueName, false, consumer); //register consumer in queue
            return this;
        }

        #endregion

        public Component start () {
            this.openChannel ()
                .initQueue ()
                .registerConsumer ();
            return this;
        }

        virtual protected void onRecieve (string msg, MessageHandlerContext context) {

        }

        protected void sendMessage (string routingKey, string message, IBasicProperties properties = null) {
            m_channel.BasicPublish (COMMON_EXCHANGE_NAME, routingKey, properties, Encoding.UTF8.GetBytes (message)); //send message to exchange
        }

        public void close () {
            m_channel.Dispose ();
            m_connection.Dispose ();
        }

        public void Dispose () {
            this.close ();
        }
    }
}