namespace ResturantManagmentSystem.Components
{
    internal class Order
    {
        // Here we get or set the OrderId for the Order.
        public int OrderId { get; set; }
        // Here we get or set the TableNumber 
        public int TableNumber { get; set; }
        // Name of the customer.
        public string CustomerName { get; set; }
        // List of menu items associated with the order.
        public List<string> Items { get; set; }

        // Constructor to initialize an Order object with provided values.
        public Order(int orderId, int tableNumber, string customerName, List<string> items)
        {
            // Assigning values to the properties.
            OrderId = orderId;
            TableNumber = tableNumber;
            CustomerName = customerName;
            Items = items;
        }

        // Default constructor for Order.
        public Order()
        {
            Items = new List<string>();
        }
    }
}
