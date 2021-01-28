using System.Data;
using Npgsql;
using Microsoft.Extensions.Configuration;

namespace Excel
{
    public interface IDbContext
    {
        IDbConnection GetConnection(string connectionString = "Default");
    }

    public class DbContext : IDbContext
    {
        private readonly IConfiguration _configuration;
        public DbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IDbConnection GetConnection(string connectionName = "Default")
        {
            var connectionString = _configuration.GetConnectionString(connectionName);
            return new NpgsqlConnection(connectionString);
        }
    }
}