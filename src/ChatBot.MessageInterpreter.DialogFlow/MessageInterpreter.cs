using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatBot.MessageInterpreter.Contract;
using ChatBot.MessageInterpreter.Contract.Models;
using Google.Cloud.Dialogflow.V2;
using Shouldly;

namespace ChatBot.MessageInterpreter.DialogFlow
{
    public class MessageInterpreter : IMessageInterpreter
    {
        private readonly DialogFlowConfiguration _configuration;
        private EntityTypesClient _entityTypesClient;
        private SessionsClient _sessionsClient;

        public MessageInterpreter(DialogFlowConfiguration configuration)
        {
            configuration.ShouldNotBeNull();
            _configuration = configuration;
        }

        public async Task<InterpretationResult> InterpretMessageAsync(string message, string contextId)
        {
            await EnsureSessionsClient().ConfigureAwait(false);

            var detectIntent = await _sessionsClient.DetectIntentAsync(new DetectIntentRequest
            {
                QueryInput = new QueryInput
                {
                    Text = new TextInput
                    {
                        Text = message,
                        LanguageCode = "de-DE"
                    }
                },
                Session = CreateSessionPath(_configuration.ProjectId, contextId)
            }).ConfigureAwait(false);

            return new InterpretationResult
            {
                AnswerString = detectIntent.QueryResult.FulfillmentText,
                DetectedIntent = detectIntent.QueryResult.Intent.Name,
                IsCompleted = detectIntent.QueryResult.AllRequiredParamsPresent,
                Parameters = detectIntent.QueryResult.Parameters.Fields
                    .ToDictionary(parameter => parameter.Key,
                        parameter => parameter.Value.ToString().Trim('"'))
            };
        }

        public async Task<IEnumerable<KnowledgeTerm>> GetAllKnownTermsAsync()
        {
            await EnsureEntityTypesClient();
            var entityType = await _entityTypesClient
                .GetEntityTypeAsync(
                    CreateEntityTypePath(_configuration.ProjectId, _configuration.KeywordsEntityTypeGuid))
                .ConfigureAwait(false);
            return entityType.Entities.Select(entity => new KnowledgeTerm
            {
                Keyword = entity.Value,
                Synonyms = entity.Synonyms
            });
        }

        public async Task DeleteKnownTermAsync(string term)
        {
            await EnsureEntityTypesClient();
            await _entityTypesClient.BatchDeleteEntitiesAsync(new BatchDeleteEntitiesRequest
            {
                ParentAsEntityTypeName =
                    new EntityTypeName(_configuration.ProjectId, _configuration.KeywordsEntityTypeGuid),
                LanguageCode = "de-DE",
                EntityValues = {term}
            });
        }

        public async Task<bool> DefinitionTypeExistsAsync(string definitionType)
        {
            await EnsureEntityTypesClient();
            var definitionTypes = await _entityTypesClient
                .GetEntityTypeAsync(CreateEntityTypePath(_configuration.ProjectId,
                    _configuration.DefinitionTypeEntityTypeGuid))
                .ConfigureAwait(false);
            return definitionTypes.Entities
                .Any(entity =>
                    entity.Value.Equals(definitionType, StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task<KnowledgeTerm> CreateKnowledgeTermAsync(string term, IEnumerable<string> synonyms)
        {
            await EnsureEntityTypesClient();
            var enumerable = synonyms as string[] ?? synonyms.ToArray();

            await _entityTypesClient
                .BatchCreateEntitiesAsync(new BatchCreateEntitiesRequest
                {
                    ParentAsEntityTypeName = new EntityTypeName(_configuration.ProjectId,
                        _configuration.KeywordsEntityTypeGuid),
                    Entities =
                    {
                        new EntityType.Types.Entity {Value = term, Synonyms = {enumerable}}
                    }
                });

            return new KnowledgeTerm
            {
                Synonyms = enumerable,
                Keyword = term
            };
        }

        private static string CreateSessionPath(string projectId, string contextId)
        {
            return $"projects/{projectId}/agent/sessions/{contextId}";
        }

        private static string CreateEntityTypePath(string projectId, string entityTypeGuid)
        {
            return $"projects/{projectId}/agent/entityTypes/{entityTypeGuid}";
        }

        private async Task EnsureSessionsClient()
        {
            _sessionsClient ??= await new SessionsClientBuilder
            {
                JsonCredentials = _configuration.ServiceAccountJson
            }.BuildAsync();
        }

        private async Task EnsureEntityTypesClient()
        {
            _entityTypesClient ??= await new EntityTypesClientBuilder
                {JsonCredentials = _configuration.ServiceAccountJson}.BuildAsync();
        }
    }
}