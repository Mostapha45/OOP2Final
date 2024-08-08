using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResturantManagmentSystem.Components
{
    internal class MenuItem
    {
        // Properties to store information about the menu item.

        // Here we get or set the OrderId for the Menu item 
        public int ItemId { get; set; }
        // Name of the menu item.
        public string Name { get; set; }
        // Cost of the menu item.
        public double Cost { get; set; }
        // Description of the menu item.
        public string Description { get; set; }
        
        // Create a isEditing property to see if the MenuItem is being editied
        public bool IsEditing { get; set; }

        // Constructor to initialize a MenuItem object with provided values intially setting IsEditing to false.
        public MenuItem(int itemId, string name, double cost, string description)
        {
            // Assigning values to the properties.
            ItemId = itemId;
            Name = name;
            Cost = cost;
            Description = description;
            IsEditing = false;
        }

        // Default constructor for MenuItem.
        public MenuItem()
        {
            IsEditing = false;
        }
    }
}
