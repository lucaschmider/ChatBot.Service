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

        private async Task EnsureSessionsClient()
        {
            if (_sessionsClient == null)
                _sessionsClient = await new SessionsClientBuilder
                {
                    JsonCredentials = _configuration.ServiceAccountJson
                }.BuildAsync();
        }
    }
}