using BronhomunalEvolved;
using Confluent.Kafka;
using vk_api_balancer;

vk_api_balancer.Config _config = new vk_api_balancer.Config();
Client client = new Client(_config.KafkaServer);

client.Subscribe(_config.SubscribeTopic);
client.RecieveMessage += (o,m) => {
    BhVkApi.SendMessage("echo: "+m.Text,Int64.Parse(m.IntegratorDestinationData));
};

BhVkApi.StartVkApi(_config,client);
var config = new ProducerConfig { BootstrapServers = _config.KafkaServer };

BhVkApi.MessageReceived += (s, m) =>
{
	Console.WriteLine("EVENT: "+m.Text);
    // If serializers are not specified, default serializers from
    // `Confluent.Kafka.Serializers` will be automatically used where
    // available. Note: by default strings are encoded as UTF8.
    using (var p = new ProducerBuilder<Null, string>(config).Build())
    {
        try
        {
            Logger.Log("Producing message...");
            p.Produce(_config.SubscribeTopic, new Message<Null, string> { Value = "BACK-ECHO: "+m.Text });
            Logger.Log("Produced shit");
            //Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
        }
        catch (ProduceException<Null, string> e)
        {
            Console.WriteLine($"Delivery failed: {e.Error.Reason}");
        }
    }
    //m.Respond("ECHO-TEST: "+m.Text);
};

Task.Run(async () =>
{
    

    
});

//BhVkApi.SendMessage("fucc diss",15565);