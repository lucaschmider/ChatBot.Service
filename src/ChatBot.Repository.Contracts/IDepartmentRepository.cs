using System.Collections.Generic;
using System.Threading.Tasks;
using ChatBot.Repository.Contracts.Models;

namespace ChatBot.Repository.Contracts
{
    /// <summary>
    ///     Provides crud operations for departments
    /// </summary>
    public interface IDepartmentRepository
    {
        /// <summary>
        ///     Checks whether a department exists
        /// </summary>
        /// <param name="departmentName"></param>
        /// <returns></returns>
        bool DepartmentExists(string departmentName);

        /// <summary>
        ///     Creates a new department
        /// </summary>
        /// <param name="departmentName"></param>
        /// <returns></returns>
        Task<Department> CreateDepartmentAsync(string departmentName);

        /// <summary>
        ///     Returns all registered departments
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Department>> GetAllDepartmentsAsync();
    }
}