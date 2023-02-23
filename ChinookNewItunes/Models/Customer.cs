namespace ChinookNewItunes.Models
{
    public readonly record struct Customer(int CustomerId, string FirstName, string LastName, string Country, string PostalCode, string Phone, string Email);
}
