using System.Collections.Generic;
using System.ComponentModel;

namespace ChatBot.Business.Contracts.MasterData.Models
{
    /// <summary>
    ///     Represents the schema of a model
    /// </summary>
    public class DataSchemaModel
    {
        /// <summary>
        ///     The scheme of a department
        /// </summary>
        public static readonly DataSchemaModel Department = new DataSchemaModel
        {
            Collection = "departments",
            Fields = new[]
            {
                new DataFieldModel
                {
                    Key = "departmentName", Name = "Abteilung", Type = DataFieldModel.DataFieldType.Text, Options = new List<object>()
                }
            }
        };

        public static readonly DataSchemaModel Knowledge = new DataSchemaModel
        {
            Collection = "knowledge",
            Fields = new[]
            {
                new DataFieldModel
                {
                    Key = "name", Name = "Titel", Type = DataFieldModel.DataFieldType.Text
                },
                new DataFieldModel
                {
                    Key = "keywords", Name = "Synonyme (Kommasepariert)", Type = DataFieldModel.DataFieldType.Text
                },
                new DataFieldModel
                {
                    Key = "definitiontype", Name = "Definition", Type = DataFieldModel.DataFieldType.Options,
                    Options = new List<object> {"GPM", "Prince2"}
                },
                new DataFieldModel
                {
                    Key = "description", Name = "Beschreibung", Type = DataFieldModel.DataFieldType.Text
                }
            }
        };

        /// <summary>
        ///     The name of the model
        /// </summary>
        public string Collection { get; set; }

        /// <summary>
        ///     The fields contained in the model
        /// </summary>
        public IEnumerable<DataFieldModel> Fields { get; set; }

        /// <summary>
        ///     Represents the schema of a single field
        /// </summary>
        public class DataFieldModel
        {
            public enum DataFieldType
            {
                [Description("text")] Text,
                [Description("options")] Options
            }

            /// <summary>
            ///     The name of the property in the model
            /// </summary>
            public string Key { get; set; }

            /// <summary>
            ///     The human friendly name of the field
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            ///     The type of the field
            /// </summary>
            public DataFieldType Type { get; set; }

            /// <summary>
            ///     The valid options for the field
            /// </summary>
            public List<object> Options { get; set; }
        }
    }
}