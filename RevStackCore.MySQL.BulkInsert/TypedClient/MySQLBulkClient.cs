using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using CsvHelper;
using CsvHelper.TypeConversion;
using Dapper;
using MySql.Data.MySqlClient;
using RevStackCore.DataImport;

namespace RevStackCore.MySQL.BulkInsert
{
    public class MySQLBulkClient<TEntity> where TEntity : class
    {
        private readonly string _connectionString;
        private readonly string _type;
        public MySQLBulkClient(string connectionString)
        {
            _connectionString = connectionString;
            _type = typeof(TEntity).Name;
        }

        public IDbConnection Db
        {
            get
            {
                var connection = new MySqlConnection(_connectionString);
                IDbConnection db = connection;
                return db;
            }
        }

        public int BulkInsert(IEnumerable<TEntity> entities)
        {
            var stream = ExportCsvStream(entities);
            var connection = new MySqlConnection(_connectionString);
            using (MySqlConnection db = connection)
            {
                stream.Position = 0;
                var bulk = new MySqlBulkLoader(db);
                bulk.SourceStream = stream;
                bulk.TableName = _type;
                bulk.FieldTerminator = ",";
                //bulk.LineTerminator = "\n";
                bulk.NumberOfLinesToSkip = 1;
                bulk.FieldQuotationCharacter = '"';
                int result = bulk.Load();
                stream.Dispose();
                return result;
            }
        }

        public int BulkUpdate(IEnumerable<TEntity> entities)
        {
            var stream = ExportCsvStream(entities);
            var connection = new MySqlConnection(_connectionString);
            using (MySqlConnection db = connection)
            {
                stream.Position = 0;
                var bulk = new MySqlBulkLoader(db);
                bulk.SourceStream = stream;
                bulk.TableName = _type;
                bulk.FieldTerminator = ",";
                //bulk.LineTerminator = "\n";
                bulk.NumberOfLinesToSkip = 1;
                bulk.FieldQuotationCharacter = '"';
                int result = bulk.Load();
                stream.Dispose();
                return result;
            }
        }

        public int BulkDelete()
        {
            var connection = new MySqlConnection(_connectionString);
            using (MySqlConnection db = connection)
            {
                string sql = "Delete From " + _type;
                var query = db.Execute(sql);
                return query;
            }
        }

        private Stream ExportCsvStream<T>(IEnumerable<T> items, bool useQuotes = true) where T : class
        {
            
            var memoryStream = new MemoryStream();
            var streamWriter = new StreamWriter(memoryStream);
            var csv = new CsvWriter(streamWriter);
            if (useQuotes)
            {
                csv.Configuration.ShouldQuote = (field, context) => true;
            }
            var dateOptions = new TypeConverterOptions { Formats = new[] { "yyyy-MM-dd HH:mm:ss" } };
            var boolConverter = new MySQLBooleanConverter();
            csv.Configuration.TypeConverterOptionsCache.AddOptions<DateTime>(dateOptions);
            csv.Configuration.TypeConverterCache.AddConverter<bool>(boolConverter);
            csv.WriteRecords<T>(items);
            streamWriter.Flush();
            csv.Flush();
            return memoryStream;

        }
    }
}
