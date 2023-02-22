﻿using ChinookNewItunes.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

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
        public IEnumerable<Customer> GetPageOfCustomers(int limit, int offset)
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();

            var sql = "SELECT CustomerID, FirstName, LastName, Country, " +
                "PostalCode, Phone, Email FROM Customer ORDER BY CustomerId OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY;";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@limit", System.Data.SqlDbType.Int).Value = limit;
            command.Parameters.Add("@offset", System.Data.SqlDbType.Int).Value = offset;

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
                //Log
            }
            return success;
        }
        public bool UpdateCustomer(Customer customer, string customerId)
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
                    command.Parameters.Add("@customerId", System.Data.SqlDbType.NVarChar).Value = customerId;
                    command.Parameters.Add("@firstName", System.Data.SqlDbType.NVarChar).Value = customer.FirstName;
                    command.Parameters.Add("@lastName", System.Data.SqlDbType.NVarChar).Value = customer.LastName;
                    command.Parameters.Add("@country", System.Data.SqlDbType.NVarChar).Value = customer.Country;
                    command.Parameters.Add("@postalCode", System.Data.SqlDbType.NVarChar).Value = customer.PostalCode;
                    command.Parameters.Add("@phone", System.Data.SqlDbType.NVarChar).Value = customer.Phone;
                    command.Parameters.Add("@email", System.Data.SqlDbType.NVarChar).Value = customer.Email;
                    success = command.ExecuteNonQuery() > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    //Log
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

        public static string SafeGetString(SqlDataReader reader, int colIndex)
        {
            if (!reader.IsDBNull(colIndex))
                return reader.GetString(colIndex);
            return string.Empty;
        }
    }
}