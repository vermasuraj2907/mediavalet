using Azure;
using Azure.Data.Tables;
using MediaValet.Modal;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MediaValet.Storage
{
    public class ConfirmationTable
    {
        TableClient tableClient = null;
        const string tableName = "confirmations";
        private void Createtableclient()
        {
            string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
            tableClient = new TableClient(connectionString, tableName);
        }
        private void CreateTable()
        {
            if (tableClient == null)
            {
                Createtableclient();
            }

            // Create the table if it doesn't already exist.
            try
            {
               var obj = tableClient.CreateIfNotExists();
                // Now attempt to create the same table unconditionally.

            }
            catch (RequestFailedException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void SaveOrder(ConfirmationOrder confirmationOrder)
        {
           CreateTable();
            var tabresponse = tableClient.AddEntity<ConfirmationOrder>(confirmationOrder);
        }
        public ConfirmationOrder GetOrder(string PartitionKey,string RowKey)
        {
            CreateTable();
            try
            {
                var tabresponse = tableClient.GetEntity<ConfirmationOrder>(PartitionKey, RowKey);
                return tabresponse.Value;
            }
            catch (RequestFailedException ex) when (ex.Status == (int)HttpStatusCode.NotFound)
            {

            }
            return null; 
        }
        public OrderIds GetOrderIds(string PartitionKey, string RowKey)
        {
            CreateTable();
            try
            {
                var tabresponse = tableClient.GetEntity<OrderIds>(PartitionKey, RowKey);
                return tabresponse.Value;
            }
            catch (RequestFailedException ex) when (ex.Status == (int)HttpStatusCode.NotFound)
            {

            }
            return null;
        }
        public void SaveOrderIds(OrderIds confirmationOrder)
        {
            CreateTable();
            try
            {
                var tabresponse = tableClient.AddEntity<OrderIds>(confirmationOrder);
            }
            catch (RequestFailedException ex) 
            {

            }
        }
        public int UpdateOrderId(string PartitionKey, string RowKey)
        {
            CreateTable();
            try
            {
                var tabresponse =  tableClient.GetEntity<OrderIds>(PartitionKey, RowKey);
                tabresponse.Value.OrderId = tabresponse.Value.OrderId + 1;
                 tableClient.DeleteEntity(PartitionKey, RowKey);
                 SaveOrderIds(tabresponse.Value);
                return tabresponse.Value.OrderId;
            }
            catch (RequestFailedException ex) when (ex.Status == (int)HttpStatusCode.NotFound)
            {

            }
            return 0;
        }
    }
}
