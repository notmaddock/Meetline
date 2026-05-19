using Meetline.Modules.Users.Infrastructure.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Meetline.Modules.Users.Tests;

public abstract class UsersDbTestBase : IAsyncDisposable
{
    private readonly SqliteConnection _connection;
    protected readonly UsersDbContext Context;

    protected UsersDbTestBase()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<UsersDbContext>()
            .UseSqlite(_connection)
            .Options;

        Context = new UsersDbContext(options);
        Context.Database.EnsureCreated();
    }

    public async ValueTask DisposeAsync()
    {
        await Context.DisposeAsync();
        await _connection.DisposeAsync();
    }
}