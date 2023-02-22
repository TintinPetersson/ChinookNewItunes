
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

            AddCustomer(customerRepository);

            UpdateCustomer(customerRepository);

            SelectCustomersPerCountry(customerRepository);
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
            Console.WriteLine("\n*** Retrieve all Customers ***\n");
            PrintCustomers(repository.GetAllCustomers());
            Console.WriteLine("\n*** Retrieve Page of Select Customers ***\n");
            PrintCustomers(repository.GetPageOfCustomers(10, 10));
        }
        static void SelectCustomer(ICustomerRepository repository)
        {
            Console.WriteLine("\n** Retrieve a Customer by ID **");
            PrintCustomer(repository.GetCustomerByID("3"));
            Console.WriteLine("\n** Retrieve a Customer by Name **");
            PrintCustomer(repository.GetCustomerByName("Frank"));
        }
        static void AddCustomer(ICustomerRepository repository)
        {
            Customer test = new Customer()
            {
                FirstName = "Filip",
                LastName = "Testson",
                PostalCode = "533 21",
                Country = "Sweden",
                Phone = "1234567890",
                Email = "Filip.testson@test.com"
            };
            if (repository.AddNewCustomer(test))
            {
                Console.WriteLine("\n** Insert worked! **");
            }
            else
            {
                Console.WriteLine("\n** Insert didn't work. **");
            }
        }
        static void UpdateCustomer(ICustomerRepository repository)
        {
            Customer test2 = new Customer()
            {
                FirstName = "UpdatedTest2",
                LastName = "Bond",
                PostalCode = "53330",
                Country = "Sweden",
                Phone = "12434244423",
                Email = "test2.testson@test.com"
            };
            if (repository.UpdateCustomer(test2, "60"))
            {
                Console.WriteLine("\n** Update customer worked! **");
            }
            else
            {
                Console.WriteLine("\n** Update customer didn't work. **");
            }
        }
        private static void SelectCustomersPerCountry(CustomerRepository customerRepository)
        {
            var customerCounts = customerRepository.GetCustomerCountByCountry();
            Console.WriteLine("\n** Customers Per Country **");
            foreach (var customerCount in customerCounts)
            {
                Console.WriteLine($"{customerCount.Key}: {customerCount.Value}");
            }
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