
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

            SelectCustomer(customerRepository);

            AddCustomer(customerRepository);

            UpdateCustomer(customerRepository);

            SelectCustomersPerCountry(customerRepository);

            SelectHighestSpendingCustomer(customerRepository);

            PrintMostPopularGenresForCustomer(customerRepository, 12);
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
            var customerCountries = customerRepository.GetCustomerCountByCountry();
            Console.WriteLine("\n** Customer Per Country **");
            Console.WriteLine("{0,-20}{1,-20}", "Country", "Count");
            Console.WriteLine("--------------------------");
            foreach (var customerCountry in customerCountries)
            {
                Console.WriteLine("{0,-20}{1,-20}", customerCountry.Country, customerCountry.CustomerCount);
            }
        }
        private static void SelectHighestSpendingCustomer(CustomerRepository customerRepository)
        {
            var customerSpenders = customerRepository.GetHighestSpendingCustomers();
            Console.WriteLine("\n** Highest Spending Customers **");
            Console.WriteLine("{0,-15}{1,-20}", "Customer", "Total Spent");
            Console.WriteLine("----------------------------");
            foreach (var customerSpender in customerSpenders)
            {
                Console.WriteLine("{0,-10}{1,-20}", customerSpender.CustomerId, customerSpender.TotalSpent);
            }
        }
        private static void PrintMostPopularGenresForCustomer(CustomerRepository customerRepository, int customerId)
        {
            List<CustomerGenre> popularGenres = customerRepository.GetMostPopularGenreForCustomer(customerId);

            if (popularGenres.Count > 0)
            {
                Console.WriteLine($"\n** Most popular genres for customer: {customerId}. **");

                int count = 0;

                foreach (CustomerGenre customerGenre in popularGenres)
                {
                    if (popularGenres[0].GenreCount == popularGenres[count].GenreCount)
                    {
                        Console.WriteLine($"- {customerGenre.GenreName}: {customerGenre.GenreCount} tracks");
                    }
                    count++;
                }
            }
            else
            {
                Console.WriteLine($"\n** No genres found for customer: {customerId}. **");
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