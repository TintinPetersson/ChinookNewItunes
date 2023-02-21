using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinookNewItunes.Models
{
    public readonly record struct Customer(int CustomerId, string FirstName, string LastName, string Country, string PostalCode, string Phone, string Email);
}
