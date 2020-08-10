using System.Threading.Tasks;
using ChatBot.Business.Contracts.MasterData.Models;

namespace ChatBot.Business.Contracts.MasterData
{
    /// <summary>
    ///     Provides methods to manage master data
    /// </summary>
    public interface IMasterDataBusiness
    {
        /// <summary>
        ///     Creates a new department
        /// </summary>
        /// <param name="departmentName"></param>
        /// <returns></returns>
        Task<DepartmentModel> CreateDepartmentAsync(string departmentName);

        /// <summary>
        ///     Deletes a department
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        Task DeleteDepartmentAsync(string departmentId);

        /// <summary>
        ///     Returns the schema of a model
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>

        Task<DataSchemaModel> GetSchema(MasterDataType type);
    }
}