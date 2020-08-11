namespace ChatBot.Repository.Contracts.Models
{
    /// <summary>
    ///     Represents a department as stored in the database
    /// </summary>
    public class Department
    {
        /// <summary>
        ///     The id of the department
        /// </summary>
        public string DepartmentId { get; set; }

        /// <summary>
        ///     The name of the department
        /// </summary>
        public string DepartmentName { get; set; }
    }
}