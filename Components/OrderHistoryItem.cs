using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResturantManagmentSystem.Components
{
    internal class OrderHistoryItem
    {
        // Here we get or set the OrderId for the item 
        public int OrderId { get; set; }
        // Here we get or set the TableNumber 
        public int TableNumber { get; set; }
        // Here we get or set the name of the customer.
        public string CustomerName { get; set; }
        // List of menu items associated with the order.
        public List<string> Items { get; set; }

        // Constructor to initialize an OrderHistoryItem object with provided values.
        public OrderHistoryItem(int orderId, int tableNumber, string customerName, List<string> items)
        {
            // Assigning values to the properties.
            OrderId = orderId;
            TableNumber = tableNumber;
            CustomerName = customerName;
            Items = items;
        }

        // Default constructor for OrderHistoryItem.
        public OrderHistoryItem()
        {
            Items = new List<string>();
        }
    }
}
