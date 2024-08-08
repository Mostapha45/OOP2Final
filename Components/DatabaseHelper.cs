using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using Microsoft.Data.Sqlite;
using ResturantManagmentSystem.Components;

//This is the DatabaseHelper class which will Create our database and their tables with the required columns as well as their constraints, This class also holds the methods that we will be calling to Add, Update and retrieve User Records, Menu Items and Order Records. This class will also help us manage the order history.
namespace ResturantManagmentSystem.Components
{
    internal class DatabaseHelper
    {
        private string databaseName;

        public bool IsInfoCovered { get; set; }

        // Constructor to create the DatabaseHelper with a name
        public DatabaseHelper(string dbName)
        {
            databaseName = dbName;
            CreateDatabase(); // Ensure database and tables are created when the helper is instantiated
        }

        // CreateDatabase Method in order to create the database tables if they do not exist along with their columns and constraints. We first create a new connection to the database,
        // then open it, we then call createCommand and CommandText to execute the SQL Queries then because CREATE TABLE IS a non-query command we use ExecuteNonQuery to run it.
        public void CreateDatabase()
        {
            string connectionString = $"Data Source={databaseName}.db";
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Users (
                        Users_id INTEGER PRIMARY KEY AUTOINCREMENT,
                        first_name TEXT NOT NULL,
                        last_name TEXT NOT NULL,
                        Users_email TEXT NOT NULL,
                        Users_phone TEXT NOT NULL,
                        position TEXT NOT NULL
                    );

                    CREATE TABLE IF NOT EXISTS MenuItem (
                        Item_id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Item_name TEXT NOT NULL,
                        Item_cost REAL NOT NULL,
                        Item_description TEXT NOT NULL
                    );

                    CREATE TABLE IF NOT EXISTS Orders (
                        Order_id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Table_number INTEGER NOT NULL,
                        Customer_name TEXT NOT NULL,
                        Menu_items TEXT NOT NULL
                    );
                ";
                command.ExecuteNonQuery();
            }
        }

        // DeleteDatabase method to delete the database file.
        public void DeleteDatabase()
        {
            File.Delete($"{databaseName}.db");
        }

        // GetUsers method to retrieve all of the users from the User table using the sql query to select the information in users,then using the execute reader command to begin reading the users information while it also loops through each record, inside this loop the values are being populated from information in the current record and then added to the users list.
        public List<User> GetUsers()
        {
            string connectionString = $"Data Source={databaseName}.db";
            var users = new List<User>();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Users";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var user = new User
                        {
                            UserId = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            Email = reader.GetString(3),
                            Phone = reader.GetString(4),
                            Position = reader.GetString(5)
                        };
                        users.Add(user);
                    }
                }
            }

            return users;
        }


        // AddUser method created to Add different users to the table. In this method we use the connectionString variable to specify our path to the db, we then set up our try block to handle exceptions that may occur and then we set a new connection the database using our SqliteConnection variable,
        // once that is done we open the connection and begin executing sql statements, we then populate the users table with a new user with the correct values using the command.Parameters.AddWithValue so that both paremeters are binded together and inserted correctly.
        public void AddUser(string firstName, string lastName, string email, string phone, string position)
        {
            string connectionString = $"Data Source={databaseName}.db";
            IsInfoCovered = false;

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = @"
                        INSERT INTO Users (first_name, last_name, Users_email, Users_phone, position)
                        VALUES ($firstName, $lastName, $email, $phone, $position)
                    ";
                    command.Parameters.AddWithValue("$firstName", firstName);
                    command.Parameters.AddWithValue("$lastName", lastName);
                    command.Parameters.AddWithValue("$email", email);
                    command.Parameters.AddWithValue("$phone", phone);
                    command.Parameters.AddWithValue("$position", position);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                IsInfoCovered = true;
            }
        }


        // UpdateUser method created to update an existing user from the table. In this method we use the connectionString variable to specify our path to the db, we then set up our try block to handle exceptions that may occur and then we set a new connection the database using our SqliteConnection variable,
        // once that is done we open the connection and begin executing sql statements which will update the Users information with the new info by setting all the values and then updating it for the users id which has been selected. We also use the parameters.AddWithValue in order to be able to bind the code with the sql commands. Then our catch block sets the IsInfoCovered varaible to true in order to catch any exception that happens.
        public void UpdateUser(int id, string firstName, string lastName, string email, string phone, string position)
        {
            string connectionString = $"Data Source={databaseName}.db";

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = @"
                        UPDATE Users 
                        SET first_name = $firstName, last_name = $lastName, Users_email = $email, Users_phone = $phone, position = $position 
                        WHERE Users_id = $id
                    ";
                    command.Parameters.AddWithValue("$id", id);
                    command.Parameters.AddWithValue("$firstName", firstName);
                    command.Parameters.AddWithValue("$lastName", lastName);
                    command.Parameters.AddWithValue("$email", email);
                    command.Parameters.AddWithValue("$phone", phone);
                    command.Parameters.AddWithValue("$position", position);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                IsInfoCovered = true;
            }
        }


        //This is our deleteUser method which deletes a user from the database and application by using their id.
        public void DeleteUser(int id)
        {
            string connectionString = $"Data Source={databaseName}.db";

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    DELETE FROM Users 
                    WHERE Users_id = $id
                ";
                command.Parameters.AddWithValue("$id", id);

                command.ExecuteNonQuery();
            }
        }

        // This is the GetMenuItems method which will return the menu items from the MenuItem table and uses a loop to go through each record in the results but while inside the loop a new menu item object will be created and then populated with the correct data,
        // and then we return the list of MenuItems.
        public List<MenuItem> GetMenuItems()
        {
            string connectionString = $"Data Source={databaseName}.db";
            var menuItems = new List<MenuItem>();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM MenuItem";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item = new MenuItem
                        {
                            ItemId = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Cost = reader.GetDouble(2),
                            Description = reader.GetString(3)
                        };
                        menuItems.Add(item);
                    }
                }
            }

            return menuItems;
        }

        //This is the AddMenuItem method which will add a new Menu item to the list by connecting to the database and using a sql command to insert values into the MenuItem table with their respective information
        // we also have a exception with a try block to ensure that errors are handled and caught if IsInfoCovered is set to true.
        public void AddMenuItem(string name, double cost, string description)
        {
            string connectionString = $"Data Source={databaseName}.db";
            IsInfoCovered = false;

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = @"
                        INSERT INTO MenuItem (Item_name, Item_cost, Item_description)
                        VALUES ($name, $cost, $description)
                    ";
                    command.Parameters.AddWithValue("$name", name);
                    command.Parameters.AddWithValue("$cost", cost);
                    command.Parameters.AddWithValue("$description", description);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                IsInfoCovered = true;
            }
        }


        //Here we have the UpdateMenuItem method that takes 4 parameters and creates a new connection to the database, we then create an sql query that will insert the correct values by using the .AddWithValue to bind the sql commands with the method commands as well.
        public void UpdateMenuItem(int id, string name, double cost, string description)
        {
            string connectionString = $"Data Source={databaseName}.db";

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    UPDATE MenuItem 
                    SET Item_name = $name, Item_cost = $cost, Item_description = $description 
                    WHERE Item_id = $id
                ";
                command.Parameters.AddWithValue("$id", id);
                command.Parameters.AddWithValue("$name", name);
                command.Parameters.AddWithValue("$cost", cost);
                command.Parameters.AddWithValue("$description", description);

                command.ExecuteNonQuery();
            }
        }


        //This is the DeleteMenuItem method which takes one parameter that is the MenuItemId and connects to the database which we then run a sql query to search for the itemid which we are deleting and execute the query to remove the item.
        public void DeleteMenuItem(int id)
        {
            string connectionString = $"Data Source={databaseName}.db";

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    DELETE FROM MenuItem 
                    WHERE Item_id = $id
                ";
                command.Parameters.AddWithValue("$id", id);

                command.ExecuteNonQuery();
            }
        }

        //This is the GetOrders method which executes a sql query to select all the orders from the orders table and then uses a loop to go through each record, inside this loop the Order_id, Table_number, Customer_name, and Menu_items are retrieved from the orders table we then split the menu items string into a list of items and create a new order object with the data we retrieved and add it tot the orders list which then returns the list of orders.
        public List<Order> GetOrders()
        {
            string connectionString = $"Data Source={databaseName}.db";
            var orders = new List<Order>();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Order_id, Table_number, Customer_name, Menu_items FROM Orders";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int orderId = reader.GetInt32(0);
                        int tableNumber = reader.GetInt32(1);
                        string customerName = reader.GetString(2);
                        string menuItems = reader.GetString(3);

                        var items = new List<string>(menuItems.Split(',')); 

                        var order = new Order(orderId, tableNumber, customerName, items);
                        orders.Add(order);
                    }
                }
            }

            return orders;
        }


        //This is the AddOrder method which will use a loop to go through each item in the Items list and then we use a command to insert a order into the orders table using the tableNumber,CustomerName and item attributes we also make use of an exception block that sets the variable to true if an error occurs during runtime.
        public void AddOrder(int tableNumber, string customerName, List<string> items)
        {
            string connectionString = $"Data Source={databaseName}.db";
            IsInfoCovered = false;

            if (string.IsNullOrEmpty(customerName))
            {
                IsInfoCovered = true;
                return;
            }

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();

                    foreach (var item in items)
                    {
                        var command = connection.CreateCommand();
                        command.CommandText = @"
                            INSERT INTO Orders (Table_number, Customer_name, Menu_items)
                            VALUES ($tableNumber, $customerName, $item)
                        ";
                        command.Parameters.AddWithValue("$tableNumber", tableNumber);
                        command.Parameters.AddWithValue("$customerName", customerName);
                        command.Parameters.AddWithValue("$item", item);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                IsInfoCovered = true;
            }
        }

        //Here is the DeleteOrder method which will use the sql query to delete a order from order history using the table number which will then remove it from the databse as well
        public void DeleteOrder(int tableNumber)
        {
            string connectionString = $"Data Source={databaseName}.db";

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    DELETE FROM Orders 
                    WHERE Table_number = $tableNumber
                ";
                command.Parameters.AddWithValue("$tableNumber", tableNumber);

                command.ExecuteNonQuery();
            }
        }

        // this is the GetOrderHistory method which will select the order and the respecitve columns from Orders we use a while loop in the method to retrieve all the required information and then create a new orderhistoryitem object with it, which will then be added to the order history list.
        public List<OrderHistoryItem> GetOrderHistory()
        {
            var orderHistory = new List<OrderHistoryItem>();
            string connectionString = $"Data Source={databaseName}.db";

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Order_id, Table_number, Customer_name, Menu_items FROM Orders";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var order = new OrderHistoryItem
                        {
                            OrderId = reader.GetInt32(0),
                            TableNumber = reader.GetInt32(1),
                            CustomerName = reader.GetString(2),
                            Items = reader.GetString(3).Split(',').ToList()
                        };
                        orderHistory.Add(order);
                    }
                }
            }
            return orderHistory;
        }


        //This is the DeleteOrderHistory method which we use to remove a order history item from the orders table which uses an sql query to do so.
        public void DeleteOrderHistory(int orderId)
        {
            string connectionString = $"Data Source={databaseName}.db";

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    DELETE FROM Orders 
                    WHERE Order_id = $orderId
                ";
                command.Parameters.AddWithValue("$orderId", orderId);

                command.ExecuteNonQuery();
            }
        }
    }
}
