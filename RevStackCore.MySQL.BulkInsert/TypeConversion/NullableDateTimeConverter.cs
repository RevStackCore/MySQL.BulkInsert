using System;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace RevStackCore.MySQL.BulkInsert
{
    // LOAD DATA reads an EMPTY field into a datetime column as the MySQL
    // zero-date '0000-00-00' (permissive sql_mode coercion, not an error) —
    // which MySqlConnector then THROWS on at read time, breaking every Dapper
    // query that touches the table. CsvHelper's default null rendering is the
    // empty string, so a null DateTime? on any bulk-inserted entity poisoned
    // its table (R&S person.deceased_date, 2026-09-01; Fairfield 2026-08-28).
    // \N is LOAD DATA's native NULL sentinel: null now lands as real NULL.
    public class MySQLNullableDateTimeConverter : DefaultTypeConverter
    {
        public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            if (value == null) return "\\N";
            return ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
