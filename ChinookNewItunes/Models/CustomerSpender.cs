using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinookNewItunes.Models
{
    public readonly record struct CustomerSpender(int CustomerId, decimal TotalSpent);
}
