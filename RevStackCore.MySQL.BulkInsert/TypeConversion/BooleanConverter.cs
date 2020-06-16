using System;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace RevStackCore.MySQL.BulkInsert
{
    public class MySQLBooleanConverter : DefaultTypeConverter
    {
        public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            var boolValue = (bool)value;

            return boolValue ? "1" : "0";
        }
    }
}
