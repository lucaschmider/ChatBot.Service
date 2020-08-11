namespace ChatBot.Service.Models
{
    /// <summary>
    ///     Represents the request that is sent to create a new department
    /// </summary>
    public class CreateDepartmentRequest
    {
        /// <summary>
        ///     The name of the new department
        /// </summary>
        public string DepartmentName { get; set; }
    }
}