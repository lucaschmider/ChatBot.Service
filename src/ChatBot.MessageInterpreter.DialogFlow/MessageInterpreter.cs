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
                Session = $"projects/hidden-howl-282919/agent/sessions/{contextId}"
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
                    "projects/hidden-howl-282919/agent/entityTypes/0f587498-d18f-429a-bf4c-88c5fb9f5c63")
                .ConfigureAwait(false);
            return entityType.Entities.Select(entity => new KnowledgeTerm
            {
                Keyword = entity.Value,
                Synonyms = entity.Synonyms
            });
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