using System;
using Microsoft.Data.SqlClient;

namespace InventoryManagementSystem
{
    class Program
    {
        // Update your server & database name here
        static string connectionString = @"Server=LAPTOP-0K5QK01J\SQLEXPRESS;Database=InventoryDB;Trusted_Connection=True;TrustServerCertificate=True;";

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("==== Inventory Management System ====");
                Console.WriteLine("1. View Products");
                Console.WriteLine("2. Add Product");
                Console.WriteLine("3. Update Product");
                Console.WriteLine("4. Delete Product");
                Console.WriteLine("5. Search Product");
                Console.WriteLine("6. Exit");

                Console.Write("\nSelect an option: ");
                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Invalid choice. Press Enter to continue...");
                    Console.ReadLine();
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        ViewProducts();
                        break;
                    case 2:
                        AddProduct();
                        break;
                    case 3:
                        UpdateProduct();
                        break;
                    case 4:
                        DeleteProduct();
                        break;
                    case 5:
                        SearchProduct();
                        break;
                    case 6:
                        return; // Exit program
                    default:
                        Console.WriteLine("Invalid choice. Press Enter to continue...");
                        Console.ReadLine();
                        break;
                }
            }
        }

        static void ViewProducts()
        {
            Console.WriteLine("\n=== All Products ===");
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM Products";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Console.WriteLine("\nID\tName\t\tQuantity\tSize\tUnit\tPrice");
                        Console.WriteLine("------------------------------------------------");

                        while (reader.Read())
                        {
                            int quantity = (int)reader["Quantity"];
                            string lowStock = quantity < 5 ? " (Low Stock!)" : "";
                            Console.WriteLine(
                                $"{reader["ProductID"],-5} +" +
                                $"{reader["Name"],-15} +" +
                                $"{quantity,-8} +" +
                                $"{reader["Size"],-6} +" +
                                $"{reader["Unit"],-8} +" +
                                $"{reader["Price"],-8}{lowStock}"
                             );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving products: " + ex.Message);
            }

            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }

        static void AddProduct()
        {
            Console.WriteLine("\n=== Add Product ===");

            Console.Write("Enter product name: ");
            string name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Product name cannot be empty. Press Enter to return to menu.");
                Console.ReadLine();
                return;
            }

            Console.Write("Enter quantity (stock count): ");
            if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity < 0)
            {
                Console.WriteLine("Invalid quantity. Press Enter to return to menu.");
                Console.ReadLine();
                return;
            }

            Console.Write("Enter size (e.g 1, 500, 2.5): ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal size))
            {
                Console.WriteLine("Invalid size. Please Enter to return. ");
                Console.ReadLine();
                return;
            }

            Console.Write("Enter unit (kg / g / L / ml / pcs): ");
            string unit = Console.ReadLine().ToLower();
            if (unit != "kg" && unit != "g" && unit != "l" && unit != "ml" && unit != "pcs")
            {
                Console.WriteLine("Invalid unit. Please Enter to return.");
                Console.ReadLine();
                return;
            }


            Console.Write("Enter price: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price) || price < 0)
            {
                Console.WriteLine("Invalid price. Press Enter to return to menu.");
                Console.ReadLine();
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Products (Name, Quantity, Size, Unit, Price) VALUES (@name, @quantity, @size, @unit, @price)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@quantity", quantity);
                        cmd.Parameters.AddWithValue("@size", size);
                        cmd.Parameters.AddWithValue("@unit", unit);
                        cmd.Parameters.AddWithValue("@price", price);
                        cmd.ExecuteNonQuery();
                    }
                }

                Console.WriteLine("Product added successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding product: " + ex.Message);
            }

            Console.WriteLine("Press Enter to return to menu...");
            Console.ReadLine();
        }

        static void UpdateProduct()
        {
            Console.WriteLine("\n=== Update Product ===");

            Console.Write("Enter ProductID to update: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ProductID. Press Enter to return to menu.");
                Console.ReadLine();
                return;
            }

            Console.Write("Enter new name: ");
            string name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Product name cannot be empty. Press Enter to return to menu.");
                Console.ReadLine();
                return;
            }

            Console.Write("Enter new quantity: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity < 0)
            {
                Console.WriteLine("Invalid quantity. Press Enter to return to menu.");
                Console.ReadLine();
                return;
            }

            Console.Write("Enter new size (e.g 1, 500, 2.5): ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal size))
            {
                Console.WriteLine("Invalid size. Please Enter to return. ");
                Console.ReadLine();
                return;
            }

            Console.Write("Enter new unit (kg / g / L / ml / pcs): ");
            string unit = Console.ReadLine().ToLower();
            if (unit != "kg" && unit != "g" && unit != "l" && unit != "ml" && unit != "pcs")
            {
                Console.WriteLine("Invalid unit. Please Enter to return.");
                Console.ReadLine();
                return;
            }

            Console.Write("Enter new price: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price) || price < 0)
            {
                Console.WriteLine("Invalid price. Press Enter to return to menu.");
                Console.ReadLine();
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE Products SET Name=@name, Quantity=@quantity, Size=@size, Unit=@unit, Price=@price WHERE ProductID=@id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@quantity", quantity);
                        cmd.Parameters.AddWithValue("@size", size);
                        cmd.Parameters.AddWithValue("@unit", unit);
                        cmd.Parameters.AddWithValue("@price", price);
                        cmd.Parameters.AddWithValue("@id", id);

                        int rows = cmd.ExecuteNonQuery();
                        Console.WriteLine(rows > 0 ? "Product updated!" : "Product not found.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating product: " + ex.Message);
            }

            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }

        static void DeleteProduct()
        {
            Console.WriteLine("\n=== Delete Product ===");

            Console.Write("Enter ProductID to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ProductID. Press Enter to return to menu.");
                Console.ReadLine();
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM Products WHERE ProductID=@id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        int rows = cmd.ExecuteNonQuery();
                        Console.WriteLine(rows > 0 ? "Product deleted!" : "Product not found.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting product: " + ex.Message);
            }

            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }

        static void SearchProduct()
        {
            Console.WriteLine("\n=== Search Product ===");

            Console.Write("Enter product name to search: ");
            string name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Search term cannot be empty. Press Enter to return to menu.");
                Console.ReadLine();
                return;
            }

            Console.Write("Enter size to search (e.g 1, 500, 2.5): ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal size))
            {
                Console.WriteLine("Invalid size. Please Enter to return. ");
                Console.ReadLine();
                return;
            }

            Console.Write("Enter unit to search (kg / g / L / ml / pcs): ");
            string unit = Console.ReadLine().ToLower();
            if (unit != "kg" && unit != "g" && unit != "l" && unit != "ml" && unit != "pcs")
            {
                Console.WriteLine("Invalid unit. Please Enter to return.");
                Console.ReadLine();
                return;
            }
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM Products WHERE Name LIKE @name AND Size=@size AND Unit=@unit";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", "%" + name + "%");
                        cmd.Parameters.AddWithValue("@size", size);
                        cmd.Parameters.AddWithValue("@unit", unit);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            Console.WriteLine("\nID\tName\t\tQuantity\tSize\tUnit\tPrice");
                            Console.WriteLine("-----------------------------------------------");

                            while (reader.Read())
                            {
                                int quantity = (int)reader["Quantity"];
                                string lowStock = quantity < 5 ? " (Low Stock!)" : "";
                                Console.WriteLine($"{reader["ProductID"],-5}" +
                                    $" {reader["Name"],-15} " +
                                    $"{quantity,-8} " +
                                    $"{reader["Size"],-6}" +
                                    $"{reader["Unit"],-5}" +
                                    $"{reader["Price"],-8}{lowStock}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error searching product: " + ex.Message);
            }

            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }
    }
}
