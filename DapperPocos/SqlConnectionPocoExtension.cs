using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PocoExtension
{
    public static class SqlConnectionPocoExtension
    {

        public static string GetInputPoco(this SqlConnection sqlConnection, string storedProcedureName)
        {
            var sql = @"
SELECT *
FROM sys.parameters p
WHERE p.[object_id] = object_id(@objName)
";
            // querying sys.parameters to find out if the parameters are nullable
            // and if they are output parameters
            var parmDetails = sqlConnection.Query<ParameterDetails>(sql, new { objName = storedProcedureName }, commandType: CommandType.Text);

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
                var trimmedName = name.TrimStart('@');
                string type = item.Type;

                // length in bytes
                var length = item.Length;

                string stype = type;

                // if a string type, add the length specifier
                // for char and varchar 1 byte per character
                if(type == "char" || type == "varchar")
                {
                    if(length == -1)
                        stype += "(max)";
                    else
                        stype += "(" + length + ")";
                }
                // for nchar and nvarchar 2 bytes per character
                else if (type == "nchar" || type == "nvarchar")
                {
                    if(length == -1)
                        stype += "(max)";
                    else
                        stype += "(" + length / 2 + ")";
                }

                var details = parmDetails.Single(x => x.name == name);
                var nullable = details.is_nullable == true;
                var isOutput = details.is_output;

                AppendProperty(sb, stype, nullable, isOutput, trimmedName);
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
                // the output parameters only apply to input poco
                bool isOutput = false;

                AppendProperty(sb, sqlType, nullable, isOutput, name);
            }
            sb.AppendLine("}");
            return sb.ToString();
        }

        private static void AppendProperty(StringBuilder stringBuilder, string sqlType, bool nullable, bool isOutput, string name)
        {
            sqlType = sqlType.Trim();

            // handle null or empty column names
            if (name != null)
                name = name.Trim();

            string cc = string.Empty;

            if(string.IsNullOrEmpty(name))
            {
                // convert to string representation of null or empty
                name = name == null ? "null" : "string.Empty";
                cc = "//"; // preparing to comment out related property in OutputPoco
                stringBuilder.AppendLine($"    // TODO: Commenting out property because Column Name is {name}.");
            }

            string csType = null;
            string length = null;
            if (TryMap(sqlType, nullable, out csType, out length))
            {
                stringBuilder.Append($"    {cc}public {csType} {name} {{ get; set; }}");
                if (isOutput)
                    stringBuilder.Append($" // IsOutput: {isOutput}");
                //if ((csType == "string" || csType == "string?") && !string.IsNullOrEmpty(length))
                //    stringBuilder.Append($" // Length: {length}");
                if (csType == "string" && !string.IsNullOrEmpty(length))
                {
                    stringBuilder.Append($" // Length: {length}");
                    if (nullable)
                        stringBuilder.Append(" // Nullable");
                }
                stringBuilder.AppendLine();
            }
            else
            {
                stringBuilder.AppendLine($"    // TODO: Add Mapping for Unknown SQL Server Data Type: {sqlType}");
                stringBuilder.AppendLine($"    // nullable type: {nullable}");
                if (isOutput)
                    stringBuilder.AppendLine($"    // IsOutput: {isOutput}");

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
                case "text":
                case "ntext":
                    csType = "string";
                    stringLength = type;
                    break;
                case "char":
                case "nchar":
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
