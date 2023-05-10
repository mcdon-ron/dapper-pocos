﻿using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace PocoExtension
{
    public static class SqlConnectionPocoExtension
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
            // add a note for the user about nullable parameters
            // because sp_HELP doesn't indicate if a parameter is nullable
            sb.AppendLine("    // TODO: decide if the following properties are nullable");
            foreach (var item in list)
            {
                string name = item.Parameter_name;
                name = name.TrimStart('@');
                string stype = item.Type;

                // for now assuming parameters are not nullable
                // leaving it to the user to decide
                AppendProperty(sb, stype, false, name);
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
                string sqlType = item.system_type_name;
                bool nullable = item.is_nullable;

                AppendProperty(sb, sqlType, nullable, name);
            }
            sb.AppendLine("}");
            return sb.ToString();
        }

        private static void AppendProperty(StringBuilder stringBuilder, string sqlType, bool nullable, string name)
        {
            sqlType = sqlType.Trim();
            name = name.Trim();

            string csType = null;
            string length = null;
            if (TryMap(sqlType, nullable, out csType, out length))
            {
                if (csType == "string" && !string.IsNullOrEmpty(length))
                    stringBuilder.AppendLine($"    public {csType} {name} {{ get; set; }} // Length: {length}");
                else
                    stringBuilder.AppendLine($"    public {csType} {name} {{ get; set; }}");
            }
            else
            {
                stringBuilder.AppendLine($"    // TODO: Add Mapping for Unknown SQL Server Data Type: {sqlType}");
                stringBuilder.AppendLine($"    // nullable type: {nullable}");
                stringBuilder.AppendLine($"    // public object {name} {{ get; set; }}");
            }
        }

        private static string[] ParseSqlType(string sqlType)
        {
            var result = new string[] {null, null};

            // split the type and any length specifier
            // like nvarchar(x)
            var parts = sqlType.Trim(')').Split('(');

            // the type name
            result[0] = parts[0].Trim().ToLower();

            // if we have a length specifier
            if (parts.Length > 1)
            {
                // the length specifier
                result[1] = parts[1].Trim();
            }

            return result;
        }

        public static bool TryMap(string sqlType, bool nullable, out string csType, out string stringLength)
        {
            // for handling nullable value types
            var n = nullable ? "?" : string.Empty;

            var parsedType = ParseSqlType(sqlType);
            var type = parsedType[0];
            stringLength = parsedType[1];

            // sql types that are commented out
            // are those I don't know how to
            // properly map to clr types
            switch (type)
            {
                case "bigint":
                    csType = "long" + n; // Int64
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
                    csType = "bool" + n; // Boolean
                    break;
                case "char":
                case "nchar":
                case "text":
                case "ntext":
                case "varchar":
                case "nvarchar":
                    if (stringLength == "1")
                        csType = "char"; // Char
                    else
                        csType = "string"; // String
                    break;
                case "date":
                case "datetime":
                case "datetime2":
                case "smalldatetime":
                    csType = "DateTime" + n;
                    break;
                case "datetimeoffset":
                    csType = "DateTimeOffset" + n;
                    break;
                case "decimal":
                case "money":
                case "numeric":
                case "smallmoney":
                    csType = "decimal" + n; // Decimal
                    break;
                case "float":
                    csType = "double" + n; // Double
                    break;
                case "int":
                    csType = "int" + n; // Int32
                    break;
                case "real":
                    csType = "float" + n; // Single
                    break;
                case "smallint":
                    csType = "short" + n; // Int16
                    break;
                //case "sql_variant":
                //return "Object";
                case "time":
                    csType = "TimeSpan" + n;
                    break;
                case "tinyint":
                    csType = "byte" + n; // Byte
                    break;
                case "uniqueidentifier":
                    csType = "Guid" + n;
                    break;
                case "xml":
                    // based on https://github.com/StackExchange/Dapper/issues/427
                    // it appears Dapper can map the DbType "xml" to XmlDocument or XDocument or XElement
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
