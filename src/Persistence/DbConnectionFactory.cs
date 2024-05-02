using System.Data;
using Application.Abstractions.Data;
using Npgsql;

namespace Persistence;

public class DbConnectionFactory(NpgsqlDataSource dataSource) : IDbConnectionFactory
{
    public IDbConnection CreateOpenConnection()
    {
        NpgsqlConnection connection = dataSource.OpenConnection();

        return connection;
    }
}