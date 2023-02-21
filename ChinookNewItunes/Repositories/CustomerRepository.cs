using ChinookNewItunes.Models;
using Microsoft.Data.SqlClient;

namespace ChinookNewItunes.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        public string ConnectionString { get; set; } = string.Empty;
        public IEnumerable<Customer> GetAllCustomers()
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();
            var sql = "SELECT CustomerID, FirstName, LastName, Country, " +
                "PostalCode, Phone, Email FROM Customer";
            using var command = new SqlCommand(sql, connection);
            using SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                yield return new Customer(
                    reader.GetInt32(0),
                    SafeGetString(reader, 1),
                    SafeGetString(reader, 2),
                    SafeGetString(reader, 3),
                    SafeGetString(reader, 4),
                    SafeGetString(reader, 5),
                    SafeGetString(reader, 6)
                    );
            }
        }
        public Customer GetCustomerByID(string customerId)
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();
            var sql = "SELECT CustomerID, FirstName, LastName, Country, " +
                "PostalCode, Phone, Email FROM Customer WHERE CustomerId = @customerId";
            using var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@customerId", System.Data.SqlDbType.NVarChar).Value = customerId;
            using SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                return new Customer(
                    reader.GetInt32(0),
                    SafeGetString(reader, 1),
                    SafeGetString(reader, 2),
                    SafeGetString(reader, 3),
                    SafeGetString(reader, 4),
                    SafeGetString(reader, 5),
                    SafeGetString(reader, 6)
                    );
            }
            return new Customer(); // TODO: Change?
        }
        public Customer GetCustomerByName(string customerName)
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();
            var sql = "SELECT CustomerID, FirstName, LastName, Country, " +
                "PostalCode, Phone, Email FROM Customer WHERE FirstName LIKE @customerName";
            using var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@customerName", System.Data.SqlDbType.NVarChar).Value = customerName + '%';
            using SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                return new Customer(
                    reader.GetInt32(0),
                    SafeGetString(reader, 1),
                    SafeGetString(reader, 2),
                    SafeGetString(reader, 3),
                    SafeGetString(reader, 4),
                    SafeGetString(reader, 5),
                    SafeGetString(reader, 6)
                    );
            }
            return new Customer();// TODO: Change?
        }
        public bool AddNewCustomer(Customer customer)
        {
            throw new NotImplementedException();
        }
        public bool DeleteCustomer(string id)
        {
            throw new NotImplementedException();
        }
        public bool UpdateCustomer(Customer customer)
        {
            throw new NotImplementedException();
        }
        public static string SafeGetString(SqlDataReader reader, int colIndex)
        {
            if (!reader.IsDBNull(colIndex))
                return reader.GetString(colIndex);
            return string.Empty;
        }
    }
}