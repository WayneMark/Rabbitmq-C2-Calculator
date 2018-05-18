using System;
using System.Diagnostics;
using ComponentLib;
namespace Cal.Components {
    public class Sub : ThreadComponet {
        public Sub (string routingKey, string queueName = null, string rbmqUsername = "ziyuandev", string rbmqPassword = "123456") : base (routingKey, queueName, rbmqUsername, rbmqPassword) {

        }
        override protected void onRecieve (string msg, MessageHandlerContext context) {
            try {
                var msgChips = msg.Split (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (msgChips[0].ToLower () == "-") {
                    double a = Convert.ToDouble (msgChips[1]);
                    double b = Convert.ToDouble (msgChips[2]);
                    double result = a - b;
                    context.SendBack (result.ToString ());
                }
            } catch (Exception e) {
                Debug.WriteLine ("ERROR: " + e.Message);
                context.SendBack ("invalid message content");
            }
        }
    }
}