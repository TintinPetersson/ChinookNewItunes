using ChinookNewItunes.Models;

namespace ChinookNewItunes.Repositories
{
    public interface ICustomerRepository
    {
        public Customer GetCustomerByID(int customerId);
        public Customer GetCustomerByName(string customerName);
        public IEnumerable<Customer> GetAllCustomers();
        public IEnumerable<Customer> GetPageOfCustomers(int limit, int offset);
        public bool AddNewCustomer(Customer customer);
        public bool UpdateCustomer(Customer customer, int customerId);
        public IEnumerable<CustomerCountry> GetCustomerCountByCountry();
        public IEnumerable<CustomerSpender> GetHighestSpendingCustomers();
        public List<CustomerGenre> GetMostPopularGenreForCustomer(int customerId);
    }
}
