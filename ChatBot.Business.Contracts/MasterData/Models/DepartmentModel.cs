namespace ChatBot.Business.Contracts.MasterData.Models
{
    /// <summary>
    ///     Represents a department
    /// </summary>
    public class DepartmentModel
    {
        /// <summary>
        ///     The id for the department
        /// </summary>
        public string DepartmentId { get; set; }

        /// <summary>
        ///     The name of the department
        /// </summary>
        public string DepartmentName { get; set; }
    }
}