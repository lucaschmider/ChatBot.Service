using System.Threading.Tasks;

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
    }
}