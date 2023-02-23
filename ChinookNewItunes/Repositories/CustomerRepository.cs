using ChinookNewItunes.Models;
using Microsoft.Data.SqlClient;
using System.Data;

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
        public Customer GetCustomerByID(int customerId)
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();

            var sql = "SELECT CustomerID, FirstName, LastName, Country, " +
                "PostalCode, Phone, Email FROM Customer WHERE CustomerId = @customerId";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@customerId", SqlDbType.Int).Value = customerId;

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
            return new Customer();
        }
        public Customer GetCustomerByName(string customerName)
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();

            var sql = "SELECT CustomerID, FirstName, LastName, Country, " +
                "PostalCode, Phone, Email FROM Customer WHERE FirstName LIKE @customerName";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@customerName", SqlDbType.NVarChar).Value = customerName + '%';

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
            return new Customer();
        }
        public IEnumerable<Customer> GetPageOfCustomers(int limit, int offset)
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();

            var sql = "SELECT CustomerID, FirstName, LastName, Country, " +
                "PostalCode, Phone, Email FROM Customer ORDER BY CustomerId OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY;";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@limit", SqlDbType.Int).Value = limit;
            command.Parameters.Add("@offset", SqlDbType.Int).Value = offset;

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
        public bool AddNewCustomer(Customer customer)
        {
            bool success = false;
            var sql =
                    "INSERT INTO Customer (FirstName, LastName, Country, PostalCode, Phone, Email) " +
                    "VALUES (@firstName, @lastName, @country, @postalCode, @phone, @email); ";
            try
            {
                using var connection = new SqlConnection(ConnectionString);
                connection.Open();

                using var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@firstName", customer.FirstName);
                command.Parameters.AddWithValue("@lastName", customer.LastName);
                command.Parameters.AddWithValue("@country", customer.Country);
                command.Parameters.AddWithValue("@postalCode", customer.PostalCode);
                command.Parameters.AddWithValue("@phone", customer.Phone);
                command.Parameters.AddWithValue("@email", customer.Email);
                success = command.ExecuteNonQuery() > 0 ? true : false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return success;
        }
        public bool UpdateCustomer(Customer customer, int customerId)
        {
            {
                bool success = false;
                var sql =
                        "UPDATE Customer SET FirstName = @firstName, LastName = @lastName, Country =  @country, " +
                        "PostalCode = @postalCode, Phone = @phone, Email = @email WHERE CustomerId = @customerId";

                try
                {
                    using var connection = new SqlConnection(ConnectionString);
                    connection.Open();

                    using var command = new SqlCommand(sql, connection);
                    command.Parameters.Add("@customerId", SqlDbType.Int).Value = customerId;
                    command.Parameters.Add("@firstName", SqlDbType.NVarChar).Value = customer.FirstName;
                    command.Parameters.Add("@lastName", SqlDbType.NVarChar).Value = customer.LastName;
                    command.Parameters.Add("@country", SqlDbType.NVarChar).Value = customer.Country;
                    command.Parameters.Add("@postalCode", SqlDbType.NVarChar).Value = customer.PostalCode;
                    command.Parameters.Add("@phone", SqlDbType.NVarChar).Value = customer.Phone;
                    command.Parameters.Add("@email", SqlDbType.NVarChar).Value = customer.Email;
                    success = command.ExecuteNonQuery() > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                return success;
            }
        }
        public IEnumerable<CustomerCountry> GetCustomerCountByCountry()
        {
            List<CustomerCountry> customerCountries = new List<CustomerCountry>();
            string query = "SELECT Country, COUNT(*) AS Number_of_Customers " +
                           "FROM Customer " +
                           "GROUP BY Country " +
                           "ORDER BY Number_of_Customers DESC;";

            using var connection = new SqlConnection(ConnectionString);
            connection.Open();

            SqlCommand command = new SqlCommand(query, connection);

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string country = reader.GetString(0);
                int customerCount = reader.GetInt32(1);
                customerCountries.Add(new CustomerCountry { Country = country, CustomerCount = customerCount });
            }

            reader.Close();
            return customerCountries;
        }
        public IEnumerable<CustomerSpender> GetHighestSpendingCustomers()
        {
            List<CustomerSpender> customerSpenders = new List<CustomerSpender>();
            string query = "SELECT CustomerId, SUM(Total) AS TotalSpent " +
                           "FROM Invoice " +
                           "GROUP BY CustomerId " +
                           "ORDER BY TotalSpent DESC;";

            using var connection = new SqlConnection(ConnectionString);
            connection.Open();

            SqlCommand command = new SqlCommand(query, connection);

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                int customerId = reader.GetInt32(0);
                decimal totalSpent = reader.GetDecimal(1);
                customerSpenders.Add(new CustomerSpender { CustomerId = customerId, TotalSpent = totalSpent });
            }
            reader.Close();

            return customerSpenders;
        }
        public List<CustomerGenre> GetMostPopularGenreForCustomer(int customerId)
        {
            List<CustomerGenre> popularGenres = new List<CustomerGenre>();

            string sqlQuery =
                        @"SELECT TOP 2 customer.CustomerId, genre.Name AS GenreName, COUNT(track.GenreId) AS GenreCount
                        FROM Customer customer
                        INNER JOIN Invoice invoice ON customer.CustomerId = invoice.CustomerId
                        INNER JOIN InvoiceLine invoiceLine ON invoice.InvoiceId = invoiceLine.InvoiceId
                        INNER JOIN Track track ON invoiceLine.TrackId = track.TrackId
                        INNER JOIN Genre genre ON track.GenreId = genre.GenreId
                        WHERE customer.CustomerId = @customerId
                        GROUP BY customer.CustomerId, genre.Name
                        ORDER BY GenreCount DESC";

            using var connection = new SqlConnection(ConnectionString);
            connection.Open();

            SqlCommand command = new SqlCommand(sqlQuery, connection);
            command.Parameters.AddWithValue("@CustomerId", customerId);

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                CustomerGenre customerGenre = new CustomerGenre();

                customerGenre.CustomerId = reader.GetInt32(0);
                customerGenre.GenreName = reader.GetString(1);
                customerGenre.GenreCount = reader.GetInt32(2);

                if (customerGenre.GenreCount == 0)
                {
                    break;
                }

                popularGenres.Add(customerGenre);
            }

            return popularGenres;

        }

        public static string SafeGetString(SqlDataReader reader, int colIndex)
        {
            if (!reader.IsDBNull(colIndex))
                return reader.GetString(colIndex);
            return string.Empty;
        }
    }
}