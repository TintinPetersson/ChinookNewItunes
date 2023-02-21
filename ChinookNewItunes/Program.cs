
using ChinookNewItunes.Models;
using ChinookNewItunes.Repositories;

namespace NotItunesSQLClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var customerRepository = new CustomerRepository { ConnectionString = ConnectionStringHelper.GetConnectionString() };
            SelectAllCustomers(customerRepository);
            // POCO : Plain Old C# Object, and it is what we call model objects!
            SelectCustomer(customerRepository);
            // CRUD
            // Get all customers
            // Call on all the different methods that we create in repository's and where we read/write to
            // database, if we read for data base we convert database tables to c# objects with:
            // 1, Connect 2, Make a command 3, Read data from database 4, convert to c# object!
            // We use interfaces to have abstract methods in this program class that is competently detach for
            // the database so we can switch database with out changing the program class("Front-end")!!
        }
        static void SelectAllCustomers(ICustomerRepository repository)
        {
            PrintCustomers(repository.GetAllCustomers());
        }
        static void SelectCustomer(ICustomerRepository repository)
        {
            Console.WriteLine("\nRetrieve a Customer by ID");
            PrintCustomer(repository.GetCustomerByID("3"));
            Console.WriteLine("\nRetrieve a Customer by Name");
            PrintCustomer(repository.GetCustomerByName("Frank"));
        }
        static void AddCustomer(ICustomerRepository repository)
        {
            Customer test = new Customer()
            {
                CustomerId = 23,
                FirstName = "Test",
                LastName = "Testson",
                PostalCode = "533 21",
                Country = "Sweden",
                Phone = "1234567890",
                Email = "test.testson@test.com"
            };
            if (repository.AddNewCustomer(test))
            {
                Console.WriteLine("Insert worked!");
            }
            else
            {
                Console.WriteLine("Insert didn't work...");
            }
        }
        static void UpdateCustomer(ICustomerRepository repository)
        {
            //PrintCustomer(repository.GetCustomer("Astrid"));
        }
        static void DeleteCustomer(ICustomerRepository repository)
        {
            //PrintCustomer(repository.GetCustomer("Astrid"));
        }
        private static void PrintCustomers(IEnumerable<Customer> customers)
        {
            foreach (Customer customer in customers)
            {
                PrintCustomer(customer);
            }
        }
        private static void PrintCustomer(Customer customer)
        {
            Console.WriteLine($"--- {customer.CustomerId}. {customer.FirstName} {customer.LastName} {customer.Phone} {customer.PostalCode} {customer.Country}");
        }
    }
}