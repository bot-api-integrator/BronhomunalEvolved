using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BronhomunalEvolved
{
	public class Client
	{
		public event EventHandler<Message> RecieveMessage;
		private bool _work = true;
        private string _server;

        public Client() { }
        public Client(string server)
		{
            _server = server;
		}
		public async void StartAsync(String topic)
		{
			await Task.Run(()=>Listen(topic));
		}

        public void Stop()
		{
            _work = false;
		}
		private void Listen(String topic)
		{
            var conf = new ConsumerConfig
            {
                BootstrapServers = _server,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using (var c = new ConsumerBuilder<Ignore, string>(conf).Build())
            {
                c.Subscribe(topic);

                CancellationTokenSource cts = new CancellationTokenSource();

                try
                {
                    while (_work)
                    {
                        try
                        {
                            var cr = c.Consume(cts.Token);
                            Console.WriteLine($"Consumed message '{cr.Value}' at: '{cr.TopicPartitionOffset}'.");
                            var recieveMessage = JsonSerializer.Deserialize<RecieveMessage>(cr.Message.Value, new JsonSerializerOptions
                            {
                                WriteIndented = true,
                                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                            });
                            RecieveMessage?.Invoke(this, new Message(recieveMessage));
                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"Error occured: {e.Error.Reason}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    c.Close();
                }
            }
        }
		public void SendMessage(Message message)
		{
            var conf = new ProducerConfig { BootstrapServers = _server };

            Action<DeliveryReport<Null, string>> handler = r =>
            Console.WriteLine(!r.Error.IsError
                ? $"Delivered message to {r.TopicPartitionOffset}"
                : $"Delivery Error: {r.Error.Reason}");

            using (var p = new ProducerBuilder<Null, string>(conf).Build())
            {
                
                p.Produce(message.IntegratorTopic, new Message<Null, string> { Value = message.ToString()}, handler);
                // wait for up to 10 seconds for any inflight messages to be delivered.
                //p.Flush(TimeSpan.FromSeconds(10));
            }
        }

	}
}
