using MediaValet.Modal;
using MediaValet.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaValet.BL
{
    public class SuperVisorBL
    {
        private int GetOrderId()
        {
            int OrderId = 0;
            ConfirmationTable confirmationTable = new ConfirmationTable();
            var datafromconfirmationtable = confirmationTable.GetOrderIds("OrderId", "OrderId");
            if (datafromconfirmationtable == null)
            {
                 confirmationTable.SaveOrderIds(new OrderIds
                {
                    PartitionKey = "OrderId",
                    RowKey = "OrderId",
                    OrderId = 1
                });
                OrderId = 1;
            }
            else
            {
                OrderId = confirmationTable.UpdateOrderId("OrderId", "OrderId");
            }
            return OrderId;
        }
        public Orders GetOrder(string AgentId)
        {
            ConfirmationTable confirmationTable = new ConfirmationTable();
            var datafromconfirmationtable =  confirmationTable.GetOrder(AgentId, AgentId);
            OrderQueue orderQueue = new OrderQueue();
            int OrderId = Convert.ToInt32(GetOrderId());
            int magicNumber = 0;
            Random random = new Random();
            magicNumber = random.Next(1, 10);
             orderQueue.InsertMessage(new Orders
            {
                OrderId = OrderId,
                MagicNumber = magicNumber,
                OrderText = "Order Number " + OrderId.ToString()
            });
            string MessageId = string.Empty;
            string popmessage = string.Empty;
            Orders queueOrder =  orderQueue.getMessage(out MessageId, out popmessage);
            orderQueue.DeleteQueue(MessageId, popmessage);
            return queueOrder;
        }
    }
}
