using System.Collections.Generic;
using System.Threading.Tasks;
using ChatBot.Repository.Contracts.Models;

namespace ChatBot.Repository.Contracts
{
    /// <summary>
    ///     Provides methods to manage explanations, keywords and
    /// </summary>
    public interface IKnowledgeRepository
    {
        /// <summary>
        ///     Returns the explanation for the given keyword according to the definitionType
        /// </summary>
        /// <param name="definitionType"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        Task<string> GetDefinitionAsync(string definitionType, string keyword);

        /// <summary>
        ///     Returns all definitions
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Knowledge>> GetAllDefinitionsAsync();
    }
}