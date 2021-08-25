using System; // Namespace for Console output
using System.Configuration; // Namespace for ConfigurationManager
using System.Threading.Tasks; // Namespace for Task
using Azure.Storage.Queues; // Namespace for Queue storage types
using Azure.Storage.Queues.Models; // Namespace for PeekedMessage
using MediaValet.Modal;

namespace MediaValet.Storage
{
    public class OrderQueue
    {
        string queueName = "orders";
        QueueClient queueClient = null;
        //-------------------------------------------------
        // Create the queue service client
        //-------------------------------------------------
        public void CreateQueueClient()
        {
            // Get the connection string from app settings
            string connectionString = ConfigurationManager.AppSettings["ConnectionString"];

            // Instantiate a QueueClient which will be used to create and manipulate the queue
            queueClient = new QueueClient(connectionString, queueName);
        }
        public bool CreateQueue()
        {
            try
            {
                if (queueClient == null)
                {
                    CreateQueueClient();
                }

                // Create the queue
                 queueClient.CreateIfNotExists();

                if (queueClient.Exists())
                {
                    Console.WriteLine($"Queue created: '{queueClient.Name}'");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Make sure the Azurite storage emulator running and try again.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}\n\n");
                Console.WriteLine($"Make sure the Azurite storage emulator running and try again.");
                return false;
            }
        }
        public void InsertMessage(Orders messageobj)
        {
            string message = string.Empty;
            CreateQueueClient();
            if (queueClient == null || queueClient.Exists() == false)
            {
               CreateQueue();
            }
            if (queueClient.Exists())
            {
                // Send a message to the queue
                message = Newtonsoft.Json.JsonConvert.SerializeObject(messageobj);
                queueClient.SendMessage(message);
            }

            Console.WriteLine($"Inserted: {message}");
        }

        //-------------------------------------------------
        // Peek at a message in the queue
        //-------------------------------------------------
        public Orders getMessage(out string MessageId, out string popmessage)
        {
            Orders orders = new Orders();
            MessageId = string.Empty;
            popmessage = string.Empty;
            // Get the connection string from app settings
            CreateQueueClient();
            if (queueClient == null || queueClient.Exists() == false)
            {
                CreateQueue();
            }
            if (queueClient.Exists())
            {
                // Peek at the next message
                var peekedMessage = queueClient.ReceiveMessage();
                orders = peekedMessage.Value.Body.ToObjectFromJson<Orders>();
                MessageId = peekedMessage.Value.MessageId;
                popmessage = peekedMessage.Value.PopReceipt;
                // Display the message
                Console.WriteLine($"Peeked message: '{peekedMessage.Value.Body}'");
            }
            return orders;
        }

        //-------------------------------------------------
        // Delete the queue
        //-------------------------------------------------
        public void DeleteQueue(string MessageId,string popmessage)
        {
            CreateQueueClient();
            if (queueClient.Exists())
            {
                // Delete the queue
                queueClient.DeleteMessage(MessageId, popmessage);
            }

            Console.WriteLine($"Queue deleted: '{queueClient.Name}'");
        }

    }
}
