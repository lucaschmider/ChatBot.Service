namespace ChatBot.Service.Models
{
    /// <summary>
    ///     Represents a department as it is returned 
    /// </summary>
    public class DepartmentResponse
    {
        /// <summary>
        ///     The id of the department
        /// </summary>
        public string _id { get; set; }

        /// <summary>
        ///     The name of the department
        /// </summary>
        public string DepartmentName { get; set; }
    }
}