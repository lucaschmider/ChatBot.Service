using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatBot.Repository.Contracts;
using ChatBot.Repository.Contracts.Models;
using ChatBot.Repository.MongoDb.Configurations;
using ChatBot.Repository.MongoDb.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Shouldly;

namespace ChatBot.Repository.MongoDb
{
    public class KnowledgeRepository : RepositoryBase<InternalKnowledge>, IKnowledgeRepository
    {
        private readonly ILogger<KnowledgeRepository> _logger;

        public KnowledgeRepository(MongoDbConfiguration configuration, ILogger<KnowledgeRepository> logger) : base(
            configuration)
        {
            logger.ShouldNotBeNull();
            _logger = logger;
        }

        public async Task<string> GetDefinitionAsync(string definitionType, string keyword)
        {
            _logger.LogInformation($"Loading definition for {keyword}");
            var definitionCursor = await Collection.FindAsync(knowledge =>
                knowledge.DefinitionType == definitionType && knowledge.Keyword == keyword);
            return definitionCursor.FirstOrDefault()?.Description;
        }

        public async Task<IEnumerable<Knowledge>> GetAllDefinitionsAsync()
        {
            _logger.LogInformation("Loading all definitions");
            var definitions = await Collection
                .FindAsync(x => true)
                .ConfigureAwait(false);
            return definitions.ToList().Select(definition => definition.Map());
        }
    }
}