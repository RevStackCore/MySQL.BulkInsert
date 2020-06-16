# RevStackCore.MySQL.BulkInsert

Extends the MySQL implementation of the RevStack Repository Pattern to allow for fast bulk operations

# Nuget Installation

``` bash
Install-Package RevStackCore.MySQL.BulkInsert

```

# Repositories

```cs
public interface IBulkRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
{
    int BulkInsert(IEnumerable<TEntity> entities);
    int BulkUpdate(IEnumerable<TEntity> entities);
    int BulkDelete();
    IDbConnection Db { get; }
}
 
```

# Implementations
MySQLBulkRepository<TEntity,Tkey> implements IBulkRepository<TEntity,TKey> for basic Crud operations, Find and Bulk Operations

# Connection String
For bulk operations, the AllowLoadLocalInfile=true must be included in the connection string.

``` bash
server=localhost;uid=root;pwd=**************;database=MyDb;Allow User Variables=True;AllowLoadLocalInfile=true
```


# Usage

## MySQLDbContext
Inject an instance of MySQLDbContext with the my sql connection string passed in the constructor.
```cs
var dbContext = new MySQLDbContext(connectionString);
```

## Dependency Injection

```cs
using RevStackCore.Pattern.SQL; //IBulkRepository interface
using RevStackCore.MySQL.BulkInsert; 

class Program
{
    static void main(string[] args)
    {
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var db = serviceProvider.GetService<MySQLDbContext>();
        var dataService = serviceProvider.GetService<IDataService>();
        db.RegisterTable<Person, string>()
          .RegisterTable<Tag, int>()
          .RegisterTable<Contact, int>();

        dataService.MyMethod();
    }
    private static void ConfigureServices(IServiceCollection services)
    {
        string connectionString = "";
        services
            .AddSingleton(x => new MySQLDbContext(connectionString))
            .AddSingleton<IBulkRepository<MyTableEntity, int>, MySQLBulkRepository<MyTableEntity, int>>()
            .AddSingleton<IDataService, DataService>();

        services.AddLogging();
        //enforce class name to table name for databse reads/writes
        SQLExtensions.SetTableNameMapper();
    }
}

```



# Asynchronous Services
```cs

public interface IBulkService<TEntity, TKey> : IService<TEntity, TKey> where TEntity : class, IEntity<TKey>
{
    int BulkInsert(IEnumerable<TEntity> entities);
    int BulkUpdate(IEnumerable<TEntity> entities);
    int BulkDelete();
    Task<int> BulkInsertAsync(IEnumerable<TEntity> entities);
    Task<int> BulkUpdateAsync(IEnumerable<TEntity> entities);
    Task<int> BulkDeleteAsync();
    IDbConnection Db { get; }
}

```

# Implementations
BulkService<TEntity,Tkey> implements IBulkService<TEntity,TKey> for basic Async Crud operations, FindAsync and asynchronous bulk operations









