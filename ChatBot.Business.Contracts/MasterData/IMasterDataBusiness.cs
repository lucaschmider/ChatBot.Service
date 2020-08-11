using System;
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
        /// <summary>
        ///     Creates a new department
        /// </summary>
        /// <param name="departmentName"></param>
        /// <returns></returns>
        Task<DepartmentModel> CreateDepartmentAsync(string departmentName);

        /// <summary>
        ///     Returns the schema of a model
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<DataSchemaModel> GetSchema(MasterDataType type);

        /// <summary>
        ///     Returns a list of all known word and definitions
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<KnowledgeModel>> GetKnowledgeBaseAsync();
        
        /// <summary>
        ///     Deletes the specified definition from the knowledge base.
        ///     Further deletes the term from the message interpreter if
        ///     it is no longer needed.
        /// </summary>
        /// <param name="definitionType"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        Task DeleteTermAsync(string definitionType, string keyword);
    }
}