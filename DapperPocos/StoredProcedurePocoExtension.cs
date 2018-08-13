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
            var set = sqlConnection.QueryMultiple("sp_HELP", new { objname = storedProcedureName }, commandType: CommandType.StoredProcedure);
            set.Read();
            if (set.IsConsumed)
                return "// N/A";

            var list = set.Read();
            var sb = new StringBuilder();
            sb.AppendLine("public class InputPoco");
            sb.AppendLine("{");
            foreach (var item in list)
            {
                string name = item.Parameter_name;
                name = name.TrimStart('@');
                string stype = item.Type;

                AppendProperty(sb, stype, name);
            }
            sb.AppendLine("}");
            return sb.ToString();
        }

        public static string GetOutputPoco(this SqlConnection sqlConnection, string sql)
        {
            var list = sqlConnection.Query("sp_describe_first_result_set", new { tsql = sql }, commandType: CommandType.StoredProcedure);
            var sb = new StringBuilder();
            sb.AppendLine("public class OutputPoco");
            sb.AppendLine("{");
            foreach (var item in list)
            {
                string name = item.name;
                string stype = item.system_type_name;

                AppendProperty(sb, stype, name);
            }
            sb.AppendLine("}");
            return sb.ToString();
        }

        private static void AppendProperty(StringBuilder stringBuilder, string sqlType, string name)
        {
            string csType = null;
            if (TryMap(sqlType, out csType))
                stringBuilder.AppendLine($"    public {csType} {name} {{ get; set; }}");
            else
            {
                stringBuilder.AppendLine($"    // TODO: Add Mapping for Unknown SQL Server Data Type: {sqlType}");
                stringBuilder.AppendLine($"    // public object {name} {{ get; set; }}");
            }
        }

        public static bool TryMap(string sqlType, out string csType)
        {
            // drop any length specifier
            // like nvarchar(x) -> nvarchar
            var type = sqlType.Split('(')[0];
            // sql types that are commented out
            // are those I don't know how to
            // properly map to clr types
            switch (type)
            {
                case "bigint":
                    csType = "long"; // Int64
                    break;
                case "binary":
                case "image":
                //case "FILESTREAM":
                case "rowversion":
                case "timestamp":
                case "varbinary":
                    csType = "byte[]"; // Byte[]
                    break;
                case "bit":
                    csType = "bool"; // Boolean
                    break;
                case "char":
                case "nchar":
                case "text":
                case "ntext":
                case "varchar":
                case "nvarchar":
                    csType = "string"; // String
                    break;
                case "date":
                case "datetime":
                case "datetime2":
                case "smalldatetime":
                    csType = "DateTime";
                    break;
                case "datetimeoffset":
                    csType = "DateTimeOffset";
                    break;
                case "decimal":
                case "money":
                case "numeric":
                case "smallmoney":
                    csType = "decimal"; // Decimal
                    break;
                case "float":
                    csType = "double"; // Double
                    break;
                case "int":
                    csType = "int"; // Int32
                    break;
                case "real":
                    csType = "float"; // Single
                    break;
                case "smallint":
                    csType = "short"; // Int16
                    break;
                //case "sql_variant":
                //return "Object";
                case "time":
                    csType = "TimeSpan";
                    break;
                case "tinyint":
                    csType = "byte"; // Byte
                    break;
                case "uniqueidentifier":
                    csType = "Guid";
                    break;
                case "xml":
                    // based on https://github.com/StackExchange/Dapper/issues/427
                    // it apprears Dapper can map the DbType "xml" to XmlDocument or XDocument or XElement
                    // choosing XDocument based on https://stackoverflow.com/a/1542101/135280
                    csType = "XDocument";
                    break;
                default:
                    csType = null;
                    return false;
            }
            return true;
        }
    }
}
