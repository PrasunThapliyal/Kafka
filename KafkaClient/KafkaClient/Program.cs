
namespace KafkaClient
{
    using System;
    using System.Threading.Tasks;

    class Program
    {

        private const string BootstrapServer = "onxv1338.ott.ciena.com:9092";
        private const string TopicName = "testTopic";

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
