using System;
using System.Collections.Generic;
using ComponentLib;
namespace Cal.Components {
    public class CalUnit : ThreadComponet {
        public Dictionary<string, string> calDic { get; private set; }
        public CalUnit (string routingKey, string queueName, string rbmqUsername, string rbmqPassword, string rbmqServerUrl) : base (routingKey, queueName, rbmqUsername, rbmqPassword,rbmqServerUrl) {
            calDic = new Dictionary<string, string> ();
        }
        override protected void onRecieve (string msg, MessageHandlerContext context) {
            double result;
            if (double.TryParse (msg, out result))
                Console.WriteLine ($"{calDic[context.Properties.CorrelationId]} = {msg}");
            else
                Console.WriteLine ($"Cannot calculate: {calDic[context.Properties.CorrelationId]} ");
        }

        public void calculate (string op, double a, double b, string opComponetRoutingKey = null) {

            string routingKey = "chips.";
            if (string.IsNullOrEmpty (opComponetRoutingKey)) {
                switch (op) {
                    case "+":
                        routingKey += "add";
                        break;
                    case "-":
                        routingKey += "sub";
                        break;
                    case "*":
                        routingKey += "mul";
                        break;
                    case "/":
                        routingKey += "div";
                        break;
                    default:
                        Console.WriteLine ($"Cannot calculate {a} {op} {b} .");
                        return;
                }
            } else
                routingKey = opComponetRoutingKey;

            var properties = m_channel.CreateBasicProperties ();
            properties.CorrelationId = Guid.NewGuid ().ToString ();
            properties.ReplyTo = RoutingKey;
            sendMessage (routingKey, $"{op},{a},{b}", properties);

            calDic.Add (properties.CorrelationId, $"{a} {op} {b}");
        }
    }
}