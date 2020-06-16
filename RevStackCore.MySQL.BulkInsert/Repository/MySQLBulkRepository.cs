using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using MySql.Data.MySqlClient;
using RevStackCore.Extensions.SQL;
using RevStackCore.Pattern;
using RevStackCore.Pattern.SQL;
using RevStackCore.SQL.Client;

namespace RevStackCore.MySQL.BulkInsert
{
    public class MySQLBulkRepository<TEntity, TKey> : IBulkRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
    {
        private readonly TypedClient<TEntity, MySqlConnection, TKey> _typedClient;
        private readonly MySQLBulkClient<TEntity> _bulkClient;
        public MySQLBulkRepository(MySQLDbContext context)
        {
            _typedClient = new TypedClient<TEntity, MySqlConnection, TKey>(context.ConnectionString, SQLLanguageType.MySQL);
            _bulkClient = new MySQLBulkClient<TEntity>(context.ConnectionString);
        }

        
        public TEntity Add(TEntity entity)
        {
            return _typedClient.Insert(entity);
        }

        public int BulkInsert(IEnumerable<TEntity> entities)
        {
            return _bulkClient.BulkInsert(entities);
        }

        public int BulkUpdate(IEnumerable<TEntity> entities)
        {
            return _bulkClient.BulkUpdate(entities);
        }

        public int BulkDelete()
        {
            return _bulkClient.BulkDelete();
        }

        public void Delete(TEntity entity)
        {
            _typedClient.Delete(entity);
        }

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _typedClient.Find(predicate);
        }

        public IEnumerable<TEntity> Get()
        {
            return _typedClient.GetAll();
        }

        public TEntity GetById(TKey id)
        {
            return _typedClient.GetById(id);
        }

        public TEntity Update(TEntity entity)
        {
            return _typedClient.Update(entity);
        }

        public IDbConnection Db
        {
            get
            {
                return _typedClient.Db;
            }
        }
    }
}
