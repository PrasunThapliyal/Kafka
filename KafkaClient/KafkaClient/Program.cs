
namespace KafkaClient
{
    using System;
    using System.Threading.Tasks;

    class Program
    {
        // On onxv1339, Kafka config is stored here: [x] /bp2/data/config/server-config

        private const string BootstrapServer = "localhost:9092";
        private const string TopicName = "bp.equipmenttopologyplanning.v1.websocketgenericpushtopic";
        //private const string TopicName = "TestTopic";

        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine("Enter processor code \n(1)Producer\n(2)Consumer");
            var selected = Console.ReadKey(false).KeyChar;
            Console.WriteLine("\n");

            switch (selected)
            {
                case '1':
                    var producer = new Producer(BootstrapServer);
                    await producer.StartSendingMessages(TopicName);
                    break;
                case '2':
                    var consumer = new Consumer(BootstrapServer);
                    consumer.StartReceivingMessages(TopicName);
                    break;
            }

            Console.WriteLine("Closing application");
            Console.ReadKey();
        }
    }
}
