using System.Data.Common;
using Application.Abstractions.Data;
using Npgsql;

namespace Persistence;

public class DbConnectionFactory(NpgsqlDataSource dataSource) : IDbConnectionFactory
{
    public async ValueTask<DbConnection> OpenConnectionAsync()
    {
        return await dataSource.OpenConnectionAsync();
    }
}
