using System.Threading;
namespace ComponentLib {
    public class ThreadComponet : Component {
        private Thread m_thread;
        private bool m_running = true;
        public ThreadComponet (string routingKey, string queueName, string rbmqUsername, string rbmqPassword, string rbmqServerUrl) : base (routingKey, queueName, rbmqUsername, rbmqPassword, rbmqServerUrl) {
            m_thread = new Thread (() => {
                using (var scopeComponet = this.start ()) { //start rabbitmq component
                    while (m_running) {
                        Thread.Sleep (100);
                    }
                }
            });
            m_thread.Name = queueName;
            m_thread.Start ();
        }

        public void stopWork () {
            m_running = false;
        }
    }
}