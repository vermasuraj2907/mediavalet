using MediaValet.Modal;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MediaValet.Agent
{
    public class Program
    {
        static void Main(string[] args)
        {
            Guid agentId = Guid.NewGuid();
            int magicNumber = 0;
            Random random = new Random();
            magicNumber = random.Next(1, 10);
            string rowKey = agentId.ToString();
            Console.WriteLine("I’m agent AgentId, my magic number is " + magicNumber);
            string URL = ConfigurationManager.AppSettings["OrderApiURL"].ToString();
            while (true)
            {
                string result = PostService(URL + "\\getorder?agentid=" + agentId, "get");
                Orders orders = Newtonsoft.Json.JsonConvert.DeserializeObject<Orders>(result);
                if (orders.MagicNumber == magicNumber)
                {
                    Console.WriteLine("Oh no, my magic number was found");
                    break;
                }
                else
                {
                    ConfirmationOrder confirmationOrder = new ConfirmationOrder
                    {
                        AgentId = agentId.ToString(),
                        OrderId = orders.OrderId,
                        OrderStatus = "Processed",
                        PartitionKey = agentId.ToString(),
                        RowKey = rowKey+"_"+ orders.OrderId.ToString(),
                    };
                    PostService(URL + "\\agentordersave", "post", Newtonsoft.Json.JsonConvert.SerializeObject(confirmationOrder));
                    Console.WriteLine($"Send order {orders.OrderId} with random number {orders.MagicNumber}");
                }

            }
            Console.ReadLine();
        }

        public static string PostService(string URL, string type, string obj = "")
        {
            string result = string.Empty;
            using (WebClient webClient = new WebClient())
            {
                webClient.Headers.Add("Content-Type", "application/json");
                if (type.ToLower() == "get")
                {
                        result = webClient.DownloadString(URL);
                }
                if (type.ToLower() == "post")
                {
                    result = webClient.UploadString(URL, obj);
                }
            }
            return result;
        }
    }
}
