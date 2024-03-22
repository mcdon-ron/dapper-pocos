namespace PocoExtension
{
    public class ParameterDetails
    {
        public int object_id { get; set; }
        public string name { get; set; } // Length: 128
        public int parameter_id { get; set; }
        public byte system_type_id { get; set; }
        public int user_type_id { get; set; }
        public short max_length { get; set; }
        public byte precision { get; set; }
        public byte scale { get; set; }
        public bool is_output { get; set; }
        public bool is_cursor_ref { get; set; }
        public bool has_default_value { get; set; }
        public bool is_xml_document { get; set; }
        // TODO: Add Mapping for Unknown SQL Server Data Type: sql_variant
        // nullable type: True
        // public object default_value { get; set; }
        public int xml_collection_id { get; set; }
        public bool is_readonly { get; set; }
        public bool? is_nullable { get; set; }
        public int? encryption_type { get; set; }
        public string encryption_type_desc { get; set; } // Length: 64
        public string encryption_algorithm_name { get; set; } // Length: 128
        public int? column_encryption_key_id { get; set; }
        public string column_encryption_key_database_name { get; set; } // Length: 128
    }
}
