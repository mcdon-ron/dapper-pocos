using System.Data;
using Dapper;
using System.Data.SqlClient;
using System.Text;
using System;

namespace PocoExtension
{
    public static class StoredProcedurePocoExtension
    {
        public static string GetInputPoco(this SqlConnection sqlConnection, string storedProcedureName)
        {
            var set = sqlConnection.QueryMultiple($"sp_HELP N'{storedProcedureName}'", CommandType.StoredProcedure);
            set.Read();
            if (set.IsConsumed)
                return "// N/A";

            var list = set.Read();
            var sb = new StringBuilder();
            sb.AppendLine("public class InputPoco");
            sb.AppendLine("{");
            foreach (var item in list)
            {
                string type = Map(item.Type);
                string name = item.Parameter_name;
                //name = name.Substring(1); // drop the leading '@'
                sb.AppendLine($"    public {type} {name.Substring(1)} {{ get; set; }}");
            }
            sb.AppendLine("}");
            return sb.ToString();
        }

        public static string GetOutputPoco(this SqlConnection sqlConnection, string storedProcedureName)
        {
            var list = sqlConnection.Query($"sp_describe_first_result_set N'{storedProcedureName}'", CommandType.StoredProcedure);
            var sb = new StringBuilder();
            sb.AppendLine("public class OutputPoco");
            sb.AppendLine("{");
            foreach (var item in list)
            {
                string type = Map(item.system_type_name);
                string name = item.name;
                sb.AppendLine($"    public {type} {name} {{ get; set; }}");
            }
            sb.AppendLine("}");
            return sb.ToString();
        }

        public static string Map(string sqlTypeName)
        {
            // drop any length specifier
            // like nvarchar(x) -> nvarchar
            var typeName = sqlTypeName.Split('(')[0];
            // sql types that are commented out
            // are those I don't know how to
            // properly map to clr types
            switch (typeName)
            {
                case "bigint":
                    return "long"; // Int64
                case "binary":
                case "image":
                //case "FILESTREAM":
                case "rowversion":
                case "timestamp":
                case "varbinary":
                    return "byte[]"; // Byte[]
                case "bit":
                    return "bool"; // Boolean
                case "char":
                case "nchar":
                case "text":
                case "ntext":
                case "varchar":
                case "nvarchar":
                    return "string"; // String
                case "date":
                case "datetime":
                case "datetime2":
                case "smalldatetime":
                    return "DateTime";
                case "datetimeoffset":
                    return "DateTimeOffset";
                case "decimal":
                case "money":
                case "numeric":
                case "smallmoney":
                    return "decimal"; // Decimal
                case "float":
                    return "double"; // Double
                case "int":
                    return "int"; // Int32
                case "real":
                    return "float"; // Single
                case "smallint":
                    return "short"; // Int16
                //case "sql_variant":
                //return "Object";
                case "time":
                    return "TimeSpan";
                case "tinyint":
                    return "byte"; // Byte
                case "uniqueidentifier":
                    return "Guid";
                case "xml":
                    // based on https://github.com/StackExchange/Dapper/issues/427
                    // it apprears Dapper can map the DbType "xml" to XmlDocument or XDocument or XElement
                    // choosing XDocument based on https://stackoverflow.com/a/1542101/135280
                    return "XDocument";
                default:
                    throw new ArgumentOutOfRangeException(nameof(sqlTypeName), sqlTypeName, "Unhandled SQL Type");
            }
        }
    }
}
