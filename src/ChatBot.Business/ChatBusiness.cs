using System;
using System.Threading.Tasks;
using ChatBot.Business.Contracts.Chat;
using ChatBot.MessageInterpreter.Contract;
using ChatBot.Repository.Contracts;
using ChatBot.Repository.Contracts.Models;
using Shouldly;

namespace ChatBot.Business
{
    public class ChatBusiness : IChatBusiness
    {
        private readonly IChatRepository _chatRepository;
        private readonly IMessageInterpreter _messageInterpreter;

        public ChatBusiness(IMessageInterpreter messageInterpreter, IChatRepository chatRepository)
        {
            messageInterpreter.ShouldNotBeNull();
            chatRepository.ShouldNotBeNull();

            _messageInterpreter = messageInterpreter;
            _chatRepository = chatRepository;
        }

        public async Task HandleMessageAsync(string message, string userId)
        {
            var messageInterpretation = await _messageInterpreter
                .InterpretMessageAsync(message, userId)
                .ConfigureAwait(false);

            var explanation = string.Empty;
            if (messageInterpretation.IsCompleted)
            {
                explanation = await GetExplanationAsync(
                        messageInterpretation.Parameters["definitiontype"],
                        messageInterpretation.Parameters["keyword"])
                    .ConfigureAwait(false);
            }

            await _chatRepository.SendMessageAsync(new ChatMessage
            {
                ConversationFinished = messageInterpretation.IsCompleted,
                CreateDate = DateTime.Now,
                Message = messageInterpretation.IsCompleted ? explanation : messageInterpretation.AnswerString,
                Recipient = userId
            }).ConfigureAwait(false);
        }

        private async Task<string> GetExplanationAsync(string definitionType, string keyword)
        {
            return await Task.FromResult($"Ok, ein {keyword} nach {definitionType} ist [...].");
        }
    }
}