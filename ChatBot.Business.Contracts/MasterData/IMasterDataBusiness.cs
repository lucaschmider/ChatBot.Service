using System.Collections.Generic;
using System.Threading.Tasks;
using ChatBot.Business.Contracts.MasterData.Models;

namespace ChatBot.Business.Contracts.MasterData
{
    /// <summary>
    ///     Provides methods to manage master data
    /// </summary>
    public interface IMasterDataBusiness
    {
        Task<DepartmentModel> CreateDepartmentAsync(string departmentName);
        Task<IEnumerable<DepartmentModel>> GetDepartmentsAsync();
        Task DeleteDepartmentAsync(string departmentId);
    }
}