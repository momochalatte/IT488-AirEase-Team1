using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentPage
{
    public class DatabaseService
    {
        private readonly string _connectionString;
        public DatabaseService(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task<List<Order>> GetOrdersAsync()
        {
            using var connection = new System.Data.SqlClient.SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new System.Data.SqlClient.SqlCommand("SELECT * FROM Orders", connection);
            var reader = await command.ExecuteReaderAsync();

            var orders = new List<Order>();
            while (await reader.ReadAsync())
            {
                orders.Add(new Orders
                {
                    OrderId = reader.GetInt32(0),
                    ProductName = reader.GetString(1),
                    Quantity = reader.GetInt32(2),
                    Price = reader.GetDecimal(3)
                });
            }
            return orders;
        }
    }