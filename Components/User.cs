using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResturantManagmentSystem.Components
{
    internal class User
    {
        // Properties to store information about the user.

        // Unique identifier for the user.
        public int UserId { get; set; }
        // First name of the user.
        public string FirstName { get; set; }
        // Last name of the user.
        public string LastName { get; set; }
        // Email address of the user.
        public string Email { get; set; }
        // Phone number of the user.
        public string Phone { get; set; }
        // Position or role of the user.
        public string Position { get; set; }


        // Creating a isEditing property for the user to ensure it is able to be editied.
        public bool IsEditing { get; set; }



        // Constructor to initialize a User object with provided values and assigning the default value of false to IsEditing till it is decided that it needs to be edited.
        public User(int userId, string firstName, string lastName, string email, string phone, string position)
        {
            // Assigning values to the properties.
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
            Position = position;
            IsEditing = false; 
        }

        // Default constructor for User.
        public User()
        {
            IsEditing = false;
        }
    }
}
