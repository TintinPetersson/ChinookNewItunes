using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinookNewItunes.Repositories
{
    public class ConnectionStringHelper
    {
        public static string Tintins = "0850";
        public static string Filips = "4689";

        public static string GetConnectionString()
        {
            SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder();
            //connectionStringBuilder.DataSource = $"(localdb)\\MSSQLLocalDB";
            connectionStringBuilder.DataSource = $"N-SE-01-{Filips}\\SQLEXPRESS";
            connectionStringBuilder.InitialCatalog = "Chinook";
            connectionStringBuilder.IntegratedSecurity = true;
            connectionStringBuilder.TrustServerCertificate = true;

            return connectionStringBuilder.ConnectionString;
        }
    }
}
