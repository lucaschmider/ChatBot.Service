using System.Collections.Generic;

namespace ChatBot.Service.Models
{
    public class MasterDataSchema
    {
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
            /// <summary>
            ///     The name of the property in the model
            /// </summary>
            public string Key { get; set; }

            /// <summary>
            ///     The human friendly name of the field
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            ///     The type of the field (either 'text' or 'options')
            /// </summary>
            public string Type { get; set; }

            /// <summary>
            ///     The valid options for the field
            /// </summary>
            public IEnumerable<object> Options { get; set; }
        }
    }
}