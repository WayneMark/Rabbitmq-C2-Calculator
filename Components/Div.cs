using System;
using System.Diagnostics;
using ComponentLib;
namespace Cal.Components {
    public class Div : ThreadComponet {
        public Div (string routingKey, string queueName, string rbmqUsername, string rbmqPassword, string rbmqServerUrl) : base (routingKey, queueName, rbmqUsername, rbmqPassword,rbmqServerUrl) {

        }
        override protected void onRecieve (string msg, MessageHandlerContext context) {
            try {
                var msgChips = msg.Split (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (msgChips[0].ToLower () == "/") {
                    double a = Convert.ToDouble (msgChips[1]);
                    double b = Convert.ToDouble (msgChips[2]);
                    if (b == 0.0)
                        throw new Exception ("b cannot be 0");
                    double result = a / b;
                    context.SendBack (result.ToString ());
                }
            } catch (Exception e) {
                Debug.WriteLine ("ERROR: " + e.Message);
                context.SendBack ("invalid message content");
            }
        }
    }
}